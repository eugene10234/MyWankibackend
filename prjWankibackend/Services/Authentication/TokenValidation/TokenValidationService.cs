using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using prjWankibackend.Configurations.Authentication;
using prjWankibackend.Models.Account.Jwt.DTO;
using prjWankibackend.Services.Authentication.TokenValidation;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text.Json;

namespace prjWankibackend.Services.Authentication.TokenValidation
{
    public class TokenValidationService : ITokenValidationService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly GoogleConfig _googleConfig;
        private readonly ILogger<TokenValidationService> _logger;
        private readonly HttpClient _httpClient;

        public TokenValidationService(
            IOptions<JwtConfig> jwtConfig,
            IOptions<GoogleConfig> googleConfig,
            ILogger<TokenValidationService> logger,
            HttpClient httpClient)
        {
            _jwtConfig = jwtConfig.Value;
            _googleConfig = googleConfig.Value;
            _logger = logger;
            _httpClient = httpClient;
        }

        public string DetermineTokenSource(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jsonToken?.Issuer == _googleConfig.Issuer)
                {
                    return "Google";
                }
                else if (jsonToken?.Issuer == _jwtConfig.Issuer)
                {
                    return "Custom";
                }

                return "Unknown";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error determining token source");
                return "Unknown";
            }
        }

        public bool ValidateGoogleToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var validationParameters = _googleConfig.ValidationParameters;

                handler.ValidateToken(token, validationParameters, out _);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Google token validation failed");
                return false;
            }
        }

        public bool ValidateCustomToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var validationParameters = _jwtConfig.ValidationParameters;

                handler.ValidateToken(token, validationParameters, out _);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Custom token validation failed");
                return false;
            }
        }

        //public async Task<GoogleUserDTO> GetGoogleUserInfo(string token)
        //{
        //    try
        //    {
        //        var request = new HttpRequestMessage(HttpMethod.Get, "https://www.googleapis.com/oauth2/v3/userinfo");
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //        var response = await _httpClient.SendAsync(request);
        //        response.EnsureSuccessStatusCode();

        //        var content = await response.Content.ReadAsStringAsync();
        //        return JsonSerializer.Deserialize<GoogleUserDTO>(content);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error getting Google user info");
        //        throw;
        //    }
        //}
    }
}
