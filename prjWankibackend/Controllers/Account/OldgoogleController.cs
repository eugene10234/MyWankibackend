using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prjWankibackend.Models.Account.Jwt;
using prjWankibackend.Models.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjWankibackend.Controllers.Account
{

    [Route("api/[controller]")]
    [ApiController]
    public class OldgoogleController : ControllerBase
    {
        private WealthierAndKinderContext _context;
        private JwtHelper _jwtHelper;

        public OldgoogleController(WealthierAndKinderContext context, JwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
            _context = context;
        }
        /// <summary>
        /// 驗證 Google 登入授權
        /// </summary>
        /// <returns></returns>
        [Authorize("GooglePolicy")]
        [HttpGet("ValidGoogleLogin")]
        public IActionResult ValidGoogleLogin()
        {
            var claims = _jwtHelper.DecodeGoogleToken(this.Request);
            
            string? formCredential = Request.Form["credential"]; //回傳憑證
            string? formToken = Request.Form["g_csrf_token"]; //回傳令牌
            string? cookiesToken = Request.Cookies["g_csrf_token"]; //Cookie 令牌

            // 驗證 Google Token
            GoogleJsonWebSignature.Payload? payload = VerifyGoogleToken(formCredential, formToken, cookiesToken).Result;
            string msg = "";
            if (payload == null)
            {
                // 驗證失敗
                msg = "驗證 Google 授權失敗";
            }
            else
            {
                //驗證成功，取使用者資訊內容
                msg = "驗證 Google 授權成功" + "<br>";
                msg += "email:" + payload.Email + "<br>";
                msg += "name:" + payload.Name + "<br>";
                msg += "picture:" + payload.Picture;
            }

            return Ok(msg);
        }

        /// <summary>
        /// 驗證 Google Token
        /// </summary>
        /// <param name="formCredential"></param>
        /// <param name="formToken"></param>
        /// <param name="cookiesToken"></param>
        /// <returns></returns>
        [HttpGet("VerifyGoogleToken")]
        public async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string? formCredential, string? formToken, string? cookiesToken)
        {
            // 檢查空值
            if (formCredential == null || formToken == null && cookiesToken == null)
            {
                return null;
            }

            GoogleJsonWebSignature.Payload? payload;
            try
            {
                // 驗證 token
                if (formToken != cookiesToken)
                {
                    return null;
                }

                // 驗證憑證
                IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
                string GoogleApiClientId = Config.GetSection("GoogleApiClientId").Value;
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { GoogleApiClientId }
                };
                payload = await GoogleJsonWebSignature.ValidateAsync(formCredential, settings);
                if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
                {
                    return null;
                }
                if (payload.ExpirationTimeSeconds == null)
                {
                    return null;
                }
                else
                {
                    DateTime now = DateTime.Now.ToUniversalTime();
                    DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
                    if (now > expiration)
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
            return payload;
        }
        // GET: api/<OldgoogleController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<OldgoogleController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OldgoogleController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OldgoogleController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OldgoogleController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
