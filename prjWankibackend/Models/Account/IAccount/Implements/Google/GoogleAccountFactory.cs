using prjWankibackend.Models.Account.Interfaces;
using System;
using prjWankibackend.Models.Database;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Account.Jwt;
using prjWankibackend.Models.Account.Jwt.DTO;
using System.IdentityModel.Tokens.Jwt;

namespace prjWankibackend.Models.Account.IAccount.Implements.Google
{
    internal class GoogleAccountFactory : IAccountFactory
    {
        
        public TPersonMember SignIn(WealthierAndKinderContext context, IEnumerable<Claim> claims)
        {
            JwtHelper jwtHelper = new JwtHelper();
            GoogleUserDTO googleUser = jwtHelper.GenericDecodeClaimsTo<GoogleUserDTO>(claims);
            TPersonMember newAccount = new TPersonMember()
            {
                FProvider = "Google",
                FSubId = googleUser.sub,
                FFirstName = googleUser.givenName,
                FLastName = googleUser.familyName,
                FEmail = googleUser.email,
                FEmailVerified = googleUser.emailVerified,
                FMemberImagePath = googleUser.picture
            };
            context.TPersonMembers.Add(newAccount);
            context.SaveChanges();
            //Console.WriteLine($"GoogleAccountFactory {amount}!");
            return tryGetMember(context, claims);
        }
        public TPersonMember tryGetMember(WealthierAndKinderContext context, IEnumerable<Claim> claims)
        {
            JwtHelper jwtHelper = new JwtHelper();
            GoogleUserDTO googleUser = jwtHelper.GenericDecodeClaimsTo<GoogleUserDTO>(claims);
            TPersonMember personMember = context.TPersonMembers.FirstOrDefault(x => x.FSubId == googleUser.sub);
            //if (customerDb != null)
            return personMember;
        }
        public IAccountOp build(WealthierAndKinderContext context, TPersonMember personMember)
        {
            return new GoogleAccountOp();
        }
    }
}
