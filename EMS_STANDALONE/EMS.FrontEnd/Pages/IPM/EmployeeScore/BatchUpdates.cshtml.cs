using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Workflow.Transfer.Workflow;
using System.Text;
using Utilities.API;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMS.FrontEnd.Pages.IPM.EmployeeScore
{
    public class BatchUpdatesModel : SharedClasses.Utilities
    {

        [BindProperty]
        public EMS.IPM.Transfer.EmployeeScore.BatchEmployeesScoreForm BatchEmployeesScore { get; set; }

        public BatchUpdatesModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env) { }

        public async Task<JsonResult> OnPostAsync()
        {

            var URL = string.Concat(_ipmBaseURL,
                               _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("BatchUpdateEmployeesScore").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(BatchEmployeesScore, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                StringBuilder stringIDs = new StringBuilder();

                foreach (var obj in BatchEmployeesScore.IDs)
                {
                    stringIDs.Append(string.Concat(obj + ", "));
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                .AddAuditLog(new Security.Transfer.AuditLog.Form
                {
                    EventType = Common_AuditLog.EventType.EDIT.ToString(),
                    TableName = "Employee Score",
                    TableID = 0,
                    Remarks = string.Concat("Status of Employee Score Transaction/s No. " + stringIDs + "changed to " + BatchEmployeesScore.Status),
                    IsSuccess = true,
                    CreatedBy = _globalCurrentUser.UserID
                }); ;

                var workflowURL = string.Concat(_workflowBaseURL,
                        _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("EmployeeScore").GetSection("BatchUpdateEmployeeScoreStatusHistory").Value, "?",
                        "userid=", _globalCurrentUser.UserID);

                var (IsSuccess1, Message1) = await SharedUtilities.PostFromAPI(BatchEmployeesScore, workflowURL);
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetBatchStatusDropDown(string CurrentStatus)
        {
            var roles = (await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                    .GetSystemUserRoleDropDownByUserID(_globalCurrentUser.UserID)).ToList();

            var status = (await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetNextWorkflowStep(new GetNextWorkflowStepInput
                {
                    WorkflowCode = "IPM",
                    CurrentStepCode = CurrentStatus,
                    RoleIDDelimited = string.Join(",", roles.Select(x => x.Value).ToArray())
                })).ToList();

            _resultView.Result =
                status.Select(x => new SelectListItem
                {
                    Value = x.Code,
                    Text = x.Description
                }).ToList();

            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }

    }
}
