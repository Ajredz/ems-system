   using EMS.Workflow.Core.Accountability;
using EMS.Workflow.Transfer.Accountability;
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
    public class AccountabilityController : ControllerBase
    {
        private readonly IAccountabilityService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public AccountabilityController(IAccountabilityService service)
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
        public async Task<IActionResult> GetList([FromQuery] APICredentials credentials, [FromQuery] GetAccountabilityListInput param)
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

        /// <summary>
        /// Get details by ID
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-details-by-accountability-id")]
        public async Task<IActionResult> GetDetailsByAccountabilityID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetDetailsByAccountabilityID(credentials, ID).ConfigureAwait(true);
        }

        /// <summary>
        /// Adding of new records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Post([FromQuery] APICredentials credentials, [FromBody] AccountabilityForm param)
        {
            return await _service.Post(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Updating of records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Put([FromQuery] APICredentials credentials, [FromBody] AccountabilityForm param)
        {
            return await _service.Put(credentials, param).ConfigureAwait(true);
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

        [HttpDelete]
        [Route("bulk-employee-accountability-delete")]
        public async Task<IActionResult> BulkEmployeeAccountabilityDelete([FromQuery] APICredentials credentials, [FromQuery] string ID)
        {
            return await _service.BulkEmployeeAccountabilityDelete(credentials, ID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-accountability-dropdown")]
        public async Task<IActionResult> GetAccountabilityDropdown([FromQuery] APICredentials credentials)
        {
            return await _service.GetAccountabilityDropdown(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-accountability-by-employee-id")]
        public async Task<IActionResult> GetEmployeeAccountabilityByEmployeeID([FromQuery] APICredentials credentials, [FromQuery] int EmployeeID)
        {
            return await _service.GetEmployeeAccountabilityByEmployeeID(credentials, EmployeeID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-accountability-details")]
        public async Task<IActionResult> GetAccountabilityDetails([FromQuery] APICredentials credentials, [FromQuery] int AccountabilityID)
        {
            return await _service.GetAccountabilityDetails(credentials, AccountabilityID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-employee-preloaded-accountability")]
        public async Task<IActionResult> AddEmployeePreLoadedAccountability([FromQuery] APICredentials credentials, [FromBody] AddEmployeePreLoadedAccountabilityInput param)
        {
            return await _service.AddEmployeePreLoadedAccountability(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-employee-accountability")]
        public async Task<IActionResult> AddEmployeeAccountability([FromQuery] APICredentials credentials, [FromBody] TagToEmployeeForm param)
        {
            return await _service.AddEmployeeAccountability(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-accountability-by-id")]
        public async Task<IActionResult> GetEmployeeAccountabilityByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetEmployeeAccountabilityByID(credentials, ID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-accountability-status-history")]
        public async Task<IActionResult> GetEmployeeAccountabilityStatusHistory([FromQuery] APICredentials credentials, [FromQuery] int EmployeeAccountabilityID)
        {
            return await _service.GetEmployeeAccountabilityStatusHistory(credentials, EmployeeAccountabilityID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-employee-accountability-status-history")]
        public async Task<IActionResult> AddEmployeeAccountabilityStatusHistory([FromQuery] APICredentials credentials, [FromBody] EmployeeAccountabilityForm param)
        {
            return await _service.AddEmployeeAccountabilityStatusHistory(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-employee-comments")]
        public async Task<IActionResult> PostEmployeeComments([FromQuery]APICredentials credentials, [FromBody] EmployeeAccountabilityCommentsForm param)
        {
            return await _service.PostEmployeeComments(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-comments")]
        public async Task<IActionResult> GetEmployeeComments([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetEmployeeComments(credentials, ID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-employee-attachment")]
        public async Task<IActionResult> PostEmployeeAttachment([FromQuery]APICredentials credentials, [FromBody] EmployeeAccountabilityAttachmentForm param)
        {
            return await _service.PostEmployeeAttachment(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-attachment")]
        public async Task<IActionResult> GetEmployeeAttachment([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetEmployeeAttachment(credentials, ID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-my-accountabilities-list")]
        public async Task<IActionResult> GetMyAccountabilitiesList([FromQuery] APICredentials credentials, [FromBody] GetMyAccountabilitiesListInput input)
        {
            return await _service.GetMyAccountabilitiesList(credentials, input).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("batch-accountability-add")]
        public async Task<IActionResult> BatchAccountabilityAdd([FromQuery]APICredentials credentials, [FromBody] BatchAccountabilityAddInput param)
        {
            return await _service.BatchAccountabilityAdd(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("upload-insert")]
        public async Task<IActionResult> UploadInsert([FromQuery] APICredentials credentials, [FromBody] List<AccountabilityUploadFile> param)
        {
            return await _service.UploadInsert(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-all-last-comment-by-employee-id")]
        public async Task<IActionResult> GetAllLastCommentByEmployeeId([FromQuery] APICredentials credentials, [FromQuery] int EmployeeId)
        {
            return await _service.GetAllLastCommentByEmployeeId(credentials, EmployeeId).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-all-employee-accountability")]
        public async Task<IActionResult> GetAllEmployeeAccountability([FromQuery] APICredentials credentials)
        {
            return await _service.GetAllEmployeeAccountability(credentials).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("change-status")]
        public async Task<IActionResult> PostChangeStatus([FromQuery] APICredentials credentials, [FromBody] ChangeStatusInput param)
        {
            return await _service.PostChangeStatus(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-employee-by-employee-accountability-ids")]
        public async Task<IActionResult> GetEmployeeByEmployeeAccountabilityIDs([FromQuery] APICredentials credentials, [FromBody] List<int> EmployeeAccountabilityID)
        {
            return await _service.GetEmployeeByEmployeeAccountabilityIDs(credentials, EmployeeAccountabilityID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-employee-accountability-status-percentage")]
        public async Task<IActionResult> GetEmployeeAccountabilityStatusPercentage([FromBody] GetEmployeeAccountabilityStatusPercentageInput param)
        {
            return await _service.GetEmployeeAccountabilityStatusPercentage(param).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("get-employee-accountability-exit-clearance")]
        public async Task<IActionResult> GetEmployeeAccountabilityExitClearance([FromBody] int ID)
        {
            return await _service.GetEmployeeAccountabilityExitClearance(ID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-employee-accountability-list")]
        public async Task<IActionResult> GetEmployeeAccountabilityList([FromBody] GetMyAccountabilitiesListInput param)
        {
            return await _service.GetEmployeeAccountabilityList(param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-accountability-dashboard")]
        public async Task<IActionResult> GetAccountabilityDashboard([FromBody] GetAccountabilityDashboardInput param)
        {
            return await _service.GetAccountabilityDashboard(param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-check-employee-cleared")]
        public async Task<IActionResult> GetCheckEmployeeCleared([FromBody] string EmployeeID)
        {
            return await _service.GetCheckEmployeeCleared(EmployeeID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-employee-cleared-list")]
        public async Task<IActionResult> GetEmployeeClearedList([FromBody] ClearedEmployeeListInput param)
        {
            return await _service.GetEmployeeClearedList(param).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("get-cleared-employee-by-id")]
        public async Task<IActionResult> GetClearedEmployeeByID([FromBody] int ID)
        {
            return await _service.GetClearedEmployeeByID(ID).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("add-cleared-employee-comments")]
        public async Task<IActionResult> PostClearedEmployeeComments([FromQuery] APICredentials credentials, [FromBody] PostClearedEmployeeCommentsInput param)
        {
            return await _service.PostClearedEmployeeComments(credentials,param).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("get-cleared-employee-comments")]
        public async Task<IActionResult> GetClearedEmployeeComments([FromBody] int ClearedEmployeeID)
        {
            return await _service.GetClearedEmployeeComments(ClearedEmployeeID).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("get-cleared-employee-status-history")]
        public async Task<IActionResult> GetClearedEmployeeStatusHistory([FromBody] int ClearedEmployeeID)
        {
            return await _service.GetClearedEmployeeStatusHistory(ClearedEmployeeID).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("get-employee-accountability")]
        public async Task<IActionResult> GetEmployeeAccountability([FromBody] int EmployeeID)
        {
            return await _service.GetEmployeeAccountability(EmployeeID).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("add-cleared-employee-computation")]
        public async Task<IActionResult> PostClearedEmployeeComputation([FromQuery] APICredentials credentials, [FromBody] PostClearedEmployeeComputationInput param)
        {
            return await _service.PostClearedEmployeeComputation(credentials, param).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("add-cleared-employee-change-status")]
        public async Task<IActionResult> PostClearedEmployeeChangeStatus([FromQuery] APICredentials credentials, [FromBody] PostClearedEmployeeStatusInput param)
        {
            return await _service.PostClearedEmployeeChangeStatus(credentials, param).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("get-cleared-employee-by-employee-id")]
        public async Task<IActionResult> GetClearedEmployeeByEmployeeID([FromQuery] APICredentials credentials, [FromBody] int EmployeeID)
        {
            return await _service.GetClearedEmployeeByEmployeeID(EmployeeID).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("add-cleared-employee-agreed")]
        public async Task<IActionResult> AddClearedEmployeeAgreed([FromQuery] APICredentials credentials, [FromBody] int ID)
        {
            return await _service.AddClearedEmployeeAgreed(ID).ConfigureAwait(true);
        }
    }
}