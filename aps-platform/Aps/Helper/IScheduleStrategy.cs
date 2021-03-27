using System.Collections.Generic;
using System.Threading.Tasks;
using Aps.Services;
using Aps.Shared.Entity;

namespace Aps.Helper
{
    public interface IScheduleStrategy
    {
        public Task<ScheduleRecord> Schedule();
    }
}