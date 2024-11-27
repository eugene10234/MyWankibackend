using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Database;

namespace prjWankibackend.Controllers.shop
{
    [Route("api/[controller]")]
    [ApiController]
    public class TProductsController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;

        public TProductsController(WealthierAndKinderContext context)
        {
            _context = context;
        }

        // GET: api/TProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TProduct>>> GetTProducts()
        {
            return await _context.TProducts.ToListAsync();
        }

        // GET: api/TProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TProduct>> GetTProduct(int id)
        {
            var tProduct = await _context.TProducts.FindAsync(id);

            if (tProduct == null)
            {
                return NotFound();
            }

            return tProduct;
        }

        // POST: api/TProducts
        [HttpPost]
        public async Task<ActionResult<TProduct>> PostTProduct(TProduct tProduct)
        {
            _context.TProducts.Add(tProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTProduct), new { id = tProduct.FProductId }, tProduct);
        }

        // PUT: api/TProducts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTProduct(int id, TProduct tProduct)
        {
            if (id != tProduct.FProductId)
            {
                return BadRequest();
            }

            _context.Entry(tProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TProductExists(id))
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

        // DELETE: api/TProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTProduct(int id)
        {
            var tProduct = await _context.TProducts.FindAsync(id);
            if (tProduct == null)
            {
                return NotFound();
            }

            _context.TProducts.Remove(tProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TProductExists(int id)
        {
            return _context.TProducts.Any(e => e.FProductId == id);
        }
    }
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TProductController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;

        public TProductController(WealthierAndKinderContext context)
        {
            _context = context;
        }

        // GET: api/TProduct/Search
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<TProduct>>> SearchProducts(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest("Search term cannot be empty.");
            }

            var products = await _context.TProducts
                .Where(p => p.FDescription.Contains(searchTerm) || p.FProductName.Contains(searchTerm))
                .ToListAsync();

            if (products == null || !products.Any())
            {
                return NotFound();
            }

            return products;
        }
    }
}
