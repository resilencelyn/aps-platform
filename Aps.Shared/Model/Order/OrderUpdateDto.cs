using System;

namespace Aps.Shared.Model
{
    public class OrderUpdateDto
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime EarliestStartTime { get; set; }
        public DateTime LatestEndTime { get; set; }

        public string ProductId { get; set; }
        public int Amount { get; set; }
    }
}