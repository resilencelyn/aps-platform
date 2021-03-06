using Aps.Shared.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aps.Services
{
    public interface IAssemblyProcessRepository
    {
        Task<IEnumerable<ApsAssemblyProcess>> GetApsAssemblyProcessesAsync();
        Task<ApsAssemblyProcess> GetApsAssemblyProcessAsync(string assemblyProcessIdId);
        Task<IEnumerable<ApsAssemblyProcess>> GetAssemblyProcessesAsync(IEnumerable<string> assemblyProcessIds);
        Task AddAssemblyProcess(ApsAssemblyProcess assemblyProcess);
        void UpdateAssemblyProcess(ApsAssemblyProcess assemblyProcess);
        void DeleteAssemblyProcess(ApsAssemblyProcess assemblyProcess);
        Task<bool> AssemblyProcessExistsAsync(string assemblyProcessId);
        Task<bool> SaveAsync();
    }
}
