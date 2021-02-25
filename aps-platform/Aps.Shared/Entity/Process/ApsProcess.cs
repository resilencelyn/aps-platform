using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Aps.Shared.Entity
{
    // [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductionMode
    {
        [EnumMember(Value = "批处理")] Bp = 1,
        [EnumMember(Value = "流处理")] Sp = 2,
    }

    public enum ResourceType
    {
        [EnumMember(Value = nameof(人员))] 人员 = 1,
        [EnumMember(Value = nameof(设备))] 设备 = 2,
        [EnumMember(Value = nameof(机床))] 机床 = 3,
    }

    public enum Workspace
    {
        [EnumMember(Value = nameof(加工))] 加工 = 1,
        [EnumMember(Value = nameof(装配))] 装配 = 2,
    }

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