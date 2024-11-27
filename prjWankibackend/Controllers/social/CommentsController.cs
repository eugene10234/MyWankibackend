using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Database;
using System;
using System.Threading.Tasks;

namespace prjWankibackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;

        public CommentsController(WealthierAndKinderContext context)
        {
            _context = context;
        }

        // POST: api/Comments
        [HttpPost]
        public async Task<ActionResult<TComment>> CreateComment(TComment comment)
        {
            try
            {
                // 設置創建和更新時間
                comment.FCratedAt = DateTime.Now;
                comment.FUpdateAt = DateTime.Now;

                _context.TComments.Add(comment);
                await _context.SaveChangesAsync();

                // 更新貼文的評論數
                var post = await _context.TPosts.FindAsync(comment.FPostId);
                if (post != null)
                {
                    // 可以在 TPost 添加 Comments 屬性來追蹤評論數
                    var commentCount = await _context.TComments
                        .CountAsync(c => c.FPostId == post.FPostId);
                }

                return Ok(new
                {
                    success = true,
                    data = comment
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // GET: api/Comments/Post/{postId}
        [HttpGet("Post/{postId}")]
        public async Task<ActionResult> GetCommentsByPostId(int postId)
        {
            try
            {
                var comments = await _context.TComments
                    .Where(c => c.FPostId == postId)
                    .OrderByDescending(c => c.FCratedAt)
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    data = comments
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
