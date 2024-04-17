using EMS.Plantilla.Core.Position;
using EMS.Plantilla.Transfer.Position;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Plantilla.API.Controllers
{
    /// <summary>
    /// Position to be assigned on Organizational Groups
    /// </summary>
    [Route("plantilla/[controller]")]
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
        /// Get records to be displayed on JQGrid
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-list")]
        public async Task<IActionResult> GetList([FromQuery] APICredentials credentials, [FromQuery] GetListInput param)
        {
            return await _service.GetList(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Adding of new records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Post([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.Post(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Updating of records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Put([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.Put(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Deleting of records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.Delete(credentials, ID).ConfigureAwait(true);
        }

        /// <summary>
        /// Get records by ID
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetByID([FromQuery] APICredentials credentials, [FromQuery] GetByIDInput param)
        {
            return await _service.GetByID(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Get records to be displayed on dropdown elements
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-dropdown")]
        public async Task<IActionResult> GetDropDown([FromQuery] APICredentials credentials)
        {
            return await _service.GetDropDown(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-dropdown-by-position-level")]
        public async Task<IActionResult> GetDropDownByPositionLevel([FromQuery] APICredentials credentials, [FromQuery] int PositionLevelID)
        {
            return await _service.GetDropDownByPositionLevel(credentials, PositionLevelID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] APICredentials credentials)
        {
            return await _service.GetAll(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-dropdown-detailed-by-position-level")]
        public async Task<IActionResult> GetDropDownDetailedByPositionLevel([FromQuery] APICredentials credentials, [FromQuery] int PositionLevelID)
        {
            return await _service.GetDropDownDetailedByPositionLevel(credentials, PositionLevelID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-last-modified")]
        public async Task<IActionResult> GetLastModified([FromQuery] SharedUtilities.UNIT_OF_TIME unit, [FromQuery] int value)
        {
            return await _service.GetLastModified(unit, value).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-id-by-autocomplete")]
        public async Task<IActionResult> GetIDByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetAutoCompleteInput param)
        {
            return await _service.GetIDByAutoComplete(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-dropdown-with-count-by-org-group")]
        public async Task<IActionResult> GetDropdownWithCountByOrgGroup([FromQuery] APICredentials credentials, [FromQuery] GetDropdownByOrgGroupInput param)
        {
            return await _service.GetDropdownWithCountByOrgGroup(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-code-dropdown")]
        public async Task<IActionResult> GetCodeDropDown([FromQuery] APICredentials credentials)
        {
            return await _service.GetCodeDropDown(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-position-with-level-by-autocomplete")]
        public async Task<IActionResult> GetPositionWithLevelByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetAutoCompleteInput param)
        {
            return await _service.GetPositionWithLevelByAutoComplete(credentials, param).ConfigureAwait(true);
        }
    }
}