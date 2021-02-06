using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aps.Shared.Entity
{
    public enum ProductionMode
    {
        Bp = 1,
        Sp = 2,
    }

    public enum ResourceType
    {
        人员 = 1,
        设备 = 2,
        机床 = 3,
    }

    public enum Workspace
    {
        加工 = 1,
        装配 = 2,
    }

    public abstract class ApsProcess
    {
        [Key] public string PartId { get; set; }
        public string PartName { get; set; }
        [Required] public ProductionMode ProductionMode { get; set; }

        [Required] public uint ProductionTime { get; set; }

        public uint? MinimumProductionQuantity { get; set; }
        public uint? MaximumProductionQuantity { get; set; }
        [Required] public Workspace Workspace { get; set; }
        [Required] public List<ApsProcessResource> ApsResources { get; set; } = new List<ApsProcessResource>();
    }
}