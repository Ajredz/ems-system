using EMS.FrontEnd.SharedClasses;
using EMS.Manpower.Transfer.MRF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Manpower.MRF.Admin
{
    public class IndexModel : MRF.IndexModel
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = true) : base(iconfiguration, env, IsAdminAccess)
        { }

        public override async Task OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/MANPOWER/MRF/ADMIN/ADD")).Count() > 0 ? "true" : "false";
                ViewData["HasExportFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/MANPOWER/MRF/ADMIN/EXPORT")).Count() > 0 ? "true" : "false";
            }

            if (_globalCurrentUser != null)
            {
                ViewData["OrgListFilter"] = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _manpowerBaseURL, _env)
                        .GetReferenceValueByRefCode(ReferenceCodes_Manpower.ORGLIST_FILTER.ToString())).FirstOrDefault().Value; 
            }
        }

    }
}