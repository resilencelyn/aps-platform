using Aps.Shared.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aps.Shared.Model
{
    public class ManufactureProcessDto : ProcessDto
    {
        [Display(Name = "前置半成品工序ID")] public string PrevPartId { get; set; }
    }
}