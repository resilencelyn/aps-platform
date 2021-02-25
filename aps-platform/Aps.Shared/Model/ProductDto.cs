using System.Collections.Generic;

namespace Aps.Shared.Model
{
    public class ProductDto
    {
        public string Id { get; set; }

        public List<ProductSemiProductDto> AssembleBySemiProducts { get; set; } = new List<ProductSemiProductDto>();
    }
}