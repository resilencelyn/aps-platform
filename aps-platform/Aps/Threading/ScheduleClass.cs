using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aps.Helper;
using Aps.Infrastructure;
using Aps.Infrastructure.Repositories;
using Aps.Services;
using Aps.Shared.Entity;
using Aps.Shared.Extensions;
using Google.OrTools.Sat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;

namespace Aps.Threading
{
    public class ScheduleModel
    {
        public ScheduleType ScheduleType { get; }
        public CpModel Model { get; set; }
        public ScheduleRecord ScheduleRecord { get; set; }

        public Dictionary<ManufactureJobNavigation, ApsManufactureJob> ScheduleManufactureJobs { get; set; }

        public Dictionary<AssemblyJobNavigation, ApsAssemblyJob> ScheduleAssemblyJobs { get; set; }

        public List<ApsJob> DistinctJobs { get; set; }
        public List<ApsResource> Resources { get; set; }
        public IntVar[,] ResourcePerformed { get; set; }

        public DateTime? StartTime { get; set; }
        public Dictionary<Guid, List<ApsManufactureJob>> BatchJobDictionary { get; set; }

        public ScheduleModel(CpModel model, ScheduleRecord scheduleRecord,
            Dictionary<ManufactureJobNavigation, ApsManufactureJob> scheduleManufactureJobs,
            List<ApsJob> jobs, List<ApsResource> resources, IntVar[,] resourcePerformed,
            Dictionary<Guid, List<ApsManufactureJob>> batchJobDictionary, DateTime? startTime,
            Dictionary<AssemblyJobNavigation, ApsAssemblyJob> scheduleAssemblyJobs, ScheduleType scheduleType)
        {
            ScheduleType = scheduleType;
            Model = model ?? throw new ArgumentNullException(nameof(model));

            ScheduleRecord = scheduleRecord ?? throw new ArgumentNullException(nameof(scheduleRecord));
            ScheduleManufactureJobs = scheduleManufactureJobs ??
                                      throw new ArgumentNullException(nameof(scheduleManufactureJobs));
            DistinctJobs = jobs ?? throw new ArgumentNullException(nameof(jobs));
            Resources = resources ?? throw new ArgumentNullException(nameof(resources));
            ResourcePerformed = resourcePerformed ?? throw new ArgumentNullException(nameof(resourcePerformed));
            BatchJobDictionary = batchJobDictionary ?? throw new ArgumentNullException(nameof(batchJobDictionary));
            StartTime = startTime;
            ScheduleAssemblyJobs = scheduleAssemblyJobs;
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

            var cpSolverStatus = await Task.Run(() => _solver.Solve(ScheduleModel.Model));
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

            var startTime = ScheduleModel.StartTime.GetValueOrDefault(DateTime.Now);

            foreach (var job in ScheduleModel.ScheduleManufactureJobs.Values.ToList())
            {
                job.Vars.Deconstruct(out var startVar, out var endVar, out _);

                var startValue = _solver.Value(startVar);
                job.Start = startTime.AddMinutes(solutionDictionary.ContainsKey(startVar)
                    ? solutionDictionary[startVar]
                    : startValue);

                var endValue = _solver.Value(endVar);
                job.End = startTime.AddMinutes(solutionDictionary.ContainsKey(endVar)
                    ? solutionDictionary[endVar]
                    : endValue);

                job.ScheduleRecord = ScheduleModel.ScheduleRecord;
            }

            foreach (var job in ScheduleModel.ScheduleAssemblyJobs.Values.ToList())
            {
                job.Vars.Deconstruct(out var startVar, out var endVar, out _);

                var startValue = _solver.Value(startVar);
                job.Start = startTime.AddMinutes(solutionDictionary.ContainsKey(startVar)
                    ? solutionDictionary[startVar]
                    : startValue);

                var endValue = _solver.Value(endVar);
                job.End = startTime.AddMinutes(solutionDictionary.ContainsKey(endVar)
                    ? solutionDictionary[endVar]
                    : endValue);

                job.ScheduleRecord = ScheduleModel.ScheduleRecord;
            }


            ScheduleModel.ScheduleRecord.ScheduleStartTime = startTime;
            ScheduleModel.ScheduleRecord.ScheduleFinishTime = ScheduleModel.DistinctJobs.Max(x => x.End);

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
            context.UpdateRange(ScheduleModel.ScheduleAssemblyJobs.Values.ToList());

            await context.SaveChangesAsync();


            var scheduleRecordRepository =
                scope.ServiceProvider.GetRequiredService<IRepository<ScheduleRecord, Guid>>();
            await scheduleRecordRepository.UpdateAsync(ScheduleModel.ScheduleRecord);

            await context.SaveChangesAsync();


            if (ScheduleModel.ScheduleType == ScheduleType.Insert)
            {
                foreach (var resource in ScheduleModel.Resources)
                {
                    var latestJobDateTime = resource.WorkJobs.Where(x =>
                    {
                        return x switch
                        {
                            ApsAssemblyJob assemblyJob => ScheduleModel.ScheduleAssemblyJobs.ContainsValue(assemblyJob),
                            ApsManufactureJob manufactureJob => ScheduleModel.ScheduleManufactureJobs.ContainsValue(
                                manufactureJob),
                            _ => false
                        };
                    }).Max(x => x.End.GetValueOrDefault());

                    var lateJobs = resource.WorkJobs.Where(x =>
                        x.Start > ScheduleModel.ScheduleRecord.Orders.Max(order => order.LatestEndTime)).ToList();

                    var delayTime = latestJobDateTime - lateJobs.Min(x => x.Start).GetValueOrDefault();

                    lateJobs.ForEach(j =>
                    {
                        j.Start += delayTime;
                        j.End += delayTime;
                    });
                }
            }
        }
    }
}