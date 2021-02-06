using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aps.Shared.Entity
{
    public class ApsAssemblyProcess : ApsProcess
    {
        [Required]
        public List<ApsAssemblyProcessSemiProduct> InputSemiFinishedProducts { get; set; }
            = new List<ApsAssemblyProcessSemiProduct>();

        [Required] public ApsProduct OutputFinishedProduct { get; set; }
        [ForeignKey(nameof(ApsProduct))] public string OutputFinishedProductId { get; set; }
    }
}