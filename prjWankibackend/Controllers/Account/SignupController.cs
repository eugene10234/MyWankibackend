using Microsoft.AspNetCore.Mvc;
using prjWankibackend.Controllers.Account.DTO;
using prjWankibackend.Controllers.Account.Services.Signup;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjWankibackend.Controllers.Account
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignupController : ControllerBase
    {
        private readonly ISignupService _signupService;

        public SignupController(ISignupService signupService)
        {
            _signupService = signupService;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<SignupResponseDto>> Signup([FromBody] LocalSignupRequestDto request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


                var result = await _signupService.SignupAsync(request);
                return Ok(result);
           

        }
    }
}

