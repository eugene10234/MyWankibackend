using prjWankibackend.Services.Account.Password.DTO;

namespace prjWankibackend.Services.Account.Password
{
    public interface IPasswordService
    {

        Task SendResetPasswordEmailAsync(ForgetPasswordDTO dto);
        Task ResetPasswordAsync(ResetPasswordDTO dto);

    }
}
