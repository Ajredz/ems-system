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

namespace EMS.FrontEnd.Pages.IPM.KPIScore
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.IPM.Transfer.KPIScore.Form KPIScore { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }
        public async Task OnGetAsync(int OrgID)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/KPIScore/DELETE")).Count() > 0 ? "true" : "false";
            }

            if (_globalCurrentUser != null)
            {
                //KPIScore = await new Common_KPIScore(_iconfiguration, _globalCurrentUser, _env).GetKPIScore(OrgID);

                ViewData["OrgGroupSelectList"] =
                    await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupDropDown(0);
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {   
            if (ModelState.IsValid)
            {
                var URL = string.Concat(_ipmBaseURL,
                   _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPIScore").GetSection("Edit").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

                //var oldValue = await new Common_KPIScore(_iconfiguration, _globalCurrentUser, _env).GetKPIScore(KPIScore.ID);

                var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(KPIScore, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {
                    StringBuilder Remarks = new StringBuilder();
                    
                    //if (KPIScore.Code != oldValue.Code)
                    //{
                    //    Remarks.Append(string.Concat("Code changed from ", oldValue.Code, " to ", KPIScore.Code, ". "));
                    //}

                    //if (KPIScore.Description != oldValue.Description)
                    //{
                    //    Remarks.Append(string.Concat("Description changed from ", oldValue.Description, " to ", KPIScore.Description, ". "));
                    //}

                    if (Remarks.Length > 0)
                    {
                        /*Add AuditLog*/
                        await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                            .AddAuditLog(new Security.Transfer.AuditLog.Form
                            {
                                EventType = Common_AuditLog.EventType.EDIT.ToString(),
                                TableName = "KPIScore",
                                TableID = KPIScore.ID,
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