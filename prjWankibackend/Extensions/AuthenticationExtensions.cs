using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Options;
using prjWankibackend.Configurations.Authentication;

namespace prjWankibackend.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var jwtConfig = scope.ServiceProvider.GetRequiredService<IOptions<JwtConfig>>().Value;
            var googleConfig = scope.ServiceProvider.GetRequiredService<IOptions<GoogleConfig>>().Value;


            services
                .AddAuthentication(options =>
                {
                    // 認證middleware配置
                    // 修改這裡，使用 JwtBearerDefaults.AuthenticationScheme
                    options.DefaultAuthenticateScheme = "DefaultPolicy";
                    options.DefaultChallengeScheme = "DefaultPolicy";
                    options.DefaultScheme = "DefaultPolicy";
                })
                .AddJwtBearer("CustomJWT", options => ConfigureCustomJwt(options, jwtConfig))
                .AddJwtBearer("Google", options => ConfigureGoogleJwt(options, googleConfig))
                .AddPolicyScheme("DefaultPolicy", "DefaultPolicy", options =>
                    ConfigureDefaultPolicy(options, jwtConfig, googleConfig));

            // 添加授權策略
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

            return services;
        }

        private static void ConfigureCustomJwt(JwtBearerOptions options, JwtConfig jwtConfig)
        {
            options.TokenValidationParameters = jwtConfig.ValidationParameters;
        }

        private static void ConfigureGoogleJwt(JwtBearerOptions options, GoogleConfig googleConfig)
        {
            // 配置 Google ID token 驗證
            options.Authority = googleConfig.Issuer;
            options.Audience = googleConfig.Audience;//YOUR_GOOGLE_CLIENT_ID
            options.TokenValidationParameters = googleConfig.ValidationParameters;
        }

        private static void ConfigureDefaultPolicy(PolicySchemeOptions options,
            JwtConfig jwtConfig, GoogleConfig googleConfig)
        {
            // 配置策略方案
            options.ForwardDefaultSelector = context =>
            {
                // 從請求標頭獲取 Authorization
                string authorization = context.Request.Headers[HeaderNames.Authorization];

                // 如果沒有 Authorization 標頭，返回 Google 方案
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
                    if (jsonToken.Issuer == googleConfig.Issuer)
                    {
                        return "Google";
                    }
                    else if (jsonToken.Issuer == jwtConfig.Issuer)
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
            };
        }
    }
}
