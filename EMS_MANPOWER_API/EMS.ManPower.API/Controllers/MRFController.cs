using EMS.Manpower.Core.MRF;
using EMS.Manpower.Transfer.MRF;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Manpower.API.Controllers
{
    [Route("manpower/[controller]")]
    [ApiController]
    public class MRFController : ControllerBase
    {
        private readonly IMRFService _service;

        public MRFController(IMRFService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("get-list")]
        public async Task<IActionResult> GetList([FromQuery]APICredentials credentials, [FromBody] GetListInput param)
        {
            return await _service.GetList(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetByID([FromQuery]APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetByID(credentials, ID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Post([FromQuery]APICredentials credentials, [FromBody] Form param)
        {
            return await _service.Post(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Put([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.Put(credentials, param).ConfigureAwait(true);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.Delete(credentials, ID).ConfigureAwait(true);
        }

        [HttpDelete]
        [Route("cancel")]
        public async Task<IActionResult> Cancel([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.Cancel(credentials, ID).ConfigureAwait(true);
        }

        [HttpDelete]
        [Route("remove-applicant")]
        public async Task<IActionResult> RemoveApplicant([FromQuery] APICredentials credentials, [FromQuery] RemoveApplicantInput param)
        {
            return await _service.RemoveApplicant(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-applicant")]
        public async Task<IActionResult> AddApplicant([FromQuery] APICredentials credentials, [FromBody] MRFPickApplicantForm param)
        {
            return await _service.AddApplicant(credentials, param).ConfigureAwait(true);
        }
        
        [HttpPut]
        [Route("update-for-hiring")]
        public async Task<IActionResult> UpdateForHiring([FromQuery] APICredentials credentials, [FromBody] UpdateForHiringApplicantInput param)
        {
            return await _service.UpdateForHiring(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-applicant-by-mrf-id")]
        public async Task<IActionResult> GetApplicantByMRFID([FromQuery] APICredentials credentials, [FromQuery] int MRFID)
        {
            return await _service.GetApplicantByMRFID(credentials, MRFID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("validate-existing-actual")]
        public async Task<IActionResult> ValidateMRFExistingActual([FromQuery] APICredentials credentials, [FromBody] ValidateMRFExistingActualInput param)
        {
            return await _service.ValidateMRFExistingActual(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-status")]
        public async Task<IActionResult> UpdateStatus([FromQuery] APICredentials credentials, [FromBody] UpdateStatusInput param)
        {
            return await _service.UpdateStatus(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-approval-history")]
        public async Task<IActionResult> GetApprovalHistory([FromQuery] APICredentials credentials, [FromQuery] MRFApprovalHistoryForm param)
        {
            return await _service.GetApprovalHistory(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-comments")]
        public async Task<IActionResult> PostComments([FromQuery]APICredentials credentials, [FromBody] MRFCommentsForm param)
        {
            return await _service.PostComments(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-comments")]
        public async Task<IActionResult> GetComments([FromQuery] APICredentials credentials, [FromQuery] int MRFID)
        {
            return await _service.GetComments(credentials, MRFID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-applicant-comments")]
        public async Task<IActionResult> PostApplicantComments([FromQuery]APICredentials credentials, [FromBody] MRFApplicantCommentsForm param)
        {
            return await _service.PostApplicantComments(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-applicant-comments")]
        public async Task<IActionResult> GetApplicantComments([FromQuery] APICredentials credentials, [FromQuery] int MRFID)
        {
            return await _service.GetApplicantComments(credentials, MRFID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("validate-applicant-is-tagged")]
        public async Task<IActionResult> ValidateApplicantIsTagged([FromQuery] APICredentials credentials, [FromQuery] int ApplicantID)
        {
            return await _service.ValidateApplicantIsTagged(credentials, ApplicantID).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("hr-cancel-mrf")]
        public async Task<IActionResult> HRCancelMRF([FromQuery] APICredentials credentials, [FromBody] MRFCancelForm param)
        {
            return await _service.HRCancelMRF(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-current-workflow-step")]
        public async Task<IActionResult> UpdateCurrentWorkflowStep([FromQuery] APICredentials credentials, [FromBody] UpdateCurrentWorkflowStepInput param)
        {
            return await _service.UpdateCurrentWorkflowStep(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-mrf-existing-applicant-list")]
        public async Task<IActionResult> GetMRFExistingApplicantList([FromQuery] APICredentials credentials, [FromQuery] GetMRFExistingApplicantListInput param)
        {
            return await _service.GetMRFExistingApplicantList(credentials, param).ConfigureAwait(true);
        }
        
        [HttpGet]
        [Route("get-mrf-id-dropdown-by-applicant-id")]
        public async Task<IActionResult> GetMRFIDDropdownByApplicantID([FromQuery] APICredentials credentials, [FromQuery] int ApplicantID)
        {
            return await _service.GetMRFIDDropdownByApplicantID(credentials, ApplicantID).ConfigureAwait(true);
        }
        
        [HttpGet]
        [Route("get-applicant-by-mrf-id-and-id")]
        public async Task<IActionResult> GetApplicantByMRFIDAndID([FromQuery] APICredentials credentials, [FromQuery] GetApplicantByMRFIDAndIDInput param)
        {
            return await _service.GetApplicantByMRFIDAndID(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-mrf-transaction-id")]
        public async Task<IActionResult> GetByMRFTransactionID([FromQuery] APICredentials credentials, [FromQuery] string MRFTransactionID)
        {
            return await _service.GetByMRFTransactionID(credentials, MRFTransactionID).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("revise")]
        public async Task<IActionResult> Revise([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.Revise(credentials, param).ConfigureAwait(true);
        }

        //API FOR GET ONLINE MRF
        [HttpGet]
        [Route("get-list-mrf-online")]
        public async Task<IActionResult> GetListMrfOnline()
        {
            return await _service.GetListMrfOnline().ConfigureAwait(true);
        }
        // GET THE LAST INSERTED DATA

        [HttpGet]
        [Route("get-last-mrf-applicant")]
        public async Task<IActionResult> GetLastApplicant(string FirstName,string LastName)
        {
            return await _service.GetLastApplicant(FirstName, LastName).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-kickout-question")]
        public async Task<IActionResult> GetKickoutQuestion([FromQuery] string PositionID)
        {
            return await _service.GetKickoutQuestion(PositionID).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("add-applicant-kickout-question")]
        public async Task<IActionResult> PostApplicantKickoutQuestion([FromQuery] APICredentials credentials, [FromBody] ApplicantKickoutQuestionInput param)
        {
            return await _service.PostApplicantKickoutQuestion(credentials, param).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("add-kickout-question")]
        public async Task<IActionResult> PostKickoutQuestion([FromQuery] APICredentials credentials, [FromBody] AddKickoutQuestionInput param)
        {
            return await _service.PostKickoutQuestion(credentials, param).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-kickout-question-list")]
        public async Task<IActionResult> GetKickoutQuestionList([FromQuery] APICredentials credentials, [FromBody] AddKickoutQuestionInput param)
        {
            return await _service.GetKickoutQuestionList(credentials, param).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-kickout-question-by-id")]
        public async Task<IActionResult> GetKickoutQuestionByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetKickoutQuestionByID(credentials, ID).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("edit-kickout-question")]
        public async Task<IActionResult> EditKickoutQuestion([FromQuery] APICredentials credentials, [FromBody] AddKickoutQuestionInput param)
        {
            return await _service.EditKickoutQuestion(credentials, param).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("add-kickout-question-to-mrf")]
        public async Task<IActionResult> AddKickoutQuestionToMRF([FromQuery] APICredentials credentials, [FromBody] AddKickoutQuestionToMRFInput param)
        {
            return await _service.AddKickoutQuestionToMRF(credentials, param).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-mrf-kickout-question-by-mrf-id")]
        public async Task<IActionResult> GetMRFKickoutQuestionByMRFID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetMRFKickoutQuestionByMRFID(credentials, ID).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-mrf-kickout-question-by-id")]
        public async Task<IActionResult> GetMRFKickoutQuestionByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetMRFKickoutQuestionByID(credentials, ID).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("edit-kickout-question-to-mrf")]
        public async Task<IActionResult> EditKickoutQuestionToMRF([FromQuery] APICredentials credentials, [FromBody] AddKickoutQuestionToMRFInput param)
        {
            return await _service.EditKickoutQuestionToMRF(credentials, param).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("remove-kickout-question-to-mrf")]
        public async Task<IActionResult> RemoveKickoutQuestionToMRF([FromQuery] APICredentials credentials, [FromBody] List<int> IDs)
        {
            return await _service.RemoveKickoutQuestionToMRF(credentials, IDs).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-kickout-question-autocomplete")]
        public async Task<IActionResult> GetKickoutQuestionAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetByKickoutQuestionAutoCompleteInput param)
        {
            return await _service.GetKickoutQuestionAutoComplete(credentials, param).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-mrf-auto-cancelled")]
        public async Task<IActionResult> GetMRFAutoCancelled([FromQuery] APICredentials credentials)
        {
            return await _service.GetMRFAutoCancelled(credentials).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-mrf-auto-cancelled-reminder")]
        public async Task<IActionResult> GetMRFAutoCancelledReminder([FromQuery] APICredentials credentials)
        {
            return await _service.GetMRFAutoCancelledReminder(credentials).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("mrf-change-status")]
        public async Task<IActionResult> MRFChangeStatus([FromQuery] APICredentials credentials, [FromBody] MRFChangeStatusInput param)
        {
            return await _service.MRFChangeStatus(credentials, param).ConfigureAwait(true);
        }
    }
}