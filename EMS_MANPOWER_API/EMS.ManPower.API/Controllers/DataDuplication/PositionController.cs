using EMS.Manpower.Core.DataDuplication;
using EMS.Manpower.Data.DataDuplication.Position;
using EMS.Manpower.Transfer.DataDuplication.Position;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Manpower.API.Controllers.DataDuplication
{
    /// <summary>
    /// Position
    /// </summary>
    [Route("manpower/data-duplication/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public PositionController(IPositionService service)
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

        [HttpGet]
        [Route("get-dropdown-by-position-level")]
        public async Task<IActionResult> GetDropDownByPositionLevel([FromQuery] APICredentials credentials, [FromQuery] GetDropDownInput param)
        {
            return await _service.GetDropDownByPositionLevel(credentials, param).ConfigureAwait(true);
        }
         
        [HttpGet]
        [Route("get-dropdown-by-parent-position-id")]
        public async Task<IActionResult> GetDropDownByParentPositionID([FromQuery] APICredentials credentials, [FromQuery] GetDropDownByParentPositionIDInput param)
        {
            return await _service.GetDropDownByParentPositionID(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-dropdown-by-org-group")]
        public async Task<IActionResult> GetDropdownByOrgGroup([FromQuery] APICredentials credentials, [FromQuery] GetDropdownByOrgGroupInput param)
        {
            return await _service.GetDropdownByOrgGroup(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("sync")]
        public async Task<IActionResult> Sync([FromBody] List<Position> param)
        {
            return await _service.Sync(param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetByID(credentials, ID).ConfigureAwait(true);
        }

    }
}