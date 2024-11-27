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
    public class TCustomerInvestAccountsController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;

        public TCustomerInvestAccountsController(WealthierAndKinderContext context)
        {
            _context = context;
        }

        // GET: api/TCustomerInvestAccounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TCustomerInvestAccount>>> GetTCustomerInvestAccounts()
        {
            return await _context.TCustomerInvestAccounts.ToListAsync();
        }

        // GET: api/TCustomerInvestAccounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TCustomerInvestAccount>> GetTCustomerInvestAccount(int id)
        {
            var tCustomerInvestAccount = await _context.TCustomerInvestAccounts.FindAsync(id);

            if (tCustomerInvestAccount == null)
            {
                return NotFound();
            }

            return tCustomerInvestAccount;
        }

        // PUT: api/TCustomerInvestAccounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTCustomerInvestAccount(int id, TCustomerInvestAccount tCustomerInvestAccount)
        {
            if (id != tCustomerInvestAccount.FInvestAccountId)
            {
                return BadRequest();
            }

            _context.Entry(tCustomerInvestAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TCustomerInvestAccountExists(id))
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

        // POST: api/TCustomerInvestAccounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TCustomerInvestAccount>> PostTCustomerInvestAccount(TCustomerInvestAccount tCustomerInvestAccount)
        {
            _context.TCustomerInvestAccounts.Add(tCustomerInvestAccount);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TCustomerInvestAccountExists(tCustomerInvestAccount.FInvestAccountId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTCustomerInvestAccount", new { id = tCustomerInvestAccount.FInvestAccountId }, tCustomerInvestAccount);
        }

        // DELETE: api/TCustomerInvestAccounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTCustomerInvestAccount(int id)
        {
            var tCustomerInvestAccount = await _context.TCustomerInvestAccounts.FindAsync(id);
            if (tCustomerInvestAccount == null)
            {
                return NotFound();
            }

            _context.TCustomerInvestAccounts.Remove(tCustomerInvestAccount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TCustomerInvestAccountExists(int id)
        {
            return _context.TCustomerInvestAccounts.Any(e => e.FInvestAccountId == id);
        }
    }
}
