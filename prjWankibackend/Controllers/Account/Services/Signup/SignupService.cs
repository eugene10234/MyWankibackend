using System.Security.Cryptography;
using Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Controllers.Account.DTO;
using prjWankibackend.Models.Database;
using System.Text;
using Humanizer;

namespace prjWankibackend.Controllers.Account.Services.Signup
{
    public class SignupService : ISignupService
    {
        private readonly WealthierAndKinderContext _context;
        //private readonly IPasswordHasher<TPersonMember> _passwordHasher;

        public SignupService(
            WealthierAndKinderContext context,
            IPasswordHasher<TPersonMember> passwordHasher)
        {
            _context = context;
            //_passwordHasher = passwordHasher;
        }

        public async Task<SignupResponseDto> SignupAsync(LocalSignupRequestDto request)
        {
            // 檢查用戶名是否已存在
            if (await _context.TPersonMembers.AnyAsync(u => u.FAccount == request.username))
            {
                throw new Exception("用戶名已被使用");
            }

            // 檢查 email 是否已存在
            if (await _context.TPersonMembers.AnyAsync(u => u.FEmail == request.email))
            {
                throw new Exception("email 已被使用");
            }

            // 創建新用戶
            TPersonMember user = new TPersonMember
            {
                FAccount = request.username,
                FProvider = "Local",
                FEmail = request.email,
                // 密碼加密
                //FPassword = HashPassword(request.password),
                FPassword = request.password,
                FRegDate = DateTime.UtcNow
            };

           

            // 保存到數據庫
            _context.TPersonMembers.Add(user);
            await _context.SaveChangesAsync();

            // 返回結果
            return new SignupResponseDto
            {
                Success = true,
                Message = "註冊成功",
                User = new UserDto
                {
                    Id = user.FPersonSid.ToString(),
                    Username = user.FAccount,
                    Email = user.FEmail,
                    CreatedAt = (DateTime)user.FRegDate
                }
            };
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}