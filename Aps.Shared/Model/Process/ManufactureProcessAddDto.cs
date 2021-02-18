using System.ComponentModel.DataAnnotations;

namespace Aps.Shared.Model
{
    public class ManufactureProcessAddDto:ProcessAddDto
    {
        [Display(Name = "前置半成品工序ID")] public string PrevPartId { get; set; }
    }
}