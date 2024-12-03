using prjWankibackend.Services.Account.AccountAbstractFactory.Interfaces;
using prjWankibackend.Services.Authentication.Jwt;

namespace prjWankibackend.Services.Account.AccountAbstractFactory
{
    public interface IAbstractServiceFactory
    {
        public IAccountService DetermineTokenAction(string token);
    }
}
