using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.Manpower.Transfer.DataDuplication.OrgGroup;
using EMS.Plantilla.Transfer.OrgGroup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using Utilities.API.ReferenceMaintenance;

namespace EMS.FrontEnd.Pages.Plantilla.OrgGroup.Admin
{
    public class ViewModel : OrgGroup.ViewModel
    {

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = true) : base(iconfiguration, env, IsAdminAccess)
        {
            
        }

        public override async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/ADMIN/DELETE")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/ADMIN/EDIT")).Count() > 0 ? "true" : "false";
                ViewData["HasEditDetailsFeature"] = ViewData["HasEditFeature"] + "" == "false" ? "true" : "false";
            }

            OrgGroup = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(ID);

            //ViewData["RegionSelectList"] = await new Common_Region(_iconfiguration, _globalCurrentUser).GetRegionDropDown(OrgGroup.RegionID);

            var result = await new SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.ORGGROUPTYPE.ToString());
            ViewData["OrgTypeSelectList"] = result.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description
                }).ToList();

            ViewData["OrgGroupSelectList"] = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupDropDown();

            if (OrgGroup.OrgGroupTagList?.Count > 0)
            {
                foreach (var item in OrgGroup.OrgGroupTagList)
                {
                    List<ReferenceValue> referenceValues =
                    await new SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(item.Code);

                    ViewData[string.Concat(item.Code, "SelectList")] = referenceValues.Select(
                        x => new SelectListItem
                        {
                            Value = x.Value,
                            Text = x.Description,
                            Selected = x.Value == item.Value
                        }).ToList();
                }
            }

            var CategorySelectList = await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.ORGGROUPCATEGORY.ToString());
            ViewData["CategorySelectList"] = CategorySelectList.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description
                }).ToList();

            if (OrgGroup.ParentOrgID > 0)
            {
                var parentOrg = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(OrgGroup.ParentOrgID.Value);
                OrgGroup.ParentOrgDescription = string.Concat(parentOrg.Code, " - ", parentOrg.Description);
            }
            if (OrgGroup.CSODAM > 0)
            {
                var OrgCSODAM = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(Convert.ToInt32(OrgGroup.CSODAM));
                ViewData["CSODAM"] = OrgCSODAM.Code + " - " + OrgCSODAM.Description;
                ViewData["CSODAMValue"] = OrgGroup.CSODAM;
            }
            if (OrgGroup.HRBP > 0)
            {
                var OrgHRBP = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(Convert.ToInt32(OrgGroup.HRBP));
                ViewData["HRBP"] = OrgHRBP.Code + " - " + OrgHRBP.Description;
                ViewData["HRBPValue"] = OrgGroup.HRBP;
            }
            if (OrgGroup.RRT > 0)
            {
                var OrgRRT = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(Convert.ToInt32(OrgGroup.RRT));
                ViewData["RRT"] = OrgRRT.Code + " - " + OrgRRT.Description;
                ViewData["RRTValue"] = OrgGroup.RRT;
            }
        }


    }
}