using System.ComponentModel.DataAnnotations;

namespace Aps.Entity
{
    public enum ResourceType
    {
        人员 = 1,
        设备 = 2,
        机床 = 3,
    }

    public class ApsResource
    {
        [Key] public string ResourceId { get; set; }
        [Required] public ResourceType ResourceType { get; set; }

        public int? Amount { get; set; }
        [Required] public string ResourceAttributes { get; set; }
        [Required] public string BasicAttributes { get; set; }
        [Required] public Workspace Workspace { get; set; }
    }

    public enum Workspace
    {
        加工 = 1,
        装配 = 2,
    }
}