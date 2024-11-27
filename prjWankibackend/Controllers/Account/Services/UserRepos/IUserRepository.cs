using prjWankibackend.Controllers.Account.DTO;

namespace prjWankibackend.Controllers.Account.Services.UserRepos
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByAccountAsync(string account);
        Task UpdateAsync(User user);
    }
}
