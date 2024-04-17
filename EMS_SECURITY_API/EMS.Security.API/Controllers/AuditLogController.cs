using EMS.Security.Core.AuditLog;
using EMS.Security.Transfer.AuditLog;
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
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogService _service;


        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public AuditLogController(IAuditLogService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-list")]
        public async Task<IActionResult> GetList([FromQuery] APICredentials credentials, [FromQuery] GetListInput param)
        {
            return await _service.GetList(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.Add(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Get ID and description of records to be displayed for auto complete elements
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-event-type-by-autocomplete")]
        public async Task<IActionResult> GetEventTypeByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetAutoCompleteInput param)
        {
            return await _service.GetEventTypeByAutoComplete(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Get ID and description of records to be displayed for auto complete elements
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-table-name-by-autocomplete")]
        public async Task<IActionResult> GetTableNameByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetAutoCompleteInput param)
        {
            return await _service.GetTableNameByAutoComplete(credentials, param).ConfigureAwait(true);
        }

    }
}