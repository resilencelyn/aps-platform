using System;

namespace Aps.Shared.Model
{
    public class JobDto
    {
        public int Id { get; set; }
        public string ApsOrderId { get; set; }
        public string ProductId { get; set; }
        public int ProductInstanceId { get; set; }


        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int Duration { get; set; }
    }
}