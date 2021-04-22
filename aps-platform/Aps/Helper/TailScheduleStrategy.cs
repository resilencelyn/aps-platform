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
            var resourceAvailableTime = ComputeResourceAvailableTimeCompute(_resources.ToList());

            _scheduleTool.StartTime = ComputeScheduleStartTime(_resources.ToList());
            _scheduleTool.SetProductPrerequisite(_orders);
            _scheduleTool.GenerateProcess();
            _scheduleTool.GenerateJob();
            _scheduleTool.SetStreamJob();
            _scheduleTool.SetBatchJob();
            _scheduleTool.AssignResource(_resources);
            _scheduleTool.SetResourceAvailableTime(resourceAvailableTime);
            _scheduleTool.SetPreJobConstraint();
            
            _scheduleTool.SetAssemblyConstraint();
            
            _scheduleTool.SetOrderFinishTimeConstraint();
            _scheduleTool.SetObjective();
            var scheduleRecord = await _scheduleTool.Solve(ScheduleType.Tail);

            return scheduleRecord;
        }

        public static Dictionary<ApsResource, TimeSpan> ComputeResourceAvailableTimeCompute(
            List<ApsResource> resources)
        {
            var resourceAvailableTime = new Dictionary<ApsResource, TimeSpan>();

            var earliestCompleteResourceTime = resources.Min(x =>
                x.WorkJobs.Max(job => job.End));

            foreach (var resource in resources)
            {
                var resourceCompleteTime = resource.WorkJobs.Max(x => x.End);
                if (resourceCompleteTime < DateTime.Now)
                {
                    resourceAvailableTime.Add(resource, TimeSpan.Zero);
                }
                else
                {
                    var completeResourceTimeSpan = resourceCompleteTime - earliestCompleteResourceTime;
                    var ceilingTime = completeResourceTimeSpan.GetValueOrDefault(TimeSpan.Zero);

                    resourceAvailableTime.Add(resource, ceilingTime);
                }
            }

            return resourceAvailableTime;
        }

        public static DateTime? ComputeScheduleStartTime(List<ApsResource> resources)
        {
            var earliestCompleteResourceTime = resources.Min(x =>
                x.WorkJobs.Max(job => job.End));

            return earliestCompleteResourceTime;
        }
    }
}