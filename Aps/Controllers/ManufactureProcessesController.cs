using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aps.Infrastructure;
using Aps.Shared.Entity;

namespace Aps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufactureProcessesController : ControllerBase
    {
        private readonly ApsContext _context;

        public ManufactureProcessesController(ApsContext context)
        {
            _context = context;
        }

        // GET: api/ManufactureProcesses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApsManufactureProcess>>> GetApsManufactureProcesses()
        {
            return await _context.ApsManufactureProcesses.ToListAsync();
        }

        // GET: api/ManufactureProcesses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApsManufactureProcess>> GetApsManufactureProcess(string id)
        {
            var apsManufactureProcess = await _context.ApsManufactureProcesses.FindAsync(id);

            if (apsManufactureProcess == null)
            {
                return NotFound();
            }

            return apsManufactureProcess;
        }

        // PUT: api/ManufactureProcesses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApsManufactureProcess(string id, ApsManufactureProcess apsManufactureProcess)
        {
            if (id != apsManufactureProcess.PartId)
            {
                return BadRequest();
            }

            _context.Entry(apsManufactureProcess).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApsManufactureProcessExists(id))
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

        // POST: api/ManufactureProcesses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApsManufactureProcess>> PostApsManufactureProcess(ApsManufactureProcess apsManufactureProcess)
        {
            _context.ApsManufactureProcesses.Add(apsManufactureProcess);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApsManufactureProcessExists(apsManufactureProcess.PartId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApsManufactureProcess", new { id = apsManufactureProcess.PartId }, apsManufactureProcess);
        }

        // DELETE: api/ManufactureProcesses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApsManufactureProcess(string id)
        {
            var apsManufactureProcess = await _context.ApsManufactureProcesses.FindAsync(id);
            if (apsManufactureProcess == null)
            {
                return NotFound();
            }

            _context.ApsManufactureProcesses.Remove(apsManufactureProcess);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApsManufactureProcessExists(string id)
        {
            return _context.ApsManufactureProcesses.Any(e => e.PartId == id);
        }
    }
}
