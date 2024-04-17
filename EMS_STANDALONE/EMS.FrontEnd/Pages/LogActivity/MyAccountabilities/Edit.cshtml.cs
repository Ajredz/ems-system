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
using EMS.Workflow.Transfer.Accountability;
using EMS.Workflow.Transfer.Workflow;
using EMS.Workflow.Transfer.EmailServerCredential;

namespace EMS.FrontEnd.Pages.LogActivity.MyAccountabilities
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.Accountability.EmployeeAccountabilityForm EmployeeAccountabilityForm { get; set; }

        [BindProperty]
        public EMS.Workflow.Transfer.Accountability.EmployeeAccountabilityCommentsForm CommentsForm { get; set; }

        [BindProperty]
        public EMS.Workflow.Transfer.Accountability.EmployeeAccountabilityAttachmentForm AttachmentForm { get; set; }

        public bool _IsClearance = false;

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false, bool IsClearance = false) : base(iconfiguration, env)
        { 
            _IsAdminAccess = IsAdminAccess;
            _IsClearance = IsClearance;
        }

        public async Task OnGet(int ID)
        {
            if (_globalCurrentUser != null)
            {
                ViewData["HasEdit"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARANCE/EDITCLEARANCE")).Count() > 0 ? "true" : "false";
                ViewData["HasChangeStatus"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARANCE/CHANGESTATUS")).Count() > 0 ? "true" : "false";
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/ALLACCOUNTABILITIES/DELETE")).Count() > 0 ? "true" : "false";

                EmployeeAccountabilityForm =
                                await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                                .GetEmployeeAccountabilityByID(ID);

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
                        employees.PersonalInformation.FirstName, " ", employees.PersonalInformation.MiddleName, " (", employees.Code, ")");
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


                var description = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetWorkflowStepByWorkflowIDAndCode(new GetWorkflowStepByWorkflowIDAndCodeInput
                    {
                        WorkflowCode = "ACCOUNTABILITY",
                        Code = EmployeeAccountabilityForm.Status
                    });

                ViewData["CurrentStatusDescription"] = description.Description;

                var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployee(EmployeeAccountabilityForm.EmployeeID);
                var employeeorg = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(employee.OrgGroupID);
                var employeeposition = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPosition(employee.PositionID);
                ViewData["EmployeeName"] =
                    string.Concat("(", employee.Code, ") ",employee.PersonalInformation.LastName, ", "
                    , employee.PersonalInformation.FirstName, " ", string.IsNullOrEmpty(employee.PersonalInformation.MiddleName) ?
                    "" : employee.PersonalInformation.MiddleName);
                ViewData["EmployeeOrg"] = string.Concat(employeeorg.Code," - ", employeeorg.Description);
                ViewData["EmployeePosition"] = string.Concat(employeeposition.Code, " - ", employeeposition.Title," | ",employee.EmploymentStatus);
            }
        }

        public async Task<JsonResult> OnPostEmployee()
        {
            var Id = EmployeeAccountabilityForm.ID;
            List<long> IDs = new List<long>() { EmployeeAccountabilityForm.ID };
            var NewStatus = EmployeeAccountabilityForm.Status;
            var OrgId = EmployeeAccountabilityForm.OrgGroupID;
            var EmpId = EmployeeAccountabilityForm.EmployeeID;
            var Remarks = EmployeeAccountabilityForm.Remarks;

            EmployeeAccountabilityForm.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_workflowBaseURL,
                    _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("AddEmployeeAccountabilityStatusHistory").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(EmployeeAccountabilityForm, URL);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*var AllStatus = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetWorkflow(4)).WorkflowStepList;
                var CurrentStatus = AllStatus.Where(x => x.StepCode.Equals(EmployeeAccountabilityForm.Status)).FirstOrDefault();
                if (CurrentStatus.SendEmailToApprover || CurrentStatus.SendEmailToRequester)
                {
                    var NextStatus = AllStatus.Where(x => x.Order.Equals(CurrentStatus.Order + 1)).ToList();

                    var GetInactiveEmploymentStatus = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                        .GetReferenceValueByRefCode("ESTATUS")).Where(x => x.Value.Equals("INACTIVE")).Select(y => y.Description).FirstOrDefault();

                    var EmployeeAccountability = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                        .GetEmployeeByEmployeeAccountabilityIDs(IDs)).Item1;
                    var Employee = ((await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                        .GetEmployeeByIDs(EmployeeAccountability.Select(x => x.EmployeeID).Distinct().ToList())).Item1)
                        .Where(x => (GetInactiveEmploymentStatus.Contains(x.EmploymentStatus) ? !string.IsNullOrEmpty(x.PersonalInformation.Email) : !string.IsNullOrEmpty(x.CorporateEmail))).ToList();
                    var Position = (await new Common_Position(_iconfiguration, _globalCurrentUser, _env)
                        .GetAll());

                    if (CurrentStatus.SendEmailToRequester)
                    {
                        var OrgGroup = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                            .GetOrgGroupByIDs(Employee.Select(x => x.OrgGroupID).ToList())).Item1;
                        var emails = (from left in Employee
                                      join right in EmployeeAccountability on left.ID equals right.EmployeeID
                                      join rightPosition in Position on left.PositionID equals rightPosition.ID
                                      join rightOrgGroup in OrgGroup on left.OrgGroupID equals rightOrgGroup.ID
                                      select new EmailLogsInput()
                                      {
                                          PositionTitle = String.Concat(rightPosition.Code, " - ", rightPosition.Title),
                                          Status = CurrentStatus.StepDescription,
                                          SystemCode = "EMS ACCOUNTABILITY",
                                          SenderName = "EMS ACCOUNTABILITY",
                                          FromEmailAddress = "noreply@motortrade.com.ph",
                                          Name = String.Concat("(", left.Code, ") ", left.PersonalInformation.LastName, ", ", left.PersonalInformation.FirstName, " ", left.PersonalInformation.MiddleName.Substring(0, 1)),
                                          ToEmailAddress = (GetInactiveEmploymentStatus.Contains(left.EmploymentStatus) ? left.PersonalInformation.Email : left.CorporateEmail),
                                          Subject = String.Concat("CHANGE STATUS: ACCOUNTABILITY | ", right.Title, " | ", CurrentStatus.StepDescription),
                                          Body = MessageUtilities.EMAIL_BODY_TRAINING
                                                    .Replace("&lt;FormName&gt;", "ACCOUNTABILITY")
                                                    .Replace("&lt;RecordID&gt;", right.ID.ToString().PadLeft(7, '0'))
                                                    .Replace("&lt;EmployeeName&gt;", String.Concat(left.PersonalInformation.LastName, ", ", left.PersonalInformation.FirstName, " ", left.PersonalInformation.MiddleName.Substring(0, 1)))
                                                    .Replace("&lt;Remarks&gt;", EmployeeAccountabilityForm.Remarks)
                                                    .Replace("&lt;CurrentStatus&gt;", CurrentStatus.StepDescription)
                                                    .Replace("&lt;UpdatedBy&gt;", String.Concat("(", _globalCurrentUser.Username, ") ", _globalCurrentUser.LastName ?? "", ", ", _globalCurrentUser.FirstName ?? "", " ", string.IsNullOrEmpty(_globalCurrentUser.MiddleName ?? "").ToString().Substring(0, 1)))
                                                    .Replace("&lt;UpdatedDate&gt;", DateTime.Now.ToString("yyyy-MM-dd HH:mm")),
                                      }).ToList();

                        var (IsSuccessEmail, MessageEmail) = await new Common_EmailServerCredential(_iconfiguration, _globalCurrentUser, _env)
                            .AddMultipleEmailLogs(emails);
                    }
                    if (CurrentStatus.SendEmailToApprover)
                    {
                        List<int> UserRoleID = new List<int>();
                        foreach (var Item in NextStatus.Select(x => x.WorkflowStepApproverList).ToList())
                        {
                            UserRoleID.AddRange(Item.Select(x => x.RoleID).ToList());
                        }
                        var SystemUserIDApprover = (await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                            .GetUserByRoleIDs(UserRoleID.Distinct().ToList())).Select(x => x.ID).ToList();
                        var ApproverEmployee = ((await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                            .GetEmployeeByUserIDs(SystemUserIDApprover)).Item1).Where(x => !string.IsNullOrEmpty(x.CorporateEmail)).ToList();
                        var OrgGroup = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                            .GetOrgGroupByIDs(ApproverEmployee.Select(x => x.OrgGroupID).ToList())).Item1;

                        var emails = (from left in ApproverEmployee
                                      join right in EmployeeAccountability on left.OrgGroupID equals right.OrgGroupID
                                      join rightEmployee in Employee on right.EmployeeID equals rightEmployee.ID
                                      join rightPosition in Position on left.PositionID equals rightPosition.ID
                                      join rightOrgGroup in OrgGroup on left.OrgGroupID equals rightOrgGroup.ID
                                      select new EmailLogsInput()
                                      {
                                          PositionTitle = String.Concat(rightPosition.Code, " - ", rightPosition.Title),
                                          Status = CurrentStatus.StepDescription,
                                          SystemCode = "EMS ACCOUNTABILITY",
                                          SenderName = "EMS ACCOUNTABILITY",
                                          FromEmailAddress = "noreply@motortrade.com.ph",
                                          Name = String.Concat("(", left.Code, ") ", left.PersonalInformation.LastName, ", ", left.PersonalInformation.FirstName, " ", left.PersonalInformation.MiddleName.Substring(0, 1)),
                                          ToEmailAddress = left.CorporateEmail,
                                          Subject = String.Concat("CHANGE STATUS: ACCOUNTABILITY | ", right.Title, " | ", CurrentStatus.StepDescription),
                                          Body = MessageUtilities.EMAIL_BODY_TRAINING
                                                    .Replace("&lt;FormName&gt;", "ACCOUNTABILITY")
                                                    .Replace("&lt;RecordID&gt;", right.ID.ToString().PadLeft(7, '0'))
                                                    .Replace("&lt;EmployeeName&gt;", String.Concat("(", rightEmployee.Code, ") ", rightEmployee.PersonalInformation.LastName, ", ", rightEmployee.PersonalInformation.FirstName, " ", rightEmployee.PersonalInformation.MiddleName.Substring(0, 1)))
                                                    .Replace("&lt;Remarks&gt;", EmployeeAccountabilityForm.Remarks)
                                                    .Replace("&lt;CurrentStatus&gt;", CurrentStatus.StepDescription)
                                                    .Replace("&lt;UpdatedBy&gt;", String.Concat("(", _globalCurrentUser.Username, ") ", _globalCurrentUser.LastName ?? "", ", ", _globalCurrentUser.FirstName ?? "", " ", string.IsNullOrEmpty(_globalCurrentUser.MiddleName ?? "").ToString().Substring(0, 1)))
                                                    .Replace("&lt;UpdatedDate&gt;", DateTime.Now.ToString("yyyy-MM-dd HH:mm")),
                                      }).ToList();

                        var (IsSuccessEmail, MessageEmail) = await new Common_EmailServerCredential(_iconfiguration, _globalCurrentUser, _env)
                            .AddMultipleEmailLogs(emails);
                    }

                }*/
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "EmployeeAccountability",
                        TableID = EmployeeAccountabilityForm.ID,
                        Remarks = string.Concat(EmployeeAccountabilityForm.ID, " Employee Accountability Edit"),
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
  
        public async Task<JsonResult> OnGetEmployeeComments(int ID)
        {
            _resultView.Result = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeComments(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostSaveEmployeeComments()
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
   
        public async Task<JsonResult> OnGetEmployeeAttachment(int ID)
        {
            _resultView.Result = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeAttachment(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostSaveEmployeeAttachment()
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

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }


    }
}