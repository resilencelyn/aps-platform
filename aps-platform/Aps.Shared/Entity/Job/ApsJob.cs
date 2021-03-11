using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aps.Shared.Entity
{
    public class ApsJob
    {
        [Key]
        public Guid Id { get; set; }
        public ApsOrder ApsOrder { get; set; }
        public ApsProduct ApsProduct { get; set; }
        public ProductInstance ProductInstance { get; set; }

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public TimeSpan Duration { get; set; }

        public List<ApsResource> ApsResource { get; set; } = new List<ApsResource>();
    }
}