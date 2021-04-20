using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aps.Services;
using Aps.Shared.Entity;

namespace Aps.Helper
{
    public class NormalScheduleStrategy : IScheduleStrategy
    {
        private readonly IScheduleTool _scheduleTool;
        private readonly IEnumerable<ApsOrder> _orders;
        private readonly IEnumerable<ApsResource> _resources;


        public NormalScheduleStrategy(IScheduleTool scheduleTool,
            IEnumerable<ApsOrder> orders,
            IEnumerable<ApsResource> resources)
        {
            _scheduleTool = scheduleTool;
            _orders = orders;
            _resources = resources;
        }

        public async Task<ScheduleRecord> Schedule()
        {
            

            _scheduleTool.SetProductPrerequisite(_orders);
            _scheduleTool.GenerateProcess();
            _scheduleTool.GenerateJob();
            _scheduleTool.SetStreamJob();
            _scheduleTool.SetBatchJob();
            _scheduleTool.AssignResource(_resources);
            _scheduleTool.SetPreJobConstraint();
            _scheduleTool.SetAssemblyConstraint();
            _scheduleTool.SetObjective();
            var scheduleRecord = await _scheduleTool.Solve(ScheduleType.Normal);

            return scheduleRecord;
        }
    }
}