using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace prjWankibackend.Configurations.Authentication
{
    public class JwtConfig
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 发布者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 接受者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 过期时间（min）
        /// </summary>
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime NotBefore => DateTime.Now;

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expiration => DateTime.Now.AddMinutes(AccessTokenExpirationMinutes);

        /// <summary>
        /// 密钥Bytes
        /// </summary>
        private SecurityKey SigningKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

        /// <summary>
        /// 加密后的密钥，使用HmacSha256加密
        /// </summary>
        public SigningCredentials SigningCredentials =>
            new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);

        /// <summary>
        /// 认证用的密钥
        /// </summary>
        public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

        public TokenValidationParameters ValidationParameters => new TokenValidationParameters
        {
            //Token頒發機構
            ValidIssuer = Issuer,
            //頒給誰
            ValidAudience = Audience,
            //這裡的key要進行加密
            IssuerSigningKey = SymmetricSecurityKey,
            //是否驗證Token有效期，使用當前時間與Token的Claims中的NotBefore和Expires對比
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.FromSeconds(30)//緩衝時間，預設五分鐘
        };
    }

}
