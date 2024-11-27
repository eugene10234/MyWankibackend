using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.DTO.social;
using prjWankibackend.Models.Database;

namespace prjWankibackend.Controllers.social
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAngular")]
    public class SearchController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;

        public SearchController(WealthierAndKinderContext context)
        {
            _context = context;
        }

        [HttpGet("posts")]
        public async Task<ActionResult<IEnumerable<PostDTO>>> SearchPosts(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return BadRequest(new { message = "搜尋關鍵字不能為空" });
            }

            var posts = await (from p in _context.TPosts
                               join c in _context.TComments on p.FPostId equals c.FPostId into comments
                               where p.FPostContent.Contains(q) ||
                                     comments.Any(c => c.FContent.Contains(q))
                               select new PostDTO
                               {
                                   FPostId = p.FPostId,
                                   FMemberId = p.FMemberId,
                                   FUserName = p.FUserName,
                                   FPostContent = p.FPostContent,
                                   FMemberType = p.FMemberType,
                                   FLikes = p.FLikes.HasValue ? p.FLikes.Value : 0,
                                   FCreatedTime = p.FCreatedTime.HasValue ? p.FCreatedTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                                   IsLiked = false,
                                   Comments = comments.Count(),
                                   CommentList = comments.Select(c => new CommentDTO
                                   {
                                       FCommentId = c.FCommentId,
                                       FPostId = c.FPostId.Value,
                                       FMemberId = c.FMemberId,
                                       FContent = c.FContent,
                                       FCratedAT = c.FCratedAt.HasValue ? c.FCratedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                                       FUpdateAt = c.FUpdateAt.HasValue ? c.FUpdateAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : null
                                   }).ToList(),
                                   Reposts = 0
                               })
                             .Take(20)
                             .ToListAsync();

            return Ok(posts);
        }
    }
}
