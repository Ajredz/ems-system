using EMS.Plantilla.Data.DBContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Plantilla.API.Controllers
{
    /// <summary>
    /// For Testing Connection
    /// </summary>
    [Route("Plantilla/[controller]")]
    [ApiController]
    public class TestConnectionController : Core.Shared.Utilities
    {
        private readonly PlantillaContext _dbContext;

        public TestConnectionController(PlantillaContext dbContext, IConfiguration iconfiguration) : base(dbContext, iconfiguration)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("test")]
        public async Task<IActionResult> Test([FromQuery] APICredentials credentials)
        {
            if(await _dbContext.Database.CanConnectAsync())
                return new OkObjectResult(MessageUtilities.SCSSMSG_PLANTILLA_API_STATUS);
            else 
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_PLANTILLA_API_STATUS);
        }
    }
}