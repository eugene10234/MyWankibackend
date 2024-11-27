using prjWankibackend.Models.Account.Interfaces;

namespace prjWankibackend.Models.Account.IAccount.Implements.Google
{
    internal class GoogleAccountOp : IAccountOp
    {
        public void Login()
        {
            Console.WriteLine("GoogleLogin!");
        }
    }
}
