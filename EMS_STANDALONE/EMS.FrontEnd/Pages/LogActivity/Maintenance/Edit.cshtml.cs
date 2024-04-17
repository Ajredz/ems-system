using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Utilities.API;

namespace EMS.FrontEnd.Pages.LogActivity.Maintenance
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.Form LogActivity { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync(int ID)
        {

            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/MAINTENANCE/DELETE")).Count() > 0 ? "true" : "false";
            }

            LogActivity = await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env).GetLogActivity(ID);

            var result1 = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                                .GetReferenceValueByRefCode(ReferenceCodes_Workflow.ACTIVITY_MODULE.ToString());
            ViewData["ModuleSelectList"] = result1.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description
                }).ToList();


            var result2 = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                .GetReferenceValueByRefCode(ReferenceCodes_Workflow.ACTIVITY_TYPE.ToString());
            ViewData["TypeSelectList"] = result2.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description
                }).ToList();

            var activitySubType = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
               .GetReferenceValueByRefCode(LogActivity.Type);
            ViewData["SubTypeSelectList"] =
                activitySubType.Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description,
                    Selected = x.Value.Equals(LogActivity.SubType)

                }).ToList();
        }

        public async Task<JsonResult> OnPostAsync()
        {
            LogActivity.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_workflowBaseURL,
                               _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("Edit").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(LogActivity, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}