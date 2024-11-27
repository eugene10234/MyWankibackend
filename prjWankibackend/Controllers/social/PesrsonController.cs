using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Controllers.social.DTO.social;
using prjWankibackend.Models.Database;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowAngular")]
[Authorize]
public class PersonController : ControllerBase
{
    private readonly WealthierAndKinderContext _context;

    public PersonController(WealthierAndKinderContext context)
    {
        _context = context;
    }

    [HttpGet("member/{memberId}")]  // 修改路由
    public async Task<ActionResult<PersonMemberDTO>> GetPersonData(string memberId)
    {
        if (string.IsNullOrEmpty(memberId))
        {
            return BadRequest("會員 ID 不能為空");
        }

        var person = await _context.TPersonMembers
            .Where(p => p.FMemberId == memberId)
            .Select(p => new PersonMemberDTO
            {
                FMemberId = p.FMemberId,
                FUserName = p.FUserName,
                FEmail = p.FEmail,
                FTotalHelpPoint = p.FTotalHelpPoint,
                FMemberImagePath = p.FMemberImagePath
            })
            .FirstOrDefaultAsync();

        if (person == null)
        {
            return NotFound("找不到會員資料");
        }

        return Ok(person);
    }

    [HttpGet("posts/{memberId}")]
    public async Task<ActionResult<IEnumerable<dynamic>>> GetUserPosts(string memberId)
    {
        if (string.IsNullOrEmpty(memberId))
        {
            return BadRequest("會員 ID 不能為空");
        }

        var posts = await _context.TPosts
            .Where(p => p.FMemberId == memberId)
            .OrderByDescending(p => p.FCreatedTime)
            .Select(p => new
            {
                fPostId = p.FPostId,
                fMemberId = p.FMemberId,
                fPostContent = p.FPostContent,
                fCreatedTime = p.FCreatedTime.HasValue ? 
                    p.FCreatedTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                fLikes = (int)p.FLikes,
                fUserName = p.FUserName,
                fMemberType = p.FMemberType,
                userAvatar = _context.TPersonMembers
                    .Where(m => m.FMemberId == p.FMemberId)
                    .Select(m => m.FMemberImagePath)
                    .FirstOrDefault(),
                comments = _context.TComments
                    .Count(c => c.FPostId == p.FPostId),
                reposts = 0
            })
            .ToListAsync();

        return Ok(posts);
    }

}
