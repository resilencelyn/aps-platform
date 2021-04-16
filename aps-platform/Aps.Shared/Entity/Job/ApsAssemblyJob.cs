using System.Collections.Generic;

namespace Aps.Shared.Entity
{
    public class ApsAssemblyJob : ApsJob
    {
        public ApsAssemblyProcess ApsAssemblyProcess { get; set; }
        public ScheduleRecord ScheduleRecord { get; set; }
        public List<ApsManufactureJob> ManufactureJobs { get; set; }
    }
}