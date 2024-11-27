using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace prjWankibackend.Controllers.shop
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TOrderDetailController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;

        public TOrderDetailController(WealthierAndKinderContext context)
        {
            _context = context;
        }

        // POST: api/TOrderDetail
        [EnableCors("AllowAngular")]
        [HttpPost]
        public async Task<ActionResult<TOrderDetail>> CreateTOrderDetail(TOrderDetail tOrderDetail)
        {
            try
            {
                if (tOrderDetail == null)
                {
                    return BadRequest("Invalid order detail data");
                }

                // 驗證必要欄位
                if (tOrderDetail.FOrderId <= 0)
                {
                    return BadRequest("Invalid OrderId");
                }

                _context.TOrderDetails.Add(tOrderDetail);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetTOrderDetail),
                    new { id = tOrderDetail.FOrderDetailId },
                    tOrderDetail
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/TOrderDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TOrderDetail>>> GetTOrderDetails()
        {
            return await _context.TOrderDetails.ToListAsync();
        }

        // GET: api/TOrderDetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TOrderDetail>> GetTOrderDetail(int id)
        {
            var tOrderDetail = await _context.TOrderDetails.FindAsync(id);

            if (tOrderDetail == null)
            {
                return NotFound();
            }

            return tOrderDetail;
        }

        // PUT: api/TOrderDetail/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTOrderDetail(int id, TOrderDetail tOrderDetail)
        {
            if (id != tOrderDetail.FOrderDetailId)
            {
                return BadRequest();
            }

            _context.Entry(tOrderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TOrderDetailExists(id))
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

        // DELETE: api/TOrderDetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTOrderDetail(int id)
        {
            var tOrderDetail = await _context.TOrderDetails.FindAsync(id);
            if (tOrderDetail == null)
            {
                return NotFound();
            }

            _context.TOrderDetails.Remove(tOrderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TOrderDetailExists(int id)
        {
            return _context.TOrderDetails.Any(e => e.FOrderDetailId == id);
        }
    }
}
