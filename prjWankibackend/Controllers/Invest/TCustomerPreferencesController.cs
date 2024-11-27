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
    public class TCustomerPreferencesController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;

        public TCustomerPreferencesController(WealthierAndKinderContext context)
        {
            _context = context;
        }

        // GET: api/TCustomerPreferences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TCustomerPreference>>> GetTCustomerPreferences()
        {
            return await _context.TCustomerPreferences.ToListAsync();
        }

        // GET: api/TCustomerPreferences/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TCustomerPreference>> GetTCustomerPreference(int id)
        {
            var tCustomerPreference = await _context.TCustomerPreferences.FindAsync(id);

            if (tCustomerPreference == null)
            {
                return NotFound();
            }

            return tCustomerPreference;
        }

        // PUT: api/TCustomerPreferences/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTCustomerPreference(int id, TCustomerPreference tCustomerPreference)
        {
            if (id != tCustomerPreference.FPreferId)
            {
                return BadRequest();
            }

            _context.Entry(tCustomerPreference).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TCustomerPreferenceExists(id))
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

        // POST: api/TCustomerPreferences
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TCustomerPreference>> PostTCustomerPreference(TCustomerPreference tCustomerPreference)
        {
            _context.TCustomerPreferences.Add(tCustomerPreference);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTCustomerPreference", new { id = tCustomerPreference.FPreferId }, tCustomerPreference);
        }

        // DELETE: api/TCustomerPreferences/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTCustomerPreference(int id)
        {
            var tCustomerPreference = await _context.TCustomerPreferences.FindAsync(id);
            if (tCustomerPreference == null)
            {
                return NotFound();
            }

            _context.TCustomerPreferences.Remove(tCustomerPreference);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TCustomerPreferenceExists(int id)
        {
            return _context.TCustomerPreferences.Any(e => e.FPreferId == id);
        }
    }
}
