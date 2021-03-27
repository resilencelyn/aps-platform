using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aps.Infrastructure;
using Aps.Infrastructure.Repositories;
using Aps.Services;
using Aps.Shared.Entity;
using Aps.Shared.Extensions;
using Google.OrTools.Sat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Aps.Threading
{
    public class ScheduleModel
    {
        public CpModel Model { get; set; }
        public ScheduleRecord ScheduleRecord { get; set; }

        public Dictionary<ManufactureJobNavigation, ApsManufactureJob> ScheduleManufactureJobs { get; set; }

        public Dictionary<AssemblyJobNavigation, ApsAssemblyJob> ScheduleAssemblyJobs { get; set; }

        public List<ApsJob> DistinctJobs { get; set; }
        public List<ApsResource> Resources { get; set; }
        public IntVar[,] ResourcePerformed { get; set; }

        public Dictionary<Guid, List<ApsManufactureJob>> BatchJobDictionary { get; set; }

        public ScheduleModel(CpModel model, ScheduleRecord scheduleRecord,
            Dictionary<ManufactureJobNavigation, ApsManufactureJob> scheduleManufactureJobs,
            List<ApsJob> jobs, List<ApsResource> resources, IntVar[,] resourcePerformed,
            Dictionary<Guid, List<ApsManufactureJob>> batchJobDictionary)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));

            ScheduleRecord = scheduleRecord ?? throw new ArgumentNullException(nameof(scheduleRecord));

            ScheduleManufactureJobs = scheduleManufactureJobs ??
                                      throw new ArgumentNullException(nameof(scheduleManufactureJobs));
            DistinctJobs = jobs ?? throw new ArgumentNullException(nameof(jobs));

            Resources = resources ?? throw new ArgumentNullException(nameof(resources));

            ResourcePerformed = resourcePerformed ?? throw new ArgumentNullException(nameof(resourcePerformed));
            BatchJobDictionary = batchJobDictionary ?? throw new ArgumentNullException(nameof(batchJobDictionary));
        }
    }

    public class ScheduleClass : IScheduleClass
    {
        private readonly CpSolver _solver = new CpSolver();

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ScheduleModel ScheduleModel { get; set; }

        public ScheduleClass(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }


        public async void ExecuteAsync()
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

                job.ScheduleRecord = ScheduleModel.ScheduleRecord;
            }


            using var scope = _serviceScopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApsContext>();


            for (int i = 0; i < ScheduleModel.DistinctJobs.Count; i++)
            {
                var job = ScheduleModel.DistinctJobs[i];
                for (int j = 0; j < ScheduleModel.Resources.Count; j++)
                {
                    var resource = ScheduleModel.Resources[j];


                    if (_solver.Value(ScheduleModel.ResourcePerformed[i, j]) == 1)
                    {
                        job.ApsResource.Add(resource);
                    }
                }
            }

            foreach (var scheduleManufactureJob in ScheduleModel.ScheduleManufactureJobs.Values)
            {
                if (scheduleManufactureJob.BatchId != Guid.Empty && scheduleManufactureJob.ApsResource.Any())
                {
                    foreach (var batchJob in ScheduleModel.BatchJobDictionary[scheduleManufactureJob.BatchId])
                    {
                        batchJob.ApsResource = scheduleManufactureJob.ApsResource;
                    }
                }
            }

            context.UpdateRange(ScheduleModel.ScheduleManufactureJobs.Values.ToList());

            await context.SaveChangesAsync();


            var scheduleRecordRepository =
                scope.ServiceProvider.GetRequiredService<IRepository<ScheduleRecord, Guid>>();
            await scheduleRecordRepository.UpdateAsync(ScheduleModel.ScheduleRecord);

            await context.SaveChangesAsync();
        }
    }
}