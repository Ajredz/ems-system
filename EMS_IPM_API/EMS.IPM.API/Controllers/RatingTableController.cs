using EMS.IPM.Core.RatingTable;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Utilities.API;

namespace EMS.IPM.API.Controllers
{
    /// <summary>
    /// RatingTable to be assigned on Organizational Groups
    /// </summary>
    [Route("IPM/[controller]")]
    [ApiController]
    public class RatingTableController : ControllerBase
    {
        private readonly IRatingTableService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public RatingTableController(IRatingTableService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] APICredentials credentials)
        {
            return await _service.GetAll(credentials).ConfigureAwait(true);
        }

    }
}