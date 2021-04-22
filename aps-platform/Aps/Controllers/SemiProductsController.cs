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
        /// <summary>
        /// 查询所有半成品
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SemiProductDto>>> GetApsSemiProducts()
        {
            var semiProducts = await _repository.GetAllListAsync();
            var returnDto = _mapper.Map<List<ApsSemiProduct>, IEnumerable<SemiProductDto>>(semiProducts);
            return Ok(returnDto);
        }

        // GET: api/ApsSemiProducts/5
        /// <summary>
        /// 通过ID查询半成品
        /// </summary>
        /// <param name="id">半成品ID</param>
        /// <reponse code="200">查询成功</reponse>
        /// <reponse code="404">查询失败，半成品不存在</reponse>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<SemiProductDto>> GetApsSemiProduct(string id)
        {
            var apsSemiProduct = await _context.ApsSemiProducts.FindAsync(id);

            if (apsSemiProduct == null)
            {
                return NotFound();
            }

            return _mapper.Map<ApsSemiProduct, SemiProductDto>(apsSemiProduct);
        }

        /// <summary>
        /// 修改半成品的基本属性
        /// </summary>
        /// <param name="id">半成品ID</param>
        /// <param name="model">更新后的半成品</param>
        /// <response code="204">更新成功</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PutApsSemiProduct(string id, SemiProductAddOrUpdateDto model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var semiProduct = _mapper.Map<SemiProductAddOrUpdateDto, ApsSemiProduct>(model);


            try
            {
                await _repository.UpdateAsync(semiProduct);
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

        /// <summary>
        /// 添加半成品
        /// </summary>
        /// <param name="model">所添加的半成品</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<SemiProductDto>> CreateSemiProduct(SemiProductAddOrUpdateDto model)
        {
            var semiProduct = _mapper.Map<SemiProductAddOrUpdateDto, ApsSemiProduct>(model);

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


        /// <summary>
        /// 删除半成品
        /// </summary>
        /// <param name="id">删除半成品的ID</param>
        /// <response code="204">删除成功</response>
        /// <response code="404">未能找到所删除的半成品</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
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

        /// <summary>
        /// 查询半成品的所有加工工序
        /// </summary>
        /// <param name="semiProductId"></param>
        /// <returns></returns>
        [HttpGet("{semiProductId}/process/")]
        public async Task<ActionResult<IEnumerable<ManufactureProcessDto>>> GetProcessesFromSemiProduct(
            string semiProductId)
        {
            var semiProduct = await _repository.FirstOrDefaultAsync(x => x.Id == semiProductId);
            List<ApsManufactureProcess> manufactureProcesses = semiProduct.ApsManufactureProcesses;

            var returnDto =
                _mapper.Map<List<ApsManufactureProcess>, IEnumerable<ManufactureProcessDto>>(manufactureProcesses);

            return Ok(returnDto);
        }


        /// <summary>
        /// 查询半成品的加工工序
        /// </summary>
        /// <param name="semiProductId"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        [HttpGet("{semiProductId}/process/{processId}", Name = nameof(GetProcessFromSemiProduct))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ManufactureProcessDto>> GetProcessFromSemiProduct(string semiProductId,
            string processId)
        {
            var process = await _manufactureRepository.FirstOrDefaultAsync(x => x.Id == processId);

            if (process == null)
            {
                return NotFound();
            }

            var returnDto = _mapper.Map<ApsManufactureProcess, ManufactureProcessDto>(process);
            return Ok(returnDto);
        }


        /// <summary>
        /// 为半成品添加加工工序
        /// </summary>
        /// <param name="semiProductId">半成品ID</param>
        /// <param name="model">添加的工序</param>
        /// <returns></returns>
        [HttpPost("{semiProductId}/process/")]
        [ProducesResponseType(typeof(ManufactureProcessDto), 201)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ManufactureProcessDto>> AddProcessForSemiProduct(string semiProductId,
            [FromBody] ManufactureProcessAddDto model)
        {
            var manufactureProcess = _mapper.Map<ManufactureProcessAddDto, ApsManufactureProcess>(model);
            var semiProduct = await _repository.FirstOrDefaultAsync(x => x.Id == semiProductId);
            

            var processInserted = await _manufactureRepository.UpdateAsync(manufactureProcess);

            semiProduct.ApsManufactureProcesses.Add(processInserted);
            await _repository.UpdateAsync(semiProduct);

            var returnDto = _mapper.Map<ApsManufactureProcess, ManufactureProcessDto>(processInserted);

            return CreatedAtAction(nameof(GetProcessFromSemiProduct),
                new {semiProductId, processId = returnDto.Id}, returnDto);
        }

        /// <summary>
        /// 为半成品删除删除加工工序
        /// </summary>
        /// <param name="semiProductId">半成品ID</param>
        /// <param name="processId">工序ID</param>
        /// <returns></returns>
        [HttpDelete("{semiProductId}/process/{processId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteProcessForSemiProduct(string semiProductId,
            string processId)
        {
            var process = await _manufactureRepository.FirstOrDefaultAsync(x => x.Id == processId);

            if (process == null)
            {
                return NotFound();
            }

            await _manufactureRepository.DeleteAsync(process);

            return NoContent();
        }

        private bool ApsSemiProductExists(string id)
        {
            return _context.ApsSemiProducts.Any(e => e.Id == id);
        }
    }
}