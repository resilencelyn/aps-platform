

namespace Aps.Entity
{
    public enum ProductionMode
    {
        Bp = 1,
        Sp = 2,
    }

    public class ApsManufactureProcess : ApsProcess
    {
        public string PrevPartId { get; set; }
        public ApsManufactureProcess PrevPart { get; set; }
    }
}