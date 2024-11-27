using prjWankibackend.DTO.social;
using prjWankibackend.Models.Database;

namespace prjWankibackend.Services.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostDTO>> GetAllPostsAsync();
        Task<PostDTO> GetPostByIdAsync(int id);
        Task<TPost> CreatePostAsync(TPost post);
        Task UpdatePostAsync(int id, TPost post);
        Task DeletePostAsync(int id);
        Task<bool> PostExistsAsync(int id);
        Task<IEnumerable<CommentDTO>> GetCommentsAsync(int postId);
    }
} 
