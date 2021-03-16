using System.Runtime.Serialization;

namespace Aps.Shared.Extensions
{
    public enum RecordState
    {
        [EnumMember(Value = "未知模型")] UnKnow = 1,
        [EnumMember(Value = "最优")] Optimal = 2,
        [EnumMember(Value = "可满足约束")] Feasible = 3,
        [EnumMember(Value = "不满足约束")] Infeasible = 4,
    }
}