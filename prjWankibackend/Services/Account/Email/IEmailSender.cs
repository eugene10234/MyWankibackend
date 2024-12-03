using MimeKit;

namespace prjWankibackend.Services.Account.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string recipientName, string recipientEmail, string subject, BodyBuilder bodyBuilder);
        public Task<BodyBuilder> GetResetPasswordEmailBody(string resetLink, string recipientName);
    }
}
