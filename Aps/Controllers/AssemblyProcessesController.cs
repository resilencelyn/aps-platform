﻿using Aps.Infrastructure;
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


        [HttpPost(Name = nameof(PostApsAssemblyProcess))]
        public async Task<ActionResult<AssemblyProcessDto>> PostApsAssemblyProcess(
            [FromBody] AssemblyProcessAddDto model)
        {
            var assemblyProcess = _mapper.Map<AssemblyProcessAddDto, ApsAssemblyProcess>(model);

            try
            {
                await _assemblyProcessRepository.AddAssemblyProcess(assemblyProcess);
            }
            catch (DbUpdateException)
            {
                if (ApsAssemblyProcessExists(model.Id))
                {
                    return Conflict();
                }

                throw;
            }

            var returnDto = _mapper.Map<ApsAssemblyProcess, AssemblyProcessDto>(assemblyProcess);
            return CreatedAtRoute(nameof(GetApsAssemblyProcess), new {id = model.Id}, returnDto);
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

            return NoContent();
        }

        private bool ApsAssemblyProcessExists(string id)
        {
            return _context.ApsAssemblyProcesses.Any(e => e.Id == id);
        }
    }
}