using prjWankibackend.Configurations.Authentication;
using prjWankibackend.Models.Account.IAccount.Implements.Google;
using prjWankibackend.Models.Account.IAccount.Implements.Person;
using prjWankibackend.Services.Account.AccountAbstractFactory;
using prjWankibackend.Services.Authentication.Jwt;
using prjWankibackend.Services.Authentication.TokenValidation;

using prjWankibackend.Services.Authentication;
using prjWankibackend.Services.Account.Email;

using prjWankibackend.Services.Account.Password;
using prjWankibackend.Services.Account.Signup;
using prjWankibackend.Services.Account.UserRepos;
using prjWankibackend.Services.Account.Member;

namespace prjWankibackend.Services
{
    public class AccountModule
    {
    }
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAccountServicesScopes(this IServiceCollection services)
        {
            //AccountAbstractFactory
            services.AddAbstractAccountServiceScopes();
            //Signup
            services.AddScoped<ISignupService, SignupService>();
            //Member
            services.AddScoped<IMemberService, MemberService>();
            //Password
            services.AddScoped<IPasswordService, PasswordService>();
            //UserRepos
            services.AddScoped<IUserRepository, UserRepository>();
            //Email
            services.AddScoped<IEmailSender, EmailSender>();
            return services;
        }
    }
}
