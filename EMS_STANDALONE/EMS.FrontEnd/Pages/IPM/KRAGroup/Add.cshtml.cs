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

namespace EMS.FrontEnd.Pages.IPM.KRAGroup
{
    public class AddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.IPM.Transfer.KRAGroup.Form KRAGroup { get; set; }

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet()
        {
            if (_globalCurrentUser != null)
            {
                KRAGroup = new EMS.IPM.Transfer.KRAGroup.Form();

                var kraType = await new Common_Reference(_iconfiguration, _globalCurrentUser, _ipmBaseURL, _env)
                               .GetReferenceValueByRefCode(EMS.IPM.Transfer.Enums.ReferenceCodes.KRA_TYPE.ToString());
                
                ViewData["TypeSelectList"] = kraType.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var URL = string.Concat(_ipmBaseURL,
                   _iconfiguration.GetSection("IPMService_API_URL").GetSection("KRAGroup").GetSection("Add").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

                var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(KRAGroup, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "KRAGroup",
                            TableID = 0, // New Record, no ID yet
                            Remarks = string.Concat(KRAGroup.Name, " added"),
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