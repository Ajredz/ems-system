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

namespace EMS.FrontEnd.Pages.LogActivity.Checklist
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.ApplicantLogActivityForm ApplicantLogActivityForm { get; set; }
        
        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.EmployeeLogActivityForm EmployeeLogActivityForm { get; set; }

        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.ApplicantLogActivityCommentsForm CommentsForm { get; set; }

        [BindProperty]
        public EMS.Recruitment.Transfer.Applicant.ApplicantAttachmentForm AttachmentForm { get; set; }

        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.ApplicantLogActivityAttachmentForm LogActivityAttachmentForm { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet(int ID, int ApplicantID, int EmployeeID)
        {
            if (_globalCurrentUser != null)
            {
                if (ApplicantID > 0)
                {
                    ApplicantLogActivityForm =
                                await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                                .GetApplicantLogActivityByID(ID); 
                }

                if (EmployeeID > 0)
                {
                    EmployeeLogActivityForm =
                                await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                                .GetEmployeeLogActivityByID(ID); 
                }

                var activityType = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                    .GetReferenceValueByRefCode(ReferenceCodes_Workflow.ACTIVITY_TYPE.ToString());

                if (ApplicantID > 0)
                {
                    var applicant = await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env)
                                .GetApplicant(ApplicantID);
                    ViewData["ApplicantName"] = string.Concat(applicant.PersonalInformation.LastName, ", "
                        , applicant.PersonalInformation.FirstName, string.IsNullOrEmpty(applicant.PersonalInformation.MiddleName) ? 
                        "" : applicant.PersonalInformation.MiddleName); 
                }

                if (EmployeeID > 0)
                {
                    var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                .GetEmployee(EmployeeID);
                    ViewData["EmployeeName"] =
                        string.Concat(employee.PersonalInformation.LastName, ", "
                        , employee.PersonalInformation.FirstName, string.IsNullOrEmpty(employee.PersonalInformation.MiddleName) ?
                        "" : employee.PersonalInformation.MiddleName);

                    
                }

                ViewData["ApplicantID"] = ApplicantID;

                ViewData["TypeSelectList"] =
                activityType.Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description,
                    Selected = x.Value.Equals(ApplicantID > 0 ? ApplicantLogActivityForm.Type : EmployeeLogActivityForm.Type)
                }).ToList();

                var activitySubType = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                    .GetReferenceValueByRefCode(ApplicantID > 0 ? ApplicantLogActivityForm.Type : EmployeeLogActivityForm.Type);

                //ViewData["CurrentStatusDescription"] =
                //(await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                //    .GetReferenceValueByRefCodeAndValue(ReferenceCodes_Workflow.ACTIVITY_STAT_FILTER.ToString()
                //    , ApplicantID > 0 ? ApplicantLogActivityForm.CurrentStatus[2..] : EmployeeLogActivityForm.CurrentStatus[2..])).Description;

                ViewData["SubTypeSelectList"] =
                activitySubType.Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description,
                    Selected = x.Value.Equals(ApplicantID > 0 ? ApplicantLogActivityForm.SubType : EmployeeLogActivityForm.SubType)

                }).ToList();

                //var status = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                //    .GetReferenceValueByRefCodeAndValuePrefix(new EMS.Workflow.Transfer.Reference.GetByRefCodeAndValuePrefixInput
                //    {
                //        RefCode = ReferenceCodes_Workflow.ACTIVITY_STATUS.ToString(),
                //        // Get Next Set of Status based on the Current Status' Prefix, ex. 1_, 2_, 3_
                //        ValuePrefix = (Convert.ToInt32((ApplicantID > 0 ? ApplicantLogActivityForm.CurrentStatus : EmployeeLogActivityForm.CurrentStatus).Substring(0, 1)) + 1) + ""
                //    });

                //ViewData["StatusSelectList"] =
                //status.Select(x => new SelectListItem
                //{
                //    Value = x.Value,
                //    Text = x.Description,
                //    Selected = x.Value.Equals(ApplicantID > 0 ? ApplicantLogActivityForm.CurrentStatus : EmployeeLogActivityForm.CurrentStatus)
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

                var description = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetWorkflowStepByWorkflowIDAndCode(new GetWorkflowStepByWorkflowIDAndCodeInput
                    {
                        WorkflowCode = "TASK",
                        Code = EmployeeLogActivityForm.CurrentStatus
                    });

                ViewData["CurrentStatusDescription"] = description.Description;

                ViewData["OrgGroupSelectList"] =
                    await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                    .GetOrgGroupDropDown(ApplicantID > 0 ? ApplicantLogActivityForm.AssignedOrgGroupID : EmployeeLogActivityForm.AssignedOrgGroupID);

                if ((ApplicantID > 0 ? ApplicantLogActivityForm.AssignedUserID : EmployeeLogActivityForm.AssignedUserID) > 0)
                {
                    List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
                    await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                    .GetSystemUserByIDs(new List<int> { ApplicantID > 0 ? ApplicantLogActivityForm.AssignedUserID : EmployeeLogActivityForm.AssignedUserID });

                    if (systemUsers != null)
                    {
                        if (ApplicantID > 0)
                        {
                            ApplicantLogActivityForm.AssignedUser
                              = systemUsers.Count == 0 ? "" : string.Concat((systemUsers.First().LastName ?? "").Trim(),
                                                  string.IsNullOrEmpty((systemUsers.First().FirstName ?? "").Trim()) ? "" : string.Concat(", ", systemUsers.First().FirstName),
                                                  string.IsNullOrEmpty((systemUsers.First().MiddleName ?? "").Trim()) ? "" : string.Concat(" ", systemUsers.First().MiddleName));
                        }
                        else
                        {
                            EmployeeLogActivityForm.AssignedUser
                                  = systemUsers.Count == 0 ? "" : string.Concat((systemUsers.First().LastName ?? "").Trim(),
                                                      string.IsNullOrEmpty((systemUsers.First().FirstName ?? "").Trim()) ? "" : string.Concat(", ", systemUsers.First().FirstName),
                                                      string.IsNullOrEmpty((systemUsers.First().MiddleName ?? "").Trim()) ? "" : string.Concat(" ", systemUsers.First().MiddleName));
                        }
                    }
                }

            }
        }

        public async Task<JsonResult> OnPostApplicant()
        {
            ApplicantLogActivityForm.CreatedBy = _globalCurrentUser.UserID;

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
                        TableName = "ApplicantLogActivityStatusHistory",
                        TableID = ApplicantLogActivityForm.ID,
                        Remarks = string.Concat(ApplicantLogActivityForm.ID, " Applicant LogActivity Status added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }
        
        public async Task<JsonResult> OnPostEmployee()
        {
            EmployeeLogActivityForm.CreatedBy = _globalCurrentUser.UserID;

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

        public async Task<JsonResult> OnGetTitleTypeDropDown(string Type, int TitleID)
        {
            _resultView.Result = await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                    .GetLogActivityDropdownByModuleAndType(new EMS.Workflow.Transfer.LogActivity.GetLogActivityByModuleTypeInput
                    {
                        Modules = new List<EMS.Workflow.Transfer.Enums.ActivityModule>() {
                            EMS.Workflow.Transfer.Enums.ActivityModule.GENERAL,
                            EMS.Workflow.Transfer.Enums.ActivityModule.RECRUITMENT
                        },
                        Type = Type,
                        SelectedID = TitleID
                    });

            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetTitleDropDownChange(int TitleID)
        {
            _resultView.Result = await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                    .GetLogActivityByID(TitleID);

            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetApplicantComments(int ID)
        {
            _resultView.Result = await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env).GetApplicantComments(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
        
        public async Task<JsonResult> OnGetEmployeeComments(int ID)
        {
            _resultView.Result = await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env).GetEmployeeComments(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostSaveApplicantComments()
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
                        TableID = CommentsForm.ApplicantLogActivityID,
                        Remarks = string.Concat(CommentsForm.ApplicantLogActivityID, " Comments added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }
        
        public async Task<JsonResult> OnPostSaveEmployeeComments()
        {
            CommentsForm.CreatedBy = _globalCurrentUser.UserID;
            var (IsSuccess, Message) =
                await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                .SaveEmployeeComments(new EMS.Workflow.Transfer.LogActivity.EmployeeLogActivityCommentsForm { 
                    Comments = CommentsForm.Comments,
                    CreatedBy = CommentsForm.CreatedBy,
                    EmployeeLogActivityID = CommentsForm.ApplicantLogActivityID
                });
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
                        TableID = CommentsForm.ApplicantLogActivityID,
                        Remarks = string.Concat(CommentsForm.ApplicantLogActivityID, " Comments added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetApplicantAttachment(int ID)
        {
            _resultView.Result = await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env)
                .GetAttachment(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
        
        public async Task<JsonResult> OnGetEmployeeAttachment(int ID)
        {
            _resultView.Result = await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeAttachment(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostSaveApplicantAttachment()
        {
            var (IsSuccess, Message) =
                await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env)
                .SaveAttachment(AttachmentForm);
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

        public async Task<JsonResult> OnPostSaveEmployeeAttachment()
        {
            var (IsSuccess, Message) =
                await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                .SaveEmployeeAttachment(new EMS.Workflow.Transfer.LogActivity.EmployeeLogActivityAttachmentForm { 
                    EmployeeLogActivityID = LogActivityAttachmentForm.ApplicantLogActivityID,
                    AddAttachmentForm = LogActivityAttachmentForm.AddAttachmentForm,
                    DeleteAttachmentForm = LogActivityAttachmentForm.DeleteAttachmentForm
                });
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

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
    }
}