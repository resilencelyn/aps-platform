using Aps.Shared.Entity;
using Google.OrTools.Sat;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aps.Services
{
    public interface IScheduleTool
    {
        CpModel Model { get; }
        CpSolver Solver { get; }
        IEnumerable<ApsOrder> OrdersList { get; set; }
        Dictionary<ApsProduct, int> ProductPrerequisite { get; }
        Dictionary<ApsSemiProduct, int> SemiProductPrerequisite { get; }
        List<ApsManufactureProcess> ManufactureProcesses { get; }
        IEnumerable<ApsAssemblyProcess> AssemblyProcesses { get; }
        IDictionary<ApsManufactureProcess, int> ManufactureProcessRequisite { get; }
        Dictionary<(ApsOrder, ProductInstance, SemiProductInstance, ApsManufactureProcess), ScheduleManufactureJob> ScheduleManufactureJobs { get; }
        Dictionary<ApsOrder, List<ProductInstance>> ProductInstances { get; }
        Dictionary<(ApsOrder order, ProductInstance productInstance), List<SemiProductInstance>> SemiProductInstances { get; }
        void SetPrerequisite(List<ApsOrder> orders);
        void GenerateProcess();
        void GenerateJobsFromOrders();
        void SetBatchJob();
        Task AssignResource();
        void SetPreJobConstraint();
        void SetObjective();
        Task<IEnumerable<ScheduleManufactureJob>> Solve();
    }
}