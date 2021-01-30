using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Aps.Entity
{
    public class ApsProduct
    {
        [Key] public string ProductId { get; set; }
        [Required] public List<ApsSemiProduct> AssembleBySemiProducts { get; set; } = new();
    }
}