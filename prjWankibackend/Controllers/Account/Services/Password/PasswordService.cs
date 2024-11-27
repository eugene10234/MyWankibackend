using Google;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using prjWankibackend.Models.Database;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Controllers.Account.DTO;
using prjWankibackend.Controllers.Account.Services.EmailSender;
using prjWankibackend.Controllers.Account.Services.Password.DTO;
using prjWankibackend.Controllers.Account.Services.UserRepos;
using prjWankibackend.Models.Account.Jwt;

namespace prjWankibackend.Controllers.Account.Services.Password
{

    public class PasswordService : IPasswordService
    {
        //private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<PasswordService> _logger;

        public PasswordService(
            //IConfiguration configuration,
            IUserRepository userRepository,
            IEmailSender emailSender,
            ILogger<PasswordService> logger)
        {
            //_configuration = configuration;
            _userRepository = userRepository;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task SendResetPasswordEmailAsync(ForgetPasswordDTO dto)
        {
            var user = await _userRepository.GetByAccountAsync(dto.Account);
            if (user == null || user.Email != dto.Email)
            {
                throw new ApplicationException("帳號或電子郵件不正確");
            }

            var token = GenerateResetToken(user);
            var resetLink = $"{JwtHelper.JwtConfig.Audience}/#/member/password/reset?token={token}";

            var emailBody = $@"
            <h2>密碼重設</h2>
            <p>您好，我們收到了您的密碼重設請求。</p>
            <p>請點擊以下連結重設密碼：</p>
            <p><a href='{resetLink}'>{resetLink}</a></p>
            <p>此連結將在24小時後失效。</p>
            <p>如果您沒有要求重設密碼，請忽略此信件。</p>";

            try
            {
                var recipientName = user.Username ?? "用戶";
                var subject = "使用者密碼重設";
                var str_url = "";
                var bodyBuilder = await _emailSender.GetResetPasswordEmailBody(resetLink, recipientName);
                await _emailSender.SendEmailAsync(
                    recipientName,
                    user.Email,
                    subject,
                    bodyBuilder
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "寄送重設密碼郵件失敗");
                throw new ApplicationException("寄送重設密碼郵件失敗，請稍後再試");
            }
        }

        public async Task ResetPasswordAsync(ResetPasswordDTO dto)
        {
            var userId = ValidateResetToken(dto.Token);
            if (userId == null)
            {
                throw new ApplicationException("無效或過期的重設連結");
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException("找不到使用者");
            }

            var hashedPassword = dto.NewPassword;//HashPassword(dto.NewPassword);
            user.Password = hashedPassword;

            await _userRepository.UpdateAsync(user);
        }

        private string GenerateResetToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtHelper.JwtConfig.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("purpose", "reset_password")
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string ValidateResetToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtHelper.JwtConfig.SecretKey);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var purpose = jwtToken.Claims.First(x => x.Type == "purpose").Value;

                if (purpose != "reset_password")
                {
                    return null;
                }

                return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            catch
            {
                return null;
            }
        }

        //private string HashPassword(string password)
        //{
        //    return BCrypt.Net.BCrypt.HashPassword(password);
        //}
    }
}
