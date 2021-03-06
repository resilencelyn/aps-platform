using Aps.Shared.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Aps.Shared.Enum;

namespace Aps.Shared.Model
{
    public class ResourceDto
    {
        [Key] public string Id { get; set; }
        [Required] public ResourceType Type { get; set; }

        public int? Amount { get; set; }
        [Required] public List<ResourceClassWithResourceDto> ResourceAttributes { get; set; }
        [Required] public Workspace Workspace { get; set; }

        public ResourceState ResourceState { get; set; }

        public List<JobDto> WorkJobs { get; set; }
    }
}