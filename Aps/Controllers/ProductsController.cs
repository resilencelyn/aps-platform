using Aps.Infrastructure;
using Aps.Infrastructure.Repositories;
using Aps.Shared.Entity;
using Aps.Shared.Model;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApsProductsController : ControllerBase
    {
        private readonly ApsContext _context;
        private readonly IRepository<ApsProduct, string> _repository;
        private readonly IMapper _mapper;

        public ApsProductsController(ApsContext context, IRepository<ApsProduct, string> repository, IMapper mapper)
        {
            _context = context;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/ApsProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProduct()
        {
            return Ok(_mapper.Map<List<ApsProduct>, IEnumerable<ProductDto>>(await _repository.GetAllListAsync()));
        }

        // GET: api/ApsProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(string id)
        {
            var apsProduct = await _repository.FirstOrDefaultAsync(x =>
                string.Equals(x.Id, id, StringComparison.InvariantCultureIgnoreCase));

            if (apsProduct == null)
            {
                return NotFound();
            }

            return _mapper.Map<ApsProduct, ProductDto>(apsProduct);
        }


        [HttpPut("{id}", Name = nameof(PutProduct))]
        public async Task<IActionResult> PutProduct(string id, ApsProduct apsProduct)
        {
            if (id != apsProduct.Id)
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


        [HttpPost]
        public async Task<ActionResult<ApsProduct>> PostApsProduct(ProductAddDto product)
        {
            var apsProduct = _mapper.Map<ProductAddDto, ApsProduct>(product);
            ApsProduct productInserted;
            try
            {
                productInserted = await _repository.InsertAsync(apsProduct);
            }
            catch (DbUpdateException)
            {
                if (ApsProductExists(product.Id))
                {
                    return Conflict();
                }

                throw;
            }

            return CreatedAtAction(nameof(GetProduct), new {id = productInserted.Id},
                _mapper.Map<ApsProduct, ProductDto>(productInserted));
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

            await _repository.DeleteAsync(apsProduct);

            return NoContent();
        }

        private bool ApsProductExists(string id)
        {
            return _context.ApsProducts.Any(e => e.Id == id);
        }
    }
}