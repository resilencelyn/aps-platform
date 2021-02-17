namespace Aps.Shared.Model
{
    public class ResourceClassWithResourceDto
    {
        public string ApsResourceId { get; set; }
        public int ResourceClassId { get; set; }
        public string ResourceClassName { get; set; }

        public bool IsBasic { get; set; }
    }
}