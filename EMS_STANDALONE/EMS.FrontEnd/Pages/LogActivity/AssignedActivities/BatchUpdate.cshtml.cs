using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.LogActivity.AssignedActivities
{
    public class BatchUpdateModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.BatchTaskForm BatchTask { get; set; }

        public BatchUpdateModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task<JsonResult> OnPostAsync()
        {

            var URL = string.Concat(_workflowBaseURL,
                               _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("BatchUpdateLogActivity").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(BatchTask, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

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
                    WorkflowCode = "TASK",
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