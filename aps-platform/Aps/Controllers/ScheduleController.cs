using System;
using Aps.Infrastructure;
using Aps.Services;
using Aps.Shared.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aps.Helper;
using Aps.Infrastructure.Repositories;
using Aps.Shared.Enum;
using Aps.Shared.Extensions;
using Aps.Shared.Model;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace Aps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ApsContext _context;
        private readonly IScheduleTool _scheduleTool;
        private readonly IMapper _mapper;
        private readonly IRepository<ScheduleRecord, Guid> _scheduleRecordRepository;

        public ScheduleController(ApsContext context, IScheduleTool scheduleTool, IMapper mapper,
            IRepository<ScheduleRecord, Guid> scheduleRecordRepository)
        {
            _context = context;
            _scheduleTool = scheduleTool ?? throw new ArgumentNullException(nameof(mapper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _scheduleRecordRepository = scheduleRecordRepository ??
                                        throw new ArgumentNullException(nameof(ScheduleRecordRepository));
        }

        /// <summary>
        /// 简单排程
        /// </summary>
        /// <remarks>简单排程，适用于初始情况资源都为可用的情况下</remarks>
        /// <response code="200">开始排程计算</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status200OK, "开始排程，并返回一个未完成的排程记录，可以使用记" +
                                                  "录ID稍后查询排程进度", typeof(ScheduleRecordDto))]
        public async Task<ActionResult<ScheduleRecordDto>> Schedule()
        {
            var orders = await _context.ApsOrders
                // .AsNoTracking()
                .Take(1)
                .Include(x => x.Product)
                .ThenInclude(x => x.ApsAssemblyProcess)
                .ThenInclude(x => x.ApsResources)
                .ThenInclude(x => x.ResourceClass)
                .Include(x => x.Product)
                .ThenInclude(x => x.AssembleBySemiProducts)
                .ThenInclude(x => x.ApsSemiProduct)
                .ThenInclude(x => x.ApsManufactureProcesses)
                .ThenInclude(x => x.ApsResources)
                .ThenInclude(x => x.ResourceClass)
                .Include(x => x.Product)
                .ThenInclude(x => x.AssembleBySemiProducts)
                .ThenInclude(x => x.ApsSemiProduct)
                .ThenInclude(x => x.ApsManufactureProcesses)
                .ThenInclude(x => x.PrevPart)
                // .AsSplitQuery()
                .ToListAsync();


            var resources = await _context.ApsResources
                .Include(x => x.ResourceAttributes)
                .ThenInclude(x => x.ResourceClass)
                .Where(x => x.Type != ResourceType.人员
                            && x.Workspace != Workspace.装配)
                .ToListAsync();

            var scheduleContext = new ScheduleContext
            {
                ScheduleStrategy = new NormalScheduleStrategy(_scheduleTool, orders, resources)
            };

            var scheduleRecord = await scheduleContext.ExecuteSchedule();

            var returnDto = _mapper.Map<ScheduleRecord, ScheduleRecordDto>(scheduleRecord);
            return Ok(returnDto);
        }

        /// <summary>
        /// 查询所有排程记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(StatusCodes.Status200OK, "查询成功", typeof(ScheduleRecordDto))]
        public async Task<ActionResult<IEnumerable<ScheduleRecordDto>>> GetScheduleRecords()
        {
            var scheduleRecords = await _scheduleRecordRepository.GetAllListAsync();

            var returnDto = _mapper.Map<List<ScheduleRecord>, IEnumerable<ScheduleRecordDto>>(scheduleRecords);
            return Ok(returnDto);
        }

        /// <summary>
        /// 查询排程记录
        /// </summary>
        /// <param name="recordId">排程记录ID</param>
        /// <returns></returns>
        [HttpGet("{recordId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ScheduleRecordDto>> GetScheduleRecord(Guid recordId)
        {
            var scheduleRecord = await _scheduleRecordRepository.FirstOrDefaultAsync(x => x.Id == recordId);

            if (scheduleRecord == null)
            {
                return NotFound();
            }

            var returnDto = _mapper.Map<ScheduleRecord, ScheduleRecordDto>(scheduleRecord);
            return returnDto;
        }

        /// <summary>
        /// 滚动排程
        /// </summary>
        /// <returns></returns>
        [HttpPost("{TailSchedule}")]
        public async Task<ActionResult<ScheduleRecordDto>> TailSchedule()
        {
            var orders = await _context.ApsOrders
                .AsNoTracking()
                .Take(1)
                .Include(x => x.Product)
                .ThenInclude(x => x.ApsAssemblyProcess)
                .ThenInclude(x => x.ApsResources)
                .ThenInclude(x => x.ResourceClass)
                .Include(x => x.Product)
                .ThenInclude(x => x.AssembleBySemiProducts)
                .ThenInclude(x => x.ApsSemiProduct)
                .ThenInclude(x => x.ApsManufactureProcesses)
                .ThenInclude(x => x.ApsResources)
                .ThenInclude(x => x.ResourceClass)
                .Include(x => x.Product)
                .ThenInclude(x => x.AssembleBySemiProducts)
                .ThenInclude(x => x.ApsSemiProduct)
                .ThenInclude(x => x.ApsManufactureProcesses)
                .ThenInclude(x => x.PrevPart)
                // .AsSplitQuery()
                .ToListAsync();

            var resources = await _context.ApsResources
                .Include(x => x.ResourceAttributes)
                .ThenInclude(x => x.ResourceClass)
                .Where(x => x.Type != ResourceType.人员
                            && x.Workspace != Workspace.装配)
                .ToListAsync();

            var scheduleContext = new ScheduleContext
            {
                ScheduleStrategy = new TailScheduleStrategy(_scheduleTool, orders, resources)
            };

            var scheduleRecord = await scheduleContext.ExecuteSchedule();
            var returnDto = _mapper.Map<ScheduleRecord, ScheduleRecordDto>(scheduleRecord);

            return Ok(returnDto);
        }


        /// <summary>
        /// 插单（未完成）
        /// </summary>
        /// <returns></returns>
        [HttpPost("{InsertSchedule}")]
        [ProducesResponseType(typeof(ScheduleRecordDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<ScheduleRecordDto>> InsertSchedule()
        {
            var orders = await _context.ApsOrders
                .AsNoTracking()
                .Take(1)
                .Include(x => x.Product)
                .ThenInclude(x => x.ApsAssemblyProcess)
                .ThenInclude(x => x.ApsResources)
                .ThenInclude(x => x.ResourceClass)
                .Include(x => x.Product)
                .ThenInclude(x => x.AssembleBySemiProducts)
                .ThenInclude(x => x.ApsSemiProduct)
                .ThenInclude(x => x.ApsManufactureProcesses)
                .ThenInclude(x => x.ApsResources)
                .ThenInclude(x => x.ResourceClass)
                .Include(x => x.Product)
                .ThenInclude(x => x.AssembleBySemiProducts)
                .ThenInclude(x => x.ApsSemiProduct)
                .ThenInclude(x => x.ApsManufactureProcesses)
                .ThenInclude(x => x.PrevPart)
                // .AsSplitQuery()
                .ToListAsync();

            var resources = await _context.ApsResources
                .Include(x => x.ResourceAttributes)
                .ThenInclude(x => x.ResourceClass)
                .Where(x => x.Type != ResourceType.人员
                            && x.Workspace != Workspace.装配)
                .ToListAsync();

            //TODO Complete Insert Controller

            var scheduleContext = new ScheduleContext
            {
                ScheduleStrategy = new InsertScheduleStrategy(_scheduleTool, orders, resources)
            };

            var scheduleRecord = await scheduleContext.ExecuteSchedule();

            ScheduleRecordDto scheduleRecordDto = _mapper.Map<ScheduleRecord, ScheduleRecordDto>(scheduleRecord);
            return Ok(scheduleRecordDto);
        }
    }
}