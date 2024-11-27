using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace prjWankibackend.Models.Account.Jwt
{
    public class GoogleConfig
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

    }

}
