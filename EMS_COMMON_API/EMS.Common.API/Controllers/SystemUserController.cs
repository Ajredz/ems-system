using EMS.Common.Core.SystemUser;
using EMS.Common.Data.DBContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Common.API.Controllers
{
    /// <summary>
    /// Employee to be assigned on Organizational Groups
    /// </summary>
    [Route("common/[controller]")]
    [ApiController]
    public class SystemUserController : ControllerBase
    {
        private readonly ISystemUserService _service;

        public SystemUserController(ISystemUserService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("getuserdetailsbyusername")]
        public async Task<IActionResult> GetSystemUserByUsername([FromQuery] string username, [FromQuery] int userid)
        {
            return await _service.GetSystemUserByUsername(username, userid).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("getuserdetails")]
        public async Task<IActionResult> GetSystemUser([FromQuery] string username, [FromQuery] string password, [FromQuery] int userid)
        {
            return await _service.GetSystemUser(username, password, userid).ConfigureAwait(true);
        }
    }
}
