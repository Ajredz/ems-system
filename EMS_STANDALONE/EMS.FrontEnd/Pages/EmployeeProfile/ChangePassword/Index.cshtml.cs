using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.EmployeeProfile.ChangePassword
{
    public class IndexModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Security.Transfer.SystemUser.ChangePasswordInput ChangePassword { get; set; }

        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task<JsonResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var URL = string.Concat(_securityBaseURL,
                   _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("ChangePassword").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

                var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(ChangePassword, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.CHANGE_PASSWORD.ToString(),
                            TableName = "SystemUser",
                            TableID = _globalCurrentUser.UserID,
                            Remarks = "Change Password.",
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
