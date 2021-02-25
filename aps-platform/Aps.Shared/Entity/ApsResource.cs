using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aps.Shared.Entity
{

    public class ApsResource
    {
        [Key] public string Id { get; set; }
        [Required] public ResourceType Type { get; set; }

        public int? Amount { get; set; }
        [Required] public List<ResourceClassWithResource> ResourceAttributes { get; set; }
        [Required] public Workspace Workspace { get; set; }
    }
}