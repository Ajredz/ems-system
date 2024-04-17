using EMS.IPM.Core.DataDuplication;
using EMS.IPM.Data.DataDuplication.OrgGroup;
using EMS.IPM.Transfer.DataDuplication.OrgGroup;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.API.Controllers.DataDuplication
{
    [Route("IPM/data-duplication/[controller]")]
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
        [Route("get-by-id")]
        public async Task<IActionResult> GetByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetByID(credentials, ID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-dropdown")]
        public async Task<IActionResult> GetDropDown([FromQuery] APICredentials credentials, [FromQuery] GetDropDownInput param)
        {
            return await _service.GetDropDown(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-code-dropdown")]
        public async Task<IActionResult> GetCodeDropDown([FromQuery] APICredentials credentials, [FromQuery] GetDropDownInput param)
        {
            return await _service.GetCodeDropDown(credentials, param).ConfigureAwait(true);
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
        /// Get ID and description of records to be displayed for auto complete elements
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-filtered-id-by-autocomplete")]
        public async Task<IActionResult> GetFilteredIDByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetAutoCompleteInput param)
        {
            return await _service.GetFilteredIDByAutoComplete(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-region-autocomplete")]
        public async Task<IActionResult> GetRegionAutoComplete([FromQuery] APICredentials credentials, [FromQuery] Transfer.Shared.GetAutoCompleteInput param)
        {
            return await _service.GetRegionAutoComplete(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-branch-autocomplete")]
        public async Task<IActionResult> GetBranchAutoComplete([FromQuery] APICredentials credentials, [FromQuery] Transfer.Shared.GetAutoCompleteInput param)
        {
            return await _service.GetBranchAutoComplete(credentials, param).ConfigureAwait(true);
        }
    }
}