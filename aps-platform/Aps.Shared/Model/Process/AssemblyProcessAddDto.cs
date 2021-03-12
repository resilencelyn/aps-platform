using System.Collections.Generic;

namespace Aps.Shared.Model
{
    public class AssemblyProcessAddDto : ProcessAddDto
    {
        public string OutputFinishedProductId { get; set; }
        public List<AssemblyProcessSemiProductAddDto> InputSemiFinishedProducts { get; set; }
    }
}