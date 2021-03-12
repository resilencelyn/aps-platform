using System.Collections.Generic;

namespace Aps.Shared.Model
{
    public class SemiProductAddOrUpdateDto
    {
        public string Id { get; set; }
        public List<ManufactureProcessAddDto> ApsManufactureProcesses { get; set; }
    }
}