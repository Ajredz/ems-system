using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_IPM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.FrontEnd.Pages.IPM.KPI
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.IPM.Transfer.KPI.Form KPI { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/KPI/DELETE")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/KPI/EDIT")).Count() > 0 ? "true" : "false";
            }

            if (_globalCurrentUser != null)
            {
                KPI = await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetKPI(ID);

                ViewData["KRAGroupSelectList"] =
                    await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetKRAGroupDropdown(KPI.KRAGroup);

                ViewData["KRASubGroupSelectList"] =
                    await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetKRASubGroupDropdown(KPI.KRAGroup, KPI.KRASubGroup.Value);

                ViewData["KPITypeSelectList"] =
                    await new Common_Reference(_iconfiguration, _globalCurrentUser, _ipmBaseURL, _env).GetDropDownByRefCode(ReferenceCodes_IPM.KPI_TYPE.ToString(), KPI.Type);

                ViewData["KPISourceTypeSelectList"] =
                    await new Common_Reference(_iconfiguration, _globalCurrentUser, _ipmBaseURL, _env).GetDropDownByRefCode(ReferenceCodes_IPM.KPI_SOURCE_TYPE.ToString(), KPI.SourceType);
            }
        }
    }
}