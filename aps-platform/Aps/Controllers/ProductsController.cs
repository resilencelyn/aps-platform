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
        private readonly IRepository<ApsProductSemiProduct, string> _semiProductRepository;
        private readonly IRepository<ApsAssemblyProcess, string> _assembleProcessRepository;
        private readonly IRepository<ApsAssemblyProcessSemiProduct, string> _processSemiProductRepository;
        private readonly IAssemblyProcessRepository _assemblyProcessRepository;

        public ProductsController(ApsContext context,
            IRepository<ApsProduct, string> repository,
            IMapper mapper,
            IRepository<ApsProductSemiProduct, string> semiProductRepository,
            IRepository<ApsAssemblyProcess, string> assembleProcessRepository,
            IRepository<ApsAssemblyProcessSemiProduct, string> processSemiProductRepository,
            IAssemblyProcessRepository assemblyProcessRepository)
        {
            _context = context;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _semiProductRepository = semiProductRepository ??
                                     throw new ArgumentNullException(nameof(semiProductRepository));
            _assembleProcessRepository = assembleProcessRepository ??
                                         throw new ArgumentNullException(nameof(assembleProcessRepository));
            _processSemiProductRepository = processSemiProductRepository;
            _assemblyProcessRepository = assemblyProcessRepository;
        }

        /// <summary>
        /// 查询所有商品
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
        [HttpGet("{productId}")]
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
        [HttpPut("{productId}", Name = nameof(UpdateProduct))]
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
        [ProducesResponseType(typeof(ProductDto), 201)]
        [ProducesResponseType(500)]
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
        [HttpDelete("{productId}", Name = nameof(DeleteProduct))]
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

            if (product.ApsAssemblyProcess == null)
            {
                return BadRequest();
            }

            if (product.AssembleBySemiProducts.Any(x => x.ApsSemiProductId == productSemiProduct.ApsSemiProductId))
            {
                return Conflict();
            }

            productSemiProduct.ApsProductId = productId;
            var inserted = await _semiProductRepository.InsertAsync(productSemiProduct);
            await _processSemiProductRepository.InsertAsync(new ApsAssemblyProcessSemiProduct
            {
                Amount = productSemiProduct.Amount,
                ApsAssemblyProcessId = product.ApsAssemblyProcessId,
                ApsSemiProductId = productSemiProduct.ApsSemiProductId
            });

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
            var product = await _repository.FirstOrDefaultAsync(x => x.Id == productId);

            if (product == null)
            {
                return NotFound();
            }

            if (product.ApsAssemblyProcess == null)
            {
                return null;
            }

            if (model.ApsSemiProductId != semiProductId)
            {
                return BadRequest();
            }

            productSemiProduct.ApsProductId = productId;

            await _semiProductRepository.UpdateAsync(productSemiProduct);
            await _processSemiProductRepository.UpdateAsync(new ApsAssemblyProcessSemiProduct
            {
                Amount = productSemiProduct.Amount,
                ApsAssemblyProcessId = product.ApsAssemblyProcessId,
                ApsSemiProductId = productSemiProduct.ApsSemiProductId
            });



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
            var product = await _repository.FirstOrDefaultAsync(x => x.Id == productId);

            var semiProductRequisite = await _semiProductRepository.FirstOrDefaultAsync(x =>
                x.ApsProductId == productId && x.ApsSemiProductId == semiProductId);

            var processSemiProductRequisite = await _processSemiProductRepository.FirstOrDefaultAsync(x =>
                x.ApsAssemblyProcessId == product.ApsAssemblyProcessId && x.ApsSemiProductId == semiProductId
            );
            if (semiProductRequisite == null)
            {
                return NotFound();
            }

            await _semiProductRepository.DeleteAsync(semiProductRequisite);
            await _processSemiProductRepository.DeleteAsync(processSemiProductRequisite);
            return NoContent();
        }

        /// <summary>
        /// 查询商品装配工序
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("{productId}/process", Name = nameof(GetProductAssemblyProcesses))]
        [ProducesResponseType(typeof(AssemblyProcessDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AssemblyProcessDto>> GetProductAssemblyProcesses(string productId)
        {
            var product = await _repository.FirstOrDefaultAsync(x => x.Id == productId);
            if (product == null)
            {
                return NotFound();
            }

            var productAssemblyProcess = product.ApsAssemblyProcess;

            return _mapper.Map<ApsAssemblyProcess, AssemblyProcessDto>(productAssemblyProcess);
        }

        /// <summary>
        /// 修改装配过程的基本属性
        /// </summary>
        /// <param name="productId">装配过程ID</param>
        /// <param name="apsAssemblyProcess">更新后的装配过程</param>
        /// <response code="204">更新成功</response>
        [HttpPut("{productId}/process/", Name = nameof(UpdateAssemblyProcess))]
        public async Task<IActionResult> UpdateAssemblyProcess([FromRoute] string productId,
            [FromBody] AssemblyProcessUpdateDto apsAssemblyProcess)
        {
            var product = await _repository.FirstOrDefaultAsync(x => x.Id == productId);
            if (product.ApsAssemblyProcessId != apsAssemblyProcess.Id)
            {
                return BadRequest();
            }

            var assemblyProcess = _mapper.Map<AssemblyProcessUpdateDto, ApsAssemblyProcess>(apsAssemblyProcess);

            _assemblyProcessRepository.UpdateAssemblyProcess(assemblyProcess);

            try
            {
                await _assemblyProcessRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApsAssemblyProcessExists(assemblyProcess.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// 添加装配过程
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="model">所添加的装配过程</param>
        /// <returns></returns>
        [HttpPost("{productId}/process/", Name = nameof(PostApsAssemblyProcess))]
        public async Task<ActionResult<AssemblyProcessDto>> PostApsAssemblyProcess([FromRoute] string productId,
            [FromBody] AssemblyProcessAddDto model)
        {
            var assemblyProcess = _mapper.Map<AssemblyProcessAddDto, ApsAssemblyProcess>(model);
            assemblyProcess.OutputFinishedProductId = productId;
            try
            {
                await _assemblyProcessRepository.AddAssemblyProcess(assemblyProcess);
                await _assemblyProcessRepository.SaveAsync();
            }
            catch (DbUpdateException)
            {
                if (ApsAssemblyProcessExists(model.Id))
                {
                    return Conflict();
                }

                throw;
            }

            var returnDto = _mapper.Map<ApsAssemblyProcess, AssemblyProcessDto>(assemblyProcess);
            return CreatedAtRoute(nameof(GetProductAssemblyProcesses),
                new {productId = returnDto.OutputFinishedProductId}, returnDto);
        }

        // DELETE: api/ApsAssemblyProcesses/5
        /// <summary>
        /// 删除装配过程
        /// </summary>
        /// <param name="productId">删除的装配过程ID</param>
        /// <response code="204">删除成功</response>
        /// <response code="404">未能找到所删除的商品装配工序</response>
        [HttpDelete("{productId}/process/")]
        public async Task<IActionResult> DeleteApsAssemblyProcess(string productId)
        {
            var product = await _repository.FirstOrDefaultAsync(x => x.Id == productId);
            if (product == null)
            {
                return NotFound();
            }

            _assemblyProcessRepository.DeleteAssemblyProcess(product.ApsAssemblyProcess);
            await _assemblyProcessRepository.SaveAsync();

            return NoContent();
        }

        private bool ApsProductExists(string id)
        {
            return _context.ApsProducts.Any(e => e.Id == id);
        }

        private bool ApsAssemblyProcessExists(string id)
        {
            return _context.ApsAssemblyProcesses.Any(e => e.Id == id);
        }
    }
}