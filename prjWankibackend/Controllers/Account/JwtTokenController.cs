using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using prjWankibackend.Models.Account;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using prjWankibackend.Models.Account.Authenticate.DTO;
using prjWankibackend.Models.Database;
using Microsoft.AspNetCore.Authorization;
using prjWankibackend.Models.Account.DbExtentions;
using prjWankibackend.Models.Account.Dictionary;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjWankibackend.Controllers.Account
{

    [Route("api/[controller]")]
    [ApiController]
    public class JwtTokenController : ControllerBase
    {
        private WealthierAndKinderContext _context;
        private IConfiguration _configuration;
        public JwtTokenController(WealthierAndKinderContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //[HttpPost]
        //public object JoseJWTPost(LoginData loginData)
        //{
        //    // JWT 口令
        //    var secret = "1qaz@wsx";

        //    // TODO: 真實世界檢查帳號密碼
        //    if (loginData.AccountName == "johnson" && loginData.password == "1234")
        //    {
        //        var payload = new JwtAuthObject()
        //        {
        //            //使用者
        //            UserAccount = loginData.AccountName,
        //            //發證者
        //            iss = "Johnson",
        //            //主旨內容
        //            sub = "WebAPIDemo",
        //            //簽發時間 = 中原標準時間」（GMT+8）
        //            iat = DateTimeOffset.Now.AddHours(8).ToUnixTimeSeconds(),
        //            //生效時間 = 中原標準時間」（GMT+8）
        //            nbf = DateTimeOffset.Now.AddHours(8).ToUnixTimeSeconds(),
        //            //過期時間 = 中原標準時間」（GMT+8）+ 30 秒
        //            exp = DateTimeOffset.Now.AddHours(8).AddSeconds(30).ToUnixTimeSeconds(),
        //            //JWT ID = GUID 亂碼
        //            jti = Guid.NewGuid().ToString().Replace("-", "")
        //        };
        //        return new
        //        {
        //            Result = true,
        //            token = Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS256)
        //        };
        //    }
        //    else
        //    {
        //        return new
        //        {
        //            Result = false,
        //            token = "帳密不正確!!"
        //        };
        //    }
        //}
        [HttpPost("jwtpostback")]
        public IActionResult jwtpostback(string hello)
        {

            Console.WriteLine(hello);
            Console.WriteLine(hello);
            return Ok(hello);

        }
        [HttpPost("authenticate")]
        public IActionResult JwActionResult([FromBody] LoginData login)
        {
            var user = _context.TPersonMembers.FindUser(login);
            if (user == null) return Unauthorized();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration[DICA.JKEY]);
            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddDays(7);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.FAccount), // 修改這裡
            new Claim(ClaimTypes.Name, user.FAccount),           // 添加這行
            new Claim(JwtClaimTypes.Audience, "JWTServicePostmanClient"),
            new Claim(JwtClaimTypes.Issuer, "JWTAuthenticationServer"),
            new Claim(JwtClaimTypes.Id, user.FPersonSid.ToString()),
            new Claim(JwtClaimTypes.Account, user.FAccount),
            new Claim(JwtClaimTypes.Email, user.FEmail)
                }),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new
            {
                access_token = tokenString,
                token_type = "Bearer",
                data = new
                {
                    sid = user.FPersonSid,
                    account = user.FAccount,
                    auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                    expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
                }
            });
        }
        // GET: api/<JwtTokenController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<JwtTokenController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<JwtTokenController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<JwtTokenController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<JwtTokenController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }


}
