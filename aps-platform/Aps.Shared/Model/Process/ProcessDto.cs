using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Aps.Shared.Entity;

namespace Aps.Shared.Model
{
    public class ProcessDto
    {
        [Display(Name = "工序ID")] public string Id { get; set; }
        [Display(Name = "工序名称")] public string PartName { get; set; }

        [Display(Name = "工艺")] public ProductionMode ProductionMode { get; set; }

        [Display(Name = "工序执行时间")] public int ProductionTime { get; set; }

        [Display(Name = "最小可执行数量")] public int? MinimumProductionQuantity { get; set; }
        [Display(Name = "最大可执行数量")] public int? MaximumProductionQuantity { get; set; }
        [Display(Name = "生产车间")] public Workspace Workspace { get; set; }

        [Display(Name = "工序所需资源")] public List<ProcessResourceDto> ApsResources { get; set; }
    }
}