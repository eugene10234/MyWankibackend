using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Options;
using prjWankibackend.DTO.help;
using prjWankibackend.Models.Database;

namespace prjWankibackend.Controllers.help
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchsController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;
        public MatchsController(WealthierAndKinderContext context)
        {
            _context = context;

        }
        // GET: api/Helps
        [HttpGet("allmatch")]
        public async Task<ActionResult<IEnumerable<MatchDTO>>> GetAllMatch()
        {
            var tMatch = await _context.TMatches.OrderByDescending(m => m.FMatchDateTime).ToListAsync();
            var tHelp = await _context.THelps.ToListAsync();

            // 將 tMatch 和 tHelp 關聯起來
            var matchDTOs = tMatch.Select(match =>
            {
                var help = tHelp.FirstOrDefault(h => h.FHelpId == match.FHelpId);
                if (help == null)
                {
                    return null; // 如果找不到對應的 tHelp，則返回 null
                }

                return new MatchDTO
                {
                    HelperName = match.FHelperName,
                    HelperEmail = match.FHelperEmail,
                    Points = match.FPoint ?? 0,
                    HelpName = help.FName,
                    DistrictId = help.FDistrictId ?? 0,
                    HelpContent = help.FHelpDescribe,
                    HelpClass = help.FHelpClassId ?? 0,
                    HelpStatus = help.FHelpStatus ?? 0,
                    Latitude = help.FLatitude,
                    Longitude = help.FLongitude,
                    HelpPhone = help.FPhone,
                    MatchDate=match.FMatchDateTime?.ToString("yyyy-MM-dd"),
                };
            }).Where(dto => dto != null).ToList(); // 過濾掉 null 的項目

            return Ok(matchDTOs);
        }
    }


}
