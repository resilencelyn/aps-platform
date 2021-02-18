using System;

namespace Aps.Shared.Model
{
    public class OrderAddDto
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime EarliestStartTime { get; set; }
        public DateTime LatestEndTime { get; set; }

        public ProductDto Product { get; set; }
        public int Amount { get; set; }
    }
}