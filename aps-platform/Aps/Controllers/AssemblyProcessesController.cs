using Aps.Infrastructure;
using Aps.Services;
using Aps.Shared.Entity;
using Aps.Shared.Model;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aps.Infrastructure.Repositories;

namespace Aps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssemblyProcessesController : ControllerBase
    {
        private readonly ApsContext _context;
        private readonly IAssemblyProcessRepository _assemblyProcessRepository;
        private readonly IRepository<ApsProcessResource, string> _resourceRepository;
        private readonly IMapper _mapper;

        public AssemblyProcessesController(
            ApsContext context,
            IAssemblyProcessRepository assemblyProcessRepository,
            IRepository<ApsProcessResource, string> resourceRepository,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _assemblyProcessRepository = assemblyProcessRepository ??
                                         throw new ArgumentNullException(nameof(assemblyProcessRepository));
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// 查询装配工序
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(IEnumerable<AssemblyProcessDto>), 200)]
        [ProducesResponseType(500)]
        [HttpGet(Name = nameof(GetApsAssemblyProcesses))]
        public async Task<ActionResult<IEnumerable<AssemblyProcessDto>>> GetApsAssemblyProcesses()
        {
            var assemblyProcesses = await _assemblyProcessRepository.GetApsAssemblyProcessesAsync();
            var assemblyProcessDtos =
                _mapper.Map<IEnumerable<ApsAssemblyProcess>, IEnumerable<AssemblyProcessDto>>(assemblyProcesses);
            return Ok(assemblyProcessDtos);
        }

        /// <summary>
        /// 查询装配工序通过ID
        /// </summary>
        /// <param name="id">装配工序ID</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(AssemblyProcessDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet("{id}", Name = nameof(GetApsAssemblyProcess))]
        public async Task<ActionResult<AssemblyProcessDto>> GetApsAssemblyProcess([FromRoute] string id)
        {
            var apsAssemblyProcess = await _assemblyProcessRepository.GetApsAssemblyProcessAsync(id);

            if (apsAssemblyProcess == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ApsAssemblyProcess, AssemblyProcessDto>(apsAssemblyProcess));
        }

        

        /// <summary>
        /// 查询加工工序的资源需求
        /// </summary>
        /// <param name="processId">工序ID</param>
        /// <returns></returns>
        [HttpGet("{processId}/resource/")]
        public async Task<IEnumerable<ProcessResourceDto>> GetResourcesFromProcess(string processId)
        {
            var process = await _assemblyProcessRepository.GetApsAssemblyProcessAsync(processId);

            var resources = process.ApsResources;
            var returnDto = _mapper.Map<List<ApsProcessResource>, IEnumerable<ProcessResourceDto>>(resources);

            return returnDto;
        }


        /// <summary>
        /// 查询工序的资源需求
        /// </summary>
        /// <param name="processId">工序ID</param>
        /// <param name="resourceId">资源类别ID</param>
        /// <returns></returns>
        [HttpGet("{processId}/resource/{resourceId}", Name = nameof(GetResourceFromProcess))]
        public async Task<ActionResult<ProcessResourceDto>> GetResourceFromProcess(string processId, int resourceId)
        {
            var processResource = await _resourceRepository.FirstOrDefaultAsync(x =>
                x.ProcessId == processId && x.ResourceClassId == resourceId);

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
        /// <param name="processId">工序ID</param>
        /// <param name="model">添加的资源需求</param>
        /// <returns></returns>
        [HttpPost("{processId}/resource/")]
        public async Task<ActionResult<ProcessResourceDto>> AddResourceResourceRequisiteForProcess(string processId,
            [FromBody] ProcessResourceAddOrUpdateDto model)
        {
            var processResource = _mapper.Map<ProcessResourceAddOrUpdateDto, ApsProcessResource>(model);
            if (processResource.ProcessId != processId)
            {
                return BadRequest();
            }

            var resourceInserted = await _resourceRepository.InsertAsync(processResource);

            var returnDto = _mapper.Map<ApsProcessResource, ProcessResourceDto>(resourceInserted);
            return CreatedAtRoute(nameof(GetResourceFromProcess),
                new {processId = processId, resourceId = returnDto.ResourceClassId}, returnDto);
        }

        /// <summary>
        /// 修改工序的某个资源需求
        /// </summary>
        /// <param name="processId">工序ID</param>
        /// <param name="resourceId">资源类别ID</param>
        /// <param name="model">所更新的资源需求</param>
        /// <returns></returns>
        [HttpPut("{processId}/resource/{resourceId}")]
        public async Task<IActionResult> UpdateResourceRequisiteForProcess(string processId, int resourceId,
            [FromBody] ProcessResourceAddOrUpdateDto model)
        {
            var processResource = _mapper.Map<ProcessResourceAddOrUpdateDto, ApsProcessResource>(model);
            if (processResource.ResourceClassId != resourceId || processResource.ProcessId != processId)
            {
                return BadRequest();
            }

            await _resourceRepository.UpdateAsync(processResource);

            return NoContent();
        }

        /// <summary>
        /// 删除工序的资源需求
        /// </summary>
        /// <param name="processId">工序ID</param>
        /// <param name="resourceId">资源类别ID</param>
        /// <returns></returns>
        [HttpDelete("{processId}/resource/{resourceId}")]
        public async Task<IActionResult> DeleteResourceRequisiteForProcess(string processId, int resourceId)
        {
            var processResource = await _resourceRepository.FirstOrDefaultAsync(x =>
                x.ProcessId == processId && x.ResourceClassId == resourceId);

            if (processResource == null)
            {
                return NotFound();
            }

            await _resourceRepository.DeleteAsync(processResource);
            return NoContent();
        }

        private bool ApsAssemblyProcessExists(string id)
        {
            return _context.ApsAssemblyProcesses.Any(e => e.Id == id);
        }
    }
}