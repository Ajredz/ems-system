using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Workflow.Transfer.Accountability;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Utilities.API;

namespace EMS.FrontEnd.Pages.LogActivity.Accountability
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.Accountability.AccountabilityForm Accountability { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync(int ID)
        {

            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/ACCOUNTABILITY/DELETE")).Count() > 0 ? "true" : "false";
            }

            Accountability = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env).GetByID(ID);

        }

        public async Task<JsonResult> OnPostAsync()
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("Edit").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            var oldValue = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env).GetByID(Accountability.ID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(Accountability, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                StringBuilder Remarks = new StringBuilder();

                if (Accountability.PreloadedName != oldValue.PreloadedName)
                {
                    Remarks.Append(string.Concat("Preloaded Name changed from ", oldValue.PreloadedName, " to ", Accountability.PreloadedName, ". "));
                }

                if (Remarks.Length > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new EMS.Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "Accountability",
                            TableID = Accountability.ID,
                            Remarks = Remarks.ToString(),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }

                if (Accountability.AccountabilityDetailsList != null && Accountability.AccountabilityDetailsList.Count() > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "AccountabilityDetails",
                            TableID = Accountability.ID,
                            Remarks = string.Concat(Accountability.PreloadedName, " Details updated"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }
            }

            return new JsonResult(_resultView);
        }
    }
}