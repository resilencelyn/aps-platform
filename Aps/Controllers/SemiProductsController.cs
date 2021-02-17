using System;
using Aps.Infrastructure;
using Aps.Shared.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aps.Infrastructure.Repositories;
using Aps.Shared.Model;
using AutoMapper;

namespace Aps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApsSemiProductsController : ControllerBase
    {
        private readonly ApsContext _context;
        private readonly IRepository<ApsSemiProduct, string> _repository;
        private readonly IMapper _mapper;

        public ApsSemiProductsController(ApsContext context, IRepository<ApsSemiProduct, string> repository,
            IMapper mapper)
        {
            _context = context;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/ApsSemiProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SemiProductDto>>> GetApsSemiProducts()
        {
            return Ok(_mapper.Map<List<ApsSemiProduct>, IEnumerable<SemiProductDto>>(
                await _repository.GetAllListAsync()));
        }

        // GET: api/ApsSemiProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SemiProductDto>> GetApsSemiProduct(string id)
        {
            var apsSemiProduct = await _context.ApsSemiProducts.FindAsync(id);

            if (apsSemiProduct == null)
            {
                return NotFound();
            }

            return _mapper.Map<ApsSemiProduct, SemiProductDto>(apsSemiProduct);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutApsSemiProduct(string id, ApsSemiProduct apsSemiProduct)
        {
            if (id != apsSemiProduct.Id)
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

                throw;
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
                if (ApsSemiProductExists(apsSemiProduct.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApsSemiProduct", new {id = apsSemiProduct.Id}, apsSemiProduct);
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
            return _context.ApsSemiProducts.Any(e => e.Id == id);
        }
    }
}