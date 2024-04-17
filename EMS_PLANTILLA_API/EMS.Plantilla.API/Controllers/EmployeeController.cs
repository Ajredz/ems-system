using EMS.Plantilla.Core.Employee;
using EMS.Plantilla.Transfer.Employee;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Plantilla.API.Controllers
{
    /// <summary>
    /// Employee to be assigned on Organizational Groups
    /// </summary>
    [Route("plantilla/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-id-by-autocomplete")]
        public async Task<IActionResult> GetIDByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetAutoCompleteInput param)
        {
            return await _service.GetAutoComplete(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-user-id")]
        public async Task<IActionResult> GetByUserID([FromQuery] APICredentials credentials)
        {
            return await _service.GetByUserID(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-position-id-org-group-id")]
        public async Task<IActionResult> GetByPositionIDOrgGroupID([FromQuery] APICredentials credentials, [FromQuery] GetByPositionIDOrgGroupIDInput param)
        {
            return await _service.GetByPositionIDOrgGroupID(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-roving-by-position-id-org-group-id")]
        public async Task<IActionResult> GetRovingByPositionIDOrgGroupID([FromQuery] APICredentials credentials, [FromQuery] GetRovingByPositionIDOrgGroupIDInput param)
        {
            return await _service.GetRovingByPositionIDOrgGroupID(credentials, param).ConfigureAwait(true);
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

        [HttpGet]
        [Route("get-roving-by-employee-id")]
        public async Task<IActionResult> GetRovingByEmployeeID([FromQuery] APICredentials credentials, [FromQuery] int EmployeeID)
        {
            return await _service.GetRovingByEmployeeID(credentials, EmployeeID).ConfigureAwait(true);
        }

        /// <summary>
        /// Get records by ID
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetByID([FromQuery] APICredentials credentials, [FromQuery] GetByIDInput param)
        {
            return await _service.GetByID(credentials, param).ConfigureAwait(true);
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

        [HttpPost]
        [Route("get-by-user-ids")]
        public async Task<IActionResult> GetByUserIDs([FromQuery] APICredentials credentials, [FromBody] List<int> UserIDs)
        {
            return await _service.GetByUserIDs(credentials, UserIDs).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-family-by-employee-id")]
        public async Task<IActionResult> GetFamilyByEmployeeID([FromQuery] APICredentials credentials, [FromQuery] int EmployeeID)
        {
            return await _service.GetFamilyByEmployeeID(credentials, EmployeeID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-education-by-employee-id")]
        public async Task<IActionResult> GetEducationByEmployeeID([FromQuery] APICredentials credentials, [FromQuery] int EmployeeID)
        {
            return await _service.GetEducationByEmployeeID(credentials, EmployeeID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-working-history-by-employee-id")]
        public async Task<IActionResult> GetWorkingHistoryByEmployeeID([FromQuery] APICredentials credentials, [FromQuery] int EmployeeID)
        {
            return await _service.GetWorkingHistoryByEmployeeID(credentials, EmployeeID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employment-status-by-employee-id")]
        public async Task<IActionResult> GetEmploymentStatusByEmployeeID([FromQuery] APICredentials credentials, [FromQuery] int EmployeeID)
        {
            return await _service.GetEmploymentStatusByEmployeeID(credentials, EmployeeID).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-onboarding-current-workflow-step")]
        public async Task<IActionResult> UpdateOnboardingCurrentWorkflowStep([FromQuery] APICredentials credentials, [FromBody] UpdateOnboardingCurrentWorkflowStepInput param)
        {
            return await _service.UpdateOnboardingCurrentWorkflowStep(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-with-system-user-by-autocomplete")]
        public async Task<IActionResult> GetEmployeeWithSystemUserByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetAutoCompleteInput param)
        {
            return await _service.GetEmployeeWithSystemUserAutoComplete(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-by-codes")]
        public async Task<IActionResult> GetByCodes([FromQuery] APICredentials credentials, [FromBody] string CodesDelimited)
        {
            return await _service.GetByCodes(credentials, CodesDelimited).ConfigureAwait(true);
        }

        /// <summary>
        /// Auto Add of new employee
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("auto-add")]
        public async Task<IActionResult> AutoAdd([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.AutoAdd(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-system-user")]
        public async Task<IActionResult> UpdateSystemUser([FromQuery] APICredentials credentials, [FromBody] UpdateSystemUserInput param)
        {
            return await _service.UpdateSystemUser(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-etf-list")]
        public async Task<IActionResult> GetETFList([FromQuery] APICredentials credentials, [FromQuery] GetListInput param)
        {
            return await _service.GetETFList(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-compensation")]
        public async Task<IActionResult> UpdateCompensation([FromQuery] APICredentials credentials, [FromBody] EmployeeCompensationForm param)
        {
            return await _service.UpdateCompensation(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("upload-insert")]
        public async Task<IActionResult> UploadInsert([FromQuery] APICredentials credentials, [FromBody] List<UploadFile> param)
        {
            return await _service.UploadInsert(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("upload-insert-update-system-user")]
        public async Task<IActionResult> UploadInsertUpdateSystemUser([FromQuery] APICredentials credentials, [FromBody] List<UploadInsertUpdateSystemUserInput> param)
        {
            return await _service.UploadInsertUpdateSystemUser(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-by-username")]
        public async Task<IActionResult> GetEmployeeByUsername([FromQuery] APICredentials credentials, string Username)
        {
            return await _service.GetEmployeeByUsername(credentials, Username).ConfigureAwait(true);
        }


        [HttpPost]
        [Route("get-by-ids")]
        public async Task<IActionResult> GetByIDs([FromQuery] APICredentials credentials, [FromBody] List<int> IDs)
        {
            return await _service.GetByIDs(credentials, IDs).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-print-coe")]
        public async Task<IActionResult> GetPrintCOE([FromQuery] APICredentials credentials, int EmployeeID, int HREmployeeID)
        {
            return await _service.GetPrintCOE(credentials, EmployeeID, HREmployeeID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-old-employee-id-by-autocomplete")]
        public async Task<IActionResult> GetOldEmployeeIDByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetAutoCompleteInput param)
        {
            return await _service.GetOldEmployeeIDAutoComplete(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-old-employee-ids")]
        public async Task<IActionResult> GetByOldEmployeeIDs([FromQuery] APICredentials credentials, [FromQuery] string CodesDelimited)
        {
            return await _service.GetByOldEmployeeIDs(credentials, CodesDelimited).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-last-modified")]
        public async Task<IActionResult> GetLastModified([FromQuery] SharedUtilities.UNIT_OF_TIME unit, [FromQuery] int value)
        {
            return await _service.GetLastModified(unit, value).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-last-modified-roving")]
        public async Task<IActionResult> GetLastModifiedRoving([FromQuery] SharedUtilities.UNIT_OF_TIME unit, [FromQuery] int value)
        {
            return await _service.GetLastModifiedRoving(unit, value).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-employee-attachment")]
        public async Task<IActionResult> PostEmployeeAttachment([FromQuery] APICredentials credentials, [FromBody] EmployeeAttachmentForm param)
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
        [Route("add-employee-skills")]
        public async Task<IActionResult> PostEmployeeSkills([FromQuery] APICredentials credentials, [FromBody] EmployeeSkillsForm param)
        {
            return await _service.PostEmployeeSkills(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-employee-skills")]
        public async Task<IActionResult> PutEmployeeSkills([FromQuery] APICredentials credentials, [FromBody] EmployeeSkillsForm param)
        {
            return await _service.PutEmployeeSkills(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-skills-by-id")]
        public async Task<IActionResult> GetEmployeeSkillsById([FromQuery] APICredentials credentials, [FromQuery] int Id)
        {
            return await _service.GetEmployeeSkillsById(credentials, Id).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-skills-by-employee-id")]
        public async Task<IActionResult> GetEmployeeSkillsByEmployeeId([FromQuery] APICredentials credentials, [FromQuery] EmployeeSkillsFormInput input)
        {
            return await _service.GetEmployeeSkillsByEmployeeId(credentials, input).ConfigureAwait(true);
        }


        // LOGIN VIA EXTERNAL CLEARANCE FORM
        [HttpGet]
        [Route("external-employee-details")]
        public async Task<IActionResult> GetExternalEmployeeDetails([FromQuery] APICredentials credentials, [FromQuery] ExternalLogin param)
        {
            return await _service.GetExternalEmployeeDetails(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-corporate-email-list")]
        public async Task<IActionResult> GetCorporateEmailList([FromQuery] APICredentials credentials, [FromQuery] GetListCorporateEmailInput param)
        {
            return await _service.GetCorporateEmailList(credentials, param).ConfigureAwait(true);
        }
        [HttpPut]
        [Route("edit-employee-email")]
        public async Task<IActionResult> UpdateEmployeeEmail([FromBody] UpdateEmployeeEmailInput param)
        {
            return await _service.UpdateEmployeeEmail(param).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-employee-by-birthday")]
        public async Task<IActionResult> GetEmployeeByBirthday()
        {
            return await _service.GetEmployeeByBirthday().ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-employee-evaluation")]
        public async Task<IActionResult> GetEmployeeEvaluation()
        {
            return await _service.GetEmployeeEvaluation().ConfigureAwait(true);
        }
        [HttpPost]
        [Route("add-update-profile")]
        public async Task<IActionResult> PostUpdateProfile([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.PostUpdateProfile(credentials, param).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-email")]
        public async Task<IActionResult> GetEmail([FromQuery] GetEmailInput param)
        {
            return await _service.GetEmail(param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-id-descendant")]
        public async Task<IActionResult> GetEmployeeIDDescendant([FromQuery] int EmployeeID)
        {
            return await _service.GetEmployeeIDDescendant(EmployeeID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("convert-new-employee")]
        public async Task<IActionResult> PutConvertNewEmployee([FromQuery] APICredentials credentials, [FromBody] NewEmployeeForm param)
        {
            return await _service.PutConvertNewEmployee(credentials, param).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("convert-new-employees")]
        public async Task<IActionResult> PutConvertNewEmployees([FromQuery] APICredentials credentials, [FromBody] List<NewEmployeeForm> param)
        {
            return await _service.PutConvertNewEmployees(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("edit-employee-details")]
        public async Task<IActionResult> PutEmployeeDetails([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.PutEmployeeDetails(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("edit-draft-to-probationary")]
        public async Task<IActionResult> PutDraftToProbationary([FromQuery] APICredentials credentials, [FromBody] List<int> IDs)
        {
            return await _service.PutDraftToProbationary(credentials, IDs).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("get-employee-by-org-group")]
        public async Task<IActionResult> GetEmployeeByOrgGroup([FromBody] List<int> ID)
        {
            return await _service.GetEmployeeByOrgGroup(ID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-employee-by-position")]
        public async Task<IActionResult> GetEmployeeByPosition([FromBody] List<int> ID)
        {
            return await _service.GetEmployeeByPosition(ID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-employee-last-employment-status")]
        public async Task<IActionResult> GetEmployeeLastEmploymentStatus([FromBody] List<int> ID)
        {
            return await _service.GetEmployeeLastEmploymentStatus(ID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-employee-last-employment-status-by-date")]
        public async Task<IActionResult> GetEmployeeLastEmploymentStatusByDate([FromBody] GetEmployeeLastEmploymentStatusByDateInput param)
        {
            return await _service.GetEmployeeLastEmploymentStatusByDate(param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-employee-by-date-hired")]
        public async Task<IActionResult> GetEmployeeByDateHired([FromBody] GetEmployeeLastEmploymentStatusByDateInput param)
        {
            return await _service.GetEmployeeByDateHired(param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-employee-report")]
        public async Task<IActionResult> PostEmployeeReport([FromQuery] APICredentials credentials)
        {
            return await _service.PostEmployeeReport(credentials).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-employee-report-by-tdate")]
        public async Task<IActionResult> GetEmployeeReportByTDate([FromBody] string TDate)
        {
            return await _service.GetEmployeeReportByTDate(TDate).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-employee-report-org-by-tdate")]
        public async Task<IActionResult> GetEmployeeReportOrgByTDate([FromBody] string TDate)
        {
            return await _service.GetEmployeeReportOrgByTDate(TDate).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-employee-report-region-by-tdate")]
        public async Task<IActionResult> GetEmployeeReportRegionByTDate([FromBody] string TDate)
        {
            return await _service.GetEmployeeReportRegionByTDate(TDate).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-employee-if-exist")]
        public async Task<IActionResult> GetEmployeeIfExist([FromBody] GetEmployeeIfExistInput param)
        {
            return await _service.GetEmployeeIfExist(param).ConfigureAwait(true);
        }
    }
}