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
    public class ApsResourcesController : ControllerBase
    {
        private readonly ApsContext _context;

        public ApsResourcesController(ApsContext context)
        {
            _context = context;
        }

        // GET: api/ApsResources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApsResource>>> GetApsResources()
        {
            return await _context.ApsResources.ToListAsync();
        }

        // GET: api/ApsResources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApsResource>> GetApsResource(string id)
        {
            var apsResource = await _context.ApsResources.FindAsync(id);

            if (apsResource == null)
            {
                return NotFound();
            }

            return apsResource;
        }

        // PUT: api/ApsResources/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApsResource(string id, ApsResource apsResource)
        {
            if (id != apsResource.ResourceId)
            {
                return BadRequest();
            }

            _context.Entry(apsResource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApsResourceExists(id))
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

        // POST: api/ApsResources
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApsResource>> PostApsResource(ApsResource apsResource)
        {
            _context.ApsResources.Add(apsResource);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApsResourceExists(apsResource.ResourceId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApsResource", new { id = apsResource.ResourceId }, apsResource);
        }

        // DELETE: api/ApsResources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApsResource(string id)
        {
            var apsResource = await _context.ApsResources.FindAsync(id);
            if (apsResource == null)
            {
                return NotFound();
            }

            _context.ApsResources.Remove(apsResource);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApsResourceExists(string id)
        {
            return _context.ApsResources.Any(e => e.ResourceId == id);
        }
    }
}
