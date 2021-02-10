using System.ComponentModel.DataAnnotations.Schema;

namespace Aps.Shared.Entity
{
    public class ApsProcessResource
    {
        public string ApsProcessId { get; set; }
        public int ResourceClassId { get; set; }

        public ApsProcess ApsProcess { get; set; }
        public ResourceClass ResourceClass { get; set; }
        

        public int Amount { get; set; }
    }
}