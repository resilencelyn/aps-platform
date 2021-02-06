
namespace Aps.Shared.Entity
{
    

    public class ApsManufactureProcess : ApsProcess
    {
        public string PrevPartId { get; set; }
        public ApsManufactureProcess PrevPart { get; set; }
    }
}