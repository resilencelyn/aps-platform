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
    public class ApsResourcesController : ControllerBase
    {
        private readonly ApsContext _context;
        private readonly IRepository<ApsResource, string> _repository;
        private readonly IMapper _mapper;

        public ApsResourcesController(ApsContext context, IRepository<ApsResource, string> repository, IMapper mapper)
        {
            _context = context;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/ApsResources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceDto>>> GetResources()
        {
            return Ok(_mapper.Map<List<ApsResource>, IEnumerable<ResourceDto>>(await _repository.GetAllListAsync()));
        }

        // GET: api/ApsResources/5
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

        // PUT: api/ApsResources/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApsResource(string id, ApsResource apsResource)
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

        // POST: api/ApsResources
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApsResource>> PostApsResource(ApsResource apsResource)
        {
            _context.ApsResources.Add(apsResource);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApsResourceExists(apsResource.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApsResource", new { id = apsResource.Id }, apsResource);
        }

        // DELETE: api/ApsResources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApsResource(string id)
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