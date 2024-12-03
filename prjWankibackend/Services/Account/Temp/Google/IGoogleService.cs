using prjWankibackend.Controllers.Account.DTO;

namespace prjWankibackend.Services.Account.Temp.Google
{
    public interface IGoogleService
    {
        Task<AuthResponseDto> GoogleSignupAsync(GoogleSignupRequestDto request);
        Task<AuthResponseDto> GoogleLoginAsync(GoogleLoginRequestDto request);
        Task<GoogleUserInfo> VerifyGoogleTokenAsync(string token);
    }
}
