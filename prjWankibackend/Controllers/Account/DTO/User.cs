using prjWankibackend.Models.Database;

namespace prjWankibackend.Controllers.Account.DTO
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public string Provider { get; set; }
        public string GoogleId { get; set; }
        public bool EmailVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public string Password { get; internal set; }

        public static explicit operator User(TPersonMember member)
        {
            if (member == null)
                return null;

            return new User
            {
                Id = member.FPersonSid.ToString(),
                Email = member.FEmail,
                Username = member.FAccount,
                AvatarUrl = null,
                Provider = "local",
                GoogleId = null,
                EmailVerified = true,
                CreatedAt = (DateTime)member.FRegDate,
                LastLoginAt = member.FLastLoginAt
            };
        }

    }

    
}

