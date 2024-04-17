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

namespace EMS.FrontEnd.Pages.IPM.KPI
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.IPM.Transfer.KPI.Form KPI { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }
        public async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/KPI/DELETE")).Count() > 0 ? "true" : "false";
            }

            if (_globalCurrentUser != null)
            {
                KPI = await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetKPI(ID);

                ViewData["KRAGroupSelectList"] =
                    await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetKRAGroupDropdown(KPI.KRAGroup);

                ViewData["KRASubGroupSelectList"] =
                    await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetKRASubGroupDropdown(KPI.KRAGroup, KPI.KRASubGroup.Value);

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
                   _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPI").GetSection("Edit").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

                var oldValue = await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetKPI(KPI.ID);

                var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(KPI, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {
                    StringBuilder Remarks = new StringBuilder();
                    
                    if (KPI.Code != oldValue.Code)
                    {
                        Remarks.Append(string.Concat("Code changed from ", oldValue.Code, " to ", KPI.Code, ". "));
                    }

                    if (KPI.Description != oldValue.Description)
                    {
                        Remarks.Append(string.Concat("Description changed from ", oldValue.Description, " to ", KPI.Description, ". "));
                    }

                    if (Remarks.Length > 0)
                    {
                        /*Add AuditLog*/
                        await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                            .AddAuditLog(new Security.Transfer.AuditLog.Form
                            {
                                EventType = Common_AuditLog.EventType.EDIT.ToString(),
                                TableName = "KPI",
                                TableID = KPI.ID,
                                Remarks = Remarks.ToString(),
                                IsSuccess = true,
                                CreatedBy = _globalCurrentUser.UserID
                            });
                    }
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