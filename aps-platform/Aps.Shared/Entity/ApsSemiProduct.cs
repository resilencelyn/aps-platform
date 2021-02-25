using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aps.Shared.Entity
{
    public class ApsSemiProduct
    {
        [Key] public string Id { get; set; }

        [Required]
        public List<ApsManufactureProcess> ApsManufactureProcesses { get; set; } = new List<ApsManufactureProcess>();

        public List<ApsProductSemiProduct> ApsProductsFromRequisite { get; set; } = new List<ApsProductSemiProduct>();
    }
}