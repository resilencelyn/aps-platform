using System.Threading.Tasks;
using Aps.Shared.Entity;

namespace Aps.Helper
{
    public class ScheduleContext
    {
        public IScheduleStrategy ScheduleStrategy {private get; set; }

        public async Task<ScheduleRecord> ExecuteSchedule()
        {
            return await ScheduleStrategy.Schedule();
        }
    }
}