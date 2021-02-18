using System.Collections.Generic;

namespace Aps.Shared.Model
{
    public class ProductUpdateDto
    {
        public string Id { get; set; }
        public List<ProductSemiProductUpdateDto> AssembleBySemiProducts { get; set; }
        public AssemblyProcessUpdateDto AssemblyProcess { get; set; }
    }
}