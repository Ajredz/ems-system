using EMS.Plantilla.Core.PositionLevel;
using EMS.Plantilla.Transfer.PositionLevel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Plantilla.API.Controllers
{
    /// <summary>
    /// Position Level
    /// </summary>
    [Route("plantilla/[controller]")]
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

        /// <summary>
        /// Return List of Position Level
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] APICredentials credentials)
        {
            return await _service.GetAll(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-dropdown-by-org-group-id")]
        public async Task<IActionResult> GetByOrgGroupID([FromQuery] APICredentials credentials, [FromQuery] GetByPositionLevelIDInput param)
        {
            return await _service.GetByOrgGroupID(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-last-modified")]
        public async Task<IActionResult> GetLastModified([FromQuery] SharedUtilities.UNIT_OF_TIME unit, [FromQuery] int value)
        {
            return await _service.GetLastModified(unit, value).ConfigureAwait(true);
        }
    }
}
