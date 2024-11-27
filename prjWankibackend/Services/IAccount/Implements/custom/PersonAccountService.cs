using prjWankibackend.Services.IAccount.Interfaces;
namespace prjWankibackend.Models.Account.IAccount.Implements.Person
{
    internal class PersonAccountService : IAccountService
    {
        public void Login()
        {
            Console.WriteLine("WandkiPersonLogin!");
        }
    }   
}
