using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace 高级计划与排产.Entity
{
    public class ApsAssemblyProcess : ApsProcess
    {
        [Required] public List<ApsSemiProduct> InputSemiFinishedProducts { get; set; } = new();
        [Required] public ApsProduct OutputFinishedProduct { get; set; }
    }
}