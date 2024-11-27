using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using prjWankibackend.Services.Interfaces;
using prjWankibackend.Services;
using prjWankibackend.Middleware;
using prjWankibackend.Controllers.Account;
using System.Security.Claims;
using prjWankibackend.Models.Account.Jwt;
using prjWankibackend.Models.Account.Jwt.DTO;
using prjWankibackend.Hubs;
using prjWankibackend.DTO.help;
using prjWankibackend.Models.Account.Interfaces;
using Google;
using Microsoft.AspNetCore.Identity;
using prjWankibackend.Controllers.Account.Services.Signup;
using Microsoft.Extensions.Logging;
using prjWankibackend.Controllers.Account.Services.EmailSender;
using prjWankibackend.Controllers.Account.Services.Password;
using prjWankibackend.Controllers.Account.Services.UserRepos;
using prjWankibackend.Controllers.Member.Services.Member;
//using prjWankibackend.Controllers.Account.Services.Jwt;


var builder = WebApplication.CreateBuilder(args);

// IConfiguration 已經在 builder.Configuration 中配置好了
// 可以通過依賴注入使用

// Add services to the container.
builder.Services.AddHttpClient();  // 註冊 HttpClient
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("EmailSettings:SmtpSettings"));// 添加 SMTP 設定
builder.Services.AddControllers();// 添加控制器服務
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
         builder => builder
             .WithOrigins("http://localhost:4200")
             .AllowAnyMethod()
             .AllowAnyHeader()
             .AllowCredentials());
});
builder.Services.AddSignalR(options => {
    options.EnableDetailedErrors = true;
});
builder.Services.AddControllers();
string s = builder.Configuration.GetConnectionString("Wandki");
builder
    .Services
    .AddDbContext<WealthierAndKinderContext>(options => options.UseSqlServer(s));

//範例
//var testVaule = builder.Configuration["test1:test"];


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPostService, PostService>();
// 添加 JWT 配置
AddMyService(builder);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();
        if (jwtConfig == null)
        {
            throw new InvalidOperationException("JwtConfig section is missing in configuration");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig.Issuer,      // "CSharp"
            ValidAudience = jwtConfig.Audience,   // "Angular"
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtConfig.SecretKey))  // 使用 SecretKey 而不是 Key
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/chatHub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

MyScopesDI(app);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseCors("MyPolicy");
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation($"Request Path: {context.Request.Path}");
    await next();
});
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAngular");

//加入身份認證與授權的 Middleware 宣告
//請加入以下兩行在 app.UseHttpsRedirection(); 後面
app.UseAuthentication(); // 添加驗證中間件
app.UseAuthorization();
app.UseSession();
app.MapControllers();
app.MapHub<ChatHub>("ChatHub");
app.Run();

static void AddMyService(WebApplicationBuilder builder)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("MyPolicy", policy =>
        {
            policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
        });
    });
    var jwtConfig = new JwtConfig();
    builder.Configuration.Bind("JwtConfig", jwtConfig);
    var googleConfig = new GoogleConfig();
    builder.Configuration.Bind("GoogleConfig", googleConfig);
    JwtHelper.JwtConfig = jwtConfig;
    JwtHelper.GoogleConfig = googleConfig;
    JwtHelper jwtHelper = new JwtHelper();

    builder.Services.Configure<JwtConfig>(
        builder.Configuration.GetSection("JwtConfig"));
    builder.Services.Configure<GoogleConfig>(
        builder.Configuration.GetSection("GoogleConfig"));
    // 或者如果你想要直接注入 JwtConfig 實例
    builder.Services.AddSingleton(builder.Configuration
        .GetSection("JwtConfig")
        .Get<JwtConfig>());
    builder.Services.AddSingleton(builder.Configuration
        .GetSection("GoogleConfig")
        .Get<GoogleConfig>());

    // 添加 Identity 相關服務
    //builder.Services.AddIdentity<TPersonMember, IdentityRole>(options =>
    //    {
    //        // 可以在這裡配置密碼規則等
    //        options.Password.RequireDigit = true;
    //        options.Password.RequireLowercase = true;
    //        options.Password.RequireUppercase = true;
    //        options.Password.RequireNonAlphanumeric = true;
    //        options.Password.RequiredLength = 6;
    //    })
    //    .AddEntityFrameworkStores<WealthierAndKinderContext>();

    // 如果只需要 PasswordHasher
    builder.Services.AddScoped<IPasswordHasher<TPersonMember>, PasswordHasher<TPersonMember>>();

    // 註冊您的 SignupService
    builder.Services.AddScoped<ISignupService, SignupService>();
    builder.Services.AddScoped<IMemberService, MemberService>();
    builder.Services.AddScoped<IPasswordService, PasswordService>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IEmailSender, EmailSender>();
    
    // 註冊 JWT 服務
    //builder.Services.AddScoped<IJwtService, JwtService>();
    //// 加入日誌服務
    //builder.Services.AddLogging(logging =>
    //{
    //    logging.ClearProviders();
    //    logging.AddConsole(); // 輸出到控制台
    //    logging.AddDebug();   // 輸出到偵錯視窗

    //    // 可選：添加檔案日誌
    //    logging.AddFile("Logs/app-{Date}.txt"); // 需要安裝 Serilog.Extensions.Logging.File
    //});


    //将JwtHelper添加到Services里面
    builder.Services.AddSingleton<JwtHelper>(jwtHelper);
    jwtHelper.AddJwtService(builder.Services);
    jwtHelper.ConfigureServices(builder.Services);

}
static void MyScopesDI(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<WealthierAndKinderContext>();

        //IAccountFactory.Context = dbContext;
        // 自動執行資料庫遷移
        //dbContext.Database.Migrate();
    }

}
