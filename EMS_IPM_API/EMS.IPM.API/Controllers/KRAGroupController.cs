using EMS.IPM.Core.KRAGroup;
using EMS.IPM.Transfer.KRAGroup;
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
    public class KRAGroupController : ControllerBase
    {
        private readonly IKRAGroupService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public KRAGroupController(IKRAGroupService service)
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
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-kra-group-dropdown")]
        public async Task<IActionResult> GetKRAGroupDropDown([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetKRAGroupDropDown(credentials, ID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-autocomplete")]
        public async Task<IActionResult> GetIDByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] IPM.Transfer.Shared.GetAutoCompleteInput param)
        {
            return await _service.GetAutoComplete(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-all-kra-group")]
        public async Task<IActionResult> GetAllKRAGroup([FromQuery] APICredentials credentials)
        {
            return await _service.GetAllKRAGroup(credentials).ConfigureAwait(true);
        }
    }
}
