using Aps.Shared.Extensions;
using System.Collections.Generic;
using Aps.Shared.Enum;

namespace Aps.Shared.Model
{
    public class ResourceAddDto
    {
        public string Id { get; set; }
        public ResourceType Type { get; set; }

        public int? Amount { get; set; }
        public List<ResourceClassWithResourceAddOrUpdateDto> ResourceAttributes { get; set; }
        public Workspace Workspace { get; set; }
    }
}