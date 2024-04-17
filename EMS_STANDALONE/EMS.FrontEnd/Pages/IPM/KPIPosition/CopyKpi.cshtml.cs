using EMS.FrontEnd.SharedClasses.Common_IPM;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.IPM.KPIPosition
{
    public class CopyKpiModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.IPM.Transfer.KPIPosition.CopyKpiPositionInput CopyKpiPositionInput { get; set; }
        [BindProperty]
        public string NewPositionName { get; set; }



        public CopyKpiModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }
        public async Task OnGet()
        {
        }

        public async Task<JsonResult> OnPostAsync()
        {
            var URL = string.Concat(_ipmBaseURL,
                   _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPIPosition").GetSection("CopyKpiPosition").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(CopyKpiPositionInput, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "KPIPosition",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(NewPositionName, " KPI added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }
            return new JsonResult(_resultView);
        }
    }
}