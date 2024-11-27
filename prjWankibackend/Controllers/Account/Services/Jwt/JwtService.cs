//using Google;
//using Microsoft.IdentityModel.Tokens;
//using prjWankibackend.Controllers.Account.DTO;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;
//using prjWankibackend.Models.Account.Jwt;
//using prjWankibackend.Models.Database;
//using Microsoft.Extensions.Options;

//namespace prjWankibackend.Controllers.Account.Services.Jwt
//{

//    public class JwtService : IJwtService
//    {
//        private readonly JwtConfig _jwtConfig;
//        private readonly GoogleConfig _googleConfig;
//        private readonly IConfiguration _configuration;
//        private readonly WealthierAndKinderContext _context;

//        public JwtService(
//            IOptions<JwtConfig> jwtConfig,
//            IOptions<GoogleConfig> googleConfig,
//            IConfiguration configuration,
//            WealthierAndKinderContext context)
//        {
//            _jwtConfig = jwtConfig.Value;
//            _googleConfig = googleConfig.Value;
//            _configuration = configuration;
//            _context = context;
//        }

//        public TokenDto GenerateTokens(User user, string ipAddress)
//        {
//            // 生成 Access Token
//            var accessToken = GenerateAccessToken(user);

//            // 生成 Refresh Token
//            var refreshToken = GenerateRefreshToken(ipAddress);

//            // 保存 Refresh Token 到數據庫
//            _context.TRefreshTokens.Add(new TRefreshToken
//            {
//                FToken = refreshToken.Token,
//                FExpiryDate = refreshToken.RefreshTokenExpiration,
//                FCreatedAt = DateTime.UtcNow,
//                FCreatedByIp = ipAddress
//            });

//            _context.SaveChanges();

//            return new TokenDto
//            {
//                AccessToken = accessToken,
//                RefreshToken = refreshToken.Token,
//                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes")),
//                RefreshTokenExpiration = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshTokenExpirationDays"))
//            };
//        }

//        //public async Task<TokenDto> RefreshTokenAsync(string refreshToken, string ipAddress)
//        //{
//            //var user = await _context.TPersonMembers
//            //    .Include(u => u.RefreshTokens)
//            //    .FirstOrDefaultAsync(u =>
//            //        u.RefreshTokens.Any(t =>
//            //            t.Token == refreshToken &&
//            //            !t.IsRevoked &&
//            //            t.ExpiryDate > DateTime.UtcNow));

//            //if (user == null)
//            //    throw new Exception("Invalid refresh token");

//            //var oldRefreshToken = user.RefreshTokens.Single(t => t.Token == refreshToken);

//            //// 生成新的 tokens
//            //var newTokens = GenerateTokens(user, ipAddress);

//            //// 撤銷舊的 refresh token
//            //oldRefreshToken.RevokedAt = DateTime.UtcNow;
//            //oldRefreshToken.RevokedByIp = ipAddress;
//            //oldRefreshToken.ReplacedByToken = newTokens.RefreshToken;
//            //oldRefreshToken.IsRevoked = true;

//            //await _context.SaveChangesAsync();

//            //return newTokens;
//        //}

//        //public async Task RevokeTokenAsync(string refreshToken, string ipAddress)
//        //{
//        //    var token = await _context.TRefreshTokens
//        //        .Include(t => t.User)
//        //        .FirstOrDefaultAsync(t => t.Token == refreshToken);

//        //    if (token == null)
//        //        throw new Exception("Invalid refresh token");

//        //    if (token.IsRevoked)
//        //        throw new Exception("Token already revoked");

//        //    // 撤銷 token
//        //    token.RevokedAt = DateTime.UtcNow;
//        //    token.RevokedByIp = ipAddress;
//        //    token.IsRevoked = true;

//        //    await _context.SaveChangesAsync();
//        //}
//        public string GenerateAccessToken(List<Claim> claims)
//        {
//            var jwtSecurityToken = new JwtSecurityToken(
//                _jwtConfig.Issuer,
//                _jwtConfig.Audience,
//                claims,
//                _jwtConfig.NotBefore,
//                _jwtConfig.Expiration,
//                _jwtConfig.SigningCredentials
//            );
//            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
//            return token;
//        }
//        private string GenerateAccessToken(User user)
//        {
//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
//            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var claims = new[]
//            {
//            new Claim(ClaimTypes.NameIdentifier, user.Id),
//            new Claim(ClaimTypes.email, user.email),
//            new Claim(ClaimTypes.Name, user.username)
//        };

//            var token = new JwtSecurityToken(
//                issuer: _configuration["Jwt:Issuer"],
//                audience: _configuration["Jwt:Audience"],
//                claims: claims,
//                expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes")),
//                signingCredentials: credentials
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }

//        private (string Token, DateTime RefreshTokenExpiration) GenerateRefreshToken(string ipAddress)
//        {
//            var randomNumber = new byte[32];
//            using var rng = RandomNumberGenerator.Create();
//            rng.GetBytes(randomNumber);

//            return (
//                Token: Convert.ToBase64String(randomNumber),
//                RefreshTokenExpiration: DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshTokenExpirationDays"))
//            );
//        }

//        public Task<TokenDto> RefreshTokenAsync(string refreshToken, string ipAddress)
//        {
//            throw new NotImplementedException();
//        }

//        public Task RevokeTokenAsync(string refreshToken, string ipAddress)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}

