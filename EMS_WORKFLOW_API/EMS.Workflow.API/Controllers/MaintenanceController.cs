using EMS.Workflow.Core.Workflow;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Workflow.API.Controllers
{
    /// <summary>
    /// Workflow to be assigned
    /// </summary>
    [Route("workflow/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly IWorkflowService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public MaintenanceController(IWorkflowService service)
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
        [Route("get-dropdown")]
        public async Task<IActionResult> GetDropDown([FromQuery] APICredentials credentials)
        {
            return await _service.GetDropDown().ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-id-by-autocomplete")]
        public async Task<IActionResult> GetIDByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetAutoCompleteInput param)
        {
            return await _service.GetIDByAutoComplete(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-id-workflow-step-by-autocomplete")]
        public async Task<IActionResult> GetIDWorkflowStepByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetWorkflowStepAutoCompleteInput param)
        {
            return await _service.GetIDWorkflowStepByAutoComplete(credentials, param).ConfigureAwait(true);
        }
        
        [HttpGet]
        [Route("get-code-workflow-step-by-autocomplete")]
        public async Task<IActionResult> GetCodeWorkflowStepByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetWorkflowStepAutoCompleteInput param)
        {
            return await _service.GetCodeWorkflowStepByAutoComplete(credentials, param).ConfigureAwait(true);
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

        [HttpGet]
        [Route("get-workflow-step-dropdown")]
        public async Task<IActionResult> GetWorkflowStepDropDown([FromQuery] APICredentials credentials, [FromQuery] string WorkflowCode)
        {
            return await _service.GetWorkflowStepDropDown(credentials, WorkflowCode).ConfigureAwait(true);
        }
        
        [HttpGet]
        [Route("get-workflow-step-by-workflow-code-and-code")]
        public async Task<IActionResult> GetWorkflowStepByWorkflowCodeAndCode([FromQuery] APICredentials credentials, [FromQuery] GetWorkflowStepByWorkflowIDAndCodeInput param)
        {
            return await _service.GetWorkflowStepByWorkflowCodeAndCode(credentials, param).ConfigureAwait(true);
        }
        
        [HttpGet]
        [Route("get-last-step-by-workflow-code")]
        public async Task<IActionResult> GetLastStepByWorkflowCode([FromQuery] APICredentials credentials, [FromQuery] string WorkflowCode)
        {
            return await _service.GetLastStepByWorkflowCode(credentials, WorkflowCode).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-workflow-step-by-workflow-code")]
        public async Task<IActionResult> GetWorkflowStepByWorkflowCode([FromQuery] APICredentials credentials, [FromQuery] string WorkflowCode)
        {
            return await _service.GetWorkflowStepByWorkflowCode(credentials, WorkflowCode).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-next-workflow-step")]
        public async Task<IActionResult> GetNextWorkflowStep([FromQuery] APICredentials credentials, [FromQuery] GetNextWorkflowStepInput param)
        {
            return await _service.GetNextWorkflowStep(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-workflow-step-by-role")]
        public async Task<IActionResult> GetWorkflowStepByRole([FromQuery] APICredentials credentials, [FromQuery] GetWorkflowStepByRoleInput param)
        {
            return await _service.GetWorkflowStepByRole(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-all-workflow-step")]
        public async Task<IActionResult> GetAllWorkflowStep([FromQuery] APICredentials credentials, [FromQuery] GetAllWorkflowStepInput param)
        {
            return await _service.GetAllWorkflowStep(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-roles-by-workflow-step-code")]
        public async Task<IActionResult> GetRolesByWorkflowStepCode([FromQuery] APICredentials credentials, [FromQuery] GetRolesByWorkflowStepCodeInput param)
        {
            return await _service.GetRolesByWorkflowStepCode(credentials, param).ConfigureAwait(true);
        }

    }
}