using EMS.Security.Data.DBContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Security.API.Controllers
{
    /// <summary>
    /// For Testing Connection
    /// </summary>
    [Route("Security/[controller]")]
    [ApiController]
    public class TestConnectionController : Core.Shared.Utilities
    {
        private readonly SystemAccessContext _dbContext;

        public TestConnectionController(SystemAccessContext dbContext, IConfiguration iconfiguration) : base(dbContext, iconfiguration)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("test")]
        public async Task<IActionResult> Test([FromQuery] APICredentials credentials)
        {
            if(await _dbContext.Database.CanConnectAsync())
                return new OkObjectResult(MessageUtilities.SCSSMSG_SECURITY_API_STATUS);
            else 
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_SECURITY_API_STATUS);
        }
    }
}