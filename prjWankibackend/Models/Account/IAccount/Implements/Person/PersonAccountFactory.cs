using prjWankibackend.Models.Account.Interfaces;
using prjWankibackend.Models.Database;
using System.Security.Claims;

namespace prjWankibackend.Models.Account.IAccount.Implements.Person
{
    internal class PersonAccountFactory : IAccountFactory
    {

        public TPersonMember SignIn(WealthierAndKinderContext context, IEnumerable<Claim> claims)
        {

            //Console.WriteLine($"PersonAccountFactory {amount} !");
            throw new NotImplementedException();
        }
        public TPersonMember tryGetMember(WealthierAndKinderContext context , IEnumerable<Claim> claims)
        {
            throw new NotImplementedException();
        }
        public IAccountOp build(WealthierAndKinderContext context, TPersonMember personMember)
        {
            throw new NotImplementedException();
        }
    }
}
