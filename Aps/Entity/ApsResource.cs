using System.ComponentModel.DataAnnotations;

namespace Aps.Entity
{
    
    public class ApsResource
    {
        [Key] public string ResourceId { get; set; }
        [Required] public ResourceType ResourceType { get; set; }

        public int? Amount { get; set; }
        [Required] public string ResourceAttributes { get; set; }
        [Required] public string BasicAttributes { get; set; }
        [Required] public Workspace Workspace { get; set; }
    }

    
}