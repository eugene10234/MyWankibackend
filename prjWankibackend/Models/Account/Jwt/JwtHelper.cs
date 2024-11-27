using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NuGet.Common;
using prjWankibackend.Models.Account.IAccount.Implements.Google;
using prjWankibackend.Models.Account.Interfaces;
using prjWankibackend.Models.Account.Jwt;
using prjWankibackend.Models.Account.Jwt.DTO;
using prjWankibackend.Models.Database;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace prjWankibackend.Models.Account.Jwt
{
    public class JwtHelper
    {
        public static JwtConfig JwtConfig { get; set; } 
        public static GoogleConfig GoogleConfig { get; set; } 

        public JwtHelper()
        {

        }
        /// <summary>
        /// 添加Jwt服务
        /// </summary>
        ///
        // 註冊 GoogleAccountFactory 作為 IAccountFactory 的實現

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddScoped(typeof(IAccountFactory<WealthierAndKinderContext>), typeof(GoogleAccountFactory<WealthierAndKinderContext>));
        }
        public void AddJwtService(IServiceCollection services)
        {
            #region AddJwtService
            services
                .AddAuthentication(options =>
                {
                    //认证middleware配置
                    // 修改這裡，使用 JwtBearerDefaults.AuthenticationScheme
                    options.DefaultAuthenticateScheme = "DefaultPolicy";
                    options.DefaultChallengeScheme = "DefaultPolicy";
                    options.DefaultScheme = "DefaultPolicy";
                
                })
                .AddJwtBearer("CustomJWT", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //Token颁发机构
                        ValidIssuer = JwtConfig.Issuer,
                        //颁发给谁
                        ValidAudience = JwtConfig.Audience,
                        //这里的key要进行加密
                        IssuerSigningKey = JwtConfig.SymmetricSecurityKey,
                        //是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.FromSeconds(30)//緩衝時間，默認五分鐘

                    };
                })
                .AddJwtBearer("Google", options =>
                {
                    // 配置 Google ID token 驗證
                    options.Authority = GoogleConfig.Issuer;
                    options.Audience = GoogleConfig.Audience;//YOUR_GOOGLE_CLIENT_ID
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = GoogleConfig.Issuer,
                        ValidateAudience = true,
                        ValidAudience = GoogleConfig.Audience,//YOUR_GOOGLE_CLIENT_ID
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };
                })
                .AddPolicyScheme("DefaultPolicy", "DefaultPolicy", options =>
                {
                // 配置策略方案
                options.ForwardDefaultSelector = context =>
                {
                    string authorization = context.Request.Headers[HeaderNames.Authorization];

                    if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer "))
                    {
                        return "CustomJWT"; // 或返回其他默認值
                    }

                    // 解析 token
                    string token = authorization.Substring("Bearer ".Length).Trim();

                    try
                    {
                        // 解析 JWT token
                        var handler = new JwtSecurityTokenHandler();
                        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                        if (jsonToken == null)
                        {
                            return "CustomJWT";
                        }

                        // 根據 token 的特徵判斷來源
                        // 例如：檢查 issuer
                        if (jsonToken.Issuer == GoogleConfig.Issuer)
                        {
                            return "Google";
                        }
                        else if (jsonToken.Issuer == JwtConfig.Issuer)
                        {
                            return "CustomJWT";
                        }

                        // 如果都不匹配，返回默認值
                        return "CustomJWT";
                    }
                    catch
                    {
                        // 如果 token 解析失敗，返回默認值
                        return "CustomJWT";
                    }
                    //// 從請求標頭獲取 Authorization
                    //string authorization = context.Request.Headers[HeaderNames.Authorization];

                    //// 如果沒有 Authorization 標頭，返回 Google 方案
                    //if (string.IsNullOrEmpty(authorization))
                    //{
                    //    return "Google";
                    //}

                    //// 根據 token 前綴判斷使用哪個方案
                    //if (authorization.StartsWith("Bearer "))
                    //{
                    //    // 可以進一步檢查 token 格式來決定使用哪個方案
                    //    return "CustomJWT";
                    //}

                    //return "Google";
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DefaultPolicy", policy =>
                {
                    policy.AddAuthenticationSchemes("CustomJWT", "Google");  // 同時支持 CustomJWT 和 Google
                    policy.RequireAuthenticatedUser(); // 要求用戶已經驗證
                });
                // 定義一個基於 CustomJWT 的政策
                options.AddPolicy("CustomPolicy", policy =>
                {
                    policy.AddAuthenticationSchemes("CustomJWT"); // 指定身份驗證方案
                    policy.RequireAuthenticatedUser();             // 要求必須已驗證
                });

                // 定義一個基於 Google 驗證的政策
                options.AddPolicy("GooglePolicy", policy =>
                {
                    policy.AddAuthenticationSchemes("Google");
                    policy.RequireAuthenticatedUser();
                });
            });
            #endregion
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <returns></returns>
        public string GetJwtToken()
        {
            var claims = new List<Claim>();
            return GetJwtToken(claims);

        }
        public string GetJwtToken(List<Claim> claims)
        {
            var jwtSecurityToken = new JwtSecurityToken(
                JwtConfig.Issuer,
                JwtConfig.Audience,
                claims,
                JwtConfig.NotBefore,
                JwtConfig.Expiration,
                JwtConfig.SigningCredentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }
        #region GetJwtToken_OldVersion
        //public string GetJwtToken(JwtUserModel user)
        //{
        //    var claims = new List<Claim>() {
        //        new Claim("UserId",user.UserId.ToString()),
        //        new Claim("UserAccount",user.UserAccount),
        //        new Claim("UserType",user.UserType.ToString()),
        //    };
        //    return GetJwtToken(claims);
        //}
        #endregion

        public string GetJwtToken(JwtUserModel user)
        {
            var claims = new List<Claim>();
            // 使用反射遍歷 JwtUserModel 的所有屬性
            foreach (var property in typeof(JwtUserModel).GetProperties())
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(user)?.ToString(); // 確保值不為 null

                if (propertyValue != null)
                {
                    claims.Add(new Claim(propertyName, propertyValue));
                }
            }
            return GetJwtToken(claims);
        }
        public IEnumerable<Claim> GetClaims(JwtUserModel user)
        {
            var claims = new List<Claim>();
            // 使用反射遍歷 JwtUserModel 的所有屬性
            foreach (var property in typeof(JwtUserModel).GetProperties())
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(user)?.ToString(); // 確保值不為 null

                if (propertyValue != null)
                {
                    claims.Add(new Claim(propertyName, propertyValue));
                }
            }
            return claims;
        }
        public string DecodeToToken(HttpRequest request)
        {
            var authorization = request.Headers["Authorization"].ToString();
            //因为我们的Jwt是自带【Bearer 】这个请求头的，所以去掉前面的头
            string token = authorization.Split(" ")[1];
            return token;
        }
        public JwtSecurityToken DecodeTokenTopayload(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
            {
                throw new ArgumentException("無效的 JWT token");
            }
            //反解密，获取其中的Claims
            var payload = handler.ReadJwtToken(token);
            return payload;
        }
        public JwtSecurityToken DecodeTopayload(HttpRequest request)
        {
            var authorization = request.Headers["Authorization"].ToString();
            //因为我们的Jwt是自带【Bearer 】这个请求头的，所以去掉前面的头
            var auth = authorization.Split(" ")[1];
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(auth))
            {
                throw new ArgumentException("無效的 JWT token");
            }
            //反解密，获取其中的Claims
            var payload = handler.ReadJwtToken(auth);
            return payload;
        }
        public IEnumerable<Claim> DecodeToClaims(HttpRequest request)
        {
            var payload = DecodeTopayload(request);
            var claims = payload.Claims;
            return claims;
        }
        public T GenericDecodeClaimsTo<T>(IEnumerable<Claim> claims) where T : new()
        {
            var instance = new T();
            // 使用反射遍歷 Generic 的所有屬性
            foreach (var property in typeof(T).GetProperties())
            {
                // 從 claims 中尋找對應的 Claim
                var claim = claims.FirstOrDefault(t => t.Type.ToLower() == property.Name.ToLower());
                if (claim != null && property.CanWrite)
                {
                    // 將 Claim 的值轉換為屬性類型並設定到屬性
                    var convertedValue = Convert.ChangeType(claim.Value, property.PropertyType);
                    property.SetValue(instance, convertedValue);
                }
            }
            return instance;
        }
        #region DecodeToJwtUserModel_V1
        //public JwtUserModel DecodeToJwtUserModel(HttpRequest request)
        //{
        //    var claims = DecodeToClaims(request);
        //    var user = new JwtUserModel()
        //    {
        //        UserId = claims.Where(t => t.Type == "UserId").First().Value,
        //        UserAccount = claims.Where(t => t.Type == "UserAccount").First().Value,
        //        UserType = claims.Where(t => t.Type == "UserType").First().Value
        //    };
        //    return user;
        //}
        #endregion
        #region DecodeToJwtUserModel_v2
        //public JwtUserModel DecodeToJwtUserModel(HttpRequest request)
        //{
        //    var claims = DecodeToClaims(request);
        //    var user = new JwtUserModel();
        //    // 使用反射遍歷 JwtUserModel 的所有屬性
        //    foreach (var property in typeof(JwtUserModel).GetProperties())
        //    {
        //        // 從 claims 中尋找對應的 Claim
        //        var claim = claims.FirstOrDefault(t => t.Type == property.name);
        //        if (claim != null && property.CanWrite)
        //        {
        //            // 將 Claim 的值轉換為屬性類型並設定到屬性
        //            var convertedValue = Convert.ChangeType(claim.Value, property.PropertyType);
        //            property.SetValue(user, convertedValue);
        //        }
        //    }
        //    return user;
        //}
        #endregion
        public JwtUserModel DecodeToJwtUserModel(HttpRequest request)
        {
            var claims = DecodeToClaims(request);
            var user = GenericDecodeClaimsTo<JwtUserModel>(claims);
            return user;
        }
        /// <summary>
        /// 获取Jwt的信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        #region DecodeGoogleToken_v1
        //public GoogleUserDTO DecodeGoogleToken(HttpRequest request)
        //{
        //    var claims = DecodeToClaims(request);
        //    var googleUser = new GoogleUserDTO
        //    {
        //        iss = claims.Where(c => c.Type == "iss").First().Value,
        //        azp = claims.Where(c => c.Type == "azp").First().Value,
        //        aud = claims.Where(c => c.Type == "aud").First().Value,
        //        sub = claims.Where(c => c.Type == "sub").First().Value,
        //        email = claims.Where(c => c.Type == "email").First().Value,
        //        emailVerified = bool.Parse(claims.Where(c => c.Type == "email_verified").First().Value ?? "false"),
        //        nbf = long.Parse(claims.Where(c => c.Type == "nbf").First().Value ?? "0"),
        //        name = claims.Where(c => c.Type == "name").First().Value,
        //        picture = claims.Where(c => c.Type == "picture").First().Value,
        //        givenName = claims.Where(c => c.Type == "given_name").First().Value,
        //        familyName = claims.Where(c => c.Type == "family_name").First().Value,
        //        iat = long.Parse(claims.Where(c => c.Type == "iat").First().Value ?? "0"),
        //        exp = long.Parse(claims.Where(c => c.Type == "exp").First().Value ?? "0"),
        //        jti = claims.Where(c => c.Type == "jti").First().Value
        //    };
        //    return googleUser;
        //}
        #endregion
        public GoogleUserDTO DecodeGoogleToken(HttpRequest request)
        {
            var claims = DecodeToClaims(request);
            var googleUser = GenericDecodeClaimsTo<GoogleUserDTO>(claims);
            return googleUser;
        }
        public enum TokenSource
        {
            Google,
            Custom,
            Unknown
        }

        public string DetermineTokenSource(JwtSecurityToken payload)
        {
            var issuer = payload.Issuer;

            // 檢查 issuer 是否是 Google 的 URL
            if (issuer ==  GoogleConfig.Issuer || issuer == "https://www.googleapis.com")
            {
                return "Google";
            }
            // 檢查是否是自定義的 issuer
            else if (issuer == JwtConfig.Issuer) // 替換成你的自定義發行者
            {
                return "Person";
            }
            // 如果格式錯誤或無法匹配，返回 Unknown
            return "Unknown";
        }
        /// <summary>
        /// Swagger添加Jwt功能
        /// </summary>
        /// <param name="options"></param>
        public static void SwaggerAddJwtHeader(SwaggerGenOptions options)
        {
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        }
                    },
                    new string[] { }
                }
            });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可，注意两者之间有空格",
                Name = "Authorization",//jwt默认的参数名称
                In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
        }

    }

}
//var authScheme = User.GetAuthenticationScheme();
public static class ClaimsPrincipalExtensions
{
    
    public static string GetAuthenticationScheme(this ClaimsPrincipal user)
    {
        if (user.HasClaim(c => c.Issuer == JwtHelper.GoogleConfig.Issuer))
        {
            return "Google";
        }
        if (user.HasClaim(c => c.Issuer == JwtHelper.JwtConfig.Issuer))
        {
            return "CustomJWT";
        }
        return "Unknown";
    }
}