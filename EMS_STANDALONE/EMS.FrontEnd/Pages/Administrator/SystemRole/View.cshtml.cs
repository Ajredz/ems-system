using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace EMS.FrontEnd.Pages.Administrator.SystemRole
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Security.Transfer.SystemRole.Form SystemRole { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/SYSTEMROLE/DELETE")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/SYSTEMROLE/EDIT")).Count() > 0 ? "true" : "false";
            }

            SystemRole = await new Common_SystemRole(_iconfiguration, _globalCurrentUser, _env).GetSystemRole(ID);
        }

        
    }
}