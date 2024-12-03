using Google;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Controllers.Account.DTO;
using prjWankibackend.Models.Database;

namespace prjWankibackend.Services.Account.UserRepos
{
    public class UserRepository : IUserRepository
    {
        private readonly WealthierAndKinderContext _context;

        public UserRepository(WealthierAndKinderContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(string userId)
        {
            var member = await _context.TPersonMembers
                .FirstOrDefaultAsync(u => u.FPersonSid.ToString() == userId);

            return member == null ? null : (User)member;
        }

        public async Task<User> GetByAccountAsync(string account)
        {
            var member = await _context.TPersonMembers
                .FirstOrDefaultAsync(u => u.FAccount == account);

            return member == null ? null : (User)member;
        }

        public async Task UpdateAsync(User user)
        {
            var member = await _context.TPersonMembers
                .FirstOrDefaultAsync(u => u.FPersonSid.ToString() == user.Id);

            if (member != null)
            {
                member.FPassword = user.Password;
                member.FLastLoginAt = DateTime.UtcNow;

                _context.TPersonMembers.Update(member);
                await _context.SaveChangesAsync();
            }
        }
    }
}
