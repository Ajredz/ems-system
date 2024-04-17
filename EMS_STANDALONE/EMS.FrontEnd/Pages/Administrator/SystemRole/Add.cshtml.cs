using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Administrator.SystemRole
{
    public class AddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Security.Transfer.SystemRole.Form SystemRole { get; set; }

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet()
        {
            if (_globalCurrentUser != null)
            {
                SystemRole = new EMS.Security.Transfer.SystemRole.Form();
                SystemRole.SystemRoleAccessList = await new Common_SystemRole(_iconfiguration, _globalCurrentUser, _env).GetRolePage();
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            SystemRole.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_securityBaseURL,
                               _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemRole").GetSection("Add").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(SystemRole, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }

    }
}