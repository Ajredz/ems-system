using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Recruitment.Core.Applicant;
using EMS.Recruitment.Transfer.Applicant;
using Microsoft.AspNetCore.Mvc;
using Utilities.API;

namespace EMS.Recruitment.API.Controllers
{
    /// <summary>
    /// Reference Maintenance
    /// </summary>
    [Route("Recruitment/[controller]")]
    [ApiController]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public ApplicantController(IApplicantService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-list")]
        public async Task<IActionResult> GetList([FromQuery] APICredentials credentials, [FromQuery] GetListInput param)
        {
            return await _service.GetList(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-applicant-picker-list")]
        public async Task<IActionResult> GetApplicantPickerList([FromQuery] APICredentials credentials, [FromQuery] GetApplicantPickerListInput param)
        {
            return await _service.GetApplicantPickerList(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-approval-list")]
        public async Task<IActionResult> GetApprovalList([FromQuery] APICredentials credentials, [FromQuery] GetListInput param)
        {
            return await _service.GetApprovalList(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Post([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.Post(credentials, param).ConfigureAwait(true);
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
        /// Get Applicant Name by ID
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-applicant-name-by-id")]
        public async Task<IActionResult> GetApplicantNameByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetApplicantNameByID(credentials, ID).ConfigureAwait(true);
		}
		
        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetByID(credentials, ID).ConfigureAwait(true);
        }
        
        [HttpPost]
        [Route("get-by-ids")]
        public async Task<IActionResult> GetByIDs([FromQuery] APICredentials credentials, [FromBody] List<int> IDs)
        {
            return await _service.GetByIDs(credentials, IDs).ConfigureAwait(true);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.Delete(credentials, ID).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Put([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.Put(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-history")]
        public async Task<IActionResult> GetHistory([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetHistory(credentials, ID).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-status")]
        public async Task<IActionResult> UpdateStatus([FromQuery] APICredentials credentials, [FromBody] ApproverResponse param)
        {
            return await _service.AddWorkflowTransaction(credentials, param).ConfigureAwait(true);
        }
        
        [HttpPut]
        [Route("update-mrf-transaction-id")]
        public async Task<IActionResult> UpdateMRFTransactionID([FromQuery] APICredentials credentials, [FromBody] UpdateMRFTransactionIDForm param)
        {
            return await _service.UpdateMRFTransactionID(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-current-workflow-step")]
        public async Task<IActionResult> UpdateCurrentWorkflowStep([FromQuery] APICredentials credentials, [FromBody] UpdateCurrentWorkflowStepInput param)
        {
            return await _service.UpdateCurrentWorkflowStep(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-attachment")]
        public async Task<IActionResult> GetAttachment([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetAttachment(credentials, ID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-attachment")]
        public async Task<IActionResult> PostAttachment([FromQuery]APICredentials credentials, [FromBody] ApplicantAttachmentForm param)
        {
            return await _service.PostAttachment(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("upload-insert")]
        public async Task<IActionResult> UploadInsert([FromQuery] APICredentials credentials, [FromBody] List<UploadFile> param)
        {
            return await _service.UploadInsert(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-employee-id")]
        public async Task<IActionResult> UpdateEmployeeID([FromQuery] APICredentials credentials, [FromBody] UpdateEmployeeIDInput param)
        {
            return await _service.UpdateEmployeeID(credentials, param).ConfigureAwait(true);
        }

        // GET THE LAST INSERTED DATA

        [HttpGet]
        [Route("get-last-applicant")]
        public async Task<IActionResult> GetLastApplicant(string FirstName, string LastName, string Birthday, string Email)
        {
            return await _service.GetLastApplicant(FirstName, LastName, Birthday, Email).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-applicant-legal-profile")]
        public async Task<IActionResult> GetApplicantLegalProfile(int ApplicantId)
        {
            return await _service.GetApplicantLegalProfile(ApplicantId).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-applicant-legal-profile")]
        public async Task<IActionResult> PostApplicantLegalProfile([FromQuery] APICredentials credentials, [FromBody] ApplicantLegalProfileInput param)
        {
            return await _service.PostApplicantLegalProfile(credentials, param).ConfigureAwait(true);
        }

    }
}