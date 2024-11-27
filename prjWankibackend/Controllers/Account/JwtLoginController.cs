using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using prjWankibackend.Models.Account.Authenticate.DTO;
using prjWankibackend.Models.Account.DbExtentions;
using prjWankibackend.Models.Account.Jwt;
using prjWankibackend.Models.Account.Jwt.DTO;
using prjWankibackend.Models.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjWankibackend.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtLoginController : ControllerBase
    {
        private WealthierAndKinderContext _context;
        private JwtHelper _jwtHelper;

        public JwtLoginController(WealthierAndKinderContext context, JwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
            _context = context;
        }
        [HttpPost("jwtpostback")]
        public IActionResult jwtpostback(string hello)
        {

            Console.WriteLine(hello);
            Console.WriteLine(hello);
            return Ok(hello);

        }

        [HttpPost("login")]
        public IActionResult PostJwtLogin(LoginData loginData)
        {
            var user = _context.TPersonMembers.FindUser(loginData);
            if (user == null) return Ok(new webMsg("wrong_input"));
            JwtUserModel jwtModel = (JwtUserModel)user;
            JwtUserDTO jwtDto = (JwtUserDTO)user;
            string token = _jwtHelper.GetJwtToken(jwtModel);
            return Ok(new webMsg(token, null));
        }


        // GET: api/<JwtLoginController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<JwtLoginController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<JwtLoginController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<JwtLoginController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<JwtLoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
