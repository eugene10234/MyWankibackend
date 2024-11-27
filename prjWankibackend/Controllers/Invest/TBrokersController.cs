using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Database;

namespace prjWankibackend.Controllers.Invest
{
    [Route("api/[controller]")]
    [ApiController]
    public class TBrokersController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;

        public TBrokersController(WealthierAndKinderContext context)
        {
            _context = context;
        }

        // GET: api/TBrokers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TBroker>>> GetTBrokers()
        {
            var brokers = await _context.TBrokers.ToListAsync();
            return Ok(brokers);
        }

        // GET: api/TBrokers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TBroker>> GetTBroker(string id)
        {
            var tBroker = await _context.TBrokers.FindAsync(id);

            if (tBroker == null)
            {
                return NotFound();
            }

            return tBroker;
        }

        // PUT: api/TBrokers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTBroker(string id, TBroker tBroker)
        {
            if (id != tBroker.FBrokerId)
            {
                return BadRequest();
            }

            _context.Entry(tBroker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TBrokerExists(id))
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

        // POST: api/TBrokers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TBroker>> PostTBroker(TBroker tBroker)
        {
            _context.TBrokers.Add(tBroker);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TBrokerExists(tBroker.FBrokerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTBroker", new { id = tBroker.FBrokerId }, tBroker);
        }

        // DELETE: api/TBrokers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTBroker(string id)
        {
            var tBroker = await _context.TBrokers.FindAsync(id);
            if (tBroker == null)
            {
                return NotFound();
            }

            _context.TBrokers.Remove(tBroker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TBrokerExists(string id)
        {
            return _context.TBrokers.Any(e => e.FBrokerId == id);
        }
    }
}
