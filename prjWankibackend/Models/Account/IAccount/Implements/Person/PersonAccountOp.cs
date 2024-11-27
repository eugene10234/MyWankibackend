using prjWankibackend.Models.Account.Interfaces;
namespace prjWankibackend.Models.Account.IAccount.Implements.Person
{
    internal class PersonAccountOp : IAccountOp
    {
        public void Login()
        {
            Console.WriteLine("WandkiPersonLogin!");
        }
    }   
}
