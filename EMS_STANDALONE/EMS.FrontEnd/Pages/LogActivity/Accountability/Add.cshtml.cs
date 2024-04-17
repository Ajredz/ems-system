using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Utilities.API;

namespace EMS.FrontEnd.Pages.LogActivity.Accountability
{
    public class AddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.Accountability.AccountabilityForm Accountability { get; set; }

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task<JsonResult> OnPostAsync()
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("Add").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(Accountability, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "Accountability",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(Accountability.PreloadedName, " added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                if (Accountability.AccountabilityDetailsList != null && Accountability.AccountabilityDetailsList.Count() > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "AccountabilityDetails",
                            TableID = 0, // New Record, no ID yet
                            Remarks = string.Concat(Accountability.PreloadedName, " Details added"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }
            }

            return new JsonResult(_resultView);
        }
    }
}