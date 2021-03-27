using System;
using System.Collections.Generic;
using Aps.Shared.Extensions;

namespace Aps.Shared.Entity
{
    public class ScheduleRecord
    {
        public Guid Id { get; set; }

        public IEnumerable<ApsOrder> Orders { get; set; }
        public DateTime? ScheduleFinishTime { get; set; }

        public List<ApsManufactureJob> Jobs { get; set; } = new List<ApsManufactureJob>();

        public List<ApsAssemblyJob> ApsAssemblyJobs { get; set; }
        public RecordState RecordState { get; set; }

    }
}