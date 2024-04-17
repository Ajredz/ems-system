using EMS.Recruitment.Core.DataDuplication;
using EMS.Recruitment.Data.DataDuplication.OrgGroup;
using EMS.Recruitment.Transfer.DataDuplication.OrgGroup;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Recruitment.API.Controllers.DataDuplication
{
    [Route("recruitment/data-duplication/[controller]")]
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


        [HttpGet]
        [Route("get-dropdown")]
        public async Task<IActionResult> GetDropDown([FromQuery] APICredentials credentials, [FromQuery] GetDropDownInput param)
        {
            return await _service.GetDropDown(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-id-by-org-type-autocomplete")]
        public async Task<IActionResult> GetIDByOrgTypeAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetByOrgTypeAutoCompleteInput param)
        {
            return await _service.GetIDByOrgTypeAutoComplete(credentials, param).ConfigureAwait(true);
        }
    }
}