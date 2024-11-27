using Microsoft.EntityFrameworkCore;
using prjWankibackend.DTO.social;
using prjWankibackend.Models.Database;
using prjWankibackend.Services.Interfaces;

namespace prjWankibackend.Services
{
    public class PostService : IPostService
    {
        private readonly WealthierAndKinderContext _context;

        public PostService(WealthierAndKinderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PostDTO>> GetAllPostsAsync()
        {
            try
            {
                var posts = await _context.TPosts
                    .OrderByDescending(post => post.FCreatedTime)
                    .Select(post => new PostDTO
                    {
                        FPostId = post.FPostId,
                        FMemberId = post.FMemberId,
                        FUserName = post.FUserName,
                        FPostContent = post.FPostContent,
                        FMemberType = post.FMemberType,
                        FLikes = post.FLikes ?? 0,
                        FCreatedTime = post.FCreatedTime != null 
                            ? ((DateTime)post.FCreatedTime).ToString("yyyy-MM-dd HH:mm:ss")
                            : string.Empty,
                        IsLiked = false,
                        UserAvatar = "../assets/images/default-avatar.png",
                        Comments = 0,
                        CommentList = new List<CommentDTO>(),
                        Reposts = 0
                    })
                    .ToListAsync();

                var allComments = await _context.TComments
                    .OrderBy(c => c.FCratedAt)
                    .ToListAsync();

                var commentCounts = await _context.TComments
                    .GroupBy(c => c.FPostId)
                    .Select(g => new { PostId = g.Key, Count = g.Count() })
                    .ToListAsync();

                foreach (var post in posts)
                {
                    var commentCount = commentCounts.FirstOrDefault(c => c.PostId == post.FPostId);
                    post.Comments = commentCount?.Count ?? 0;

                    var postComments = allComments
                        .Where(c => c.FPostId == post.FPostId)
                        .Select(c => new CommentDTO
                        {
                            FCommentId = c.FCommentId,
                            FMemberId = c.FMemberId ?? string.Empty,
                            FContent = c.FContent ?? string.Empty,
                            FCratedAT = c.FCratedAt != null 
                                ? ((DateTime)c.FCratedAt).ToString("yyyy-MM-dd HH:mm:ss")
                                : string.Empty,
                            FPostId = c.FPostId ?? 0,
                            FUpdateAt = c.FUpdateAt != null 
                                ? ((DateTime)c.FUpdateAt).ToString("yyyy-MM-dd HH:mm:ss")
                                : string.Empty
                        })
                        .ToList();

                    post.CommentList = postComments;
                }

                return posts;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting posts: {ex.Message}", ex);
            }
        }
        public async Task<PostDTO> GetPostByIdAsync(int id)
        {
            var post = await _context.TPosts.FirstOrDefaultAsync(p => p.FPostId == id);
            if (post == null) return null;

            return new PostDTO
            {
                FPostId = post.FPostId,
                FMemberId = post.FMemberId,
                FUserName = post.FUserName,
                FPostContent = post.FPostContent,
                FMemberType = post.FMemberType,
                FLikes = post.FLikes.GetValueOrDefault(0),
                FCreatedTime = post.FCreatedTime.HasValue
                    ? post.FCreatedTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                    : string.Empty,
                IsLiked = false,
                UserAvatar = "../assets/images/default-avatar.png",
                Comments = await _context.TComments.CountAsync(c => c.FPostId == post.FPostId),
                CommentList = await _context.TComments
                    .Where(c => c.FPostId == post.FPostId)
                    .OrderByDescending(c => c.FCratedAt)
                    .Select(c => new prjWankibackend.DTO.social.CommentDTO
                    {
                        FCommentId = c.FCommentId,
                        FMemberId = c.FMemberId,
                        FContent = c.FContent,
                        FCratedAT = c.FCratedAt.HasValue
                            ? c.FCratedAt.Value.ToString("yyyy-MM-dd HH:mm:ss")
                            : string.Empty,
                        FPostId = c.FPostId.GetValueOrDefault(),
                        FUpdateAt = c.FUpdateAt.HasValue
                            ? c.FUpdateAt.Value.ToString("yyyy-MM-dd HH:mm:ss")
                            : string.Empty
                    }).ToListAsync()
            };
        }

       public async Task<TPost> CreatePostAsync(TPost post)
{
    try
    {
        // 驗證必要欄位
        if (string.IsNullOrEmpty(post.FMemberId))
            throw new ArgumentException("會員 ID 不能為空");
        if (string.IsNullOrEmpty(post.FUserName))
            throw new ArgumentException("用戶名稱不能為空");
        if (string.IsNullOrEmpty(post.FPostContent))
            throw new ArgumentException("貼文內容不能為空");

        // 設置預設值
        post.FCreatedTime = DateTime.Now;
        post.FLikes = 0;
        post.FParentCommentId = null;
        post.FFinStatement = null;

        // 輸出即將新增的貼文資料
        Console.WriteLine($"準備新增貼文: {System.Text.Json.JsonSerializer.Serialize(post)}");

        _context.TPosts.Add(post);

        try
        {
            await _context.SaveChangesAsync();
            Console.WriteLine($"貼文新增成功，ID: {post.FPostId}");
            return post;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"資料庫更新錯誤: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"內部錯誤: {ex.InnerException.Message}");
            throw new Exception("儲存貼文時發生錯誤", ex);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"建立貼文時發生錯誤: {ex.Message}");
        if (ex.InnerException != null)
            Console.WriteLine($"內部錯誤: {ex.InnerException.Message}");
        throw;
    }
}
        public async Task UpdatePostAsync(int id, TPost post)
        {
            try
            {
                var existingPost = await _context.TPosts.FindAsync(id);
                if (existingPost == null)
                    throw new KeyNotFoundException($"Post with ID {id} not found");

                existingPost.FPostContent = post.FPostContent;
                existingPost.FMemberType = post.FMemberType;
                // 保留原始建立時間
                existingPost.FCreatedTime = existingPost.FCreatedTime;

                _context.Entry(existingPost).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeletePostAsync(int id)
        {
            try
            {
                var post = await _context.TPosts.FindAsync(id);
                if (post == null)
                    throw new KeyNotFoundException($"Post with ID {id} not found");

                // 同時刪除相關的評論
                var comments = await _context.TPosts
                    .Where(c => c.FParentCommentId == id)
                    .ToListAsync();

                _context.TPosts.RemoveRange(comments);
                _context.TPosts.Remove(post);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> PostExistsAsync(int id)
        {
            return await _context.TPosts.AnyAsync(e => e.FPostId == id);
        }
        public async Task<IEnumerable<CommentDTO>> GetCommentsAsync(int postId)
        {
            return await _context.TComments
                .Where(c => c.FPostId == postId)
                .OrderBy(c => c.FCratedAt)
                .Select(c => new CommentDTO
                {
                    FCommentId = c.FCommentId,
                    FPostId = (int)c.FPostId,
                    FMemberId = c.FMemberId,
                    FContent = c.FContent,
                    FCratedAT = c.FCratedAt.HasValue
                        ? c.FCratedAt.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        : string.Empty,
                    FUpdateAt = c.FUpdateAt.HasValue
                        ? c.FUpdateAt.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        : string.Empty
                })
                .ToListAsync();
        }
    }
}
