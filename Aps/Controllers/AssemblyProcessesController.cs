using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aps.Entity;
using Aps.Infrastructure;

namespace Aps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApsAssemblyProcessesController : ControllerBase
    {
        private readonly ApsContext _context;

        public ApsAssemblyProcessesController(ApsContext context)
        {
            _context = context;
        }

        // GET: api/ApsAssemblyProcesses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApsAssemblyProcess>>> GetApsAssemblyProcesses()
        {
            return await _context.ApsAssemblyProcesses.ToListAsync();
        }

        // GET: api/ApsAssemblyProcesses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApsAssemblyProcess>> GetApsAssemblyProcess(string id)
        {
            var apsAssemblyProcess = await _context.ApsAssemblyProcesses.FindAsync(id);

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
        public async Task<ActionResult<ApsAssemblyProcess>> PostApsAssemblyProcess(ApsAssemblyProcess apsAssemblyProcess)
        {
            _context.ApsAssemblyProcesses.Add(apsAssemblyProcess);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApsAssemblyProcessExists(apsAssemblyProcess.PartId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApsAssemblyProcess", new { id = apsAssemblyProcess.PartId }, apsAssemblyProcess);
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
