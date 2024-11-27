using prjWankibackend.Models.Account.Jwt.DTO;

namespace prjWankibackend.Models.Database
{
    public partial class TPersonMember
    {

        public static explicit operator TPersonMember(JwtUserModel user) => new()
        {
        };
        public static explicit operator TPersonMember(GoogleUserDTO user) => new()
        {
        };
    }
}
