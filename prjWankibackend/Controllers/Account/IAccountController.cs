using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Account.Authenticate.DTO;
using prjWankibackend.Models.Account.DbExtentions;
using prjWankibackend.Models.Account.Jwt.DTO;
using prjWankibackend.Models.Account.Jwt;
using prjWankibackend.Models.Database;
using Microsoft.AspNetCore.Authorization;
using prjWankibackend.Models.Account.IAccount;
using prjWankibackend.Models.Account.Interfaces;
using prjWankibackend.Controllers.Account.DTO;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjWankibackend.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class IAccountController : ControllerBase
    {
        private WealthierAndKinderContext _context;
        private JwtHelper _jwtHelper;
        ExternalAccountHelper _externalAccountHelper;
        //SignupService _signupService;

        public IAccountController(WealthierAndKinderContext context, JwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
            _context = context;
            //_signupService = signupService;
            _externalAccountHelper = new ExternalAccountHelper();
        }

        [HttpPost("jwtPersonLogin")]
        public IActionResult PostjwtPersonLogin(LoginData loginData)
        {

            var user = _context.TPersonMembers.FindUser(loginData);
            if (user == null) return Ok(new webMsg("wrong_input"));
            JwtUserModel jwtModel = (JwtUserModel)user;
            string token = _jwtHelper.GetJwtToken(jwtModel);
            return Ok(new webMsg(token, null));
        }

        [Authorize("GooglePolicy")]
        [HttpGet("jwtGoogleLogin")]
        public IActionResult GetJwtGoogleLogin()
        {
            string token = _jwtHelper.DecodeToToken(this.Request);
            IAccountOp account = _externalAccountHelper.DetermineTokenAction(_context, token);
            //var user = _context.TPersonMembers.FindUser(loginData);
            //if (user == null) return Ok(new webMsg("wrong_input"));
            //JwtUserModel jwtModel = (JwtUserModel)user;
            return Ok(new webMsg(token, null));
        }

        [HttpPost("signup")]
        public async Task<ActionResult<SignupResponseDto>> Signup([FromBody] LocalSignupRequestDto request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //var result = await _signupService.SignupAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                // 根據異常類型返回不同的錯誤信息
                return StatusCode(500, new SignupResponseDto
                {
                    Success = false,
                    Message = "註冊失敗：" + ex.Message
                });
            }
        }
    }
}
