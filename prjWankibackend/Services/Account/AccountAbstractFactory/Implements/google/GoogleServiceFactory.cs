using System;
using prjWankibackend.Models.Database;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Account.Jwt;
using prjWankibackend.Models.Account.Jwt.DTO;
using System.IdentityModel.Tokens.Jwt;
using prjWankibackend.Services.Authentication.Jwt;
using prjWankibackend.Services.Authentication.TokenValidation;
using prjWankibackend.Models.Account.IAccount.Implements.Person;
using prjWankibackend.Services.Account.AccountAbstractFactory.Interfaces;

namespace prjWankibackend.Services.Account.AccountAbstractFactory.Implements.google
{
    internal class GoogleServiceFactory : IAccountServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IJwtService _jwtService;
        private readonly ITokenValidationService _tokenValidationService;

        public GoogleServiceFactory(IServiceProvider serviceProvider,
            IJwtService jwtService, ITokenValidationService tokenValidationService)
        {
            _serviceProvider = serviceProvider;
            _jwtService = jwtService;
            _tokenValidationService = tokenValidationService;

        }
        public IAccountService build()
        {
            return _serviceProvider.GetRequiredService<GoogleAccountService>();
        }
        //public TPersonMember SignIn(WealthierAndKinderContext context, IEnumerable<Claim> claims)
        //{
        //    JwtHelper jwtHelper = new JwtHelper();
        //    GoogleUserDTO googleUser = jwtHelper.GenericDecodeClaimsTo<GoogleUserDTO>(claims);
        //    TPersonMember newAccount = new TPersonMember()
        //    {
        //        FProvider = "Google",
        //        FSubId = googleUser.sub,
        //        FFirstName = googleUser.givenName,
        //        FLastName = googleUser.familyName,
        //        FEmail = googleUser.email,
        //        FEmailVerified = googleUser.emailVerified,
        //        FMemberImagePath = googleUser.picture
        //    };
        //    context.TPersonMembers.Add(newAccount);
        //    context.SaveChanges();
        //    //Console.WriteLine($"GoogleAccountFactory {amount}!");
        //    return tryGetMember(context, claims);
        //}
        //public TPersonMember tryGetMember(WealthierAndKinderContext context, IEnumerable<Claim> claims)
        //{
        //    JwtHelper jwtHelper = new JwtHelper();
        //    GoogleUserDTO googleUser = jwtHelper.GenericDecodeClaimsTo<GoogleUserDTO>(claims);
        //    TPersonMember personMember = context.TPersonMembers.FirstOrDefault(x => x.FSubId == googleUser.sub);
        //    //if (customerDb != null)
        //    return personMember;
        //}

    }
}
