using System.Collections.Generic;

namespace Aps.Shared.Model
{
    public class SemiProductDto
    {
        public string Id { get; set; }

        public List<ManufactureProcessDto> ApsManufactureProcesses { get; set; }
    }
}