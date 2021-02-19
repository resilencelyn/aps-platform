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
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Aps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApsContext _context;
        private readonly IRepository<ApsProduct, string> _repository;
        private readonly IMapper _mapper;
        private readonly IRepository<ApsProductSemiProduct, string> _productSemiProductRepository;

        public ProductsController(ApsContext context, IRepository<ApsProduct, string> repository, IMapper mapper,
            IRepository<ApsProductSemiProduct, string> productSemiProductRepository)
        {
            _context = context;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _productSemiProductRepository = productSemiProductRepository ??
                                            throw new ArgumentNullException(nameof(productSemiProductRepository));
        }

        /// <summary>
        /// 查询所以商品
        /// </summary>
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(500)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProduct()
        {
            return Ok(_mapper.Map<List<ApsProduct>, IEnumerable<ProductDto>>(await _repository.GetAllListAsync()));
        }

        /// <summary>
        /// 通过ID查询商品
        /// </summary>
        /// <param name="id">商品ID</param>
        /// <reponse code="200">查询成功</reponse>
        /// <reponse code="404">查询失败，商品不存在</reponse>
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
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

        /// <summary>
        /// 修改商品的基本属性
        /// </summary>
        /// <param name="id" 例如="product_5">商品ID</param>
        /// <param name="model">更新后的商品</param>
        /// <response code="204">更新成功</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpPut("{id}", Name = nameof(UpdateProduct))]
        public async Task<IActionResult> UpdateProduct([BindRequired] string id, ProductUpdateDto model)
        {
            ApsProduct product = _mapper.Map<ProductUpdateDto, ApsProduct>(model);

            if (id != product.Id)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateAsync(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApsProductExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="model">所添加的商品</param>
        [HttpPost(Name = nameof(CreateProduct))]
        public async Task<ActionResult<ProductDto>> CreateProduct(ProductAddDto model)
        {
            var apsProduct = _mapper.Map<ProductAddDto, ApsProduct>(model);
            ApsProduct productInserted;
            try
            {
                productInserted = await _repository.InsertAsync(apsProduct);
            }
            catch (DbUpdateException)
            {
                if (ApsProductExists(apsProduct.Id))
                {
                    return Conflict();
                }

                throw;
            }

            return CreatedAtAction(nameof(GetProduct), new {id = productInserted.Id},
                _mapper.Map<ApsProduct, ProductDto>(productInserted));
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <remarks>删除商品的同时也会删除商品的装配工序</remarks>
        /// <param name="id">删除商品的ID</param>
        /// <response code="204">删除成功</response>
        /// <response code="404">未能找到所删除的商品</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}", Name = nameof(DeleteProduct))]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var apsProduct = await _context.ApsProducts.FindAsync(id);
            if (apsProduct == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(apsProduct);

            return NoContent();
        }

        /// <summary>
        /// 查询商品所需总半成品
        /// </summary>
        /// <param name="productId">所查询的商品ID</param>
        [ProducesResponseType(typeof(IEnumerable<ProductSemiProductDto>), 200)]
        [ProducesResponseType(500)]
        [HttpGet("{productId}/SemiProductRequisite/")]
        public async Task<ActionResult<IEnumerable<ProductSemiProductDto>>> GetSemiProductRequisiteFromProduct(
            string productId)
        {
            ApsProduct product = await _repository.FirstOrDefaultAsync(x => x.Id == productId);
            List<ApsProductSemiProduct> semiProductsRequisite = product.AssembleBySemiProducts;

            var returnDto =
                _mapper.Map<List<ApsProductSemiProduct>, IEnumerable<ProductSemiProductDto>>(semiProductsRequisite);

            return Ok(returnDto);
        }


        /// <summary>
        /// 查询商品所需半成品
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="semiProductId">半成品ID</param>
        [ProducesResponseType(typeof(ProductSemiProductDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet("{productId}/SemiProductRequisite/{semiProductId}",
            Name = nameof(GetSemiProductRequisiteFromProduct))]
        public async Task<ActionResult<ProductSemiProductDto>> GetSemiProductRequisiteFromProduct(
            string productId, string semiProductId)
        {
            ApsProduct product = await _repository.FirstOrDefaultAsync(x => x.Id == productId);
            List<ApsProductSemiProduct> semiProductsRequisites = product.AssembleBySemiProducts;

            var productSemiProduct = semiProductsRequisites.FirstOrDefault(x => x.ApsSemiProductId == semiProductId);
            var returnDto = _mapper.Map<ApsProductSemiProduct, ProductSemiProductDto>(productSemiProduct);

            return Ok(returnDto);
        }

        /// <summary>
        /// 添加商品所需半成品
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="model">半成品</param>
        [ProducesResponseType(typeof(ProductSemiProductDto), 201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [HttpPost("{productId}/SemiProductRequisite/")]
        public async Task<ActionResult<ProductSemiProductDto>> AddSemiProductRequisiteForProduct(
            [FromRoute, BindRequired] string productId, [FromBody, BindRequired] ProductSemiProductAddDto model)
        {
            var productSemiProduct = _mapper.Map<ProductSemiProductAddDto, ApsProductSemiProduct>(model);

            var product = await _repository.FirstOrDefaultAsync(x => x.Id == productId);
            if (product == null)
            {
                return NotFound(nameof(product));
            }

            if (product.AssembleBySemiProducts.Any(x => x.ApsSemiProductId == productSemiProduct.ApsSemiProductId))
            {
                return Conflict();
            }

            productSemiProduct.ApsProductId = productId;
            var inserted = await _productSemiProductRepository.InsertAsync(productSemiProduct);

            var returnDto = _mapper.Map<ApsProductSemiProduct, ProductSemiProductDto>(inserted);
            return CreatedAtRoute(nameof(GetSemiProductRequisiteFromProduct),
                new {productId = product.Id, semiProductId = inserted.ApsSemiProductId}, returnDto);
        }

        /// <summary>
        /// 更新商品所需半成品
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="semiProductId">半成品ID</param>
        /// <param name="model">更新的半成品</param>
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPut("{productId}/SemiProductRequisite/{semiProductId}")]
        public async Task<IActionResult> UpdateSemiProductRequisiteForProduct(
            string productId, string semiProductId, ProductSemiProductUpdateDto model)
        {
            var productSemiProduct = _mapper.Map<ProductSemiProductUpdateDto, ApsProductSemiProduct>(model);

            if (model.ApsSemiProductId != semiProductId)
            {
                return BadRequest();
            }

            productSemiProduct.ApsProductId = productId;

            await _productSemiProductRepository.UpdateAsync(productSemiProduct);

            return NoContent();
        }

        /// <summary>
        /// 删除商品所需半成品
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="semiProductId"></param>
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [HttpDelete("{productId}/SemiProductRequisite/{semiProductId}")]
        public async Task<IActionResult> DeleteSemiProductRequisiteForProduct(string productId, string semiProductId)
        {
            var semiProductRequisite = await _productSemiProductRepository.FirstOrDefaultAsync(x =>
                x.ApsProductId == productId && x.ApsSemiProductId == semiProductId);
            if (semiProductRequisite == null)
            {
                return NotFound();
            }

            await _productSemiProductRepository.DeleteAsync(semiProductRequisite);

            return NoContent();
        }

        private bool ApsProductExists(string id)
        {
            return _context.ApsProducts.Any(e => e.Id == id);
        }
    }
}