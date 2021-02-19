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
    public class ResourcesController : ControllerBase
    {
        private readonly ApsContext _context;
        private readonly IRepository<ApsResource, string> _repository;
        private readonly IMapper _mapper;

        public ResourcesController(ApsContext context, IRepository<ApsResource, string> repository, IMapper mapper)
        {
            _context = context;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// 查询所有资源
        /// </summary>
        [ProducesResponseType(typeof(IEnumerable<ResourceDto>), 200)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceDto>>> GetResources()
        {
            return Ok(_mapper.Map<List<ApsResource>, IEnumerable<ResourceDto>>(await _repository.GetAllListAsync()));
        }

        /// <summary>
        /// 查询资源通过ID
        /// </summary>
        /// <param name="id">资源ID</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResourceDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ResourceDto>> GetApsResource(string id)
        {
            var apsResource = await _context.ApsResources.FindAsync(id);

            if (apsResource == null)
            {
                return NotFound();
            }

            return _mapper.Map<ApsResource, ResourceDto>(apsResource);
        }

        /// <summary>
        /// 更新资源
        /// </summary>
        /// <param name="id">资源ID</param>
        /// <param name="apsResource">更新的资源</param>
        /// <returns></returns>
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResource(string id, ApsResource apsResource)
        {
            if (id != apsResource.Id)
            {
                return BadRequest();
            }

            _context.Entry(apsResource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApsResourceExists(id))
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

        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="model">添加的资源</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResourceDto), 201)]
        [ProducesResponseType(409)]
        [HttpPost]
        public async Task<ActionResult<ResourceDto>> CreateResource([BindRequired] ResourceAddDto model)
        {
            var resource = _mapper.Map<ResourceAddDto, ApsResource>(model);
            foreach (var resourceClassWithResource in resource.ResourceAttributes)
            {
                resourceClassWithResource.ApsResourceId = resource.Id;
            }

            try
            {
                await _repository.InsertAsync(resource);
            }
            catch (DbUpdateException)
            {
                if (ApsResourceExists(resource.Id))
                {
                    return Conflict();
                }

                throw;
            }

            var returnDto = _mapper.Map<ApsResource, ResourceDto>(resource);
            return CreatedAtAction("GetApsResource", new {id = returnDto.Id}, returnDto);
        }

        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="id">资源ID</param>
        /// <returns></returns>
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResource(string id)
        {
            var apsResource = await _context.ApsResources.FindAsync(id);
            if (apsResource == null)
            {
                return NotFound();
            }

            _context.ApsResources.Remove(apsResource);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApsResourceExists(string id)
        {
            return _context.ApsResources.Any(e => e.Id == id);
        }
    }
}