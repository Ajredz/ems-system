using EMS.Manpower.Core.DataDuplication;
using EMS.Manpower.Data.DataDuplication.SystemRole;
using EMS.Manpower.Transfer.DataDuplication.SystemRole;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Manpower.API.Controllers.DataDuplication
{
    /// <summary>
    /// Roles used inside that system with specific set of functions and to be assigned into user accounts
    /// </summary>
    [Route("manpower/data-duplication/[controller]")]
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

        [HttpPost]
        [Route("sync")]
        public async Task<IActionResult> Sync([FromBody] List<SystemRole> param)
        {
            return await _service.Sync(param).ConfigureAwait(true);
        }

    }
}