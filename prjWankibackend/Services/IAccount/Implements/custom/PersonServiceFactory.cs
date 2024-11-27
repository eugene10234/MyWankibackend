﻿using prjWankibackend.Models.Account.IAccount.Implements.Google;
using prjWankibackend.Models.Database;
using prjWankibackend.Services.Authentication.Jwt;
using prjWankibackend.Services.Authentication.TokenValidation;
using prjWankibackend.Services.IAccount.Interfaces;
using System.Security.Claims;

namespace prjWankibackend.Models.Account.IAccount.Implements.Person
{
    internal class PersonServiceFactory : IAccountServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IJwtService _jwtService;
        private readonly ITokenValidationService _tokenValidationService;

        public PersonServiceFactory(IServiceProvider serviceProvider,
            IJwtService jwtService, ITokenValidationService tokenValidationService)
        {
            _serviceProvider = serviceProvider;
            _jwtService = jwtService;
            _tokenValidationService = tokenValidationService;
        }
        public IAccountService build()
        {
            return _serviceProvider.GetRequiredService<PersonAccountService>();
        }
        //public TPersonMember SignIn(WealthierAndKinderContext context, IEnumerable<Claim> claims)
        //{

        //    //Console.WriteLine($"PersonAccountFactory {amount} !");
        //    throw new NotImplementedException();
        //}
        //public TPersonMember tryGetMember(WealthierAndKinderContext context , IEnumerable<Claim> claims)
        //{
        //    throw new NotImplementedException();
        //}

    }
}