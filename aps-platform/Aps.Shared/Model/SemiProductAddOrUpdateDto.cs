using System.Collections.Generic;
using Aps.Shared.Model;

namespace Aps.Services
{
    public class SemiProductAddOrUpdateDto
    {
        public string Id { get; set; }
        public List<ManufactureProcessAddDto> ApsManufactureProcesses { get; set; }
    }
}