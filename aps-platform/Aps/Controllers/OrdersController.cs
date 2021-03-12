using Aps.Infrastructure;
using Aps.Infrastructure.Repositories;
using Aps.Shared.Entity;
using Aps.Shared.Model;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
    public class OrdersController : ControllerBase
    {
        private readonly ApsContext _context;
        private readonly IRepository<ApsOrder, string> _repository;
        private readonly IMapper _mapper;
        private readonly IRepository<ApsProduct, string> _productRepository;

        public OrdersController(ApsContext context, IRepository<ApsOrder, string> repository, IMapper mapper,
            IRepository<ApsProduct, string> productRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        // GET: api/ApsOrders
        /// <summary>
        /// 查询所有订单
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _repository.GetAllListAsync();
            return Ok(_mapper.Map<List<ApsOrder>, IEnumerable<OrderDto>>(orders));
        }

        // GET: api/ApsOrders/5
        /// <summary>
        /// 通过ID查询订单
        /// </summary>
        /// <param name="id">订单ID</param>
        /// <reponse code="200">查询成功</reponse>
        /// <reponse code="404">查询失败，订单不存在</reponse>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<OrderDto>> GetOrder(string id)
        {
            var apsOrder = await _repository.FirstOrDefaultAsync(x => x.Id == id);

            if (apsOrder == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ApsOrder, OrderDto>(apsOrder));
        }

        /// <summary>
        /// 修改订单的基本属性
        /// </summary>
        /// <param name="id" 例如="product_5">订单ID</param>
        /// <param name="model">更新后的订单</param>
        /// <response code="204">更新成功</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateOrder(string id, OrderUpdateDto model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var order = _mapper.Map<OrderUpdateDto, ApsOrder>(model);

            try
            {
                ApsProduct product = order.Product;
                var productFromDb = await _productRepository.FirstOrDefaultAsync(x => x.Id == product.Id);
                if (productFromDb == null)
                {
                    return NotFound(nameof(productFromDb));
                }

                order.Product = productFromDb;
                await _repository.UpdateAsync(order);
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

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="model">所添加的订单</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<OrderDto>> CreateOrder(OrderAddDto model)
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

            return CreatedAtAction(nameof(GetOrder), new { id = orderDto.Id }, orderDto);
        }

        // DELETE: api/ApsOrders/5
        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="id">删除订单的ID</param>
        /// <response code="204">删除成功</response>
        /// <response code="404">未能找到所删除的订单</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
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


        /// <summary>
        /// 批量删除订单
        /// </summary>
        /// <param name="ids">删除订单的ID</param>
        /// <response code="204">删除成功</response>
        /// <response code="404">未能找到所删除的订单</response>
        [HttpPost("delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteManyOrder([FromBody] IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                var apsOrder = await _context.ApsOrders.FindAsync(id);
                if (apsOrder == null)
                {
                    return NotFound();
                }

                await _repository.DeleteAsync(apsOrder);
            }

            return NoContent();
        }

        private bool ApsOrderExists(string id)
        {
            return _context.ApsOrders.Any(e => e.Id == id);
        }
    }
}