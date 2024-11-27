using prjWankibackend.Models.Account.Jwt.DTO;
using prjWankibackend.Models.Database;

namespace prjWankibackend.Models.Account.Member.DTO
{
    public class PersonDTO
    {
        public string UserType { get; set; }
        public string UserId { get; set; }

        public string UserAccount { get; set; }
        public string UserEmail { get; set; }

        public static explicit operator PersonDTO(TPersonMember user) => new()
        {
            UserType = "Person",
            UserId = user.FPersonSid.ToString(),
            UserAccount = user.FAccount,
            UserEmail = user.FEmail

        };
    }
}
