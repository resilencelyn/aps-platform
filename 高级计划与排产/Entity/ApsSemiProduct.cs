using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace 高级计划与排产.Entity
{
    public class ApsSemiProduct
    {
        [Key] public string SemiProductId { get; set; }

        [Required]
        public List<ApsManufactureProcess> ApsManufactureProcesses { get; set; } =
            new();
    }
}