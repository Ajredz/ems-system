using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using EMS.Workflow.Transfer;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.Workflow.Transfer.Workflow;
using EMS.Workflow.Transfer.EmailServerCredential;
using EMS.Workflow.Transfer.Accountability;

namespace EMS.FrontEnd.Pages.Plantilla.Employee
{
    public class UpdateAccountabilityModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.Accountability.EmployeeAccountabilityForm EmployeeAccountabilityForm { get; set; }

        [BindProperty]
        public EMS.Workflow.Transfer.Accountability.EmployeeAccountabilityCommentsForm CommentsForm { get; set; }
        
        [BindProperty]
        public EMS.Workflow.Transfer.Accountability.EmployeeAccountabilityAttachmentForm AttachmentForm { get; set; }
        
        public UpdateAccountabilityModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet(int EmployeeAccountabilityID)
        {
            if (_globalCurrentUser != null)
            {
                ViewData["HasEdit"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARANCE/EDITCLEARANCE")).Count() > 0 ? "true" : "false";
                ViewData["HasChangeStatus"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARANCE/CHANGESTATUS")).Count() > 0 ? "true" : "false";
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/ALLACCOUNTABILITIES/DELETE")).Count() > 0 ? "true" : "false";

                EmployeeAccountabilityForm = 
                    await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeAccountabilityByID(EmployeeAccountabilityID);

                // Get OrgGroup description by OrgGroup IDs
                List<EMS.Plantilla.Transfer.OrgGroup.Form> orgGroup =
                    (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                    .GetOrgGroupByIDs(new List<int> { EmployeeAccountabilityForm.OrgGroupID })).Item1;

                if (orgGroup.Count > 0)
                {
                    EmployeeAccountabilityForm.OrgGroupDescription = string.Concat(orgGroup.First().Code, " - ", orgGroup.First().Description);
                }

                if (EmployeeAccountabilityForm.PositionID != 0)
                { 
                    var position =
                           (await new Common_Position(_iconfiguration, _globalCurrentUser, _env)
                           .GetPosition(EmployeeAccountabilityForm.PositionID));
                    EmployeeAccountabilityForm.PositionDescription = string.Concat(position.Code, " - ", position.Title);
                }
                if (EmployeeAccountabilityForm.ApproverEmployeeID != 0)
                {
                    var employees =
                           (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                           .GetEmployee(EmployeeAccountabilityForm.ApproverEmployeeID));

                    EmployeeAccountabilityForm.ApproverEmployeeName = string.Concat(employees.PersonalInformation.LastName, ", ",
                        employees.PersonalInformation.FirstName," ",employees.PersonalInformation.MiddleName," (",employees.Code,")");
                }

                var accountabilityType = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                    .GetReferenceValueByRefCode(ReferenceCodes_Workflow.ACCOUNTABILITY_TYPE.ToString());

                ViewData["TypeSelectList"] =
                 accountabilityType.Select(x => new SelectListItem
                 {
                     Value = x.Value,
                     Text = x.Description,
                     Selected = x.Value.Equals(EmployeeAccountabilityForm.Type)
                 }).ToList();

                //ViewData["CurrentStatusDescription"] =
                //(await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                //    .GetReferenceValueByRefCodeAndValue(ReferenceCodes_Workflow.ACCNTABILITY_STATUS.ToString()
                //    , EmployeeAccountabilityForm.CurrentStatus)).Description;

                //var status = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                //    .GetReferenceValueByRefCode(ReferenceCodes_Workflow.ACCNTABILITY_STATUS.ToString());

                //ViewData["StatusSelectList"] =
                //status.Select(x => new SelectListItem
                //{
                //    Value = x.Value,
                //    Text = x.Description,
                //    Selected = x.Value.Equals(EmployeeAccountabilityForm.CurrentStatus)
                //}).ToList();

                var roles = (await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                       .GetSystemUserRoleDropDownByUserID(_globalCurrentUser.UserID)).ToList();

                var status = (await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetNextWorkflowStep(new GetNextWorkflowStepInput
                    {
                        WorkflowCode = "ACCOUNTABILITY",
                        CurrentStepCode = EmployeeAccountabilityForm.Status,
                        RoleIDDelimited = string.Join(",", roles.Select(x => x.Value).ToArray())
                    })).ToList();

                ViewData["StatusSelectList"] =
                status.OrderByDescending(x => x.Order).Select(x => new SelectListItem
                {
                    Value = x.Code,
                    Text = x.Description,
                    Selected = x.Code.Equals(EmployeeAccountabilityForm.Status),
                    Disabled = x.Code.Equals(EmployeeAccountabilityForm.Status) ? true : false
                }).ToList();

                var description = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetWorkflowStepByWorkflowIDAndCode(new GetWorkflowStepByWorkflowIDAndCodeInput
                    {
                        WorkflowCode = "ACCOUNTABILITY",
                        Code = EmployeeAccountabilityForm.Status
                    });

                ViewData["CurrentStatusDescription"] = description.Description;
				
				var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
				.GetEmployee(EmployeeAccountabilityForm.EmployeeID);
                ViewData["EmployeeName"] =
                    string.Concat(employee.PersonalInformation.LastName, ", "
                    , employee.PersonalInformation.FirstName, " ", string.IsNullOrEmpty(employee.PersonalInformation.MiddleName) ?
                    "" : employee.PersonalInformation.MiddleName);


            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            /*var Id = EmployeeAccountabilityForm.ID;
            List<long> IDs = new List<long>() { EmployeeAccountabilityForm.ID };
            var NewStatus = EmployeeAccountabilityForm.Status;
            var OrgId = EmployeeAccountabilityForm.OrgGroupID;
            var EmpId = EmployeeAccountabilityForm.EmployeeID;
            var Remarks = EmployeeAccountabilityForm.Remarks;
            var AccountabilityWorkflow = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(4);
            var SendEmailToRequestor = AccountabilityWorkflow.WorkflowStepList.Where(y => y.StepCode.Equals(NewStatus)).Select(x => x.SendEmailToRequester).FirstOrDefault();
            var SendEmailToApprover = AccountabilityWorkflow.WorkflowStepList.Where(y => y.StepCode.Equals(NewStatus)).Select(x => x.SendEmailToApprover).FirstOrDefault();*/

            EmployeeAccountabilityForm.CreatedBy = _globalCurrentUser.UserID;
            
            var URL = string.Concat(_workflowBaseURL,
                    _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("AddEmployeeAccountabilityStatusHistory").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(EmployeeAccountabilityForm, URL);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                .AddAuditLog(new Security.Transfer.AuditLog.Form
                {
                    EventType = Common_AuditLog.EventType.EDIT.ToString(),
                    TableName = "EmployeeAccountabilityStatusHistory",
                    TableID = EmployeeAccountabilityForm.ID,
                    Remarks = string.Concat(EmployeeAccountabilityForm.ID, " Employee Accountability Status Changed to ", EmployeeAccountabilityForm.Status),
                    IsSuccess = true,
                    CreatedBy = _globalCurrentUser.UserID
                });
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetComments(int ID)
        {
            _resultView.Result = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeComments(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
        
        public async Task<JsonResult> OnPostSaveComments()
        {
            CommentsForm.CreatedBy = _globalCurrentUser.UserID;
            var (IsSuccess, Message) = 
                await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env).SaveEmployeeComments(CommentsForm);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "EmployeeAccountabilityComments",
                        TableID = EmployeeAccountabilityForm.ID,
                        Remarks = string.Concat(EmployeeAccountabilityForm.ID, " Comments added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }
        
        public async Task<JsonResult> OnGetAttachment(int ID)
        {
            _resultView.Result = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeAttachment(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostSaveAttachment()
        {
            var (IsSuccess, Message) = 
                await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .SaveEmployeeAttachment(AttachmentForm);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "EmployeeAccountabilityAttachment",
                        TableID = EmployeeAccountabilityForm.ID,
                        Remarks = string.Concat(EmployeeAccountabilityForm.ID, " Attachment added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }

    }
}