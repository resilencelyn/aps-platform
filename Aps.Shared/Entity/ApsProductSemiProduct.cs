namespace Aps.Shared.Entity
{
    public class ApsProductSemiProduct
    {
        public ApsProduct ApsProduct { get; set; }
        public ApsSemiProduct ApsSemiProduct { get; set; }

        public string ApsProductId { get; set; }
        public string ApsSemiProductId { get; set; }

        public int Amount { get; set; }
    }
}