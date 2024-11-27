using Google;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Database;

namespace prjWankibackend.Middleware
{
    public class BlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<BlacklistMiddleware> _logger;

        public BlacklistMiddleware(RequestDelegate next, ILogger<BlacklistMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, WealthierAndKinderContext dbContext)
        {
            // 檢查是否為需要驗證的路徑
            if (ShouldCheckBlacklist(context))
            {
                var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var isBlacklisted = await dbContext.TBlacklists
                        .AnyAsync(b => b.BlockedUserId == userId && b.IsActive);

                    if (isBlacklisted)
                    {
                        _logger.LogWarning($"Blocked access attempt from blacklisted user: {userId}");

                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            error = "您的帳號已被封鎖，請聯繫客服人員"
                        });
                        return;
                    }
                }
            }

            await _next(context);
        }

        private bool ShouldCheckBlacklist(HttpContext context)
        {
            // 排除不需要檢查的路徑，例如靜態檔案或公開API
            var path = context.Request.Path.Value?.ToLower();

            if (string.IsNullOrEmpty(path)) return false;

            // 不檢查的路徑清單
            var excludedPaths = new[]
            {
            "/api/auth/login",
            "/api/auth/register",
            "/api/password/forget",
            "/api/password/reset",
            "/swagger",
            "/health"
        };

            return !excludedPaths.Any(p => path.StartsWith(p));
        }
    }

    // 擴充方法，方便在 Startup.cs 中使用
    public static class BlacklistMiddlewareExtensions
    {
        public static IApplicationBuilder UseBlacklistValidation(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BlacklistMiddleware>();
        }
    }

}
