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
    public class TTranRecordsController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;

        public TTranRecordsController(WealthierAndKinderContext context)
        {
            _context = context;
        }

        // GET: api/TTranRecords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TTranRecord>>> GetTTranRecords()
        {
            return await _context.TTranRecords.ToListAsync();
        }

        // GET: api/TTranRecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TTranRecord>> GetTTranRecord(int id)
        {
            var tTranRecord = await _context.TTranRecords.FindAsync(id);

            if (tTranRecord == null)
            {
                return NotFound();
            }

            return tTranRecord;
        }

        // GET: api/TTranRecords/member/{fmemberId}
        [HttpGet("member/{fmemberId}")]
        public async Task<ActionResult<IEnumerable<TTranRecord>>> GetTTranRecordsByMemberId(string fmemberId)
        {
            var transactions = await _context.TTranRecords
                                      .Where(t => t.FMemberId == fmemberId)
                                      .ToListAsync();

            if (transactions == null || !transactions.Any())
            {
                return NotFound();
            }

            return transactions;
        }


        // PUT: api/TTranRecords/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTTranRecord(int id, TTranRecord tTranRecord)
        {
            if (id != tTranRecord.FTranId)
            {
                return BadRequest();
            }

            _context.Entry(tTranRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TTranRecordExists(id))
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

        [HttpPost]
        public async Task<ActionResult<TTranRecord>> PostTTranRecord([FromBody] TTranRecord tTranRecord)
        {
            _context.TTranRecords.Add(tTranRecord); // 新增資料，不需要設置 fTranId

            try
            {
                await _context.SaveChangesAsync(); // 資料庫自動生成 fTranId
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

            // 返回新增的完整記錄
            return CreatedAtAction(nameof(GetTTranRecord), new { id = tTranRecord.FTranId }, tTranRecord);
        }


        // DELETE: api/TTranRecords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTTranRecord(int id)
        {
            var tTranRecord = await _context.TTranRecords.FindAsync(id);
            if (tTranRecord == null)
            {
                return NotFound();
            }

            _context.TTranRecords.Remove(tTranRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TTranRecordExists(int id)
        {
            return _context.TTranRecords.Any(e => e.FTranId == id);
        }
    }
}
