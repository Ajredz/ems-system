using EMS_SecurityService.DBContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using Microsoft.AspNetCore.Http;

namespace EMS_SecurityService.Controllers
{
    [Route("security/[controller]")]
    [ApiController]
    public class SystemPageController : SharedClasses.Utilities
    {
        public SystemPageController(SystemAccessContext dbContext, IConfiguration iconfiguration) : base(dbContext, iconfiguration)
        {
        }

        [HttpGet]
        [Route("getmenu")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Get([FromQuery] int userid)
        {
            _userID = userid;
            var SystemPage = await _dbContext.SystemPage.AsNoTracking().Where(x => !x.IsHidden)
                .Join(_dbContext.SystemRolePage.AsNoTracking(), x => x.ID, y => y.PageID, (x, y) => new { x, y })
                .Join(_dbContext.SystemUserRole.AsNoTracking(), x => x.y.RoleID, y => y.RoleID, (x, y) => new { x, y })
                .Where(x => x.y.UserID == userid && !x.x.y.IsHidden)
                .Select(x => x.x.x)
                .Distinct()
                .OrderBy(x => x.ChildOrder).ToListAsync();
            if (SystemPage.Count > 0)
                return new OkObjectResult(SystemPage);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_NO_RECORDS);
        }
    }
}