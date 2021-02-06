using System;

namespace Aps.Shared.Entity
{
    public class ProductInstance
    {
        public Guid ProductId { get; set; }
        public ApsProduct ApsProduct { get; set; }
        public ApsOrder OrderedBy { get; set; }

        public ProductInstance(Guid productId, ApsProduct apsProduct, ApsOrder orderedBy)
        {
            ProductId = productId;
            ApsProduct = apsProduct ?? throw new ArgumentNullException(nameof(apsProduct));
            OrderedBy = orderedBy ?? throw new ArgumentNullException(nameof(orderedBy));
        }
    }
}