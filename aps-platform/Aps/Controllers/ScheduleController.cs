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
        /// 查询所有排程
        /// </summary>
        /// <response code="200">查询成功</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

            return returnDto;
        }

        [HttpPost("{InsertSchedule}")]
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
            return Ok();
        }
    }
}