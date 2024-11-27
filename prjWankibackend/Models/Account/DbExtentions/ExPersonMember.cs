using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Account.Authenticate.DTO;
using prjWankibackend.Models.Account.Jwt.DTO;
using prjWankibackend.Models.Database;
namespace prjWankibackend.Models.Account.DbExtentions
{
    //public partial class TPersonMember
    //{
    //    public static explicit operator JwtUserModel(TPersonMember user) => new()
    //    {

    //    };
    //}
    //public static explicit operator JwtUserModel(TPersonMember user) => new()
    //{
    //    UserId = user.FPersonSid.ToString(),
    //    MemberId = user.FMemberId,
    //    UserAccount = user.FAccount,
    //    UserType = "Person",
    //};
    public static class TPersonMemberExtensions
    {
        #region PersonBuilderExtensionsFor_V2
        //public static PersonBuilder WorksAsA
        //    (this PersonBuilder builder, string position)
        //{
        //    builder.Actions.Add(p =>
        //    {
        //        p.Position = position;
        //    });
        //    return builder;
        //}
        #endregion
        public static TPersonMember? FindUserById
            (this IEnumerable<TPersonMember>? member, JwtUserModel user)
            => member?.FirstOrDefault(t => t.FPersonSid.Equals(Convert.ToInt32(user.UserId)));
        public static TPersonMember? FindUser
            (this IEnumerable<TPersonMember>? member, LoginData login)
            => member.FindUser(login.AccountName,login.Password);
        public static TPersonMember? FindUser
            (this IEnumerable<TPersonMember>? member, string username, string password)
            => member?.FirstOrDefault(t => t.FAccount.Equals(username) && t.FPassword.Equals(password));
    }

}
