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


        [HttpPut("{id}", Name = nameof(UpdateProduct))]
        public async Task<IActionResult> UpdateProduct(string id, ProductUpdateDto model)
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


        [HttpPost]
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

        // DELETE: api/ApsProducts/5
        [HttpDelete("{id}")]
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

        [HttpGet("{productId}/SemiProductRequisite/{semiProductId}", Name = nameof(GetSemiProductRequisiteFromProduct))]
        public async Task<ActionResult<ProductSemiProductDto>> GetSemiProductRequisiteFromProduct(
            string productId, string semiProductId)
        {
            ApsProduct product = await _repository.FirstOrDefaultAsync(x => x.Id == productId);
            List<ApsProductSemiProduct> semiProductsRequisites = product.AssembleBySemiProducts;

            var productSemiProduct = semiProductsRequisites.FirstOrDefault(x => x.ApsSemiProductId == semiProductId);
            var returnDto = _mapper.Map<ApsProductSemiProduct, ProductSemiProductDto>(productSemiProduct);

            return Ok(returnDto);
        }

        [HttpPost("{productId}/SemiProductRequisite/")]
        public async Task<ActionResult<ProductSemiProductDto>> AddSemiProductRequisiteForProduct(
            string productId, ProductSemiProductAddDto model)
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