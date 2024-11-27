using prjWankibackend.Services.IAccount.Interfaces;

namespace prjWankibackend.Models.Account.IAccount.Implements.Google
{
    internal class GoogleAccountService : IAccountService
    {
        public void Login()
        {
            Console.WriteLine("GoogleLogin!");
        }
    }
}
