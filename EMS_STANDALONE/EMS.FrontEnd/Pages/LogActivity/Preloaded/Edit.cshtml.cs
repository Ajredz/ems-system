using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Utilities.API;

namespace EMS.FrontEnd.Pages.LogActivity.Preloaded
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.LogActivityPreloadedForm LogActivityPreloaded { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync(int ID)
        {

            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/PRELOADED/DELETE")).Count() > 0 ? "true" : "false";
            }

            LogActivityPreloaded = await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env).GetLogActivityPreloadedByID(ID);
        }

        public async Task<JsonResult> OnPostAsync()
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("EditLogActivityPreloaded").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            var oldValue = await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env).GetLogActivityPreloadedByID(LogActivityPreloaded.ID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(LogActivityPreloaded, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                StringBuilder Remarks = new StringBuilder();

                if (LogActivityPreloaded.PreloadedName != oldValue.PreloadedName)
                {
                    Remarks.Append(string.Concat("Preloaded Name changed from ", oldValue.PreloadedName, " to ", LogActivityPreloaded.PreloadedName, ". "));
                }

                if (Remarks.Length > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new EMS.Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "LogActivityPreloaded",
                            TableID = LogActivityPreloaded.ID,
                            Remarks = Remarks.ToString(),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }

                if (LogActivityPreloaded.LogActivityList != null && LogActivityPreloaded.LogActivityList.Count() > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "LogActivity",
                            TableID = LogActivityPreloaded.ID,
                            Remarks = string.Concat(LogActivityPreloaded.PreloadedName, " Activity updated"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }
            }

            return new JsonResult(_resultView);
        }
    }
}