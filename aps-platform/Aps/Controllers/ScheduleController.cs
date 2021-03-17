using System;
using Aps.Infrastructure;
using Aps.Services;
using Aps.Shared.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aps.Infrastructure.Repositories;
using Aps.Shared.Model;
using AutoMapper;
using Microsoft.Extensions.Hosting;

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
            // _schedulehostedService = hostedService as ScheduleHostService ?? throw new ArgumentNullException(nameof(hostedService));
            _context = context;
            _scheduleTool = scheduleTool;
            _mapper = mapper;
            _scheduleRecordRepository = scheduleRecordRepository;
        }

        /// <summary>
        /// 查询所有排程
        /// </summary>
        [HttpPost]
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
            _scheduleTool.SetProductPrerequisite(orders);
            _scheduleTool.GenerateProcess();
            _scheduleTool.GenerateJobsFromOrders();
            _scheduleTool.SetBatchJob();
            await _scheduleTool.AssignResource();
            _scheduleTool.SetPreJobConstraint();
            _scheduleTool.SetObjective();
            var scheduleRecord = await _scheduleTool.Solve();

            var returnDto = _mapper.Map<ScheduleRecord, ScheduleRecordDto>(scheduleRecord);

            return Ok(returnDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleRecordDto>>> GetScheduleRecords()
        {
            var scheduleRecords = await _scheduleRecordRepository.GetAllListAsync();

            var returnDto = _mapper.Map<List<ScheduleRecord>, IEnumerable<ScheduleRecordDto>>(scheduleRecords);

            return Ok(returnDto);
        }

        [HttpGet("{recordId}")]
        public async Task<ActionResult<ScheduleRecordDto>> GetScheduleRecord(Guid recordId)
        {
            var scheduleRecord = await _scheduleRecordRepository.FirstOrDefaultAsync(x => x.Id == recordId);

            var returnDto = _mapper.Map<ScheduleRecord, ScheduleRecordDto>(scheduleRecord);
            return returnDto;
        }

        
    }
}