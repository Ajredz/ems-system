using EMS.Recruitment.Core.DataDuplication;
using EMS.Recruitment.Data.DataDuplication.PositionLevel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Recruitment.API.Controllers.DataDuplication
{
    /// <summary>
    /// Position Level
    /// </summary>
    [Route("recruitment/data-duplication/[controller]")]
    [ApiController]
    public class PositionLevelController : ControllerBase
    {
        private readonly IPositionLevelService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public PositionLevelController(IPositionLevelService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("sync")]
        public async Task<IActionResult> Sync([FromBody] List<PositionLevel> param)
        {
            return await _service.Sync(param).ConfigureAwait(true);
        }
    }
}