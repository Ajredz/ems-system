using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Utilities.API;

namespace EMS.FrontEnd.Pages.LogActivity.Maintenance
{
    public class AddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.Form LogActivity { get; set; }

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet()
        {
            if (_globalCurrentUser != null)
            {
                LogActivity = new EMS.Workflow.Transfer.LogActivity.Form();

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
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            LogActivity.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("Add").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(LogActivity, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}