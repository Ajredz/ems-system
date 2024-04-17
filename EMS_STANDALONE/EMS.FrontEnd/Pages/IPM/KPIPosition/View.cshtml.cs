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

namespace EMS.FrontEnd.Pages.IPM.KPIPosition
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.IPM.Transfer.KPIPosition.Form KPIPosition { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID, string EffectiveDate)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/KPIPOSITION/DELETE")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/KPIPOSITION/EDIT")).Count() > 0 ? "true" : "false";
                ViewData["HasCopyKpiFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/KPIPOSITION/COPYKPI")).Count() > 0 ? "true" : "false";
            }

            if (_globalCurrentUser != null)
            {
                KPIPosition = await new Common_KPIPosition(_iconfiguration, _globalCurrentUser, _env).GetKPIPosition(ID, EffectiveDate);

                ViewData["KPIList"] =
                    await new Common_KPIPosition(_iconfiguration, _globalCurrentUser, _env).GetAllKPI();

                ViewData["PositionSelectList"] =
                    await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionDropdown();
            }
        }
    }
}