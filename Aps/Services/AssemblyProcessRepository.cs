using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aps.Entity;
using Aps.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Aps.Services
{
    public class AssemblyProcessRepository : IAssemblyProcessRepository
    {
        private readonly ApsContext _context;

        public AssemblyProcessRepository(ApsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApsAssemblyProcess>> GetApsAssemblyProcessesAsync()
        {
            return await _context.ApsAssemblyProcesses
                .AsNoTracking()
                .Include(x => x.InputSemiFinishedProducts)
                .Include(x => x.OutputFinishedProduct)
                .Include(x => x.ApsResources)
                .ToListAsync();
        }

        public async Task<ApsAssemblyProcess> GetApsAssemblyProcessAsync(string assemblyProcessIdId)
        {
            return await _context.ApsAssemblyProcesses
                .AsNoTracking()
                .Include(x => x.InputSemiFinishedProducts)
                .Include(x => x.OutputFinishedProduct)
                .Include(x => x.ApsResources)
                .FirstOrDefaultAsync(p =>
                    string.Equals(p.PartId, assemblyProcessIdId, StringComparison.CurrentCulture));
        }

        public async Task<IEnumerable<ApsAssemblyProcess>> GetAssemblyProcessesAsync(
            IEnumerable<string> assemblyProcessIds)
        {
            return await _context.ApsAssemblyProcesses
                .AsNoTracking()
                .Include(x => x.InputSemiFinishedProducts)
                .Include(x => x.OutputFinishedProduct)
                .Include(x => x.ApsResources)
                .Where(p => assemblyProcessIds.Contains(p.PartId))
                .ToListAsync();
        }

        public void AddAssemblyProcess(ApsAssemblyProcess assemblyProcess)
        {
            _context.ApsAssemblyProcesses.Add(assemblyProcess);
        }

        public void UpdateAssemblyProcess(ApsAssemblyProcess assemblyProcess)
        {
            _context.ApsAssemblyProcesses.Attach(assemblyProcess);
        }

        public void DeleteAssemblyProcess(ApsAssemblyProcess assemblyProcess)
        {
            _context.ApsAssemblyProcesses.Remove(assemblyProcess);
        }

        public async Task<bool> AssemblyProcessExistsAsync(string assemblyProcessId)
        {
            if (string.IsNullOrEmpty(assemblyProcessId))
            {
                throw new ArgumentNullException();
            }

            return await _context.ApsAssemblyProcesses.AnyAsync(p =>
                string.Equals(p.PartId, assemblyProcessId, StringComparison.CurrentCulture));
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}