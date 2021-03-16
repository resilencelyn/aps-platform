using System.Runtime.Serialization;

namespace Aps.Shared.Extensions
{
    public enum ProductionMode
    {
        [EnumMember(Value = "批处理")] Bp = 1,
        [EnumMember(Value = "流处理")] Sp = 2,
    }
}