using System.Security.Claims;
using prjWankibackend.Models.Database;

namespace prjWankibackend.Models.Account.Interfaces
{
    public interface IAccountFactory
    {
        TPersonMember SignIn(WealthierAndKinderContext context, IEnumerable<Claim> claims);
        TPersonMember tryGetMember(WealthierAndKinderContext context, IEnumerable<Claim> claims);
        IAccountOp build(WealthierAndKinderContext context,TPersonMember personMember);
    }
}
