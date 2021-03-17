using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Aps.Shared.Extensions;

namespace Aps.Shared.Entity
{
    public class ApsJob
    {
        [Key] public Guid Id { get; set; }
        public ApsOrder ApsOrder { get; set; }
        public ApsProduct ApsProduct { get; set; }
        public ProductInstance ProductInstance { get; set; }

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public TimeSpan Duration { get; set; }

        public Workspace Workspace { get; set; }

        public ScheduleRecord ScheduleRecord { get; set; }
        public List<ApsResource> ApsResource { get; set; } = new List<ApsResource>();

    }
}