using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using prjWankibackend.Configurations.Authentication;
using prjWankibackend.Models.Account.Jwt.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace prjWankibackend.Services.Authentication.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly ILogger<JwtService> _logger;
        public JwtService(IOptions<JwtConfig> jwtConfig, ILogger<JwtService> logger)
        {
            _jwtConfig = jwtConfig.Value;
            _logger = logger;
        }
        public string ExtractTokenFromHeader(HttpRequest request)
        {
            try
            {
                var authorization = request.Headers[HeaderNames.Authorization].ToString();

                if (string.IsNullOrEmpty(authorization))
                {
                    throw new UnauthorizedAccessException("未提供授權標頭");
                }

                if (!authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    throw new UnauthorizedAccessException("無效的授權標頭格式");
                }

                return authorization["Bearer ".Length..].Trim();
            }
            catch (Exception ex) when (ex is not UnauthorizedAccessException)
            {
                _logger.LogError(ex, "從請求標頭提取 token 時發生錯誤");
                throw new UnauthorizedAccessException("提取 token 時發生錯誤");
            }
        }
        public JwtSecurityToken get(HttpRequest request)
        {
            var authorization = request.Headers["Authorization"].ToString();
            //因为我们的Jwt是自带【Bearer 】这个请求头的，所以去掉前面的头
            var auth = authorization.Split(" ")[1];
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(auth))
            {
                throw new ArgumentException("無效的 JWT token");
            }
            //反解密，获取其中的Claims
            var payload = handler.ReadJwtToken(auth);
            return payload;
        }
        public IEnumerable<Claim> GetClaims(JwtUserModel user)
        {
            var claims = new List<Claim>();
            // 使用反射遍歷 JwtUserModel 的所有屬性
            foreach (var property in typeof(JwtUserModel).GetProperties())
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(user)?.ToString(); // 確保值不為 null

                if (propertyValue != null)
                {
                    claims.Add(new Claim(propertyName, propertyValue));
                }
            }
            return claims;
        }
        public string GenerateToken(JwtUserModel user)
        {
            try
            {
                var claims = GetClaims(user);
                var jwtSecurityToken = new JwtSecurityToken(
                    issuer:_jwtConfig.Issuer,
                    audience:_jwtConfig.Audience,
                    claims:claims,
                    notBefore:_jwtConfig.NotBefore,
                    expires:_jwtConfig.Expiration,
                    signingCredentials:_jwtConfig.SigningCredentials
                );
                var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token for user {UserId}", user.UserId);
                throw;
            }
        }
        public JwtSecurityToken DecodeTopayload(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                if (!handler.CanReadToken(token))
                {
                    throw new SecurityTokenException("Invalid token format");
                }

                return handler.ReadJwtToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error decoding JWT token");
                throw;
            }
        }
        public T GenericDecodeClaimsTo<T>(IEnumerable<Claim> claims) where T : new()
        {
            var instance = new T();
            // 使用反射遍歷 Generic 的所有屬性
            foreach (var property in typeof(T).GetProperties())
            {
                // 從 claims 中尋找對應的 Claim
                var claim = claims.FirstOrDefault(t => t.Type.ToLower() == property.Name.ToLower());
                if (claim != null && property.CanWrite)
                {
                    // 將 Claim 的值轉換為屬性類型並設定到屬性
                    var convertedValue = Convert.ChangeType(claim.Value, property.PropertyType);
                    property.SetValue(instance, convertedValue);
                }
            }
            return instance;
        }
        public JwtUserDTO GetUserFromToken(string token)
        {

            var jwtToken = DecodeTopayload(token);
            var user = GenericDecodeClaimsTo<JwtUserDTO>(jwtToken.Claims);
            return user;
        }

    }
}
