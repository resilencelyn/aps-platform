using System;

namespace Aps.Shared.Entity
{
    public class ApsJob
    {
        public ApsOrder ApsOrder { get; set; }
        public ApsProduct ApsProduct { get; set; }
        public ProductInstance ProductInstance { get; set; }

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public DateTimeOffset? Duration { get; set; }

        public ApsResource ApsResource { get; set; }
    }
}