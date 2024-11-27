using System.ComponentModel.DataAnnotations;
namespace prjWankibackend.Controllers.Account.DTO
{
    public class LocalSignupRequestDto
    {
        [Required(ErrorMessage = "用戶名為必填項")]
        [MinLength(3, ErrorMessage = "用戶名最少需要3個字符")]
        public string username { get; set; }

        [Required(ErrorMessage = "密碼為必填項")]
        [MinLength(6, ErrorMessage = "密碼最少需要6個字符")]
        public string password { get; set; }

        [Required(ErrorMessage = "Email為必填項")]
        [EmailAddress(ErrorMessage = "請輸入有效的Email地址")]
        public string email { get; set; }

        [Required(ErrorMessage = "必須同意服務條款")]
        public bool agreement { get; set; }
    }

    public class SignupResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }  // 如果需要返回 JWT Token
        public UserDto User { get; set; }  // 可選：返回用戶信息
    }


}

