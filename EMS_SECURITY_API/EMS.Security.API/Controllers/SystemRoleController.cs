using EMS.Security.Core.SystemRole;
using EMS.Security.Transfer.SystemRole;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Security.API.Controllers
{
    /// <summary>
    /// Roles used inside that system with specific set of functions and to be assigned into user accounts
    /// </summary>
    [Route("security/[controller]")]
    [ApiController]
    public class SystemRoleController : ControllerBase
    {
        private readonly ISystemRoleService _service;


        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public SystemRoleController(ISystemRoleService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get ID and description of records to be displayed for auto complete elements
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-id-by-autocomplete")]
        public async Task<IActionResult> GetIDByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetAutoCompleteInput param)
        {
            return await _service.GetIDByAutoComplete(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Get records to be displayed on dropdown
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-dropdown")]
        public async Task<IActionResult> GetDropDown([FromQuery] APICredentials credentials)
        {
            return await _service.GetDropDown(credentials).ConfigureAwait(true);
        }

        /// <summary>
        /// Get roles by user id
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-by-user-id")]
        public async Task<IActionResult> GetByUserID([FromQuery] APICredentials credentials)
        {
            return await _service.GetByUserID(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-last-modified")]
        public async Task<IActionResult> GetLastModified([FromQuery] SharedUtilities.UNIT_OF_TIME unit, [FromQuery] int value)
        {
            return await _service.GetLastModified(unit, value).ConfigureAwait(true);
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
        /// Get records by ID
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetByID(credentials, ID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-system-user-role-dropdown-by-role-id")]
        public async Task<IActionResult> GetSystemUserRoleDropDownByRoleID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetSystemUserRoleDropDownByRoleID(credentials, ID).ConfigureAwait(true);
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

        [HttpGet]
        [Route("get-role-page")]
        public async Task<IActionResult> GetRolePage([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetRolePage(credentials, ID).ConfigureAwait(true);
        }

    }
}