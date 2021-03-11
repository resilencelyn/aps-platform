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
using Microsoft.AspNetCore.Http;

namespace Aps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufactureProcessesController : ControllerBase
    {
        private readonly ApsContext _context;
        private readonly IRepository<ApsManufactureProcess, string> _repository;
        private readonly IMapper _mapper;
        private readonly IRepository<ApsProcessResource, string> _resourceRepository;

        public ManufactureProcessesController(ApsContext context,
            IRepository<ApsManufactureProcess, string> repository,
            IMapper mapper,
            IRepository<ApsProcessResource, string> resourceRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
        }

        // GET: api/ManufactureProcesses
        /// <summary>
        /// 查询所有加工工艺
        /// </summary>
        [HttpGet(Name = nameof(GetManufactureProcesses))]
        public async Task<ActionResult<List<ManufactureProcessDto>>> GetManufactureProcesses()
        {
            return Ok(_mapper.Map<List<ApsManufactureProcess>, List<ManufactureProcessDto>>(
                await _repository.GetAllListAsync()));
        }

        // GET: api/ManufactureProcesses/5
        /// <summary>
        /// 通过ID查询加工工艺
        /// </summary>
        /// <param name="id">加工工艺ID</param>
        /// <reponse code="200">查询成功</reponse>
        /// <reponse code="404">查询失败，加工工艺不存在</reponse>
        [HttpGet("{id}", Name = nameof(GetManufactureProcess))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ManufactureProcessDto>> GetManufactureProcess(string id)
        {
            var apsManufactureProcess = await _repository.FirstOrDefaultAsync(x =>
                string.Equals(x.Id, id, StringComparison.InvariantCultureIgnoreCase));

            if (apsManufactureProcess == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ApsManufactureProcess, ManufactureProcessDto>(apsManufactureProcess));
        }

        /// <summary>
        /// 修改加工工艺的基本属性
        /// </summary>
        /// <param name="id">加工工艺ID</param>
        /// <param name="apsManufactureProcess">更新后的加工工艺</param>
        /// <response code="204">更新成功</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PutApsManufactureProcess(string id,
            ApsManufactureProcess apsManufactureProcess)
        {
            if (id != apsManufactureProcess.Id)
            {
                return BadRequest();
            }

            _context.Entry(apsManufactureProcess).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApsManufactureProcessExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// 添加加工工艺
        /// </summary>
        /// <param name="model">所添加的加工工艺</param>
        [HttpPost(Name = nameof(PostApsManufactureProcess))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ManufactureProcessDto>> PostApsManufactureProcess(
            ManufactureProcessAddDto model)
        {
            var apsManufactureProcess = _mapper.Map<ManufactureProcessAddDto, ApsManufactureProcess>(model);


            try
            {
                await _repository.InsertAsync(apsManufactureProcess);
            }
            catch (DbUpdateException)
            {
                if (ApsManufactureProcessExists(apsManufactureProcess.Id))
                {
                    return Conflict();
                }

                throw;
            }

            var returnDto = _mapper.Map<ApsManufactureProcess, ManufactureProcessDto>(apsManufactureProcess);

            return CreatedAtAction(nameof(GetManufactureProcess), new {id = returnDto.Id}, returnDto);
        }

        /// <summary>
        /// 删除加工工艺
        /// </summary>
        /// <param name="id">删除加工工艺的ID</param>
        /// <response code="204">删除成功</response>
        /// <response code="404">未能找到所删除的加工工艺</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteApsManufactureProcess(string id)
        {
            var apsManufactureProcess = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (apsManufactureProcess == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(apsManufactureProcess);
            return NoContent();
        }


        /// <summary>
        /// 批量删除工艺
        /// </summary>
        /// <param name="ids">所删除的多个商品ID</param>
        /// <returns></returns>
        [HttpPost("Delete/")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteManyManufactureProcesses([FromBody] IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                var process = await _repository.FirstOrDefaultAsync(x => x.Id == id);
                if (process == null)
                {
                    return NotFound();
                }

                await _repository.DeleteAsync(process);
            }

            return NoContent();
        }


        /// <summary>
        /// 查询加工工序的资源需求
        /// </summary>
        /// <param name="manufactureProcessId">工序ID</param>
        /// <returns></returns>
        [HttpGet("{manufactureProcessId}/resource/")]
        public async Task<IEnumerable<ProcessResourceDto>> GetResourcesFromManufactureProcess(
            string manufactureProcessId)
        {
            var process = await _repository.FirstOrDefaultAsync(x => x.Id == manufactureProcessId);

            var resources = process.ApsResources;
            var returnDto = _mapper.Map<List<ApsProcessResource>, IEnumerable<ProcessResourceDto>>(resources);

            return returnDto;
        }


        /// <summary>
        /// 查询加工工序的资源需求
        /// </summary>
        /// <param name="manufactureProcessId">工序ID</param>
        /// <param name="resourceId">资源类别ID</param>
        /// <returns></returns>
        [HttpGet("{manufactureProcessId}/resource/{resourceId}", Name = nameof(GetResourceFromManufactureProcess))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ProcessResourceDto>> GetResourceFromManufactureProcess(
            string manufactureProcessId, int resourceId)
        {
            var processResource = await _resourceRepository.FirstOrDefaultAsync(x =>
                x.ProcessId == manufactureProcessId && x.ResourceClassId == resourceId);

            if (processResource == null)
            {
                return NotFound();
            }

            var returnDto = _mapper.Map<ApsProcessResource, ProcessResourceDto>(processResource);
            return Ok(returnDto);
        }

        /// <summary>
        /// 添加工序的资源需求
        /// </summary>
        /// <param name="manufactureProcessId">工序ID</param>
        /// <param name="model">添加的资源需求</param>
        /// <returns></returns>
        [HttpPost("{manufactureProcessId}/resource/")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ProcessResourceDto>> AddResourceResourceRequisiteForManufactureProcess(
            string manufactureProcessId,
            [FromBody] ProcessResourceAddOrUpdateDto model)
        {
            var processResource = _mapper.Map<ProcessResourceAddOrUpdateDto, ApsProcessResource>(model);
            if (processResource.ProcessId != manufactureProcessId)
            {
                return BadRequest();
            }

            var resourceInserted = await _resourceRepository.InsertAsync(processResource);

            var returnDto = _mapper.Map<ApsProcessResource, ProcessResourceDto>(resourceInserted);
            return CreatedAtRoute(nameof(GetResourceFromManufactureProcess),
                new {processId = manufactureProcessId, resourceId = returnDto.ResourceClassId}, returnDto);
        }

        /// <summary>
        /// 修改工序的某个资源需求
        /// </summary>
        /// <param name="manufactureProcessId">工序ID</param>
        /// <param name="resourceId">资源类别ID</param>
        /// <param name="model">所更新的资源需求</param>
        /// <returns></returns>
        [HttpPut("{manufactureProcessId}/resource/{resourceId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateResourceRequisiteForManufactureProcess(string manufactureProcessId,
            int resourceId,
            [FromBody] ProcessResourceAddOrUpdateDto model)
        {
            var processResource = _mapper.Map<ProcessResourceAddOrUpdateDto, ApsProcessResource>(model);
            if (processResource.ResourceClassId != resourceId || processResource.ProcessId != manufactureProcessId)
            {
                return BadRequest();
            }

            await _resourceRepository.UpdateAsync(processResource);

            return NoContent();
        }

        /// <summary>
        /// 删除工序的资源需求
        /// </summary>
        /// <param name="manufactureProcessId">工序ID</param>
        /// <param name="resourceId">资源类别ID</param>
        /// <returns></returns>
        [HttpDelete("{manufactureProcessId}/resource/{resourceId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteResourceRequisiteForManufactureProcess(string manufactureProcessId,
            int resourceId)
        {
            var processResource = await _resourceRepository.FirstOrDefaultAsync(x =>
                x.ProcessId == manufactureProcessId && x.ResourceClassId == resourceId);

            if (processResource == null)
            {
                return NotFound();
            }

            await _resourceRepository.DeleteAsync(processResource);
            return NoContent();
        }

        private bool ApsManufactureProcessExists(string id)
        {
            return _context.ApsManufactureProcesses.Any(e => e.Id == id);
        }
    }
}