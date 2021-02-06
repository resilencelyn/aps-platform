using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aps.Shared.Entity
{
    public class ApsProduct
    {
        [Key] public string ProductId { get; set; }

        [Required]
        public List<ApsProductSemiProduct> AssembleBySemiProducts { get; set; } = new List<ApsProductSemiProduct>();

        public List<ApsOrder> ApsOrdersBy { get; set; }
        public ApsAssemblyProcess ApsAssemblyProcess { get; set; }

        [ForeignKey(nameof(ApsAssemblyProcess))]
        public string ApsAssemblyProcessId { get; set; }
    }
}