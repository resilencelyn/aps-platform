using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Aps.Services;
using Aps.Shared.Entity;

namespace Aps.Helper
{
    public class InsertScheduleStrategy : IScheduleStrategy
    {
        private readonly IScheduleTool _scheduleTool;
        private readonly ApsOrder _order;
        private readonly List<ApsResource> _resources;

        public InsertScheduleStrategy(IScheduleTool scheduleTool, ApsOrder order, List<ApsResource> resources)
        {
            _scheduleTool = scheduleTool ?? throw new ArgumentNullException(nameof(scheduleTool));
            _order = order ?? throw new ArgumentNullException(nameof(order));
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        }

        public async Task<ScheduleRecord> Schedule()
        {
            var resourceAvailableTime =
                TailScheduleStrategy.ComputeResourceAvailableTimeCompute(_resources);

            _scheduleTool.SetProductPrerequisite(new List<ApsOrder> {_order});
            _scheduleTool.GenerateProcess();
            _scheduleTool.GenerateJob();
            _scheduleTool.SetStreamJob();
            _scheduleTool.SetBatchJob();

            var jobs = GetRescheduleJobs(_resources, _order.EarliestStartTime, _order.LatestEndTime);

            var apsJobs = jobs.ToList();
            _scheduleTool.SetExternalManufactureJob(apsJobs.OfType<ApsManufactureJob>());
            _scheduleTool.SetExternalAssemblyJob(apsJobs.OfType<ApsAssemblyJob>());
            
            _scheduleTool.AssignResource(_resources);
            _scheduleTool.SetResourceAvailableTime(ComputeInsertTimeResource(_order.EarliestStartTime, _resources));
            _scheduleTool.SetPreJobConstraint();
            _scheduleTool.SetObjective();
            var scheduleRecord = await _scheduleTool.Solve(ScheduleType.Insert);
            
            return scheduleRecord;
        }

        public static Dictionary<ApsResource, TimeSpan> ComputeInsertTimeResource(
            DateTime insertTime, IEnumerable<ApsResource> resources)
        {
            var startTimeSpanDictionary = new Dictionary<ApsResource, TimeSpan>();

            foreach (var resource in resources)
            {
                var job = resource.WorkJobs.FirstOrDefault(x => x.Start <= insertTime && x.End >= insertTime
                    && x.Start != null && x.End != null);

                if (job?.End != null)
                    startTimeSpanDictionary.Add(resource, job.End.Value - insertTime);
            }

            return startTimeSpanDictionary;
        }

        public static IEnumerable<ApsJob> GetRescheduleJobs(IEnumerable<ApsResource> resources, DateTime start,
            DateTime end)
        {
            return resources.SelectMany(x => x.WorkJobs )
                .Where(j => j.Start >= start && j.End <= end).ToList()
                .ToImmutableList();
        }
    }
}