using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aps.Services;
using Aps.Shared.Entity;

namespace Aps.Helper
{
    public class TailScheduleStrategy : IScheduleStrategy
    {
        private readonly IScheduleTool _scheduleTool;
        private readonly IEnumerable<ApsOrder> _orders;
        private readonly IEnumerable<ApsResource> _resources;

        public TailScheduleStrategy(IScheduleTool scheduleTool,
            IEnumerable<ApsOrder> orders,
            IEnumerable<ApsResource> resources
        )
        {
            _scheduleTool = scheduleTool;
            _orders = orders;
            _resources = resources;
        }

        public async Task<ScheduleRecord> Schedule()
        {
            var resourceAvailableTime = ResourceAvailableTimeCompute(_resources.ToList());

            _scheduleTool.SetProductPrerequisite(_orders);
            _scheduleTool.GenerateProcess();
            _scheduleTool.GenerateJob();
            _scheduleTool.SetStreamJob();
            _scheduleTool.SetBatchJob();
            _scheduleTool.AssignResource(_resources);
            _scheduleTool.SetResourceAvailableTime(resourceAvailableTime);
            _scheduleTool.SetPreJobConstraint();
            _scheduleTool.SetObjective();
            var scheduleRecord = await _scheduleTool.Solve();

            return scheduleRecord;
        }

        public static Dictionary<ApsResource, int> ResourceAvailableTimeCompute(List<ApsResource> resources)
        {
            var resourceAvailableTime = new Dictionary<ApsResource, int>();

            var earliestCompleteResourceTime = resources.Min(x =>
                x.WorkJobs.Max(job => job.End));

            foreach (var resource in resources)
            {
                var completeResourceTime = resource.WorkJobs.Max(x => x.End) - earliestCompleteResourceTime;

                var ceilingTime =
                    (int) Math.Ceiling(completeResourceTime.GetValueOrDefault(TimeSpan.Zero).TotalMinutes);
                resourceAvailableTime.Add(resource, ceilingTime);
            }

            return resourceAvailableTime;
        }
    }
}