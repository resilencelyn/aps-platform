using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aps.Services;
using Aps.Shared.Entity;

namespace Aps.Helper
{
    public class InsertScheduleStrategy : IScheduleStrategy
    {
        private readonly IScheduleTool _scheduleTool;
        private readonly List<ApsOrder> _orders;
        private readonly List<ApsResource> _resources;

        public InsertScheduleStrategy(IScheduleTool scheduleTool, List<ApsOrder> orders, List<ApsResource> resources)
        {
            _scheduleTool = scheduleTool ?? throw new ArgumentNullException(nameof(scheduleTool));
            _orders = orders ?? throw new ArgumentNullException(nameof(orders));
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        }

        public async Task<ScheduleRecord> Schedule()
        {
            var resourceAvailableTime = TailScheduleStrategy.ResourceAvailableTimeCompute(_resources);

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
    }
}