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
    public class ApsOrdersController : ControllerBase
    {
        private readonly ApsContext _context;

        public ApsOrdersController(ApsContext context)
        {
            _context = context;
        }

        // GET: api/ApsOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApsOrder>>> GetApsOrders()
        {
            return await _context.ApsOrders.ToListAsync();
        }

        // GET: api/ApsOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApsOrder>> GetApsOrder(string id)
        {
            var apsOrder = await _context.ApsOrders.FindAsync(id);

            if (apsOrder == null)
            {
                return NotFound();
            }

            return apsOrder;
        }

        // PUT: api/ApsOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApsOrder(string id, ApsOrder apsOrder)
        {
            if (id != apsOrder.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(apsOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApsOrderExists(id))
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

        // POST: api/ApsOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApsOrder>> PostApsOrder(ApsOrder apsOrder)
        {
            _context.ApsOrders.Add(apsOrder);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApsOrderExists(apsOrder.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApsOrder", new { id = apsOrder.OrderId }, apsOrder);
        }

        // DELETE: api/ApsOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApsOrder(string id)
        {
            var apsOrder = await _context.ApsOrders.FindAsync(id);
            if (apsOrder == null)
            {
                return NotFound();
            }

            _context.ApsOrders.Remove(apsOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApsOrderExists(string id)
        {
            return _context.ApsOrders.Any(e => e.OrderId == id);
        }
    }
}
