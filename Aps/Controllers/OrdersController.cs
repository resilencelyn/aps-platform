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
    public class ApsOrdersController : ControllerBase
    {
        private readonly ApsContext _context;
        private readonly IRepository<ApsOrder, string> _repository;
        private readonly IMapper _mapper;

        public ApsOrdersController(ApsContext context, IRepository<ApsOrder, string> repository, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/ApsOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetApsOrders()
        {
            var orders = await _repository.GetAllListAsync();
            return Ok(_mapper.Map<List<ApsOrder>, IEnumerable<OrderDto>>(orders));
        }

        // GET: api/ApsOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetApsOrder(string id)
        {
            var apsOrder = await _context.ApsOrders.FindAsync(id);

            if (apsOrder == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ApsOrder, OrderDto>(apsOrder));
        }

        // PUT: api/ApsOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApsOrder(string id, ApsOrder apsOrder)
        {
            if (id != apsOrder.Id)
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
            var orderInserted = await _repository.InsertAsync(apsOrder);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApsOrderExists(apsOrder.Id))
                {
                    return Conflict();
                }

                throw;
            }

            return CreatedAtAction(nameof(GetApsOrder), new {id = orderInserted.Id}, orderInserted);
        }

        // DELETE: api/ApsOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var apsOrder = await _context.ApsOrders.FindAsync(id);
            if (apsOrder == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(apsOrder);

            return NoContent();
        }

        private bool ApsOrderExists(string id)
        {
            return _context.ApsOrders.Any(e => e.Id == id);
        }
    }
}