using prjWankibackend.Configurations.Authentication;
using prjWankibackend.Models.Account.IAccount.Implements.Google;
using prjWankibackend.Models.Account.IAccount.Implements.Person;
using prjWankibackend.Services.Authentication.Jwt;
using prjWankibackend.Services.Authentication.TokenValidation;
using prjWankibackend.Services.Authentication;
using prjWankibackend.Services.Account.AccountAbstractFactory;

namespace prjWankibackend.Services
{
    public class ServiceModule
    {
    }
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServicesScopes(this IServiceCollection services)
        {
            //Authentication
            services.AddAuthServicesScopes();
            //Account
            services.AddAccountServicesScopes();
            return services;
        }
    }
}
