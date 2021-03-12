using System.Runtime.Serialization;

namespace Aps.Shared.Extensions
{
    public enum Workspace
    {
        [EnumMember(Value = nameof(加工))] 加工 = 1,
        [EnumMember(Value = nameof(装配))] 装配 = 2,
    }
}