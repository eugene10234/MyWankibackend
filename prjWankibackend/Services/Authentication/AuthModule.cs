
using prjWankibackend.Services.Authentication.Jwt;
using prjWankibackend.Services.Authentication.TokenValidation;

namespace prjWankibackend.Services.Authentication
{
    public class AuthModule
    {
    }
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthServicesScopes(this IServiceCollection services)
        {
            //JWT
            services.AddScoped<IJwtService, JwtService>();
            //JWTTokenValidation
            services.AddScoped<ITokenValidationService, TokenValidationService>();
            return services;
        }
    }
}
