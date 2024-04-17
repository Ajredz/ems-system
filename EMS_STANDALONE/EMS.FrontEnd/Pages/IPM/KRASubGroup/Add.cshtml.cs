using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_IPM;

namespace EMS.FrontEnd.Pages.IPM.KRASubGroup
{
    public class AddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.IPM.Transfer.KRASubGroup.Form KRASubGroup { get; set; }

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet()
        {
            if (_globalCurrentUser != null)
            {
                KRASubGroup = new EMS.IPM.Transfer.KRASubGroup.Form();

                ViewData["KRAGroupSelectList"] =
                    await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetKRAGroupDropdown();
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var URL = string.Concat(_ipmBaseURL,
                   _iconfiguration.GetSection("IPMService_API_URL").GetSection("KRASubGroup").GetSection("Add").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

                var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(KRASubGroup, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "KRASubGroup",
                            TableID = 0, // New Record, no ID yet
                            Remarks = string.Concat(KRASubGroup.Name, " added"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        }); 
                }
            }
            else
            {
                _resultView.IsSuccess = false;
                _resultView.Result = string.Concat(MessageUtilities.PRE_ERRMSG_REC_SAVE,
                string.Join("<br>", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray()));
            }

            return new JsonResult(_resultView);
        }
    }
}