using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjWankibackend.Models.Account.Authenticate.DTO
{
    /// <summary>
    /// 登入資料
    /// </summary>
    public class LoginData
    {
        /// <summary>
        /// 登入帳號
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 登入密碼
        /// </summary>
        public string Password { get; set; }
    }
}
