using System.ComponentModel.DataAnnotations;

namespace prjWankibackend.Services.Account.Password.DTO
{
    public class ForgetPasswordDTO
    {

        [Required(ErrorMessage = "帳號不能為空")]
        public string Account { get; set; }

        [Required(ErrorMessage = "電子郵件不能為空")]
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件")]
        public string Email { get; set; }

    }

}
