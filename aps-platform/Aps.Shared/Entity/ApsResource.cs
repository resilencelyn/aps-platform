using Aps.Shared.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Aps.Shared.Enum;

namespace Aps.Shared.Entity
{
    public class ApsResource
    {
        [Key] public string Id { get; set; }
        [Required] public ResourceType Type { get; set; }

        public int? Amount { get; set; }
        [Required] public List<ResourceClassWithResource> ResourceAttributes { get; set; }
        [Required] public Workspace Workspace { get; set; }

        public List<ApsJob> WorkJobs { get; set; } = new List<ApsJob>();
    }
}