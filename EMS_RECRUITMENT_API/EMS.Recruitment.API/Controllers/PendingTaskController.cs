using EMS.Recruitment.Core.RecruiterTask;
using EMS.Recruitment.Transfer.RecruiterTask;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Recruitment.API.Controllers
{
    /// <summary>
    /// Pending Task to be assigned
    /// </summary>
    [Route("Recruitment/[controller]")]
    [ApiController]
    public class PendingTaskController : ControllerBase
    {
        private readonly IRecruiterTaskService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public PendingTaskController(IRecruiterTaskService service)
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
        [Route("get-pending-list")]
        public async Task<IActionResult> GetPendingList([FromQuery] APICredentials credentials, [FromQuery] GetPendingListInput param)
        {
            return await _service.GetPendingList(credentials, param).ConfigureAwait(true);
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
            return await _service.SingleUpdate(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Batch updating of records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("batch-update")]
        public async Task<IActionResult> BatchUpdate([FromQuery] APICredentials credentials, [FromBody] BatchForm param)
        {
            return await _service.BatchUpdate(credentials, param).ConfigureAwait(true);
        }
    }
}