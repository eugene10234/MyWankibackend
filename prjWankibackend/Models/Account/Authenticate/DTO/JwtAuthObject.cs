using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// ReSharper disable CommentTypo

namespace prjWankibackend.Models.Account.Authenticate.DTO
{
    /// <summary>
    /// JWT 認證物件
    /// </summary>
    public class JwtAuthObject
    {
        /// <summary>
        /// issuser : 發證者
        /// </summary>
        public string Iss { get; set; }
        /// <summary>
        /// subject : 主體內容
        /// </summary>
        public string Sub { get; set; }
        /// <summary>
        /// Expiration Time
        /// </summary>
        public long Exp { get; set; }
        /// <summary>
        /// issue at : 簽發時間
        /// </summary>
        public long Iat { get; set; }
        /// <summary>
        /// not before : 生效時間
        /// </summary>
        public long Nbf { get; set; }
        /// <summary>
        /// JWT ID
        /// </summary>
        public string Jti { get; set; }
        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string UserName { get; set; }
    }
}
