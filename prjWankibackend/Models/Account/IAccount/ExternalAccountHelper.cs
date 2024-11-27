using prjWankibackend.Models.Account.IAccount.Implements.Person;
using prjWankibackend.Models.Account.Interfaces;
using prjWankibackend.Models.Account.Jwt;
using prjWankibackend.Models.Database;
using prjWankibackend.Services.IAccount.Interfaces;

namespace prjWankibackend.Models.Account.IAccount
{
    public class ExternalAccountHelper
    {
        private JwtHelper _jwtHelper;
        private List<Tuple<string, IAccountFactory>> _namedAccountFactories =
            new List<Tuple<string, IAccountFactory>>();

        public ExternalAccountHelper()
        {
            _jwtHelper = new JwtHelper();

            foreach (var t in typeof(ExternalAccountHelper).Assembly.GetTypes())
            {
                if (typeof(IAccountServiceFactory).IsAssignableFrom(t) && !t.IsInterface)
                {
                    _namedAccountFactories.Add(Tuple.Create(
                        t.Name.Replace("AccountFactory", string.Empty), (IAccountFactory)Activator.CreateInstance(t)));
                }
            }

        }
        public IAccountOp DetermineTokenAction(WealthierAndKinderContext context, string token)
        {
            //Console.WriteLine("Available accounts");
            int acctypeindex = -1;
            var payload = _jwtHelper.DecodeTokenTopayload(token);
            for (var index = 0; index < _namedAccountFactories.Count; index++)
            {
                string acctype = _jwtHelper.DetermineTokenSource(payload);
                var tuple = _namedAccountFactories[index];
                if (acctype == tuple.Item1)
                {
                    acctypeindex = index;
                }
                //Console.WriteLine($"{index}: {tuple.Item1}");
            }
            if (acctypeindex == -1)
            {
                throw new InvalidOperationException("invalid account type");
            }

            IAccountFactory accFactory = _namedAccountFactories[acctypeindex].Item2;
            TPersonMember member = accFactory.tryGetMember(context, payload.Claims);
            if (member == null)//unsigned
            {
                member = accFactory.SignIn(context, payload.Claims);
                return accFactory.build(context, member);
            }
            return accFactory.build(context, member);
        }
    }
}
