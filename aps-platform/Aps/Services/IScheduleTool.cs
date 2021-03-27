using Aps.Shared.Entity;
using Aps.Shared.Model;
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
        Dictionary<ManufactureJobNavigation, ApsManufactureJob> ScheduleManufactureJobs { get; set; }

        Dictionary<ApsOrder, List<ProductInstance>> ProductInstances { get; }

        Dictionary<(ApsOrder order, ProductInstance productInstance), List<SemiProductInstance>> SemiProductInstances
        {
            get;
        }

        void SetProductPrerequisite(IEnumerable<ApsOrder> orders);
        void GenerateProcess();
        void GenerateJob();
        void SetStreamJob();
        void SetBatchJob();
        void AssignResource(IEnumerable<ApsResource> resources);
        public void SetPreJobConstraint();

        public void SetResourceAvailableTime(IDictionary<ApsResource, int> resourceAvailableTime);

        void SetObjective();
        Task<ScheduleRecord> Solve();
    }
}