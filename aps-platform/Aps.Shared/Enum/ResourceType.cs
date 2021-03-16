using System.Runtime.Serialization;

namespace Aps.Shared.Enum
{
    public enum ResourceType
    {
        [EnumMember(Value = nameof(人员))] 人员 = 1,
        [EnumMember(Value = nameof(设备))] 设备 = 2,
        [EnumMember(Value = nameof(机床))] 机床 = 3,
    }
}