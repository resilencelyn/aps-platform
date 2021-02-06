
namespace Aps.Shared.Entity
{
    public class ApsAssemblyProcessSemiProduct
    {
        public ApsAssemblyProcess ApsAssemblyProcess { get; set; }
        public string ApsAssemblyProcessId { get; set; }
        public ApsSemiProduct ApsSemiProduct { get; set; }
        public string ApsSemiProductId { get; set; }

        public int Amount { get; set; }
    }
}