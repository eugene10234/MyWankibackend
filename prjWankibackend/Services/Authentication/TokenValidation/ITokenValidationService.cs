using prjWankibackend.Models.Account.Jwt.DTO;

namespace prjWankibackend.Services.Authentication.TokenValidation
{
    public interface ITokenValidationService
    {
        string DetermineTokenSource(string token);
        bool ValidateGoogleToken(string token);
        bool ValidateCustomToken(string token);
        //Task<GoogleUserDTO> GetGoogleUserInfo(string token);
    }
}
