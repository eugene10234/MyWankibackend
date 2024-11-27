using Microsoft.AspNetCore.Mvc;
using prjWankibackend.Controllers.Account.DTO;
using prjWankibackend.Controllers.Account.Services.Google;

namespace prjWankibackend.Controllers.Account
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoogleController : ControllerBase
    {
        private readonly IGoogleService _googleService;

        public GoogleController(IGoogleService googleService)
        {
            _googleService = googleService;
        }

        [HttpPost("google/signup")]
        public async Task<ActionResult<AuthResponseDto>> GoogleSignup([FromBody] GoogleSignupRequestDto request)
        {
            try
            {
                if (!request.Agreement)
                {
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = "必須同意服務條款"
                    });
                }

                var result = await _googleService.GoogleSignupAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("google/login")]
        public async Task<ActionResult<AuthResponseDto>> GoogleLogin([FromBody] GoogleLoginRequestDto request)
        {
            try
            {
                var result = await _googleService.GoogleLoginAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
