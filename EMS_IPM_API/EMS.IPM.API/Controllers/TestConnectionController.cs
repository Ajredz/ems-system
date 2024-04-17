using EMS.IPM.Data.DBContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.API.Controllers
{
    /// <summary>
    /// For Testing Connection
    /// </summary>
    [Route("IPM/[controller]")]
    [ApiController]
    public class TestConnectionController : Core.Shared.Utilities
    {
        private readonly IPMContext _dbContext;

        public TestConnectionController(IPMContext dbContext, IConfiguration iconfiguration) : base(dbContext, iconfiguration)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("test")]
        public async Task<IActionResult> Test([FromQuery] APICredentials credentials)
        {
            if(await _dbContext.Database.CanConnectAsync())
                return new OkObjectResult(MessageUtilities.SCSSMSG_IPM_API_STATUS);
            else 
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_IPM_API_STATUS);
        }
    }
}