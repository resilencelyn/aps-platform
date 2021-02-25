namespace Aps.Shared.Entity
{
    public class ApsManufactureJob : ApsJob
    {
        public ApsSemiProduct ApsSemiProduct { get; set; }
        public SemiProductInstance SemiProductInstance { get; set; }
        public ApsManufactureProcess ApsManufactureProcess { get; set; }
    }
}