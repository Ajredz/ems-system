using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMS.FrontEnd.Pages.Administrator.BranchInfo
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.OrgGroup.Form OrgGroup { get; set; }


        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/BRANCHINFO/EDIT")).Count() > 0 ? "true" : "false";
            }

            OrgGroup = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(ID);

            ViewData["PsgcAddress"] = (OrgGroup.Psgc == null ? "" : (await new Common_PSGCPantillla(_iconfiguration, _globalCurrentUser, _env).GetPSGCAutoComplete(OrgGroup.Psgc, 1)).Select(x => x.Description).FirstOrDefault());

            var result = await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.ORGGROUPCATEGORY.ToString());
            ViewData["CategorySelectList"] = result.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description
                }).ToList();

            if (OrgGroup.CSODAM > 0)
            {
                var OrgCSODAM = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(Convert.ToInt32(OrgGroup.CSODAM));
                ViewData["CSODAM"] = OrgCSODAM.Code + " - " + OrgCSODAM.Description;
            }
            else
                ViewData["CSODAM"] = "-";
            if (OrgGroup.HRBP > 0)
            {
                var OrgHRBP = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(Convert.ToInt32(OrgGroup.HRBP));
                ViewData["HRBP"] = OrgHRBP.Code + " - " + OrgHRBP.Description;
            }
            else
                ViewData["HRBP"] = "-";
            if (OrgGroup.RRT > 0)
            {
                var OrgRRT = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(Convert.ToInt32(OrgGroup.RRT));
                ViewData["RRT"] = OrgRRT.Code + " - " + OrgRRT.Description;
            }
            else
                ViewData["RRT"] = "-";
        }

        
    }
}