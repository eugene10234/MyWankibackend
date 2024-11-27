using MimeKit;

namespace prjWankibackend.Controllers.Account.Services.EmailSender
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string recipientName, string recipientEmail, string subject, BodyBuilder bodyBuilder);
        public Task<BodyBuilder> GetResetPasswordEmailBody(string resetLink, string recipientName);
    }
}
