namespace Aps.Shared.Model
{
    public class ProcessResourceAddOrUpdateDto
    {
        public string ProcessId { get; set; }
        public int ResourceClassId { get; set; }
        public string ResourceClassName { get; set; }
        public int Amount { get; set; }
    }
}