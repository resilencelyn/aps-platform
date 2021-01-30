using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace 高级计划与排产.Entity
{
    public enum ProductionMode
    {
        Bp = 1,
        Sp = 2,
    }

    public class ApsManufactureProcess : ApsProcess
    {
        public string PrevPartId { get; set; }
        public ApsManufactureProcess PrevPart { get; set; }
    }
}