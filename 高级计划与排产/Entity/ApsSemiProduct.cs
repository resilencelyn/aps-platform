using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aps.Entity
{
    public class ApsSemiProduct
    {
        [Key] public string SemiProductId { get; set; }

        [Required]
        public List<ApsManufactureProcess> ApsManufactureProcesses { get; set; } =
            new();

        public List<ApsProduct> ApsAssemblyProducts { get; set; }
    }
}