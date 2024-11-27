using prjWankibackend.Controllers.Account.DTO;

namespace prjWankibackend.Controllers.Account.Services.Signup
{
    public interface ISignupService
    {
        Task<SignupResponseDto> SignupAsync(LocalSignupRequestDto request);
    }
}