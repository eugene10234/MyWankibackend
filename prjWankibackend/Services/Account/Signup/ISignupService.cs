using prjWankibackend.Controllers.Account.DTO;

namespace prjWankibackend.Services.Account.Signup
{
    public interface ISignupService
    {
        Task<SignupResponseDto> SignupAsync(LocalSignupRequestDto request);
    }
}