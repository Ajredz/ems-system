using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using EMS.FrontEnd.SharedClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using EMS.FrontEnd.SharedClasses.Common_Security;

namespace EMS.FrontEnd.Pages.IPM.KPI
{
    public class DeleteModel : SharedClasses.Utilities
    {
        public DeleteModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task<JsonResult> OnPostAsync(int ID)
        {
            var URL = string.Concat(_ipmBaseURL,
                    _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPI").GetSection("Delete").Value, "?",
                    "userid=", _globalCurrentUser.UserID, "&",
                    "id=", ID);

            var (IsSuccess, Message) = await SharedUtilities.DeleteFromAPI(URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.DELETE.ToString(),
                        TableName = "KPI",
                        TableID = ID,
                        Remarks = string.Concat(ID, " deleted"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });;
            }

            return new JsonResult(_resultView);
        }

    }
}