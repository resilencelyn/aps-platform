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
    public class ApsProductsController : ControllerBase
    {
        private readonly ApsContext _context;

        public ApsProductsController(ApsContext context)
        {
            _context = context;
        }

        // GET: api/ApsProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApsProduct>>> GetApsProduct()
        {
            return await _context.ApsProducts.ToListAsync();
        }

        // GET: api/ApsProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApsProduct>> GetApsProduct(string id)
        {
            var apsProduct = await _context.ApsProducts.FindAsync(id);

            if (apsProduct == null)
            {
                return NotFound();
            }

            return apsProduct;
        }

        // PUT: api/ApsProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApsProduct(string id, ApsProduct apsProduct)
        {
            if (id != apsProduct.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(apsProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApsProductExists(id))
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

        // POST: api/ApsProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApsProduct>> PostApsProduct(ApsProduct apsProduct)
        {
            _context.ApsProducts.Add(apsProduct);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApsProductExists(apsProduct.ProductId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApsProduct", new { id = apsProduct.ProductId }, apsProduct);
        }

        // DELETE: api/ApsProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApsProduct(string id)
        {
            var apsProduct = await _context.ApsProducts.FindAsync(id);
            if (apsProduct == null)
            {
                return NotFound();
            }

            _context.ApsProducts.Remove(apsProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApsProductExists(string id)
        {
            return _context.ApsProducts.Any(e => e.ProductId == id);
        }
    }
}
