using System.ComponentModel.DataAnnotations;

namespace prjWankibackend.Controllers.Account.DTO
{
    public class GoogleSignupRequestDto
    {
        [Required]
        public string GoogleToken { get; set; }
        public bool Agreement { get; set; }
    }

    public class GoogleLoginRequestDto
    {
        [Required]
        public string GoogleToken { get; set; }
    }

    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public UserDto User { get; set; }
        public DateTime AccessTokenExpiration { get; internal set; }
        public DateTime RefreshTokenExpiration { get; internal set; }
    }



    // Google 用戶信息模型
    public class GoogleUserInfo
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public bool EmailVerified { get; set; }
    }
}
