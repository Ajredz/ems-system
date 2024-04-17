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

namespace EMS.FrontEnd.Pages.IPM.KPIScore
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.IPM.Transfer.KPIScore.Form KPIScore { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID, string KPIType)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/KPISCORE/DELETE")).Count() > 0 ? "true" : "false";
                //ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/KPISCORE/EDIT")).Count() > 0 ? "true" : "false";
            }

            if (_globalCurrentUser != null)
            {
                KPIScore = await new Common_KPIScore(_iconfiguration, _globalCurrentUser, _env)
                    .GetKPIScore(new EMS.IPM.Transfer.KPIScore.GetByIDInput { ID = ID, KPIType = KPIType});

                if (KPIType.Equals("QUANTITATIVE"))
                {
                    var orgGroup = await new SharedClasses.Common_Plantilla.Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(KPIScore.OrgGroup);
                    ViewData["OrgGroupDescription"] = string.Concat(orgGroup.Code, " - ", orgGroup.Description);
                }
                else /*QUALITATIVE*/
                {
                    var employee = await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                        .GetEmployee(KPIScore.EmployeeID);
                    ViewData["EmployeeName"] =
                    employee == null ? "" : string.Concat((employee.PersonalInformation.LastName ?? "").Trim(),
                                                string.IsNullOrEmpty((employee.PersonalInformation.FirstName ?? "").Trim()) ? "" : string.Concat(", ", employee.PersonalInformation.FirstName),
                                                string.IsNullOrEmpty((employee.PersonalInformation.MiddleName ?? "").Trim()) ? "" : string.Concat(" ", employee.PersonalInformation.MiddleName));
                }

                ViewData["KPIType"] = KPIType;
            }
        }
    }
}