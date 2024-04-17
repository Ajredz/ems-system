using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_IPM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using EMS.FrontEnd.SharedClasses.Common_Security;
using System.Text;

namespace EMS.FrontEnd.Pages.IPM.KPIPosition
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.IPM.Transfer.KPIPosition.Form KPIPosition { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID, string EffectiveDate)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/KPIPOSITION/DELETE")).Count() > 0 ? "true" : "false";
            }

            if (_globalCurrentUser != null)
            {
                KPIPosition = await new Common_KPIPosition(_iconfiguration, _globalCurrentUser, _env).GetKPIPosition(ID, EffectiveDate);

                ViewData["AllKPIList"] =
                    await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetAllKPIDetails();

                ViewData["PositionSelectList"] =
                    await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionDropdown();
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var URL = string.Concat(_ipmBaseURL,
                   _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPIPosition").GetSection("Edit").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

                var oldValue = await new Common_KPIPosition(_iconfiguration, _globalCurrentUser, _env).GetKPIPosition(KPIPosition.PositionID, KPIPosition.EffectiveDate);

                var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(KPIPosition, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "KPIPosition",
                            TableID = KPIPosition.PositionID,
                            Remarks = string.Concat("KPI Position for ", KPIPosition.Position, " changed"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        }); ;
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