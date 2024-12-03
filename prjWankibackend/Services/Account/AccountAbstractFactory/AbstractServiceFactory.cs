using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using prjWankibackend.Configurations.Authentication;
using prjWankibackend.Models.Account.IAccount;
using prjWankibackend.Models.Account.Jwt;
using prjWankibackend.Models.Database;
using prjWankibackend.Services.Authentication.Jwt;
using prjWankibackend.Services.Authentication.TokenValidation;
using static prjWankibackend.Services.Account.AccountAbstractFactory.AbstractServiceFactory;
using prjWankibackend.Middleware;
using prjWankibackend.Services.Account.AccountAbstractFactory.Interfaces;
using prjWankibackend.Services.Account.AccountAbstractFactory.Implements.custom;
using prjWankibackend.Services.Account.AccountAbstractFactory.Implements.google;

namespace prjWankibackend.Services.Account.AccountAbstractFactory
{
    public class AbstractServiceFactory : IAbstractServiceFactory
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly IJwtService _jwtService;
        private readonly ITokenValidationService _tokenValidationService;

        public AbstractServiceFactory(IServiceProvider serviceProvider,
            IJwtService jwtService, ITokenValidationService tokenValidationService)
        {
            _serviceProvider = serviceProvider;
            _jwtService = jwtService;
            _tokenValidationService = tokenValidationService;
            GenerateFactoryList();

        }
        private List<Tuple<string, IAccountServiceFactory>> _namedAccountFactories =
            new List<Tuple<string, IAccountServiceFactory>>();

        private void GenerateFactoryList()
        {
            foreach (var t in typeof(AbstractServiceFactory).Assembly.GetTypes())
            {
                if (typeof(IAccountServiceFactory).IsAssignableFrom(t) && !t.IsInterface)
                {
                    var factory = (IAccountServiceFactory)_serviceProvider.GetRequiredService(t);
                    _namedAccountFactories.Add(Tuple.Create(
                        t.Name.Replace("ServiceFactory", string.Empty), factory));
                    //OlD:(IAccountServiceFactory)Activator.CreateInstance(t))
                }
            }

        }
        public IAccountService DetermineTokenAction(string token)
        {
            //Console.WriteLine("Available accounts");
            int acctypeindex = -1;
            var payload = _jwtService.DecodeTopayload(token);
            //for (var index = 0; index < _namedAccountFactories.Count; index++)
            //{
            //    string acctype = _tokenValidationService.DetermineTokenSource(token);
            //    var tuple = _namedAccountFactories[index];
            //    if (acctype == tuple.Item1)
            //    {
            //        acctypeindex = index;
            //    }
            //    //Console.WriteLine($"{index}: {tuple.Item1}");
            //}
            var acctype = _tokenValidationService.DetermineTokenSource(token);
            acctypeindex = _namedAccountFactories.FindIndex(tuple => tuple.Item1 == acctype);

            if (acctypeindex == -1)
            {
                throw new InvalidOperationException("invalid account type");
            }

            IAccountServiceFactory accFactory = _namedAccountFactories[acctypeindex].Item2;
            //TPersonMember member = accFactory.tryGetMember(payload.Claims);
            //if (member == null)//unsigned
            //{
            //    member = accFactory.SignIn(payload.Claims);
            //    return accFactory.build(member);
            //}
            return accFactory.build();
        }
        //public IAccountServiceFactory Create(string serviceType)
        //{
        //    return serviceType switch
        //    {
        //        "Default" => _serviceProvider.GetRequiredService<PersonServiceFactory>(),
        //        "Service1" => _serviceProvider.GetRequiredService<GoogleServiceFactory>(),
        //        _ => throw new ArgumentException($"Unknown service type: {serviceType}")
        //    };
        //}


    }
    public static class AbstractServiceFactoryScopeExtensions
    {
        public static IServiceCollection AddAbstractAccountServiceScopes(this IServiceCollection services)
        {

            services.AddScoped<IAbstractServiceFactory, AbstractServiceFactory>();
            services.AddScoped<PersonServiceFactory>();
            services.AddScoped<GoogleServiceFactory>();
            services.AddScoped<PersonAccountService>();
            services.AddScoped<GoogleAccountService>();
            return services;
        }
    }
}
