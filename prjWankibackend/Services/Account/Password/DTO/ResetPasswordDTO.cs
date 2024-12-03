using System.ComponentModel.DataAnnotations;

namespace prjWankibackend.Services.Account.Password.DTO
{
    public class ResetPasswordDTO
    {

        [Required(ErrorMessage = "缺少重設密碼令牌")]
        public string Token { get; set; }

        [Required(ErrorMessage = "請輸入新密碼")]
        [MinLength(6, ErrorMessage = "密碼長度至少需要6個字元")]
        public string NewPassword { get; set; }
    }

}
