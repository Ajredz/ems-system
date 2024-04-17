using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Administrator.SystemUser
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Security.Transfer.SystemUser.Form SystemUser { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/SYSTEMUSER/DELETE")).Count() > 0 ? "true" : "false";
            }

            SystemUser = await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env).GetSystemUserByID(ID);
        }

        public async Task<JsonResult> OnPostAsync()
        {

            var URL = string.Concat(_securityBaseURL,
                _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("UpdateSystemUserRole").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var oldValue = await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env).GetSystemUserByID(SystemUser.ID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(SystemUser, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                StringBuilder Remarks = new StringBuilder();

                if (SystemUser.FirstName != oldValue.FirstName)
                {
                    Remarks.Append(string.Concat("FirstName changed from ", oldValue.FirstName, " to ", SystemUser.FirstName, ". "));
                }
                 if (SystemUser.MiddleName != oldValue.MiddleName)
                {
                    Remarks.Append(string.Concat("MiddleName changed from ", oldValue.MiddleName, " to ", SystemUser.MiddleName, ". "));
                }
                 if (SystemUser.LastName != oldValue.LastName)
                {
                    Remarks.Append(string.Concat("LastName changed from ", oldValue.LastName, " to ", SystemUser.LastName, ". "));
                }
                 if (SystemUser.IsActive != oldValue.IsActive)
                {
                    Remarks.Append(string.Concat("IsActive changed from ", oldValue.IsActive, " to ", SystemUser.IsActive, ". "));
                }

                if (Remarks.Length > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "SystemUser",
                            TableID = SystemUser.ID,
                            Remarks = Remarks.ToString(),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        }); ;
                }
            }

            return new JsonResult(_resultView);
        }
    }
}