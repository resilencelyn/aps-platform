namespace Aps.Shared.Entity
{
    public class ApsProcessResource
    {

        public string ProcessId { get; set; }
        public int ResourceClassId { get; set; }

        public ApsProcess ApsProcess { get; set; }
        public ResourceClass ResourceClass { get; set; }


        public int Amount { get; set; }
    }
}