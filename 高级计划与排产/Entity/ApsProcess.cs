using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace 高级计划与排产.Entity
{
    public abstract class ApsProcess
    {
        [Key] public string PartId { get; set; }
        public string PartName { get; set; }
        [Required] public ProductionMode ProductionMode { get; set; }

        [Required] public uint ProductionTime { get; set; }

        public uint? MinimumProductionQuantity { get; set; }
        public uint? MaximumProductionQuantity { get; set; }
        [Required] public Workspace Workspace { get; set; }
        [Required] public List<ApsResource> ApsResources { get; set; } = new();
    }
}