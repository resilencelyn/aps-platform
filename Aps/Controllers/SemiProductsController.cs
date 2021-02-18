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
using Aps.Services;

namespace Aps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemiProductsController : ControllerBase
    {
        private readonly ApsContext _context;
        private readonly IRepository<ApsSemiProduct, string> _repository;
        private readonly IMapper _mapper;
        private readonly IRepository<ApsManufactureProcess, string> _manufactureRepository;

        public SemiProductsController(ApsContext context, IRepository<ApsSemiProduct, string> repository,
            IMapper mapper, IRepository<ApsManufactureProcess, string> manufactureRepository)
        {
            _context = context;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _manufactureRepository =
                manufactureRepository ?? throw new ArgumentNullException(nameof(manufactureRepository));
        }

        // GET: api/ApsSemiProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SemiProductDto>>> GetApsSemiProducts()
        {
            var semiProducts = await _repository.GetAllListAsync();
            var returnDto = _mapper.Map<List<ApsSemiProduct>, IEnumerable<SemiProductDto>>(semiProducts);
            return Ok(returnDto);
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


        [HttpPost]
        public async Task<ActionResult<SemiProductDto>> CreateSemiProduct(SemiProductAddDto model)
        {
            var semiProduct = _mapper.Map<SemiProductAddDto, ApsSemiProduct>(model);

            foreach (var manufactureProcess in semiProduct.ApsManufactureProcesses)
            {
                if (await _manufactureRepository.FirstOrDefaultAsync(x => x.Id == manufactureProcess.Id) != null)
                {
                    return BadRequest($"创建半成品时，其加工工序必须编写新的加工工序");
                }
            }

            try
            {
                await _repository.InsertAsync(semiProduct);
            }
            catch (DbUpdateException)
            {
                if (ApsSemiProductExists(semiProduct.Id))
                {
                    return Conflict();
                }

                throw;
            }

            var returnDto = _mapper.Map<ApsSemiProduct, SemiProductDto>(semiProduct);

            return CreatedAtAction("GetApsSemiProduct", new {id = returnDto.Id}, returnDto);
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