using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Aps.Shared.Extensions;

namespace Aps.Shared.Entity
{
    public class ApsJob
    {
        [Key] public Guid Id { get; set; }
        public ApsOrder ApsOrder { get; set; }
        public string ApsOrderId { get; set; }
        public ApsProduct ApsProduct { get; set; }
        public string ApsProductId { get; set; }
        public ProductInstance ProductInstance { get; set; }

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public TimeSpan Duration { get; set; }

        public Workspace Workspace { get; set; }

        public List<ApsResource> ApsResource { get; set; } = new List<ApsResource>();

        public ApsProcess Process { get; set; }
        
        [NotMapped]
        public JobVar Vars { get; set; }
    }
}