using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Database;

namespace prjWankibackend.Controllers.Invest
{
    [Route("api/[controller]")]
    [ApiController]
    public class TStocksController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;

        public TStocksController(WealthierAndKinderContext context)
        {
            _context = context;
        }

        // GET: api/TStocks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TStock>>> GetTStocks()
        {
            return await _context.TStocks.ToListAsync();
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchStocks(string query)
        {

            var results = await _context.TStocks
            .Where(stock => stock.FStockId.Contains(query) || stock.FStockName.Contains(query))
            .Select(stock => new { stock.FStockId, stock.FStockName })
            .ToListAsync();
            return Ok(results);
        }



        // GET: api/TStocks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TStock>> GetTStock(string id)
        {
            var tStock = await _context.TStocks.FindAsync(id);

            if (tStock == null)
            {
                return NotFound();
            }

            return tStock;
        }

        // PUT: api/TStocks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTStock(string id, TStock tStock)
        {
            if (id != tStock.FStockId)
            {
                return BadRequest();
            }

            _context.Entry(tStock).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TStockExists(id))
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

        // POST: api/TStocks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TStock>> PostTStock(TStock tStock)
        {
            _context.TStocks.Add(tStock);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TStockExists(tStock.FStockId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTStock", new { id = tStock.FStockId }, tStock);
        }

        // DELETE: api/TStocks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTStock(string id)
        {
            var tStock = await _context.TStocks.FindAsync(id);
            if (tStock == null)
            {
                return NotFound();
            }

            _context.TStocks.Remove(tStock);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TStockExists(string id)
        {
            return _context.TStocks.Any(e => e.FStockId == id);
        }


    }
}
