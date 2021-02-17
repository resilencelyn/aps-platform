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

namespace Aps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApsAssemblyProcessesController : ControllerBase
    {
        private readonly ApsContext _context;
        private readonly IAssemblyProcessRepository _assemblyProcessRepository;
        private readonly IMapper _mapper;

        public ApsAssemblyProcessesController(
            ApsContext context,
            IAssemblyProcessRepository assemblyProcessRepository,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _assemblyProcessRepository = assemblyProcessRepository ??
                                         throw new ArgumentNullException(nameof(assemblyProcessRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/ApsAssemblyProcesses
        [HttpGet(Name = nameof(GetApsAssemblyProcesses))]
        public async Task<ActionResult<IEnumerable<AssemblyProcessDto>>> GetApsAssemblyProcesses()
        {
            var assemblyProcesses = await _assemblyProcessRepository.GetApsAssemblyProcessesAsync();
            var assemblyProcessDtos =
                _mapper.Map<IEnumerable<ApsAssemblyProcess>, IEnumerable<AssemblyProcessDto>>(assemblyProcesses);
            return Ok(assemblyProcessDtos);
        }

        // GET: api/ApsAssemblyProcesses/5
        [HttpGet("{id}", Name = nameof(GetApsAssemblyProcess))]
        public async Task<ActionResult<ApsAssemblyProcess>> GetApsAssemblyProcess([FromRoute] string id)
        {
            var apsAssemblyProcess = await _assemblyProcessRepository.GetApsAssemblyProcessAsync(id);

            if (apsAssemblyProcess == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ApsAssemblyProcess, AssemblyProcessDto>(apsAssemblyProcess));
        }

        // PUT: api/ApsAssemblyProcesses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}", Name = nameof(PutApsAssemblyProcess))]
        public async Task<IActionResult> PutApsAssemblyProcess([FromRoute] string id,
            [FromBody] ApsAssemblyProcess apsAssemblyProcess)
        {
            if (id != apsAssemblyProcess.Id)
            {
                return BadRequest();
            }

            _assemblyProcessRepository.UpdateAssemblyProcess(apsAssemblyProcess);

            try
            {
                await _assemblyProcessRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApsAssemblyProcessExists(id))
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

        // POST: api/ApsAssemblyProcesses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(Name = nameof(PostApsAssemblyProcess))]
        public async Task<ActionResult<ApsAssemblyProcess>> PostApsAssemblyProcess(
            ApsAssemblyProcess assemblyProcess)
        {
            _assemblyProcessRepository.AddAssemblyProcess(assemblyProcess);
            try
            {
                await _assemblyProcessRepository.SaveAsync();
            }
            catch (DbUpdateException)
            {
                if (ApsAssemblyProcessExists(assemblyProcess.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetApsAssemblyProcess), new { id = assemblyProcess.Id }, assemblyProcess);
        }

        // DELETE: api/ApsAssemblyProcesses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApsAssemblyProcess(string id)
        {
            var assemblyProcess = await _assemblyProcessRepository.GetApsAssemblyProcessAsync(id);
            if (assemblyProcess == null)
            {
                return NotFound();
            }

            _context.ApsAssemblyProcesses.Remove(assemblyProcess);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApsAssemblyProcessExists(string id)
        {
            return _context.ApsAssemblyProcesses.Any(e => e.Id == id);
        }
    }
}