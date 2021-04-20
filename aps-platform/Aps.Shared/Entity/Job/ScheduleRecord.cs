using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Aps.Shared.Extensions;

namespace Aps.Shared.Entity
{
    public class ScheduleRecord
    {
        public Guid Id { get; set; }

        public IEnumerable<ApsOrder> Orders { get; set; }

        public DateTime? ScheduleStartTime { get; set; }
        public DateTime? ScheduleFinishTime { get; set; }

        public List<ApsManufactureJob> Jobs { get; set; } = new List<ApsManufactureJob>();

        [NotMapped]
        public List<ApsResource> Resources { get; set; } = new List<ApsResource>();

        public List<ApsAssemblyJob> ApsAssemblyJobs { get; set; }
        public RecordState RecordState { get; set; }

    }
}