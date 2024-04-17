using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
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

namespace EMS.FrontEnd.Pages.Plantilla.Employee
{
    public class UpdateLogActivityModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.EmployeeLogActivityForm EmployeeLogActivityForm { get; set; }

        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.EmployeeLogActivityCommentsForm CommentsForm { get; set; }
        
        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.EmployeeLogActivityAttachmentForm AttachmentForm { get; set; }
        
        public UpdateLogActivityModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet(int EmployeeLogActivityID)
        {
            if (_globalCurrentUser != null)
            {
                EmployeeLogActivityForm = 
                    await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeLogActivityByID(EmployeeLogActivityID);

                var activityType = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                    .GetReferenceValueByRefCode(ReferenceCodes_Workflow.ACTIVITY_TYPE.ToString());

                ViewData["TypeSelectList"] =
                 activityType.Select(x => new SelectListItem
                 {
                     Value = x.Value,
                     Text = x.Description,
                     Selected = x.Value.Equals(EmployeeLogActivityForm.Type)
                 }).ToList();

                var activitySubType = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                    .GetReferenceValueByRefCode(EmployeeLogActivityForm.Type);

                //ViewData["CurrentStatusDescription"] =
                //(await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                //    .GetReferenceValueByRefCodeAndValue(ReferenceCodes_Workflow.ACTIVITY_STAT_FILTER.ToString()
                //    , EmployeeLogActivityForm.CurrentStatus[2..])).Description;

                ViewData["SubTypeSelectList"] =
                activitySubType.Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description,
                    Selected = x.Value.Equals(EmployeeLogActivityForm.SubType)

                }).ToList();

                //var status = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                //    .GetReferenceValueByRefCodeAndValuePrefix(new EMS.Workflow.Transfer.Reference.GetByRefCodeAndValuePrefixInput{
                //        RefCode = ReferenceCodes_Workflow.ACTIVITY_STATUS.ToString(),
                //        // Get Next Set of Status based on the Current Status' Prefix, ex. 1_, 2_, 3_
                //        ValuePrefix = (Convert.ToInt32(EmployeeLogActivityForm.CurrentStatus.Substring(0, 1)) + 1) + ""
                //    });

                //ViewData["StatusSelectList"] =
                //status.Select(x => new SelectListItem
                //{
                //    Value = x.Value,
                //    Text = x.Description,
                //    Selected = x.Value.Equals(EmployeeLogActivityForm.CurrentStatus)
                //}).ToList();


                var roles = (await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                    .GetSystemUserRoleDropDownByUserID(_globalCurrentUser.UserID)).ToList();

                var status = (await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetNextWorkflowStep(new GetNextWorkflowStepInput
                    {
                        WorkflowCode = "TASK",
                        CurrentStepCode = EmployeeLogActivityForm.CurrentStatus,
                        RoleIDDelimited = string.Join(",", roles.Select(x => x.Value).ToArray())
                    })).ToList();



                ViewData["StatusSelectList"] =
                status.OrderByDescending(x => x.Order).Select(x => new SelectListItem
                {
                    Value = x.Code,
                    Text = x.Description,
                    Selected = x.Code.Equals(EmployeeLogActivityForm.CurrentStatus)
                }).ToList();

                //ViewData["OrgGroupSelectList"] =
                //    await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                //    .GetOrgGroupDropDown(EmployeeLogActivityForm.AssignedOrgGroupID);

                var description = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetWorkflowStepByWorkflowIDAndCode(new GetWorkflowStepByWorkflowIDAndCodeInput
                    {
                        WorkflowCode = "TASK",
                        Code = EmployeeLogActivityForm.CurrentStatus
                    });

                ViewData["CurrentStatusDescription"] = description.Description;


                if (EmployeeLogActivityForm.AssignedUserID > 0)
                {
                    List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
                    await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                    .GetSystemUserByIDs(new List<int> { EmployeeLogActivityForm.AssignedUserID });

                    if (systemUsers != null)
                    {
                        EmployeeLogActivityForm.AssignedUser = systemUsers.Count == 0 ? "" : string.Concat((systemUsers.First().LastName ?? "").Trim(),
                                                string.IsNullOrEmpty((systemUsers.First().FirstName ?? "").Trim()) ? "" : string.Concat(", ", systemUsers.First().FirstName),
                                                string.IsNullOrEmpty((systemUsers.First().MiddleName ?? "").Trim()) ? "" : string.Concat(" ", systemUsers.First().MiddleName));

                    } 
                }

                if (EmployeeLogActivityForm.AssignedUserID == _globalCurrentUser.UserID)
                {
                    EmployeeLogActivityForm.IsAssignToSelf = true;
                }

                var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                .GetEmployee(EmployeeLogActivityForm.EmployeeID);
                ViewData["EmployeeName"] =
                    string.Concat(employee.PersonalInformation.LastName, ", "
                    , employee.PersonalInformation.FirstName, " ", string.IsNullOrEmpty(employee.PersonalInformation.MiddleName) ?
                    "" : employee.PersonalInformation.MiddleName);

                // Get OrgGroup description by OrgGroup IDs
                List<EMS.Plantilla.Transfer.OrgGroup.Form> orgGroup =
                    (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                    .GetOrgGroupByIDs(new List<int> { EmployeeLogActivityForm.AssignedOrgGroupID })).Item1;

                if (orgGroup.Count > 0)
                {
                    EmployeeLogActivityForm.OrgGroupDescription = string.Concat(orgGroup.First().Code, " - ", orgGroup.First().Description);
                }

            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            EmployeeLogActivityForm.CreatedBy = _globalCurrentUser.UserID;

            if (EmployeeLogActivityForm.IsAssignToSelf)
            {
                EmployeeLogActivityForm.AssignedUserID = _globalCurrentUser.UserID;
            }

            var URL = string.Concat(_workflowBaseURL,
                    _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("AddEmployeeActivityStatusHistory").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(EmployeeLogActivityForm, URL);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "EmployeeLogActivityStatusHistory",
                        TableID = EmployeeLogActivityForm.ID,
                        Remarks = string.Concat(EmployeeLogActivityForm.ID, " Employee LogActivity Status added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetComments(int ID)
        {
            _resultView.Result = await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeComments(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
        
        public async Task<JsonResult> OnPostSaveComments()
        {
            CommentsForm.CreatedBy = _globalCurrentUser.UserID;
            var (IsSuccess, Message) = 
                await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env).SaveEmployeeComments(CommentsForm);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "EmployeeLogActivityComments",
                        TableID = EmployeeLogActivityForm.ID,
                        Remarks = string.Concat(EmployeeLogActivityForm.ID, " Comments added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }
        
        public async Task<JsonResult> OnGetAttachment(int ID)
        {
            _resultView.Result = await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeAttachment(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostSaveAttachment()
        {
            var (IsSuccess, Message) = 
                await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
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
                        TableName = "EmployeeLogActivityAttachment",
                        TableID = EmployeeLogActivityForm.ID,
                        Remarks = string.Concat(EmployeeLogActivityForm.ID, " Attachment added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }

    }
}