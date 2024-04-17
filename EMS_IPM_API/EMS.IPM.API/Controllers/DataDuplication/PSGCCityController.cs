using EMS.IPM.Core.DataDuplication;
using EMS.IPM.Data.DataDuplication.PSGCCity;
using EMS.IPM.Transfer.DataDuplication.PSGCCity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.API.Controllers.DataDuplication
{
    /// <summary>
    /// PSGCCity
    /// </summary>
    [Route("IPM/data-duplication/[controller]")]
    [ApiController]
    public class PSGCCityController : ControllerBase
    {
        private readonly IPSGCCityService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public PSGCCityController(IPSGCCityService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("sync")]
        public async Task<IActionResult> Sync([FromBody] List<PSGCCity> param)
        {
            return await _service.Sync(param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-dropdown")]
        public async Task<IActionResult> GetDropDown([FromQuery] APICredentials credentials, [FromQuery] GetDropDownInput param)
        {
            return await _service.GetDropDown(credentials, param).ConfigureAwait(true);
        }
    }
}