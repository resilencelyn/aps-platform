using System;
using System.Collections.Generic;
using Aps.Shared.Extensions;

namespace Aps.Shared.Entity
{
    public class ScheduleRecord
    {
        public Guid Id { get; set; }

        public ICollection<ApsOrder> Orders { get; set; }
        public DateTime? ScheduleFinishTime { get; set; }

        public ICollection<ApsJob> Jobs { get; set; } = new List<ApsJob>();
        public RecordState RecordState { get; set; }

    }
}