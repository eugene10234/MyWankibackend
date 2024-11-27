using prjWankibackend.Models.Account.Jwt.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace prjWankibackend.Services.Authentication.Jwt
{
    public interface IJwtService
    {
        public string ExtractTokenFromHeader(HttpRequest request);
        IEnumerable<Claim> GetClaims(JwtUserModel user);
        string GenerateToken(JwtUserModel user);
        JwtSecurityToken DecodeTopayload(string token);
        T GenericDecodeClaimsTo<T>(IEnumerable<Claim> claims) where T : new();
        JwtUserDTO GetUserFromToken(string token);


    }
}
