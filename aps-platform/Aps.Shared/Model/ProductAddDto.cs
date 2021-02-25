using System.Collections.Generic;

namespace Aps.Shared.Model
{
    public class ProductAddDto
    {
        public string Id { get; set; }
        public List<ProductSemiProductAddDto> AssembleBySemiProducts { get; set; }
        public AssemblyProcessAddDto AssemblyProcess { get; set; }

    }
}