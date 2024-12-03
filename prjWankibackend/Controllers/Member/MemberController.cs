using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prjWankibackend.Models.Account.DbExtentions;
using prjWankibackend.Models.Account.Member.DTO;
using prjWankibackend.Models.Database;
using prjWankibackend.Models.Account.Jwt;
using prjWankibackend.Models.Account.Jwt.DTO;
using prjWankibackend.Controllers.Member.DTO;
using System.Security.Claims;
using prjWankibackend.Controllers.Account.DTO;
using prjWankibackend.Services.Account.Member;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjWankibackend.Controllers.Member
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private WealthierAndKinderContext _context;
        private JwtHelper _jwtHelper;
        //記得在建構加jwtHelper
        public MemberController(WealthierAndKinderContext context, IMemberService memberService, JwtHelper jwtHelper)
        {
            _memberService = memberService;
            _jwtHelper = jwtHelper;
            _context = context;
        }
        //加這行可以擋掉未登入的人訪問該api
        [Authorize]
        [HttpGet("profileOninitOld")]
        public async Task<webMsg> GetProfile()
        {
            // 這行可以要到 id :jwtuser.UserId
            JwtUserModel jwtuser = _jwtHelper.DecodeToJwtUserModel(this.Request);
            var user =_context.TPersonMembers.FindUserById(jwtuser);

            return new webMsg((JwtUserDTO)user);
        }
        [HttpGet("profileOninit")]
        //[Authorize]
        [Authorize("CustomPolicy")]
        public async Task<IActionResult> profileOninit()
        {
            var authenticationType = User.Identity?.AuthenticationType;
            try
            {
                //_logger.LogInformation("開始獲取用戶資料"); // 資訊級別日誌

                //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                JwtUserModel user = _jwtHelper.DecodeToJwtUserModel(this.Request);
                if (string.IsNullOrEmpty(user.UserId))
                {
                    // _logger.LogWarning("未授權的訪問嘗試"); // 警告級別日誌
                    return Unauthorized(new { success = false, message = "未授權的訪問" });
                }

                var profile = await _memberService.GetProfileAsync(user.UserId);

                if (profile == null)
                {
                    //_logger.LogWarning("找不到用戶 {UserId} 的資料", userId); // 使用結構化日誌
                    return NotFound(new { success = false, message = "找不到用戶資料" });
                }

                //_logger.LogInformation("成功獲取用戶 {UserId} 的資料", userId);
                return Ok(new { success = true, data = profile });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "獲取用戶資料時發生錯誤"); // 錯誤級別日誌
                return StatusCode(500, new { success = false, message = "系統錯誤" });
            }
        }
        [HttpPost("profileUpdate")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateDto updateDto)
        {
            try
            {
                // 獲取當前用戶ID
                var userId = User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false, message = "未授權的訪問" });
                }

                var result = await _memberService.UpdateProfileAsync(userId, updateDto);

                if (result)
                {
                    return Ok(new { success = true, message = "個人資料更新成功" });
                }

                return BadRequest(new { success = false, message = "更新失敗" });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "更新個人資料時發生錯誤");
                return StatusCode(500, new { success = false, message = "系統錯誤" });
            }
            //return Ok();
        }
    }
}
