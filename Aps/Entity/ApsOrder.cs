using System;
using System.ComponentModel.DataAnnotations;

namespace Aps.Entity
{
    public class ApsOrder
    {
        [Key] public string OrderId { get; set; }
        public string Description { get; set; }
        [Required] public DateTime EarliestStartTime { get; set; }
        [Required] public DateTime LatestEndTime { get; set; }

        [Required] public ApsProduct Product { get; set; }
        [Required] public int Amount { get; set; }
    }
}