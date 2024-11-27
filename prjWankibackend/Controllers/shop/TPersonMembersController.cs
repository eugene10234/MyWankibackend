using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Database;

namespace prjWankibackend.Controllers.shop
{
    [Route("api/[controller]")]
    [ApiController]
    public class TPersonMembersController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;

        public TPersonMembersController(WealthierAndKinderContext context)
        {
            _context = context;
        }

        // GET: api/TPersonMembers1
        [EnableCors("AllowAngular")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TPersonMember>>> GetTPersonMembers()
        {
            return await _context.TPersonMembers.ToListAsync();
        }

        // GET: api/TPersonMembers1/5
        [EnableCors("AllowAngular")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TPersonMember>> GetTPersonMember(int id)
        {
            var tPersonMember = await _context.TPersonMembers.FindAsync(id);

            if (tPersonMember == null)
            {
                return NotFound();
            }

            return tPersonMember;
        }

        // PUT: api/TPersonMembers1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTPersonMember(int id, TPersonMember tPersonMember)
        {
            if (id != tPersonMember.FPersonSid)
            {
                return BadRequest();
            }

            _context.Entry(tPersonMember).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TPersonMemberExists(id))
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

        // POST: api/TPersonMembers1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TPersonMember>> PostTPersonMember(TPersonMember tPersonMember)
        {
            _context.TPersonMembers.Add(tPersonMember);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTPersonMember", new { id = tPersonMember.FPersonSid }, tPersonMember);
        }

        // DELETE: api/TPersonMembers1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTPersonMember(int id)
        {
            var tPersonMember = await _context.TPersonMembers.FindAsync(id);
            if (tPersonMember == null)
            {
                return NotFound();
            }

            _context.TPersonMembers.Remove(tPersonMember);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TPersonMemberExists(int id)
        {
            return _context.TPersonMembers.Any(e => e.FPersonSid == id);
        }

        // GET: api/TPersonMembers/points/{memberId}
        [EnableCors("AllowAngular")]
        [HttpGet("points/{memberId}")]
        public async Task<ActionResult<int>> GetHelpPointsByMemberId(string memberId)
        {
            var tPersonMember = await _context.TPersonMembers
                .Where(m => m.FMemberId == memberId)
                .Select(m => m.FTotalHelpPoint)
                .FirstOrDefaultAsync();

            if (tPersonMember == null)
            {
                return NotFound();
            }

            return tPersonMember ?? 0;
        }
    
    // 新增透過 FPersonSid 獲取 HelpPoint 的 GET 方法
    [EnableCors("AllowAngular")]
    [HttpGet("points/person/{personSid}")]
    public async Task<ActionResult<int>> GetHelpPointsByPersonSid(int personSid)
    {
        var tPersonMember = await _context.TPersonMembers
            .Where(m => m.FPersonSid == personSid)
            .Select(m => m.FTotalHelpPoint)
            .FirstOrDefaultAsync();

        if (tPersonMember == null)
        {
            return NotFound();
        }

        return tPersonMember ?? 0;
    }
}
}
