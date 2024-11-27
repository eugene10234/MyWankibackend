using prjWankibackend.Models.Database;

namespace prjWankibackend.Models.Account.Jwt.DTO
{
    public class JwtUserDTO
    {
        public string UserType { get; set; }
        public string UserId { get; set; }

        public string UserAccount { get; set; }
        public string UserEmail { get; set; }

        public static explicit operator JwtUserDTO(TPersonMember user) => new()
        {
            UserType = "Person",
            UserId = user.FPersonSid.ToString(),
            UserAccount = user.FAccount,
            UserEmail = user.FEmail

        };
    }
}
