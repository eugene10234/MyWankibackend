using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using prjWankibackend.DTO.help;
using prjWankibackend.Models.Database;

namespace prjWankibackend.Services.Account.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailSender(WealthierAndKinderContext context, IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }
        public async Task SendEmailAsync(string recipientName, string recipientEmail, string subject, BodyBuilder bodyBuilder)
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

        public async Task<BodyBuilder> GetResetPasswordEmailBody(string resetLink, string recipientName)
        {
            var bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset='utf-8'>
            <style>
                .email-container {{
                    max-width: 600px;
                    margin: 0 auto;
                    font-family: '微軟正黑體', Arial, sans-serif;
                    color: #333;
                }}
                .header {{
                    background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
                    padding: 30px;
                    text-align: center;
                    border-radius: 8px 8px 0 0;
                }}
                .content {{
                    background: #ffffff;
                    padding: 30px;
                    border-radius: 0 0 8px 8px;
                    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
                }}
                .button {{
                    display: inline-block;
                    padding: 12px 24px;
                    background-color: #007bff;
                    color: #ffffff;
                    text-decoration: none;
                    border-radius: 4px;
                    margin: 20px 0;
                }}
                .footer {{
                    text-align: center;
                    margin-top: 20px;
                    color: #666;
                    font-size: 12px;
                }}
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='header'>
                    <h1>密碼重設請求</h1>
                </div>
                <div class='content'>
                    <p>親愛的{recipientName}會員您好，</p>
                    <p>我們收到了您的密碼重設請求。如果這不是您本人的操作，請忽略此郵件。</p>
                    <p>若要重設密碼，請點擊下方按鈕：</p>
                    <div style='text-align: center;'>
                        <a href='{resetLink}' class='button'>重設密碼</a>
                    </div>
                    <p>或複製以下連結至瀏覽器：</p>
                    <p style='word-break: break-all;'>{resetLink}</p>
                    <p>此連結將在24小時後失效。</p>
                    <p>為了帳號安全，請勿將此郵件轉寄給他人。</p>
                </div>
                <div class='footer'>
                    <p>此為系統自動發送的郵件，請勿直接回覆。</p>
                    <p>© 2024 Your Company. All rights reserved.</p>
                </div>
            </div>
        </body>
        </html>";

            return bodyBuilder;
        }
    }

}
