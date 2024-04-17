using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Plantilla.Transfer.Employee;
using EMS.Workflow.Transfer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Plantilla.Employee
{
    public class AddAccountabilityModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.Accountability.TagToEmployeeForm TagToEmployeeForm { get; set; }

        public AddAccountabilityModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet()
        {
            if (_globalCurrentUser != null)
            {

                var accountabilityType = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                    .GetReferenceValueByRefCode(ReferenceCodes_Workflow.ACCOUNTABILITY_TYPE.ToString());

                ViewData["TypeSelectList"] =
                accountabilityType.Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description
                }).ToList();

                //var status = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                //    .GetReferenceValueByRefCode(ReferenceCodes_Workflow.ACCNTABILITY_STATUS.ToString());

                //TagToEmployeeForm.Status = Enums.AccountabilityStatus.NEW.ToString();
                //ViewData["StatusSelectList"] =
                //status.Select(x => new SelectListItem
                //{
                //    Value = x.Value,
                //    Text = x.Description,
                //    Selected = x.Value.Equals(Enums.AccountabilityStatus.NEW.ToString())
                //})
                //.Where(x => x.Value == Enums.AccountabilityStatus.NEW.ToString())
                //.ToList();

                var status = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                       .GetWorkflowStepByWorkflowCode("ACCOUNTABILITY");

                ViewData["StatusSelectList"] =
                status.Where(x => x.Code=="NEW").Select(x => new SelectListItem
                {
                    Value = x.Code,
                    Text = x.Description,
                    Selected = true
                }).ToList();
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            TagToEmployeeForm.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_workflowBaseURL,
                    _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("AddEmployeeAccountability").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

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
                        TableName = "EmployeeAccountability",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(TagToEmployeeForm.Type, " ", TagToEmployeeForm.Title, " ", TagToEmployeeForm.Description, " added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPrintCOE(int EmployeeID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetPrintCOE").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "EmployeeID=", EmployeeID,"&",
                  "HREmployeeID=", _globalCurrentUser.EmployeeID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetPrintCOEOutput(), URL);

            if (IsSuccess)
                _resultView.Result = Result;
            else
                _resultView.Result = ErrorMessage;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.PRINT.ToString(),
                        TableName = "Employee",
                        TableID = EmployeeID,
                        Remarks = string.Concat("COE Record: ", EmployeeID, " successfully printed"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            _resultView.IsSuccess = IsSuccess;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPrintClearance(int EmployeeID)
        {
            //var URL = string.Concat(_plantillaBaseURL,
            //      _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetPrintClearance").Value, "?",
            //      "userid=", _globalCurrentUser.UserID, "&",
            //      "EmployeeID=", EmployeeID,"&",
            //      "HREmployeeID=", _globalCurrentUser.EmployeeID);

            //var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetPrintClearanceOutput(), URL);

            //if (IsSuccess)
            //    _resultView.Result = Result;
            //else
            //    _resultView.Result = ErrorMessage;

            //if (IsSuccess)
            //{
            //    /*Add AuditLog*/
            //    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
            //        .AddAuditLog(new Security.Transfer.AuditLog.Form
            //        {
            //            EventType = Common_AuditLog.EventType.PRINT.ToString(),
            //            TableName = "Employee",
            //            TableID = EmployeeID,
            //            Remarks = string.Concat("COE Record: ", EmployeeID, " successfully printed"),
            //            IsSuccess = true,
            //            CreatedBy = _globalCurrentUser.UserID
            //        });
            //}

            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

    }
}