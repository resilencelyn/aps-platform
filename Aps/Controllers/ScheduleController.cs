using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aps.Infrastructure;
using Aps.Services;
using Aps.Shared.Entity;
using Microsoft.EntityFrameworkCore;

namespace Aps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ApsContext _context;
        private readonly IScheduleTool _scheduleTool;

        public ScheduleController(ApsContext context, IScheduleTool scheduleTool)
        {
            _context = context;
            _scheduleTool = scheduleTool;
        }

        [HttpGet]
        public async Task<ActionResult<List<ApsManufactureJob>>> Schedule()
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
                .ToListAsync();
            _scheduleTool.SetPrerequisite(orders);
            _scheduleTool.GenerateProcess();
            _scheduleTool.GenerateJobsFromOrders();
            _scheduleTool.SetBatchJob();
            await _scheduleTool.AssignResource();
            _scheduleTool.SetPreJobConstraint();
            _scheduleTool.SetObjective();
            var scheduleManufactureJobs = (await _scheduleTool.Solve()).Select(
                x => new
                {
                    x.Start,
                    x.End,
                }).ToList();

            return Ok(scheduleManufactureJobs);
        }
    }
}