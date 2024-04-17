using EMS.Manpower.Core.DataDuplication;
using EMS.Manpower.Data.DataDuplication.PositionLevel;
using EMS.Manpower.Transfer.DataDuplication.PositionLevel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Manpower.API.Controllers.DataDuplication
{
    /// <summary>
    /// Position Level
    /// </summary>
    [Route("manpower/data-duplication/[controller]")]
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

        /// <summary>
        /// Get ID and description of records to be displayed for auto complete elements
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-id-by-autocomplete")]
        public async Task<IActionResult> GetIDByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetAutoCompleteInput param)
        {
            return await _service.GetIDByAutoComplete(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Return List of Position Level
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-position-level-dropdown")]
        public async Task<IActionResult> GetPositionLevelDropDown([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetPositionLevelDropDown(credentials, ID).ConfigureAwait(true);
        }


        [HttpPost]
        [Route("sync")]
        public async Task<IActionResult> Sync([FromBody] List<PositionLevel> param)
        {
            return await _service.Sync(param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-dropdown-by-org-group-id")]
        public async Task<IActionResult> GetByOrgGroupID([FromQuery] APICredentials credentials, [FromQuery] GetByOrgGroupIDInput param)
        {
            return await _service.GetByOrgGroupID(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetByID(credentials, ID).ConfigureAwait(true);
        }
    }
}