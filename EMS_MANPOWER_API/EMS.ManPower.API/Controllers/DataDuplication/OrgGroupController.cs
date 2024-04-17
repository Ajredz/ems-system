using EMS.Manpower.Core.DataDuplication;
using EMS.Manpower.Data.DataDuplication.OrgGroup;
using EMS.Manpower.Transfer.DataDuplication.OrgGroup;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Manpower.API.Controllers.DataDuplication
{
    [Route("manpower/data-duplication/[controller]")]
    [ApiController]
    public class OrgGroupController : ControllerBase
    {
        private readonly IOrgGroupService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public OrgGroupController(IOrgGroupService service)
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


        [HttpPost]
        [Route("sync")]
        public async Task<IActionResult> Sync([FromBody] List<OrgGroup> param)
        {
            return await _service.Sync(param).ConfigureAwait(true);
        }


        [HttpPost]
        [Route("sync-position")]
        public async Task<IActionResult> SyncPosition([FromBody] List<OrgGroupPosition> param)
        {
            return await _service.SyncPosition(param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-dropdown")]
        public async Task<IActionResult> GetDropDown([FromQuery] APICredentials credentials, [FromQuery] GetDropDownInput param)
        {
            return await _service.GetDropDown(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetByID(credentials, ID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-id-by-org-type-autocomplete")]
        public async Task<IActionResult> GetIDByOrgTypeAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetByOrgTypeAutoCompleteInput param)
        {
            return await _service.GetIDByOrgTypeAutoComplete(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-dropdown-exclude-by-org-type")]
        public async Task<IActionResult> GetDropDownExcludeByOrgType([FromQuery] APICredentials credentials, [FromQuery] GetDropDownExcludeByOrgTypeInput param)
        {
            return await _service.GetDropDownExcludeByOrgType(credentials, param).ConfigureAwait(true);
        }
    }
}