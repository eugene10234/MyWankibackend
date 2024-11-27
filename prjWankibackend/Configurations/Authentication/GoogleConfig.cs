using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace prjWankibackend.Configurations.Authentication
{
    public class GoogleConfig
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }
        public TokenValidationParameters ValidationParameters => new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = true,
            ValidAudience = Audience,//YOUR_GOOGLE_CLIENT_ID
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    }

}
