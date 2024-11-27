using Microsoft.AspNetCore.Mvc;
using prjWankibackend.Models.Account.Jwt.DTO;
using System.Security.Claims;
using MailKit.Net.Smtp;
using prjWankibackend.Models.Account.IAccount;
using prjWankibackend.Models.Account.Interfaces;
using MailKit.Security;
using MimeKit;
using prjWankibackend.DTO.help;
using Microsoft.Extensions.Options;
using prjWankibackend.Models.Database;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjWankibackend.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class TryController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;
        private readonly SmtpSettings _smtpSettings;

        public TryController(WealthierAndKinderContext context, IOptions<SmtpSettings> smtpSettings)
        {
            _context = context;
            _smtpSettings = smtpSettings.Value;
        }



        [HttpGet("tryreflection")]
        public IActionResult tryreflection(JwtUserModel user)
        {
            var claims = new List<Claim>();
            List<(string, string)> reflist = new List<(string, string)>();
            // 使用反射遍歷 JwtUserModel 的所有屬性
            foreach (var property in typeof(JwtUserModel).GetProperties())
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(user)?.ToString(); // 確保值不為 null
                
                if (propertyValue != null)
                {
                    reflist.Add((propertyName, propertyValue));
                }
            }

            return Ok(reflist);
        }
        [HttpGet("tryAccountHelper")]
        public IActionResult tryAccountHelper()
        {
            //ExternalAccountHelper externalAccountHelper = new ExternalAccountHelper();
            //IAccountOp account = externalAccountHelper.DetermineTokenAction(this.Request);
            return Ok();
        }

        // GET: api/<TryController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TryController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
