using prjWankibackend.Configurations.Authentication;
using prjWankibackend.Models.Account.IAccount.Implements.Google;
using prjWankibackend.Models.Account.IAccount.Implements.Person;


namespace prjWankibackend.Configurations
{
    public class appsettings
    {
    }
    public  static  class ServiceConfigureExtensions
    {
        public static IServiceCollection AddServiceConfigures(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));
            services.Configure<GoogleConfig>(configuration.GetSection("GoogleConfig"));
            return services;
        }
    }
}
