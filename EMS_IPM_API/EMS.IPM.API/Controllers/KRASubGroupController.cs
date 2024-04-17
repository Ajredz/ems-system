using EMS.IPM.Core.KRASubGroup;
using EMS.IPM.Transfer.KRASubGroup;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.API.Controllers
{
    /// <summary>
    /// Position Level
    /// </summary>
    [Route("ipm/[controller]")]
    [ApiController]
    public class KRASubGroupController : ControllerBase
    {
        private readonly IKRASubGroupService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public KRASubGroupController(IKRASubGroupService service)
        {
            _service = service;
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
        /// Return List of Position Level
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="KRAGroupID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-kra-group-sub-dropdown")]
        public async Task<IActionResult> GetKRASubGroupDropDown([FromQuery] APICredentials credentials, [FromQuery] int KRAGroupID, [FromQuery] int ID)
        {
            return await _service.GetKRASubGroupDropDown(credentials, KRAGroupID, ID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-autocomplete")]
        public async Task<IActionResult> GetIDByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] IPM.Transfer.Shared.GetAutoCompleteInput param)
        {
            return await _service.GetAutoComplete(credentials, param).ConfigureAwait(true);
        }
    }
}
