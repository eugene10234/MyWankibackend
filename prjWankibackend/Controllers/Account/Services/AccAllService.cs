//using Google;
//using prjWankibackend.Controllers.Account.DTO;
//using prjWankibackend.Controllers.Account.Services.Jwt;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using Microsoft.EntityFrameworkCore;
//using prjWankibackend.Models.Database;

//namespace prjWankibackend.Controllers.Account.Services
//{
//    public class AccAllService
//    {
//        private readonly WealthierAndKinderContext _context;
//        private readonly IJwtService _jwtService;
//        private readonly IConfiguration _configuration;

//        public AccAllService(
//            WealthierAndKinderContext context,
//            IJwtService jwtService,
//            IConfiguration configuration)
//        {
//            _context = context;
//            _jwtService = jwtService;
//            _configuration = configuration;
//        }

//        // Google 登入
//        //public async Task<AuthResponseDto> GoogleLoginAsync(GoogleLoginRequestDto request, string ipAddress)
//        //{
//        //    var googleUser = await VerifyGoogleTokenAsync(request.GoogleToken);
//        //    var user = await _context.TPersonMembers
//        //    .FirstOrDefaultAsync(u => u.FEmail == googleUser.email);
//        //    if (user == null)
//        //    {
//        //        throw new Exception("用戶不存在，請先註冊");
//        //    }
//        //    // 生成 JWT 和 Refresh Token
//        //    var tokens = _jwtService.GenerateTokens(user, ipAddress);
//        //    return new AuthResponseDto
//        //    {
//        //        Success = true,
//        //        Message = "登入成功",
//        //        AccessToken = tokens.AccessToken,
//        //        RefreshToken = tokens.RefreshToken,
//        //        AccessTokenExpiration = tokens.AccessTokenExpiration,
//        //        RefreshTokenExpiration = tokens.RefreshTokenExpiration,
//        //        User = MapToUserDto(user)
//        //    };
//        //}
//        // 本地登入
//        //public async Task<AuthResponseDto> LocalLoginAsync(LocalSignupRequestDto request, string ipAddress)
//        //{
//        //    var user = await _context.TPersonMembers
//        //    .FirstOrDefaultAsync(u => u.FEmail == request.email);
//        //    if (user == null || !VerifyPassword(request.password, user.FPassword))
//        //    {
//        //        throw new Exception("帳號或密碼錯誤");
//        //    }
//        //    // 生成 JWT 和 Refresh Token
//        //    var tokens = _jwtService.GenerateTokens(user, ipAddress);
//        //    return new AuthResponseDto
//        //    {
//        //        Success = true,
//        //        Message = "登入成功",
//        //        AccessToken = tokens.AccessToken,
//        //        RefreshToken = tokens.RefreshToken,
//        //        AccessTokenExpiration = tokens.AccessTokenExpiration,
//        //        RefreshTokenExpiration = tokens.RefreshTokenExpiration,
//        //        User = MapToUserDto(user)
//        //    };
//        //}
//        // 刷新 Token（通用方法，不分登入方式）
//        //public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, string ipAddress)
//        //{
//        //    try
//        //    {
//        //        var tokens = await _jwtService.RefreshTokenAsync(refreshToken, ipAddress);
//        //        var userId = GetUserIdFromToken(tokens.AccessToken);
//        //        var user = await _context.TPersonMembers.FindAsync(userId);
//        //        return new AuthResponseDto
//        //        {
//        //            Success = true,
//        //            Message = "Token 刷新成功",
//        //            AccessToken = tokens.AccessToken,
//        //            RefreshToken = tokens.RefreshToken,
//        //            AccessTokenExpiration = tokens.AccessTokenExpiration,
//        //            RefreshTokenExpiration = tokens.RefreshTokenExpiration,
//        //            User = MapToUserDto(user)
//        //        };
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        throw new Exception("刷新 Token 失敗", ex);
//        //    }
//        //}
//        // 登出（撤銷 Refresh Token）
//        public async Task LogoutAsync(string refreshToken, string ipAddress)
//        {
//            if (string.IsNullOrEmpty(refreshToken))
//                return;
//            await _jwtService.RevokeTokenAsync(refreshToken, ipAddress);
//        }
//        private UserDto MapToUserDto(User user)
//        {
//            return new UserDto
//            {
//                Id = user.Id,
//                email = user.email,
//                username = user.username,
//                AvatarUrl = user.AvatarUrl,
//                Provider = user.Provider
//            };
//        }
//        private string GetUserIdFromToken(string token)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var jwtToken = tokenHandler.ReadJwtToken(token);
//            return jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
//        }
//    }
//}

