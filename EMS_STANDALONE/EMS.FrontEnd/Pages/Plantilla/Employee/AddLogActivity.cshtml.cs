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

namespace EMS.FrontEnd.Pages.Plantilla.Employee
{
    public class AddLogActivityModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.TagToEmployeeForm TagToEmployeeForm { get; set; }


        public AddLogActivityModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet()
        {
            if (_globalCurrentUser != null)
            {
                TagToEmployeeForm = new EMS.Workflow.Transfer.LogActivity.TagToEmployeeForm();

                var activityType = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                    .GetReferenceValueByRefCode(ReferenceCodes_Workflow.ACTIVITY_TYPE.ToString());

                ViewData["TypeSelectList"] =
                activityType.Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description
                }).ToList();

                //var activitySubType = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                //    .GetReferenceValueByRefCode(ReferenceCodes_Workflow.ACTIVITY_SUBTYPE.ToString());

                //ViewData["SubTypeSelectList"] =
                //activitySubType.Select(x => new SelectListItem
                //{
                //    Value = x.Value,
                //    Text = x.Description
                //}).ToList();

                //var status = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                //    .GetReferenceValueByRefCodeAndValuePrefix(new EMS.Workflow.Transfer.Reference.GetByRefCodeAndValuePrefixInput
                //    { 
                //        RefCode = ReferenceCodes_Workflow.ACTIVITY_STATUS.ToString(),
                //        ValuePrefix = "1_"
                //    });

                //TagToEmployeeForm.Status = string.Concat("1_", Enums.ActivityStatus.NEW.ToString());
                //ViewData["StatusSelectList"] =
                //status.Select(x => new SelectListItem
                //{
                //    Value = x.Value,
                //    Text = x.Description
                //}).ToList();

                var status = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                   .GetWorkflowStepByWorkflowCode("TASK");

                //TagToEmployeeForm.Status = string.Concat("1_", Enums.ActivityStatus.NEW.ToString());
                ViewData["StatusSelectList"] =
                status.Where(x => x.Order == 1).Select(x => new SelectListItem
                {
                    Value = x.Code,
                    Text = x.Description,
                    Selected = true
                }).ToList();


                ViewData["OrgGroupSelectList"] =
                    await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                    .GetOrgGroupDropDown();
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            TagToEmployeeForm.CreatedBy = _globalCurrentUser.UserID;

            // Get Tags from Type Prefix Value, ex. 1_0_MEDICAL, IsWithPassFail = true, IsWithAssignment = false
            TagToEmployeeForm.IsWithPassFail = TagToEmployeeForm.Type.Substring(0, 1).Equals("1");
            TagToEmployeeForm.IsWithAssignment = TagToEmployeeForm.Type.Substring(2, 1).Equals("1");
            
            if (TagToEmployeeForm.IsAssignToSelf)
            {
                TagToEmployeeForm.AssignedUserID = _globalCurrentUser.UserID;
            }

            var URL = string.Concat(_workflowBaseURL,
                    _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("AddEmployeeActivity").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            if (TagToEmployeeForm.AssignedUserID > 0)
            {
                var employee =
                    (new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeByUserID(TagToEmployeeForm.AssignedUserID)).Result;
                TagToEmployeeForm.Email = employee.CorporateEmail;

                var taggedEmployee =
                (new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployee(TagToEmployeeForm.EmployeeID)).Result;

                TagToEmployeeForm.EmployeeName = string.Concat((taggedEmployee.PersonalInformation.LastName ?? "").Trim(),
                    string.IsNullOrEmpty((taggedEmployee.PersonalInformation.FirstName ?? "").Trim()) ? "" : string.Concat(", ", taggedEmployee.PersonalInformation.FirstName),
                    string.IsNullOrEmpty((taggedEmployee.PersonalInformation.MiddleName ?? "").Trim()) ? "" : string.Concat(" ", taggedEmployee.PersonalInformation.MiddleName));
            }


            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(TagToEmployeeForm, URL);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "EmployeeLogActivity",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(TagToEmployeeForm.Type, " ", TagToEmployeeForm.SubType, " ", TagToEmployeeForm.Title, " ", TagToEmployeeForm.Description, " added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetSubType(string Type)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                    .GetReferenceValueByRefCode(Type);
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

    }
}