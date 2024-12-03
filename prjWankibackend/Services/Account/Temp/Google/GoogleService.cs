//using global::Google.Apis.Auth;
//using global::Google;
//using Google.Apis.Auth;
//using Microsoft.EntityFrameworkCore;
//using prjWankibackend.Controllers.Account.DTO;
//using prjWankibackend.Models.Database;
//using prjWankibackend.Controllers.Account.Services.Jwt;
//namespace prjWankibackend.Controllers.Account.Services.Google
//{
//    public class GoogleService : IGoogleService
//    {
//        private readonly WealthierAndKinderContext _context;
//        private readonly IConfiguration _configuration;
//        private readonly IJwtService _jwtService;

//        public GoogleService(
//            WealthierAndKinderContext context,
//            IConfiguration configuration,
//            IJwtService jwtService)
//        {
//            _context = context;
//            _configuration = configuration;
//            _jwtService = jwtService;
//        }

//        public async Task<GoogleUserInfo> VerifyGoogleTokenAsync(string token)
//        {
//            try
//            {
//                var settings = new GoogleJsonWebSignature.ValidationSettings()
//                {
//                    Audience = new[] { _configuration["Google:ClientId"] }
//                };

//                var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);

//                return new GoogleUserInfo
//                {
//                    Id = payload.Subject,
//                    email = payload.email,
//                    Name = payload.Name,
//                    Picture = payload.Picture,
//                    EmailVerified = payload.EmailVerified
//                };
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Google token 驗證失敗", ex);
//            }
//        }

//        //public async Task<AuthResponseDto> GoogleSignupAsync(GoogleSignupRequestDto request)
//        //{
//        //    var googleUser = await VerifyGoogleTokenAsync(request.GoogleToken);

//        //    // 檢查用戶是否已存在
//        //    var existingUser = await _context.TPersonMembers
//        //        .FirstOrDefaultAsync(u => u.FEmail == googleUser.email);

//        //    if (existingUser != null)
//        //    {
//        //        throw new Exception("此 email 已註冊");
//        //    }

//        //    // 創建新用戶
//        //    var user = new TPersonMember
//        //    {
//        //        FEmail = googleUser.email,
//        //        FAccount = googleUser.Name,
//        //        FMemberImagePath = googleUser.Picture,
//        //        FProvider = "Google",
//        //        FSubId = googleUser.Id,
//        //        FEmailVerified = googleUser.EmailVerified,
//        //        FRegDate = DateTime.UtcNow
//        //    };

//        //    _context.TPersonMembers.Add(user);
//        //    await _context.SaveChangesAsync();
//        //    var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
//        //    // 生成 JWT token
//        //    var tokens = _jwtService.GenerateTokens(user);

//        //    return new AuthResponseDto
//        //    {
//        //        Success = true,
//        //        Message = "註冊成功",
//        //        AccessToken = tokens.AccessToken,
//        //        RefreshToken = tokens.RefreshToken,
//        //        User = new UserDto
//        //        {
//        //            Id = user.FPersonSid.ToString(),
//        //            email = user.FEmail,
//        //            username = user.FAccount,
//        //            AvatarUrl = user.FMemberImagePath,
//        //            Provider = user.FProvider,
//        //            CreatedAt = (DateTime)user.FRegDate
//        //        }
//        //    };
//        //}

//        //public async Task<AuthResponseDto> GoogleLoginAsync(GoogleLoginRequestDto request)
//        //{
//        //    var googleUser = await VerifyGoogleTokenAsync(request.GoogleToken);

//        //    var user = await _context.TPersonMembers
//        //        .FirstOrDefaultAsync(u => u.FEmail == googleUser.email);

//        //    if (user == null)
//        //    {
//        //        throw new Exception("用戶不存在，請先註冊");
//        //    }

//        //    // 更新用戶信息
//        //    user.FMemberImagePath = googleUser.Picture;
//        //    user.FAccount = googleUser.Name;
//        //    user.FLastLoginAt = DateTime.UtcNow;

//        //    await _context.SaveChangesAsync();

//        //    // 生成 JWT token
//        //    var tokens = _jwtService.GenerateTokens(user);

//        //    return new AuthResponseDto
//        //    {
//        //        Success = true,
//        //        Message = "登入成功",
//        //        AccessToken = tokens.AccessToken,
//        //        RefreshToken = tokens.RefreshToken,
//        //        User = new UserDto
//        //        {
//        //            Id = user.FPersonSid.ToString(),
//        //            email = user.FEmail,
//        //            username = user.FAccount,
//        //            AvatarUrl = user.FMemberImagePath,
//        //            Provider = user.FProvider,
//        //            CreatedAt = (DateTime)user.FRegDate
//        //        }
//        //    };
//        //}

//        public Task<AuthResponseDto> GoogleSignupAsync(GoogleSignupRequestDto request)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<AuthResponseDto> GoogleLoginAsync(GoogleLoginRequestDto request)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
