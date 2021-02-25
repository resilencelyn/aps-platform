using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Aps.Shared.Model
{
    public class AssemblyProcessDto : ProcessDto
    {
        public string OutputFinishedProductId { get; set; }
        public List<AssemblyProcessSemiProductDto> InputSemiFinishedProducts { get; set; }
            = new List<AssemblyProcessSemiProductDto>();
    }
}