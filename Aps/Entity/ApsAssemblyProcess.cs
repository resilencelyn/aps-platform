﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aps.Entity
{
    public class ApsAssemblyProcess : ApsProcess
    {
        [Required] public List<ApsAssemblyProcessSemiProduct> InputSemiFinishedProducts { get; set; } 
            = new();
        [Required] public ApsProduct OutputFinishedProduct { get; set; }
    }
}