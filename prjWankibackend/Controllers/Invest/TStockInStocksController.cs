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
    public class TStockInStocksController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;

        public TStockInStocksController(WealthierAndKinderContext context)
        {
            _context = context;
        }

        // GET: api/TStockInStocks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TStockInStock>>> GetTStockInStocks([FromQuery] string fmemberId)
        {
            if (string.IsNullOrEmpty(fmemberId))
            {
                return BadRequest("Member ID is required");
            }

            return await _context.TStockInStocks
                                 .Where(stock => stock.FMemberId == fmemberId)
                                 .ToListAsync();
        }

        // GET: api/TStockInStocks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TStockInStock>> GetTStockInStock(int id)
        {
            var tStockInStock = await _context.TStockInStocks.FindAsync(id);

            if (tStockInStock == null)
            {
                return NotFound();
            }

            return tStockInStock;
        }

        // PUT: api/TStockInStocks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTStockInStock(int id, TStockInStock tStockInStock)
        {
            if (id != tStockInStock.FInStockId)
            {
                return BadRequest();
            }

            _context.Entry(tStockInStock).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TStockInStockExists(id))
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

        // POST: api/TStockInStocks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TStockInStock>> PostTStockInStock(TStockInStock tStockInStock)
        {
            if (string.IsNullOrEmpty(tStockInStock.FMemberId))
            {
                return BadRequest("Member ID is required");
            }

            _context.TStockInStocks.Add(tStockInStock);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTStockInStock", new { id = tStockInStock.FInStockId }, tStockInStock);
        }

        // DELETE: api/TStockInStocks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTStockInStock(int id)
        {
            var tStockInStock = await _context.TStockInStocks.FindAsync(id);
            if (tStockInStock == null)
            {
                return NotFound();
            }

            _context.TStockInStocks.Remove(tStockInStock);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TStockInStockExists(int id)
        {
            return _context.TStockInStocks.Any(e => e.FInStockId == id);
        }
    }
}
