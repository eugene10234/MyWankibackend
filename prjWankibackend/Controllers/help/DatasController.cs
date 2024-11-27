using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Database;

namespace prjWankibackend.Controllers.help
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class DatasController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;

        
        public DatasController(WealthierAndKinderContext context)
        {
            _context = context;
        }
        //呼叫地區列表
        [HttpGet("districts")]
        public async Task<ActionResult<IEnumerable<TDistrict>>> GetDistricts()
        {
          var tDistrict= _context.TDistricts.ToListAsync();
            return await tDistrict;
        }

        //呼叫求助類別列表
        [HttpGet("helpclasses")]
        public async Task<ActionResult<IEnumerable<THelpClass>>> GetHelpClasses()
        {
            var tHelpClass = _context.THelpClasses.ToListAsync();
            return await tHelpClass;
        }

        
    }
}
