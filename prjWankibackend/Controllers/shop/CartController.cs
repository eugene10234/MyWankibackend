using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjWankibackend.Controllers.shop
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;

        public CartController(WealthierAndKinderContext context)
        {
            _context = context;
        }

        // GET: api/Cart
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TOrderDetail>>> GetCartItems()
        {
            return await _context.TOrderDetails.ToListAsync();
        }

        // GET: api/Cart/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TOrderDetail>> GetCartItem(int id)
        {
            var orderDetail = await _context.TOrderDetails.FindAsync(id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return orderDetail;
        }

        // POST: api/Cart
        [HttpPost]
        public async Task<ActionResult<TOrderDetail>> PostCartItem(TOrderDetail orderDetail)
        {
            _context.TOrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCartItem), new { id = orderDetail.FOrderDetailId }, orderDetail);
        }

        // PUT: api/Cart/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartItem(int id, TOrderDetail orderDetail)
        {
            if (id != orderDetail.FOrderDetailId)
            {
                return BadRequest();
            }

            _context.Entry(orderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(id))
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

        // DELETE: api/Cart/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var orderDetail = await _context.TOrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            _context.TOrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderDetailExists(int id)
        {
            return _context.TOrderDetails.Any(e => e.FOrderDetailId == id);
        }
    }
}
