using prjWankibackend.Services.Account.AccountAbstractFactory.Interfaces;

namespace prjWankibackend.Services.Account.AccountAbstractFactory.Implements.google
{
    internal class GoogleAccountService : IAccountService
    {
        public void Login()
        {
            Console.WriteLine("GoogleLogin!");
        }
    }
}
