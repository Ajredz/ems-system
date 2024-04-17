using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Security.Transfer.SystemRole;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Utilities.API;
using EMS.FrontEnd.SharedClasses.Common_Security;
using System.Text;

namespace EMS.FrontEnd.Pages
{
    public class ForceChangePasswordModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Security.Transfer.SystemUser.ForceChangePasswordInput ChangePassword { get; set; }


        public ForceChangePasswordModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
            if (_systemURL != null)
            {
            }
        }


        public async Task<JsonResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var URL = string.Concat(_securityBaseURL,
                   _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("ForceChangePassword").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

                var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(ChangePassword, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {
                    // Assign IP Address
                    var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                    if (Request.Headers.ContainsKey("X-Forwarded-For"))
                        remoteIpAddress = Request.Headers["X-Forwarded-For"];
                    else if (Request.Headers.ContainsKey("X-Forwarded-Proto"))
                        remoteIpAddress = Request.Headers["X-Forwarded-Proto"];

                    _globalCurrentUser.IPAddress = remoteIpAddress;
                    _globalCurrentUser.ComputerName = GetComputerNameByIP(remoteIpAddress);

                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.FORCE_CHANGE_PASSWORD.ToString(),
                            TableName = "SystemUser",
                            TableID = _globalCurrentUser.UserID,
                            Remarks = "Force Change Password.",
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