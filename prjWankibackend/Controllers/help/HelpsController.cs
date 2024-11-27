using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.DTO.help;
using prjWankibackend.Models.Database;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using MailKit.Security;
using System.Diagnostics;
using Microsoft.Extensions.FileSystemGlobbing;

namespace prjWankibackend.Controllers.help
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpsController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;
        private readonly SmtpSettings _smtpSettings;

        public HelpsController(WealthierAndKinderContext context, IOptions<SmtpSettings> smtpSettings)
        {
            _context = context;
            _smtpSettings = smtpSettings.Value;
        }
        //獲取最新一筆資料的方法
        [HttpGet("latest")]
        public async Task<ActionResult<HelpDTO>> GetLatestHelp()
        {
            var tHelp = await _context.THelps.OrderByDescending(h => h.FMfdDate).FirstOrDefaultAsync();
            if (tHelp == null)
            {
                return NotFound();
            }
            var helpDTO = new HelpDTO
            {
                Name = tHelp.FName,
                Phone = tHelp.FPhone,
                HelpContent = tHelp.FHelpDescribe,
                HelpClass = tHelp.FHelpClassId ?? 0, // 顯式轉換，處理可為 null 的值
                CreateTime = tHelp.FMfdDate ?? DateTime.MinValue, // 顯式轉換，處理可為 null 的值
                Points = tHelp.FPoint ?? 0, // 顯式轉換，處理可為 null 的值
                DistrictId = tHelp.FDistrictId ?? 0,
                Status = tHelp.FHelpStatus ?? 0,
            };
            return Ok(helpDTO);
        }


        // GET: api/Helps
        [HttpGet("AllHelp")]
        public async Task<ActionResult<IEnumerable<HelpDTO>>> GetTHelps()
        {
            var tHelps = await _context.THelps.ToListAsync();


            var helpDTOs = tHelps.Select(help => new HelpDTO
            {
                Name = help.FName,
                Email = help.FEmail,
                Phone = help.FPhone,
                HelpContent = help.FHelpDescribe,
                HelpClass = help.FHelpClassId ?? 0, // 顯式轉換，處理可為 null 的值
                CreateTime = help.FMfdDate ?? DateTime.MinValue, // 顯式轉換，處理可為 null 的值
                Points = help.FPoint ?? 0, // 顯式轉換，處理可為 null 的值
                DistrictId = help.FDistrictId ?? 0, // 顯式轉換，處理可為 null 的值
                Status = help.FHelpStatus.Value, // 顯式轉換，處理可為 null 的值
                HelpId = help.FHelpId
            }).ToList();

            return Ok(helpDTOs);
        }


        [HttpGet("GetHelpsByHelpClass/{HelpClassId}")]
        public async Task<ActionResult<IEnumerable<HelpDTO>>> GetHelpsByHelpClass(int HelpClassId)
        {
            
            var tHelps = await _context.THelps.Where(h => h.FHelpClassId == HelpClassId).ToListAsync();

            var helpDTOs = tHelps.Select(help => new HelpDTO
            {
                Name = help.FName,
                Email = help.FEmail,
                Phone = help.FPhone,
                HelpContent = help.FHelpDescribe,
                HelpClass = help.FHelpClassId ?? 0, // 顯式轉換，處理可為 null 的值
                CreateTime = help.FMfdDate ?? DateTime.MinValue, // 顯式轉換，處理可為 null 的值
                Points = help.FPoint ?? 0, // 顯式轉換，處理可為 null 的值
                DistrictId = help.FDistrictId ?? 0, // 顯式轉換，處理可為 null 的值
                Status = help.FHelpStatus.Value, // 顯式轉換，處理可為 null 的值
                HelpId = help.FHelpId
            }).ToList();

            return Ok(helpDTOs);
        }

        [HttpGet("GetMatchsByHelpClass/{HelpClassId}")]
        public async Task<ActionResult<IEnumerable<MatchDTO>>> GetMatchsByHelpClass(int HelpClassId)
        {
            var matchDTOs = await _context.TMatches
           .Join(
              _context.THelps.Where(h => h.FHelpClassId == HelpClassId),
              match => match.FHelpId,
              help => help.FHelpId,
              (match, help) => new MatchDTO
              {
                  HelperName = match.FHelperName ?? "未提供",
                  HelperEmail = match.FHelperEmail ?? "未提供",
                  Points = match.FPoint ?? 0,
                  HelpName = help.FName ?? "未提供",
                  DistrictId = help.FDistrictId ?? 0,
                  HelpContent = help.FHelpDescribe ?? "未提供",
                  HelpClass = help.FHelpClassId ?? 0,
                  HelpStatus = help.FHelpStatus ?? 0,
                  Latitude = help.FLatitude ?? 0,
                  Longitude = help.FLongitude ?? 0,
                  HelpPhone = help.FPhone ?? "未提供"
              })
          .ToListAsync();

            return Ok(matchDTOs);
        }


        // GET: api/Helps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<THelp>> GetTHelp(int id)
        {
            var tHelp = await _context.THelps.FindAsync(id);

            if (tHelp == null)
            {
                return NotFound();
            }

            return tHelp;
        }

        // PUT: api/Helps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //這個方法用於處理 PUT: api/Helps/{id} 請求，根據 id 更新對應的 THelp 資料。
        //如果 id 與 tHelp 中的 FHelpId 不相同，返回 BadRequest。
        //如果更新過程中有併發異常，會檢查資料是否存在，並根據結果返回 NotFound 或重新拋出異常。
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTHelp(int id, THelp tHelp)
        {
            if (id != tHelp.FHelpId)
            {
                return BadRequest();
            }

            _context.Entry(tHelp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!THelpExists(id))
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

        enum MyStatus
        {
            t = 1,
            t2 = 2
        }

        // POST: api/Helps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //這個方法用於處理 POST: api/Helps 請求，新增一筆新的 THelp 資料到資料庫。新增成功後，返回 CreatedAtAction，其中包含新資料的位址。
        [HttpPost]
        public async Task<ActionResult<THelp>> PostTHelp(HelpDTO helpDTO)
        {
            if (helpDTO == null)
            {
                return BadRequest("HelpDTO cannot be null");
            }
            THelp help = new THelp
            {
                FName = helpDTO.Name,
                FPhone = helpDTO.Phone,
                FHelpDescribe = helpDTO.HelpContent,
                FHelpClassId = helpDTO.HelpClass,
                FHelpStatus = (int)MyStatus.t,
                FMfdDate = helpDTO.CreateTime,
                FPoint = helpDTO.Points,
                FDistrictId = helpDTO.DistrictId,
                FEmail = helpDTO.Email,
                FLatitude = helpDTO.Latitude,
                FLongitude = helpDTO.Longitude


            };

            _context.THelps.Add(help);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTHelp", new { id = help.FHelpId }, helpDTO);
        }



        [HttpPut("updateHelpStatus/{helpId}")]
        public async Task<ActionResult<HelpDTO>> UpdateHelpStatus(int helpId)
        {
            try
            {
                var tHelp = await _context.THelps.FirstOrDefaultAsync(h => h.FHelpId == helpId);
                if (tHelp == null)
                {
                    return NotFound("找不到對應的協助資料");
                }

                switch (tHelp.FHelpStatus)
                {
                    case 2:
                        tHelp.FHelpStatus = 3;
                        break;

                    default:
                        return BadRequest($"無法更新狀態：當前狀態 {tHelp.FHelpStatus} 不允許更新");
                }

                _context.Entry(tHelp).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(helpId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("updateHelpStatus1/{helpId}")]
        public async Task<ActionResult<HelpDTO>> UpdateHelpStatus1(int helpId)
        {
            try
            {
                var tHelp = await _context.THelps.FirstOrDefaultAsync(h => h.FHelpId == helpId);
                if (tHelp == null)
                {
                    return NotFound("找不到對應的協助資料");
                }

                switch (tHelp.FHelpStatus)
                {
                    case 3:
                        tHelp.FHelpStatus = 4;
                        break;
                    default:
                        return BadRequest($"無法更新狀態：當前狀態 {tHelp.FHelpStatus} 不允許更新");
                }

                _context.Entry(tHelp).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(helpId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // DELETE: api/Helps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTHelp(int id)
        {
            var tHelp = await _context.THelps.FindAsync(id);
            if (tHelp == null)
            {
                return NotFound();
            }

            _context.THelps.Remove(tHelp);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool THelpExists(int id)
        {
            return _context.THelps.Any(e => e.FHelpId == id);
        }



        [HttpPost("match")]
        public async Task<IActionResult> MatchHelp([FromBody] MatchRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request data");

            try
            {

                var result = new TMatch();

                if (request == null || request.HelpId <= 0)
                {
                    return BadRequest("Invalid request data");
                }

                var tHelp = await _context.THelps.FindAsync(request.HelpId);

                if (tHelp == null)
                {
                    return NotFound("Help data not found");
                }

                var match = new TMatch
                {
                    FHelpId = tHelp.FHelpId,
                    FHelperName = request.HelperName,
                    FHelperEmail = request.HelperEmail,
                    FMatchDateTime = request.MatchDateTime,
                    FMatchStatus = 2,
                    FPoint = tHelp.FPoint
                };
                _context.TMatches.Add(match);



                tHelp.FHelpStatus = 2;
                _context.Entry(tHelp).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok(match);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("updatematchcontent")]
        public async Task<ActionResult<UpdateMatchData>> UpdateMatchContent(UpdateMatchData MatchData)
         {
            if (MatchData == null)
            {
                return BadRequest("MatchDTO cannot be null");
            }

            var tmatch = await _context.TMatches
               .FirstOrDefaultAsync(e => e.FHelpId == MatchData.HelpId);
            if (tmatch != null)
            {
                tmatch.FGrade = MatchData.Grade;
                tmatch.FMessage = MatchData.HelpContent;
                _context.TMatches.Update(tmatch);
                await _context.SaveChangesAsync();
            }
            return Ok(MatchData);
        }











        [HttpGet("getlatestMatch")]
        public async Task<ActionResult<TMatch>> GetLatestMatch()
        {
            try
            {
                var tMatch = await _context.TMatches
                    .OrderByDescending(h => h.FMatchDateTime)
                    .FirstOrDefaultAsync();

                if (tMatch == null)
                {
                    return NotFound("未找到配對記錄");
                }

                var tHelp = await _context.THelps
                    .Where(h => h.FHelpId == tMatch.FHelpId)
                    .FirstOrDefaultAsync();

                if (tHelp == null)
                {
                    return NotFound("未找到幫助記錄");
                }

                var matchDTO = new MatchDTO
                {
                    HelperName = tMatch.FHelperName ?? "未提供",
                    HelperEmail = tMatch.FHelperEmail ?? "未提供",
                    Points = tMatch.FPoint ?? 0,
                    HelpName = tHelp.FName ?? "未提供",
                    DistrictId = tHelp.FDistrictId ?? 0,
                    HelpContent = tHelp.FHelpDescribe ?? "未提供",
                    HelpClass = tHelp.FHelpClassId ?? 0,
                    HelpStatus = tHelp.FHelpStatus ?? 0,
                    Latitude = tHelp.FLatitude,
                    Longitude = tHelp.FLongitude,
                    HelpPhone = tHelp.FPhone ?? "未提供"
                };

                var helpclass = await _context.THelpClasses
                    .Where(a => a.FHelpClassId == tHelp.FHelpClassId)
                    .Select(a => a.FHelpClass)
                    .FirstOrDefaultAsync() ?? "未分類";

                // 如果有郵箱才發送郵件
                if (!string.IsNullOrEmpty(tHelp.FEmail))
                {
                    try
                    {
                        var recipientName = tHelp.FName ?? "用戶";
                        var subject = $"[Wandki]恭喜您與 {tMatch.FHelperName} 配對成功!";
                        var body = $"您好 {recipientName} 大德,\n\n" +
                                  $"您在Wandki平台申請的求助:\n" +
                                  $"類別: {helpclass}\n" +
                                  $"內容: {tHelp.FHelpDescribe}\n" +
                                  $"已由{tMatch.FHelperName}接取任務\n" +
                                  $"對方將會與您聯繫\n" +
                                  $"以上，祝您順心\n\n" +
                                  $"Wandki團隊,\n愛您";

                        await SendEmailAsync(recipientName, tHelp.FEmail, subject, body);
                    }
                    catch
                    {
                        // 郵件發送失敗不影響返回結果
                    }
                }

                return Ok(matchDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private async Task SendEmailAsync(string recipientName, string recipientEmail, string subject, string body)
        {
            try
            {
                if (string.IsNullOrEmpty(recipientEmail))
                {
                    throw new ArgumentException("收件者郵箱不能為空");
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress(recipientName, recipientEmail));
                message.Subject = subject;
                message.Body = new TextPart("plain") { Text = body };

                using var client = new SmtpClient();

                // 添加除錯訊息
                Console.WriteLine($"嘗試連接到 SMTP 服務器: {_smtpSettings.Server}:{_smtpSettings.Port}");
                Console.WriteLine($"使用帳號: {_smtpSettings.Username}");
                Console.WriteLine($"發送給: {recipientEmail}");

                await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                Console.WriteLine("郵件發送成功！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發送郵件時發生錯誤: {ex.Message}");
                throw;
            }
        }
    }

}

