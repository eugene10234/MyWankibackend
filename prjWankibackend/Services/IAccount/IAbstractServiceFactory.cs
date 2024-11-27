using prjWankibackend.Services.Authentication.Jwt;
using prjWankibackend.Services.IAccount.Interfaces;

namespace prjWankibackend.Services.IAccount
{
    public interface IAbstractServiceFactory
    {
        public IAccountService DetermineTokenAction(string token);
    }
}
