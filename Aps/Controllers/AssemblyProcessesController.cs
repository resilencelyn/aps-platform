using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aps.Infrastructure;
using Aps.Services;
using Aps.Shared.Entity;

namespace Aps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApsAssemblyProcessesController : ControllerBase
    {
        private readonly ApsContext _context;
        private readonly IAssemblyProcessRepository _assemblyProcessRepository;

        public ApsAssemblyProcessesController(ApsContext context, IAssemblyProcessRepository assemblyProcessRepository)
        {
            _context = context;
            _assemblyProcessRepository = assemblyProcessRepository;
        }

        // GET: api/ApsAssemblyProcesses
        [HttpGet(Name = nameof(GetApsAssemblyProcesses))]
        public async Task<ActionResult<IEnumerable<ApsAssemblyProcess>>> GetApsAssemblyProcesses()
        {
            var assemblyProcesses = await _assemblyProcessRepository.GetApsAssemblyProcessesAsync();
            return Ok(assemblyProcesses);
        }

        // GET: api/ApsAssemblyProcesses/5
        [HttpGet("{id}", Name = nameof(GetApsAssemblyProcess))]
        public async Task<ActionResult<ApsAssemblyProcess>> GetApsAssemblyProcess([FromRoute] string id)
        {
            var apsAssemblyProcess = await _assemblyProcessRepository.GetApsAssemblyProcessAsync(id);

            if (apsAssemblyProcess == null)
            {
                return NotFound();
            }

            return apsAssemblyProcess;
        }

        // PUT: api/ApsAssemblyProcesses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApsAssemblyProcess(string id, ApsAssemblyProcess apsAssemblyProcess)
        {
            if (id != apsAssemblyProcess.PartId)
            {
                return BadRequest();
            }

            _context.Entry(apsAssemblyProcess).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApsAssemblyProcessExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ApsAssemblyProcesses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApsAssemblyProcess>> PostApsAssemblyProcess(
            ApsAssemblyProcess AssemblyProcess)
        {
            _assemblyProcessRepository.AddAssemblyProcess(AssemblyProcess);
            _context.ApsAssemblyProcesses.Add(AssemblyProcess);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApsAssemblyProcessExists(AssemblyProcess.PartId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApsAssemblyProcess", new { id = AssemblyProcess.PartId }, AssemblyProcess);
        }

        // DELETE: api/ApsAssemblyProcesses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApsAssemblyProcess(string id)
        {
            var apsAssemblyProcess = await _context.ApsAssemblyProcesses.FindAsync(id);
            if (apsAssemblyProcess == null)
            {
                return NotFound();
            }

            _context.ApsAssemblyProcesses.Remove(apsAssemblyProcess);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApsAssemblyProcessExists(string id)
        {
            return _context.ApsAssemblyProcesses.Any(e => e.PartId == id);
        }
    }
}