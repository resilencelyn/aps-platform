using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace 高级计划与排产.Entity
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