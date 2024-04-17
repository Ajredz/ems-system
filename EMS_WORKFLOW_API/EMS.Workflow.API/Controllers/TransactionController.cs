using EMS.Workflow.Core.Workflow;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Workflow.API.Controllers
{
    /// <summary>
    /// Workflow to be assigned
    /// </summary>
    [Route("workflow/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IWorkflowService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public TransactionController(IWorkflowService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-transaction-by-record-id")]
        public async Task<IActionResult> GetTransactionByRecordID([FromQuery] APICredentials credentials, [FromQuery] GetTransactionByRecordIDInput param)
        {
            return await _service.GetTransactionByRecordID(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Post([FromQuery] APICredentials credentials, [FromBody] AddWorkflowTransaction param)
        {
            return await _service.AddTransaction(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-last-status-update-by-record-ids")]
        public async Task<IActionResult> GetLastStatusUpdateByRecordIDs([FromQuery] APICredentials credentials, [FromBody] List<int> IDs)
        {
            return await _service.GetLastStatusUpdateByRecordIDs(credentials, IDs).ConfigureAwait(true);
        }
    }
}