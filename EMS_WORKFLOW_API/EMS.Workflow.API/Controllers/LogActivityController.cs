using EMS.Workflow.Core.LogActivity;
using EMS.Workflow.Transfer.LogActivity;
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
    public class LogActivityController : ControllerBase
    {
        private readonly ILogActivityService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public LogActivityController(ILogActivityService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-applicant-log-activity-by-applicant-id")]
        public async Task<IActionResult> GetApplicantLogActivityByApplicantID([FromQuery] APICredentials credentials, [FromQuery] int ApplicantID)
        {
            return await _service.GetApplicantLogActivityByApplicantID(credentials, ApplicantID).ConfigureAwait(true);
        }
        
        [HttpGet]
        [Route("get-employee-log-activity-by-employee-id")]
        public async Task<IActionResult> GetEmployeeLogActivityByEmployeeID([FromQuery] APICredentials credentials, [FromQuery] int EmployeeID)
        {
            return await _service.GetEmployeeLogActivityByEmployeeID(credentials, EmployeeID).ConfigureAwait(true);
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
        [Route("get-applicant-log-activity-status_history")]
        public async Task<IActionResult> GetApplicantLogActivityStatusHistory([FromQuery] APICredentials credentials, [FromQuery] int ApplicantLogActivityID)
        {
            return await _service.GetApplicantLogActivityStatusHistory(credentials, ApplicantLogActivityID).ConfigureAwait(true);
        }
        
        [HttpGet]
        [Route("get-employee-log-activity-status_history")]
        public async Task<IActionResult> GetEmployeeLogActivityStatusHistory([FromQuery] APICredentials credentials, [FromQuery] int EmployeeLogActivityID)
        {
            return await _service.GetEmployeeLogActivityStatusHistory(credentials, EmployeeLogActivityID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-applicant-activity")]
        public async Task<IActionResult> AddApplicantActivity([FromQuery] APICredentials credentials, [FromBody] TagToApplicantForm param)
        {
            return await _service.AddApplicantActivity(credentials, param).ConfigureAwait(true);
        }
        
        [HttpPost]
        [Route("add-employee-activity")]
        public async Task<IActionResult> AddEmployeeActivity([FromQuery] APICredentials credentials, [FromBody] TagToEmployeeForm param)
        {
            return await _service.AddEmployeeActivity(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-log-activity-dropdown-by-module-and-type")]
        public async Task<IActionResult> GetLogActivityDropdownByModuleAndType([FromQuery] APICredentials credentials, [FromQuery] GetLogActivityByModuleTypeInput param)
        {
            return await _service.GetLogActivityDropdownByModuleAndType(credentials, param).ConfigureAwait(true);
        }
        
        [HttpGet]
        [Route("get-log-activity-by-id")]
        public async Task<IActionResult> GetLogActivityByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetLogActivityByID(credentials, ID).ConfigureAwait(true);
        }
        
        [HttpGet]
        [Route("get-applicant-log-activity-by-id")]
        public async Task<IActionResult> GetApplicantLogActivityByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetApplicantLogActivityByID(credentials, ID).ConfigureAwait(true);
        }
        
        [HttpGet]
        [Route("get-employee-log-activity-by-id")]
        public async Task<IActionResult> GetEmployeeLogActivityByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetEmployeeLogActivityByID(credentials, ID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-applicant-activity-status-history")]
        public async Task<IActionResult> AddApplicantActivityStatusHistory([FromQuery] APICredentials credentials, [FromBody] ApplicantLogActivityForm param)
        {
            return await _service.AddApplicantActivityStatusHistory(credentials, param).ConfigureAwait(true);
        }
        
        [HttpPost]
        [Route("add-employee-activity-status-history")]
        public async Task<IActionResult> AddEmployeeActivityStatusHistory([FromQuery] APICredentials credentials, [FromBody] EmployeeLogActivityForm param)
        {
            return await _service.AddEmployeeActivityStatusHistory(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-assigned-activities-list")]
        public async Task<IActionResult> GetAssignedActivitiesList([FromQuery] APICredentials credentials, [FromQuery] GetAssignedActivitiesListInput input)
        {
            return await _service.GetAssignedActivitiesList(credentials, input).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-applicant-comments")]
        public async Task<IActionResult> PostApplicantComments([FromQuery]APICredentials credentials, [FromBody] ApplicantLogActivityCommentsForm param)
        {
            return await _service.PostApplicantComments(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-applicant-comments")]
        public async Task<IActionResult> GetApplicantComments([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetApplicantComments(credentials, ID).ConfigureAwait(true);
        }
        
        [HttpPost]
        [Route("add-applicant-attachment")]
        public async Task<IActionResult> PostApplicantAttachment([FromQuery]APICredentials credentials, [FromBody] ApplicantLogActivityAttachmentForm param)
        {
            return await _service.PostApplicantAttachment(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-applicant-attachment")]
        public async Task<IActionResult> GetApplicantAttachment([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetApplicantAttachment(credentials, ID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-employee-comments")]
        public async Task<IActionResult> PostEmployeeComments([FromQuery]APICredentials credentials, [FromBody] EmployeeLogActivityCommentsForm param)
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
        public async Task<IActionResult> PostEmployeeAttachment([FromQuery]APICredentials credentials, [FromBody] EmployeeLogActivityAttachmentForm param)
        {
            return await _service.PostEmployeeAttachment(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-attachment")]
        public async Task<IActionResult> GetEmployeeAttachment([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetEmployeeAttachment(credentials, ID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-log-activity-preloaded-dropdown")]
        public async Task<IActionResult> GetLogActivityPreloadedDropdown([FromQuery] APICredentials credentials)
        {
            return await _service.GetLogActivityPreloadedDropdown(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-log-activity-by-preloaded-id")]
        public async Task<IActionResult> GetLogActivityByPreloadedID([FromQuery] APICredentials credentials, [FromQuery] int LogActivityPreloadedID)
        {
            return await _service.GetLogActivityByPreloadedID(credentials, LogActivityPreloadedID).ConfigureAwait(true);
        }
        
        [HttpPost]
        [Route("add-applicant-preloaded-activities")]
        public async Task<IActionResult> AddApplicantPreLoadedActivities([FromQuery] APICredentials credentials, [FromBody] AddApplicantPreLoadedActivitiesInput param)
        {
            return await _service.AddApplicantPreLoadedActivities(credentials, param).ConfigureAwait(true);
        }
        
        [HttpPost]
        [Route("add-employee-preloaded-activities")]
        public async Task<IActionResult> AddEmployeePreLoadedActivities([FromQuery] APICredentials credentials, [FromBody] AddEmployeePreLoadedActivitiesInput param)
        {
            return await _service.AddEmployeePreLoadedActivities(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-log-activity-sub-type")]
        public async Task<IActionResult> GetLogActivitySubType([FromQuery] APICredentials credentials)
        {
            return await _service.GetLogActivitySubType(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-log-activity-preloaded-list")]
        public async Task<IActionResult> GetLogActivityPreloadedList([FromQuery] APICredentials credentials, [FromQuery] GetPreLoadedListInput param)
        {
            return await _service.GetLogActivityPreloadedList(credentials, param).ConfigureAwait(true);
        }
        
        [HttpGet]
        [Route("get-log-activity-preloaded-by-id")]
        public async Task<IActionResult> GetLogActivityPreloadedByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetLogActivityPreloadedByID(credentials, ID).ConfigureAwait(true);
        }

        /// <summary>
        /// Adding of new records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-log-activity-preloaded")]
        public async Task<IActionResult> AddLogActivityPreloaded([FromQuery] APICredentials credentials, [FromBody] LogActivityPreloadedForm param)
        {
            return await _service.AddLogActivityPreloaded(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Deleting of records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-log-activity-preloaded")]
        public async Task<IActionResult> DeleteLogActivityPreloaded([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.DeleteLogActivityPreloaded(credentials, ID).ConfigureAwait(true);
        }

        /// <summary>
        /// Updating of records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("edit-log-activity-preloaded")]
        public async Task<IActionResult> EditLogActivityPreloaded([FromQuery] APICredentials credentials, [FromBody] LogActivityPreloadedForm param)
        {
            return await _service.EditLogActivityPreloaded(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-preloaded-items-by-id")]
        public async Task<IActionResult> GetPreloadedItemsByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetPreloadedItemsByID(credentials, ID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-applicant-log-activity-pending-email")]
        public async Task<IActionResult> GetApplicantLogActivityPendingEmail([FromQuery] APICredentials credentials)
        {
            return await _service.GetApplicantLogActivityPendingEmail(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-log-activity-pending-email")]
        public async Task<IActionResult> GetEmployeeLogActivityPendingEmail([FromQuery] APICredentials credentials)
        {
            return await _service.GetEmployeeLogActivityPendingEmail(credentials).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-applicant-log-activity-pending-email")]
        public async Task<IActionResult> UpdateApplicantLogActivityPendingEmail([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.UpdateApplicantLogActivityPendingEmail(credentials, ID).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-employee-log-activity-pending-email")]
        public async Task<IActionResult> UpdateEmployeeLogActivityPendingEmail([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.UpdateEmployeeLogActivityPendingEmail(credentials, ID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-checklist-list")]
        public async Task<IActionResult> GetChecklistList([FromQuery] APICredentials credentials, [FromQuery] GetChecklistListInput input)
        {
            return await _service.GetChecklistList(credentials, input).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-employee-log-activity-assigned-user")]
        public async Task<IActionResult> UpdateEmployeeLogActivityAssignedUser([FromQuery] APICredentials credentials, [FromBody] UpdateEmployeeLogActivityAssignedUserForm param)
        {
            return await _service.UpdateEmployeeLogActivityAssignedUser(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-applicant-log-activity-assigned-user")]
        public async Task<IActionResult> UpdateApplicantLogActivityAssignedUser([FromQuery] APICredentials credentials, [FromBody] UpdateApplicantLogActivityAssignedUserForm param)
        {
            return await _service.UpdateApplicantLogActivityAssignedUser(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-applicant-log-activity-list")]
        public async Task<IActionResult> GetApplicantLogActivityList([FromQuery] APICredentials credentials, [FromQuery] GetApplicantLogActivityListInput input)
        {
            return await _service.GetApplicantLogActivityList(credentials, input).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-log-activity-list")]
        public async Task<IActionResult> GetEmployeeLogActivityList([FromQuery] APICredentials credentials, [FromQuery] GetChecklistListInput input)
        {
            return await _service.GetEmployeeLogActivityList(credentials, input).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("batch-update-log-activity")]
        public async Task<IActionResult> BatchUpdateLogActivity([FromQuery] APICredentials credentials, [FromBody] BatchTaskForm param)
        {
            return await _service.BatchUpdateLogActivity(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("upload-log-activity-insert")]
        public async Task<IActionResult> UploadLogActivityInsert([FromQuery] APICredentials credentials, [FromBody] List<UploadLogActivityFile> param)
        {
            return await _service.UploadLogActivityInsert(credentials, param).ConfigureAwait(true);
        }
    }
}