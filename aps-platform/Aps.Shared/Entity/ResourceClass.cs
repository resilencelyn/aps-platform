using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aps.Shared.Entity
{
    public class ResourceClass
    {
        [Key] public int Id { get; set; }
        public string ResourceClassName { get; set; }
        public List<ResourceClassWithResource> ApsResources { get; set; } = new List<ResourceClassWithResource>();
    }
}