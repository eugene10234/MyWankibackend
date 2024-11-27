using prjWankibackend.Controllers.Account.Services.Password.DTO;

namespace prjWankibackend.Controllers.Account.Services.Password
{
    public interface IPasswordService
    {

        Task SendResetPasswordEmailAsync(ForgetPasswordDTO dto);
        Task ResetPasswordAsync(ResetPasswordDTO dto);

    }
}
