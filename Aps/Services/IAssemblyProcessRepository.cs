using System.Collections.Generic;
using System.Threading.Tasks;
using Aps.Entity;

namespace Aps.Services
{
    public interface IAssemblyProcessRepository
    {
        Task<IEnumerable<ApsAssemblyProcess>> GetApsAssemblyProcessesAsync();
        Task<ApsAssemblyProcess> GetApsAssemblyProcessAsync(string assemblyProcessIdId);
        Task<IEnumerable<ApsAssemblyProcess>> GetAssemblyProcessesAsync(IEnumerable<string> assemblyProcessIds);
        void AddAssemblyProcess(ApsAssemblyProcess assemblyProcess);
        void UpdateAssemblyProcess(ApsAssemblyProcess assemblyProcess);
        void DeleteAssemblyProcess(ApsAssemblyProcess assemblyProcess);
        Task<bool> AssemblyProcessExistsAsync(string assemblyProcessId);
        Task<bool> SaveAsync();
    }
}
