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
    public class ManufactureProcessesController : ControllerBase
    {
        private readonly ApsContext _context;
        private readonly IRepository<ApsManufactureProcess, string> _repository;
        private readonly IMapper _mapper;

        public ManufactureProcessesController(ApsContext context,
            IRepository<ApsManufactureProcess, string> repository,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/ManufactureProcesses
        /// <summary>
        /// 查询所有装配工艺
        /// </summary>
        [HttpGet(Name = nameof(GetManufactureProcesses))]
        public async Task<ActionResult<List<ManufactureProcessDto>>> GetManufactureProcesses()
        {
            return Ok(_mapper.Map<List<ApsManufactureProcess>, List<ManufactureProcessDto>>(
                await _repository.GetAllListAsync()));
        }

        // GET: api/ManufactureProcesses/5
        /// <summary>
        /// 通过ID查询装配工艺
        /// </summary>
        /// <param name="id">装配工艺ID</param>
        /// <reponse code="200">查询成功</reponse>
        /// <reponse code="404">查询失败，装配工艺不存在</reponse>
        [HttpGet("{id}", Name = nameof(GetManufactureProcess))]
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
        /// 修改装配工艺的基本属性
        /// </summary>
        /// <param name="id">装配工艺ID</param>
        /// <param name="model">更新后的装配工艺</param>
        /// <response code="204">更新成功</response>
        [HttpPut("{id}")]
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
        /// 添加装配工艺
        /// </summary>
        /// <param name="model">所添加的装配工艺</param>
        [HttpPost(Name = nameof(PostApsManufactureProcess))]
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
        /// 删除装配工艺
        /// </summary>
        /// <param name="id">删除装配工艺的ID</param>
        /// <response code="204">删除成功</response>
        /// <response code="404">未能找到所删除的装配工艺</response>
        [HttpDelete("{id}")]
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

        private bool ApsManufactureProcessExists(string id)
        {
            return _context.ApsManufactureProcesses.Any(e => e.Id == id);
        }
    }
}