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
    public class ApsSemiProductsController : ControllerBase
    {
        private readonly ApsContext _context;

        public ApsSemiProductsController(ApsContext context)
        {
            _context = context;
        }

        // GET: api/ApsSemiProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApsSemiProduct>>> GetApsSemiProducts()
        {
            return await _context.ApsSemiProducts.ToListAsync();
        }

        // GET: api/ApsSemiProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApsSemiProduct>> GetApsSemiProduct(string id)
        {
            var apsSemiProduct = await _context.ApsSemiProducts.FindAsync(id);

            if (apsSemiProduct == null)
            {
                return NotFound();
            }

            return apsSemiProduct;
        }

        // PUT: api/ApsSemiProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApsSemiProduct(string id, ApsSemiProduct apsSemiProduct)
        {
            if (id != apsSemiProduct.SemiProductId)
            {
                return BadRequest();
            }

            _context.Entry(apsSemiProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApsSemiProductExists(id))
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

        // POST: api/ApsSemiProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApsSemiProduct>> PostApsSemiProduct(ApsSemiProduct apsSemiProduct)
        {
            _context.ApsSemiProducts.Add(apsSemiProduct);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApsSemiProductExists(apsSemiProduct.SemiProductId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApsSemiProduct", new { id = apsSemiProduct.SemiProductId }, apsSemiProduct);
        }

        // DELETE: api/ApsSemiProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApsSemiProduct(string id)
        {
            var apsSemiProduct = await _context.ApsSemiProducts.FindAsync(id);
            if (apsSemiProduct == null)
            {
                return NotFound();
            }

            _context.ApsSemiProducts.Remove(apsSemiProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApsSemiProductExists(string id)
        {
            return _context.ApsSemiProducts.Any(e => e.SemiProductId == id);
        }
    }
}
