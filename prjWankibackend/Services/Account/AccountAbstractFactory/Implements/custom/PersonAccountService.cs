using prjWankibackend.Services.Account.AccountAbstractFactory.Interfaces;

namespace prjWankibackend.Services.Account.AccountAbstractFactory.Implements.custom
{
    internal class PersonAccountService : IAccountService
    {
        public void Login()
        {
            Console.WriteLine("WandkiPersonLogin!");
        }
    }
}
