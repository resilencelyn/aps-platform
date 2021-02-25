namespace Aps.Shared.Entity
{
    public class ResourceClassWithResource
    {
        public ApsResource ApsResource { get; set; }
        public ResourceClass ResourceClass { get; set; }


        public string ApsResourceId { get; set; }
        public int ResourceClassId { get; set; }

        public bool IsBasic { get; set; }
    }
}