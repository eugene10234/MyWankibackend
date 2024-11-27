using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Database;
using prjWankibackend.DTO.social;
using Microsoft.AspNetCore.Cors;
using prjWankibackend.Services.Interfaces;

namespace prjWankibackend.Controllers.social
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAngular")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        #region CRUD Operations

        // READ (Get All) - 獲取所有文章
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetTPosts()
        {
            try
            {
                var posts = await _postService.GetAllPostsAsync();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "獲取文章列表失敗",
                    error = ex.Message
                });
            }
        }

        // READ (Get One) - 獲取單一文章
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetTPost(int id)
        {
            try
            {
                var post = await _postService.GetPostByIdAsync(id);
                if (post == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "找不到指定文章"
                    });
                }
                return Ok(new
                {
                    success = true,
                    data = post
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "獲取文章失敗",
                    error = ex.Message
                });
            }
        }

        // UPDATE - 更新文章
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTPost(int id, TPost tPost)
        {
            if (id != tPost.FPostId)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "文章ID不匹配"
                });
            }

            try
            {
                await _postService.UpdatePostAsync(id, tPost);
                return Ok(new
                {
                    success = true,
                    message = "文章更新成功"
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    success = false,
                    message = "找不到指定文章"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _postService.PostExistsAsync(id))
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "找不到指定文章"
                    });
                }
                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "更新文章失敗",
                    error = ex.Message
                });
            }
        }

        public class CreatePostRequest
        {
            public string FMemberId { get; set; }
            public string FUserName { get; set; }
            public string FPostContent { get; set; }
        }

        [HttpPost("create")]
        public async Task<ActionResult<TPost>> PostTPost([FromBody] CreatePostRequest request)
        {
            try
            {
                var tPost = new TPost
                {
                    FMemberId = request.FMemberId,
                    FUserName = request.FUserName,
                    FPostContent = request.FPostContent,
                    FMemberType = null,  // 設為 null
                    FLikes = 0,
                    FCreatedTime = DateTime.Now,
                    FParentCommentId = null,
                    FFinStatement = null
                };

                var createdPost = await _postService.CreatePostAsync(tPost);

                var postDto = new PostDTO
                {
                    FPostId = createdPost.FPostId,
                    FMemberId = createdPost.FMemberId,
                    FUserName = createdPost.FUserName,
                    FPostContent = createdPost.FPostContent,
                    FMemberType = null,  // 設為 null
                    FLikes = 0,
                    FCreatedTime = createdPost.FCreatedTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                    IsLiked = false,
                    Comments = 0,
                    CommentList = new List<CommentDTO>()
                };

                return Ok(new
                {
                    success = true,
                    data = postDto,
                    message = "文章發布成功"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "文章發布失敗",
                    error = ex.Message
                });
            }
        }
        // DELETE - 刪除文章
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTPost(int id)
        {
            try
            {
                await _postService.DeletePostAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "文章刪除成功"
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    success = false,
                    message = "找不到指定文章"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "刪除文章失敗",
                    error = ex.Message
                });
            }
        }

        [HttpGet("{postId}/comments")]
        public async Task<ActionResult<List<CommentDTO>>> GetComments(int postId)
        {
            var comments = await _postService.GetCommentsAsync(postId);
            return comments.ToList();
        }
        #endregion
    }
}


