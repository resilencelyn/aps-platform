using System;

namespace Aps.Shared.Entity
{
    public class ApsOrder
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime EarliestStartTime { get; set; }
        public DateTime LatestEndTime { get; set; }

        public ApsProduct Product { get; set; }
        public int Amount { get; set; }
    }
}