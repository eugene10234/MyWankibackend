using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using prjWankibackend.DTO.help;
using prjWankibackend.Models.Database;
using System.Text;
using MailKit.Net.Smtp;
using prjWankibackend.Models.Account.DbExtentions;
using MailKit.Security;
using MimeKit;
using prjWankibackend.Models.Account.Jwt.DTO;
using System.Security.Claims;
using Google;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Account.Jwt;
using prjWankibackend.Services.Account.Password;
using prjWankibackend.Services.Account.Password.DTO;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjWankibackend.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        //JwtHelper _jwtHelper;
        private readonly WealthierAndKinderContext _context;
        private readonly SmtpSettings _smtpSettings;
        //private readonly IEmailService _emailService;
        private readonly IPasswordService _authService;



            
        private readonly IPasswordService _passwordService;

        public PasswordController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        [HttpPost("forget")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDTO dto)
        {
            try
            {
                await _passwordService.SendResetPasswordEmailAsync(dto);
                return Ok(new { message = "重設密碼連結已寄出，請查看您的信箱" });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO request)
        {
            try
            {
                await _passwordService.ResetPasswordAsync(request);
                return Ok(new { message = "密碼已成功重設" });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        //------------------------------------------------------

        [HttpGet("tryAccountHelper")]
        public IActionResult tryAccountHelper()
        {
            //ExternalAccountHelper externalAccountHelper = new ExternalAccountHelper();
            //IAccountOp account = externalAccountHelper.DetermineTokenAction(this.Request);
            return Ok();
        }
        private string Forget(string userNo)
        {
            var repo = _context.TPersonMembers;
            string str_value = "";
            string str_code = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            var userData = repo.FirstOrDefault(m => m.FAccount == userNo || m.FEmail == userNo);//FindAsync(m => m.UserNo == userNo || m.ContactEmail == userNo);
            if (userData != null)
            {
                //產生新密碼
                str_value = Guid.NewGuid().ToString().ToUpper().Replace("-", "");

                //更新資料
                //userData.FPassword = str_value;//Encoding.UTF8.GetBytes(str_value);
                userData.FSubId = str_code;//----------------------
                userData.FEmailVerified = false;
                repo.Update(userData);
                _context.SaveChanges();
            }
            return str_value;

        }
        private async Task SendEmailAsync(string recipientName, string recipientEmail, string subject, BodyBuilder bodyBuilder)
        {
            try
            {
                if (string.IsNullOrEmpty(recipientEmail))
                {
                    throw new ArgumentException("收件者郵箱不能為空");
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress(recipientName, recipientEmail));
                message.Subject = subject;
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();

                // 添加除錯訊息
                Console.WriteLine($"嘗試連接到 SMTP 服務器: {_smtpSettings.Server}:{_smtpSettings.Port}");
                Console.WriteLine($"使用帳號: {_smtpSettings.Username}");
                Console.WriteLine($"發送給: {recipientEmail}");

                await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                Console.WriteLine("郵件發送成功！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發送郵件時發生錯誤: {ex.Message}");
                throw;
            }
        }

        private async Task<bool> InitiatePasswordReset(string email)
        {
            var user = await _context.TPersonMembers
                .FirstOrDefaultAsync(u => u.FEmail == email && u.FEmailVerified == true);

            if (user == null) return false;

            // 生成重設密碼 token
            var token = await GeneratePasswordResetToken(user);

            // 構建重設密碼 URL
            var resetUrl = $"{JwtHelper.JwtConfig.Audience}/reset-password?token={token}";

            // 構建郵件內容
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset=""utf-8"">
                <style>
                    .button {{ 
                        padding: 10px 20px;
                        background-color: #007bff;
                        color: white;
                        text-decoration: none;
                        border-radius: 5px;
                    }}
                </style>
            </head>
            <body>
                <p>親愛的 {user.FAccount} 您好：</p>
                <p>我們收到了重設您密碼的請求。請點擊下方連結重設密碼：</p>
                <p>
                    <a href=""{resetUrl}"" class=""button"">重設密碼</a>
                </p>
                <p>如果您沒有要求重設密碼，請忽略此郵件。</p>
                <p>此連結將在24小時後失效。</p>
            </body>
            </html>";

            // 發送郵件
            await SendEmailAsync(
                user.FAccount,
                user.FEmail,
                "密碼重設請求",
                bodyBuilder
            );

            return true;
        }

        private async Task<bool> ResetPassword(string token, string newPassword)
        {
            // 驗證 token
            var userId = ValidatePasswordResetToken(token);
            if (userId == null) return false;

            var user = await _context.TPersonMembers.FindAsync(userId);
            if (user == null) return false;

            // 更新密碼
            user.FPassword = newPassword;//HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<string> GeneratePasswordResetToken(TPersonMember user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtHelper.JwtConfig.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("userId", user.FPersonSid.ToString()),
                new Claim("purpose", "password_reset")
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

        private string ValidatePasswordResetToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(JwtHelper.JwtConfig.SecretKey);
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);

                var purpose = principal.FindFirst("purpose")?.Value;
                if (purpose != "password_reset") return null;

                return principal.FindFirst("userId")?.Value;
            }
            catch
            {
                return null;
            }
        }
        [HttpGet("trySendEmail")]
        public async Task<IActionResult> trySendEmail()
        {
            TPersonMember acc = new TPersonMember();
            acc.FEmail = "eugeneshih007@gmail.com";
            acc.FEmailVerified = true;
            acc.FAccount = "eugene007";
            if (!string.IsNullOrEmpty(acc.FEmail) && acc.FEmailVerified == true)
            {
                try
                {
                    var recipientName = acc.FAccount ?? "用戶";
                    var subject = $"使用者密碼重設";
                    var str_url = "";
                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.HtmlBody = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset=""utf-8"">
            <style>
                body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
                .container {{ padding: 20px; }}
                .button {{ 
                    display: inline-block;
                    padding: 10px 20px;
                    background-color: #007bff;
                    color: white;
                    text-decoration: none;
                    border-radius: 5px;
                    margin: 15px 0;
                }}
                .footer {{ 
                    margin-top: 20px;
                    padding-top: 20px;
                    border-top: 1px solid #eee;
                    font-size: 12px;
                    color: #666;
                }}
            </style>
        </head>
        <body>
            <div class=""container"">
                <p>親愛的使用者 {recipientName} 您好：</p>
                <p>您執行了忘記密碼的功能，以下是您新的密碼!!</p>
                <h4>newPassword</h4>
                <p>以下的連結為驗證連結，請點擊後即完成重設密碼程序!!</p>
                <p>
                    <a href=""{str_url}"" class=""button"">
                        重設密碼連結網址
                    </a>
                </p>
                <div class=""footer"">
                    <p>本信件為系統自動發出，請勿回信，謝謝!!</p>
                </div>
            </div>
        </body>
        </html>";

                    // 設置純文本版本作為備用
                    bodyBuilder.TextBody = $@"
親愛的使用者 {recipientName} 您好：

您執行了忘記密碼的功能，以下是您新的密碼!!

newPassword

以下的連結為驗證連結，請點擊後即完成重設密碼程序!!
{str_url}

本信件為系統自動發出，請勿回信，謝謝!!";
                    var body = bodyBuilder.ToString();

                    await SendEmailAsync(recipientName, acc.FEmail, subject, bodyBuilder);
                }
                catch
                {
                    // 郵件發送失敗不影響返回結果
                }
            }

            return Ok();
        }
    }
}
