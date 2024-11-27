using System.Security.Claims;
using prjWankibackend.Models.Database;

namespace prjWankibackend.Services.IAccount.Interfaces
{
    public interface IAccountServiceFactory
    {
        //TPersonMember SignIn(WealthierAndKinderContext context, IEnumerable<Claim> claims);
        //TPersonMember tryGetMember(WealthierAndKinderContext context, IEnumerable<Claim> claims);
        IAccountService build();
    }
}
