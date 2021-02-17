
using System.ComponentModel.DataAnnotations;

namespace Aps.Shared.Entity
{


    public class ApsManufactureProcess : ApsProcess
    {

        [Display(Name = "前置半成品工序ID")]
        public string PrevPartId { get; set; }
        public ApsManufactureProcess PrevPart { get; set; }
    }
}