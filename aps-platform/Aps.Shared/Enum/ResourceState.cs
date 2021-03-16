using System.Runtime.Serialization;

namespace Aps.Shared.Enum
{
    public enum ResourceState
    {
        [EnumMember(Value = "运行中")] Running,
        [EnumMember(Value = "空闲")] Free,
        // [EnumMember(Value = "故障中断")] Breakdown
    }
}