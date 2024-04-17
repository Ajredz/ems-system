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

namespace EMS.FrontEnd.Pages.IPM.KPI
{
    public class AddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.IPM.Transfer.KPI.Form KPI { get; set; }

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet()
        {
            if (_globalCurrentUser != null)
            {
                KPI = new EMS.IPM.Transfer.KPI.Form();

                ViewData["KRAGroupSelectList"] =
                    await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetKRAGroupDropdown();

                ViewData["KRASubGroupSelectList"] =
                    await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetKRASubGroupDropdown();

                ViewData["KPITypeSelectList"] =
                    await new Common_Reference(_iconfiguration, _globalCurrentUser, _ipmBaseURL, _env).GetDropDownByRefCode(ReferenceCodes_IPM.KPI_TYPE.ToString());

                ViewData["KPISourceTypeSelectList"] =
                    await new Common_Reference(_iconfiguration, _globalCurrentUser, _ipmBaseURL, _env).GetDropDownByRefCode(ReferenceCodes_IPM.KPI_SOURCE_TYPE.ToString());
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            KPI.ModifiedBy = _globalCurrentUser.UserID;

            if (ModelState.IsValid)
            {
                var URL = string.Concat(_ipmBaseURL,
                   _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPI").GetSection("Add").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

                var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(KPI, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "KPI",
                            TableID = 0, // New Record, no ID yet
                            Remarks = string.Concat(KPI.Code, " added"),
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