using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.IPM.EmployeeScoreApproval
{
    public class BatchUpdateModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.IPM.Transfer.EmployeeScore.BatchEmployeeScoreForm BatchEmployeeScore { get; set; }

        public BatchUpdateModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task<JsonResult> OnPostAsync()
        {

            var URL = string.Concat(_ipmBaseURL,
                               _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScoreKeyInApproval").GetSection("BatchUpdateEmployeeScore").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(BatchEmployeeScore, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                StringBuilder stringIDs = new StringBuilder();

                foreach (var obj in BatchEmployeeScore.IDs)
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
                    Remarks = string.Concat("Status of Employee Score Transaction/s No. " + stringIDs + "changed to " + BatchEmployeeScore.Status),
                    IsSuccess = true,
                    CreatedBy = _globalCurrentUser.UserID
                }); ;

                var workflowURL = string.Concat(_workflowBaseURL,
                        _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("EmployeeScore").GetSection("BatchUpdateEmployeeScoreStatusHistory").Value, "?",
                        "userid=", _globalCurrentUser.UserID);

                var (IsSuccess1, Message1) = await SharedUtilities.PostFromAPI(BatchEmployeeScore, workflowURL);
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetBatchStatusDropDown(string CurrentStatus)
        {

            //var status = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
            //        .GetReferenceValueByRefCodeAndValuePrefix(new EMS.Workflow.Transfer.Reference.GetByRefCodeAndValuePrefixInput
            //        {
            //            RefCode = ReferenceCodes_Workflow.APPR_ACTIVITY_STATUS.ToString(),
            //            // Get Next Set of Status based on the Current Status' Prefix, ex. 1_, 2_, 3_
            //            ValuePrefix = (Convert.ToInt32((CurrentStatus).Substring(0, 1)) + 1) + ""
            //        });

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