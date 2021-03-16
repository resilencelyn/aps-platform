using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aps.Infrastructure.Repositories;
using Aps.Shared.Entity;
using Aps.Shared.Extensions;
using Aps.Threading;
using Google.OrTools.Sat;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Aps.Services
{
    

    public class ScheduleHostService : IHostedService
    {
        private readonly CpSolver _solver = new CpSolver();
        public bool IsRun { get; set; } = false;
        private Timer _timer;

        private readonly IServiceProvider _serviceProvider;

        public ScheduleModel ScheduleModel { get; set; } = new ScheduleModel();


        public ScheduleHostService(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (IsRun == false)
            {
                return Task.CompletedTask;
            }

            _timer = new Timer(ExecuteAsync, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (IsRun == true)
            {
                IsRun = false;
            }
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async void ExecuteAsync(object state)
        {
            // Solver.StringParameters = "max_time_in_seconds:30.0";
            _solver.StringParameters = " num_search_workers:6";

            CpSolverStatus cpSolverStatus = await Task.Run(() => _solver.Solve(ScheduleModel.Model));
            Console.WriteLine(cpSolverStatus);


            switch (cpSolverStatus)
            {
                case CpSolverStatus.Unknown:
                    ScheduleModel.ScheduleRecord.RecordState = RecordState.UnKnow;
                    break;
                case CpSolverStatus.ModelInvalid:
                    throw new ApplicationException("模型错误");
                case CpSolverStatus.Feasible:
                    ScheduleModel.ScheduleRecord.RecordState = RecordState.Feasible;
                    break;
                case CpSolverStatus.Infeasible:
                    ScheduleModel.ScheduleRecord.RecordState = RecordState.Infeasible;
                    break;
                case CpSolverStatus.Optimal:
                    ScheduleModel.ScheduleRecord.RecordState = RecordState.Optimal;
                    break;
            }

            var solutionDictionary = new Dictionary<IntVar, int>();
            var objectiveValueString = $"Optimal Schedule Length: {_solver.ObjectiveValue}";

            Console.WriteLine(objectiveValueString);

            var now = DateTime.Now;
            foreach (var job in ScheduleModel.ScheduleManufactureJobs.Values.ToList())
            {
                job.Vars.Deconstruct(out var startVar, out var endVar, out _);

                var startValue = _solver.Value(startVar);
                job.Start = now.AddMinutes(solutionDictionary.ContainsKey(startVar)
                    ? solutionDictionary[startVar]
                    : startValue);

                var endValue = _solver.Value(endVar);
                job.End = now.AddMinutes(solutionDictionary.ContainsKey(endVar)
                    ? solutionDictionary[endVar]
                    : endValue);
            }


            for (int i = 0; i < ScheduleModel.Jobs.Count; i++)
            {
                var job = ScheduleModel.Jobs[i];
                for (int j = 0; j < ScheduleModel.Resources.Count; j++)
                {
                    var resource = ScheduleModel.Resources[j];

                    if (_solver.Value(ScheduleModel.ResourcePerformed[i, j]) == 1)
                    {
                        job.ApsResource.Add(resource);
                    }
                }
            }

            using var scope = _serviceProvider.CreateScope();

            var manufactureJobRepository =
                scope.ServiceProvider.GetRequiredService<IRepository<ApsManufactureJob, Guid>>();

            foreach (var scheduleManufactureJob in ScheduleModel.ScheduleManufactureJobs.Values.ToList())
            {
                await manufactureJobRepository.InsertAsync(scheduleManufactureJob);
            }

            for (var i = 0; i < ScheduleModel.Resources.Count; i++)
            {
                var resource = ScheduleModel.Resources[i];
                for (var j = 0; j < ScheduleModel.Jobs.Count; j++)
                {
                    var job = ScheduleModel.Jobs[j];

                    if (_solver.Value(ScheduleModel.ResourcePerformed[j, i]) == 1)
                    {
                        resource.WorkJobs.Add(job);
                    }
                }
            }

            var resourceRepository = scope.ServiceProvider.GetRequiredService<IRepository<ApsResource, string>>();
            await resourceRepository.SaveAsync();

            var scheduleRecordRepository =
                scope.ServiceProvider.GetRequiredService<IRepository<ScheduleRecord, Guid>>();
            await scheduleRecordRepository.UpdateAsync(ScheduleModel.ScheduleRecord);

            IsRun = false;
        }
    }
}