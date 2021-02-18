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
    public class ApsOrdersController : ControllerBase
    {
        private readonly ApsContext _context;
        private readonly IRepository<ApsOrder, string> _repository;
        private readonly IMapper _mapper;
        private readonly IRepository<ApsProduct, string> _productRepository;

        public ApsOrdersController(ApsContext context, IRepository<ApsOrder, string> repository, IMapper mapper,
            IRepository<ApsProduct, string> productRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        // GET: api/ApsOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _repository.GetAllListAsync();
            return Ok(_mapper.Map<List<ApsOrder>, IEnumerable<OrderDto>>(orders));
        }

        // GET: api/ApsOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(string id)
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

                throw;
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<OrderDto>> PostOrder(OrderAddDto model)
        {
            ApsOrder orderInserted;
            var order = _mapper.Map<OrderAddDto, ApsOrder>(model);

            var product = order.Product;
            order.Product = null;

            var productFromRepository = await _productRepository.FirstOrDefaultAsync(x => x.Id == product.Id) ??
                                        await _productRepository.InsertAsync(product);
            try
            {
                orderInserted = await _repository.InsertAsync(order);
            }
            catch (DbUpdateException)
            {
                if (ApsOrderExists(model.Id))
                {
                    return Conflict();
                }

                throw;
            }

            orderInserted.Product = productFromRepository;
            await _repository.SaveAsync();

            var orderDto = _mapper.Map<ApsOrder, OrderDto>(orderInserted);

            return CreatedAtAction(nameof(GetOrder), new {id = orderDto.Id}, orderDto);
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