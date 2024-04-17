using EMS.Manpower.Data.DBContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Manpower.API.Controllers
{
    /// <summary>
    /// For Testing Connection
    /// </summary>
    [Route("Manpower/[controller]")]
    [ApiController]
    public class TestConnectionController : Core.Shared.Utilities
    {
        private readonly ManpowerContext _dbContext;

        public TestConnectionController(ManpowerContext dbContext, IConfiguration iconfiguration) : base(dbContext, iconfiguration)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("test")]
        public async Task<IActionResult> Test([FromQuery] APICredentials credentials)
        {
            if(await _dbContext.Database.CanConnectAsync())
                return new OkObjectResult(MessageUtilities.SCSSMSG_MANPOWER_API_STATUS);
            else 
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_MANPOWER_API_STATUS);
        }
    }
}