using EMS.Recruitment.Data.DBContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Recruitment.API.Controllers
{
    /// <summary>
    /// For Testing Connection
    /// </summary>
    [Route("Recruitment/[controller]")]
    [ApiController]
    public class TestConnectionController : Core.Shared.Utilities
    {
        private readonly RecruitmentContext _dbContext;

        public TestConnectionController(RecruitmentContext dbContext, IConfiguration iconfiguration) : base(dbContext, iconfiguration)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("test")]
        public async Task<IActionResult> Test([FromQuery] APICredentials credentials)
        {
            if(await _dbContext.Database.CanConnectAsync())
                return new OkObjectResult(MessageUtilities.SCSSMSG_RECRUITMENT_API_STATUS);
            else 
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_RECRUITMENT_API_STATUS);
        }
    }
}