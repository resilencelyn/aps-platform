namespace Aps.Threading
{
    public interface IScheduleClass
    {
        ScheduleModel ScheduleModel { get; set; }
        void ExecuteAsync();
    }
}