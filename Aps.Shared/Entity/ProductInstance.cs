using System;
using System.ComponentModel.DataAnnotations;

namespace Aps.Shared.Entity
{
    public class ProductInstance
    {
        [Key]
        public Guid Id { get; set; }
        public ApsProduct ApsProduct { get; set; }
        public ApsOrder OrderedBy { get; set; }

        public ProductInstance(Guid id, ApsProduct apsProduct, ApsOrder orderedBy)
        {
            Id = id;
            ApsProduct = apsProduct ?? throw new ArgumentNullException(nameof(apsProduct));
            OrderedBy = orderedBy ?? throw new ArgumentNullException(nameof(orderedBy));
        }

        public ProductInstance()
        {
        }
    }
}