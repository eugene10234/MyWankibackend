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
    public class TOrdersController : Controller
    {
        private readonly WealthierAndKinderContext _context;

        public TOrdersController(WealthierAndKinderContext context)
        {
            _context = context;
        }

        // GET: TOrders
        public async Task<IActionResult> Index()
        {
            return View(await _context.TOrders.ToListAsync());
        }

        // GET: TOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tOrder = await _context.TOrders
                .FirstOrDefaultAsync(m => m.FOrderId == id);
            if (tOrder == null)
            {
                return NotFound();
            }

            return View(tOrder);
        }

        // GET: TOrders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FOrderId,FMemberId,FTotalHelpPoint,FStatus,FOrderTime,FExecStatus,FBeginTime,FFinishTime,FProof")] TOrder tOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tOrder);
        }

        // GET: TOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tOrder = await _context.TOrders.FindAsync(id);
            if (tOrder == null)
            {
                return NotFound();
            }
            return View(tOrder);
        }

        // POST: TOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FOrderId,FMemberId,FTotalHelpPoint,FStatus,FOrderTime,FExecStatus,FBeginTime,FFinishTime,FProof")] TOrder tOrder)
        {
            if (id != tOrder.FOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TOrderExists(tOrder.FOrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tOrder);
        }

        // GET: TOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tOrder = await _context.TOrders
                .FirstOrDefaultAsync(m => m.FOrderId == id);
            if (tOrder == null)
            {
                return NotFound();
            }

            return View(tOrder);
        }

        // POST: TOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tOrder = await _context.TOrders.FindAsync(id);
            if (tOrder != null)
            {
                _context.TOrders.Remove(tOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TOrderExists(int id)
        {
            return _context.TOrders.Any(e => e.FOrderId == id);
        }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class TOrderController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;

        public TOrderController(WealthierAndKinderContext context)
        {
            _context = context;
        }

        // GET: api/TOrder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TOrder>>> GetTOrders()
        {
            return await _context.TOrders.ToListAsync();
        }

        // GET: api/TOrder/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TOrder>> GetTOrder(int id)
        {
            var tOrder = await _context.TOrders.FindAsync(id);

            if (tOrder == null)
            {
                return NotFound();
            }

            return tOrder;
        }
        //透過fPersonSId查詢所有資料的Get
        [HttpGet("byPersonSid/{personSid}")]
        public async Task<ActionResult<IEnumerable<TOrder>>> GetTOrdersByPersonSid(string personSid)
        {
            var tOrders = await _context.TOrders
                .Where(o => o.FPersonSid == personSid)
                .ToListAsync();

            if (tOrders == null || !tOrders.Any())
            {
                return NotFound();
            }

            return tOrders;
        }
        // POST: api/TOrder
        [HttpPost]
        public async Task<ActionResult<TOrder>> PostTOrder(TOrder tOrder)
        {
            _context.TOrders.Add(tOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTOrder), new { id = tOrder.FOrderId }, tOrder);
        }

        // PUT: api/TOrder/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTOrder(int id, TOrder tOrder)
        {
            if (id != tOrder.FOrderId)
            {
                return BadRequest();
            }

            _context.Entry(tOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TOrderExists(id))
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

        // DELETE: api/TOrder/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTOrder(int id)
        {
            var tOrder = await _context.TOrders.FindAsync(id);
            if (tOrder == null)
            {
                return NotFound();
            }

            _context.TOrders.Remove(tOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TOrderExists(int id)
        {
            return _context.TOrders.Any(e => e.FOrderId == id);
        }
    }
}
