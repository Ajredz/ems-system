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

namespace EMS.FrontEnd.Pages.Recruitment.Applicant
{
    public class UpdateLogActivityModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.ApplicantLogActivityForm ApplicantLogActivityForm { get; set; }

        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.ApplicantLogActivityCommentsForm CommentsForm { get; set; }
        
        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.ApplicantLogActivityAttachmentForm AttachmentForm { get; set; }
        
        public UpdateLogActivityModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet(int ApplicantLogActivityID)
        {
            if (_globalCurrentUser != null)
            {
                ApplicantLogActivityForm = 
                    await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                    .GetApplicantLogActivityByID(ApplicantLogActivityID);

                var activityType = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                    .GetReferenceValueByRefCode(ReferenceCodes_Workflow.ACTIVITY_TYPE.ToString());

                ViewData["TypeSelectList"] =
                 activityType.Select(x => new SelectListItem
                 {
                     Value = x.Value,
                     Text = x.Description,
                     Selected = x.Value.Equals(ApplicantLogActivityForm.Type)
                 }).ToList();

                //ViewData["CurrentStatusDescription"] =
                //(await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                //    .GetReferenceValueByRefCodeAndValue(ReferenceCodes_Workflow.ACTIVITY_STAT_FILTER.ToString()
                //    , ApplicantLogActivityForm.CurrentStatus[2..])).Description;

                var roles = (await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                    .GetSystemUserRoleDropDownByUserID(_globalCurrentUser.UserID)).ToList();

                var status = (await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetNextWorkflowStep(new GetNextWorkflowStepInput { 
                        WorkflowCode = "TASK",
                        CurrentStepCode = ApplicantLogActivityForm.CurrentStatus,
                        RoleIDDelimited = string.Join(",", roles.Select(x => x.Value).ToArray())
                    })).ToList();



                ViewData["StatusSelectList"] =
                status.OrderByDescending(x => x.Order).Select(x => new SelectListItem
                {
                    Value = x.Code,
                    Text = x.Description,
                    Selected = x.Code.Equals(ApplicantLogActivityForm.CurrentStatus)
                }).ToList();

                //ViewData["OrgGroupSelectList"] =
                //    await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                //    .GetOrgGroupDropDown(ApplicantLogActivityForm.AssignedOrgGroupID);
				
                var description = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetWorkflowStepByWorkflowIDAndCode(new GetWorkflowStepByWorkflowIDAndCodeInput { 
                        WorkflowCode = "TASK",
                        Code = ApplicantLogActivityForm.CurrentStatus
                    });

                ViewData["CurrentStatusDescription"] = description.Description;

                //var status = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                //    .GetReferenceValueByRefCodeAndValuePrefix(new EMS.Workflow.Transfer.Reference.GetByRefCodeAndValuePrefixInput{
                //        RefCode = ReferenceCodes_Workflow.ACTIVITY_STATUS.ToString(),
                //        // Get Next Set of Status based on the Current Status' Prefix, ex. 1_, 2_, 3_
                //        ValuePrefix = (Convert.ToInt32(ApplicantLogActivityForm.CurrentStatus.Substring(0, 1)) + 1) + ""
                //    });

                //ViewData["StatusSelectList"] =
                //status.Select(x => new SelectListItem
                //{
                //    Value = x.Value,
                //    Text = x.Description,
                //    Selected = x.Value.Equals(ApplicantLogActivityForm.CurrentStatus)
                //}).ToList();


                if (ApplicantLogActivityForm.AssignedUserID > 0)
                {
                    List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
                    await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                    .GetSystemUserByIDs(new List<int> { ApplicantLogActivityForm.AssignedUserID });

                    if (systemUsers != null)
                    {
                        ApplicantLogActivityForm.AssignedUser = systemUsers.Count == 0 ? "" : string.Concat((systemUsers.First().LastName ?? "").Trim(),
                                                string.IsNullOrEmpty((systemUsers.First().FirstName ?? "").Trim()) ? "" : string.Concat(", ", systemUsers.First().FirstName),
                                                string.IsNullOrEmpty((systemUsers.First().MiddleName ?? "").Trim()) ? "" : string.Concat(" ", systemUsers.First().MiddleName));

                    } 
                }

                if (ApplicantLogActivityForm.AssignedUserID == _globalCurrentUser.UserID)
                {
                    ApplicantLogActivityForm.IsAssignToSelf = true;
                }

                var applicant = await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env)
                                .GetApplicant(ApplicantLogActivityForm.ApplicantID);
                ViewData["ApplicantName"] =
                    string.Concat(applicant.PersonalInformation.LastName, ", "
                    , applicant.PersonalInformation.FirstName, " ", string.IsNullOrEmpty(applicant.PersonalInformation.MiddleName) ?
                    "" : applicant.PersonalInformation.MiddleName);

                // Get OrgGroup description by OrgGroup IDs
                List<EMS.Plantilla.Transfer.OrgGroup.Form> orgGroup =
                    (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                    .GetOrgGroupByIDs(new List<int> { ApplicantLogActivityForm.AssignedOrgGroupID })).Item1;

                if (orgGroup.Count > 0)
                {
                    ApplicantLogActivityForm.OrgGroupDescription = string.Concat(orgGroup.First().Code, " - ", orgGroup.First().Description);
                }

            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            ApplicantLogActivityForm.CreatedBy = _globalCurrentUser.UserID;

            if (ApplicantLogActivityForm.IsAssignToSelf)
            {
                ApplicantLogActivityForm.AssignedUserID = _globalCurrentUser.UserID;
            }

            var URL = string.Concat(_workflowBaseURL,
                    _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("AddApplicantActivityStatusHistory").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(ApplicantLogActivityForm, URL);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "ApplicantLogActivityAttachment",
                        TableID = ApplicantLogActivityForm.ID,
                        Remarks = string.Concat(ApplicantLogActivityForm.ID, " Attachment added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetComments(int ID)
        {
            _resultView.Result = await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                .GetApplicantComments(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
        
        public async Task<JsonResult> OnPostSaveComments()
        {
            CommentsForm.CreatedBy = _globalCurrentUser.UserID;
            var (IsSuccess, Message) = 
                await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env).SaveApplicantComments(CommentsForm);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "ApplicantLogActivityComments",
                        TableID = ApplicantLogActivityForm.ID,
                        Remarks = string.Concat(ApplicantLogActivityForm.ID, " Comments added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }
        
        public async Task<JsonResult> OnGetAttachment(int ID)
        {
            _resultView.Result = await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                .GetApplicantAttachment(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostSaveAttachment()
        {
            var (IsSuccess, Message) = 
                await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                .SaveApplicantAttachment(AttachmentForm);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "ApplicantLogActivityAttachment",
                        TableID = ApplicantLogActivityForm.ID,
                        Remarks = string.Concat(ApplicantLogActivityForm.ID, " Attachment added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }


    }
}