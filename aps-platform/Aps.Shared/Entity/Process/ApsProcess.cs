using Aps.Shared.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aps.Shared.Entity
{

    public abstract class ApsProcess
    {
        [Display(Name = "工序ID")] [Key] public string Id { get; set; }
        [Display(Name = "工序名称")] public string PartName { get; set; }

        [Display(Name = "工艺")] [Required] public ProductionMode ProductionMode { get; set; }

        [Display(Name = "工序执行时间")] [Required] public TimeSpan ProductionTime { get; set; }

        [Display(Name = "最小可执行数量")] public int? MinimumProductionQuantity { get; set; }
        [Display(Name = "最大可执行数量")] public int? MaximumProductionQuantity { get; set; }
        [Display(Name = "生产车间")] [Required] public Workspace Workspace { get; set; }

        [Display(Name = "工序所需资源")]
        [Required]
        public List<ApsProcessResource> ApsResources { get; set; } = new List<ApsProcessResource>();
    }
}