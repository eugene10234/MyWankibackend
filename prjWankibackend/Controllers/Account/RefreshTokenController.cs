//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using prjWankibackend.Controllers.Account.DTO;
//using prjWankibackend.Controllers.Account.Services.Jwt;
//using prjWankibackend.Models.Database;

//namespace prjWankibackend.Controllers.Account
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class RefreshTokenController : ControllerBase
//    {
//        private readonly IJwtService _jwtService;

//        public RefreshTokenController(IJwtService jwtService)
//        {
//            _jwtService = jwtService;
//        }
//        [HttpPost("refresh-token")]
//        public async Task<ActionResult<TokenDto>> RefreshToken([FromBody] RefreshTokenRequestDto request)
//        {
//            try
//            {
//                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
//                var result = await _jwtService.RefreshTokenAsync(request.RefreshToken, ipAddress);
//                return Ok(result);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { message = ex.Message });
//            }
//        }

//        [HttpPost("revoke-token")]
//        [Authorize]
//        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequestDto request)
//        {
//            try
//            {
//                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
//                await _jwtService.RevokeTokenAsync(request.RefreshToken, ipAddress);
//                return Ok(new { message = "Token revoked" });
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { message = ex.Message });
//            }
//        }
//    }
//}
