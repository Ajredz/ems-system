using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.Plantilla.Transfer.OrgGroup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;
using Utilities.API.ReferenceMaintenance;

namespace EMS_FrontEnd.Pages.Plantilla.OrgGroup.Admin
{
    public class EditDetailsModel : OrgGroup.EditDetailsModel
    {

        public EditDetailsModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = true) : base(iconfiguration, env, IsAdminAccess)
        {
            _IsAdminAccess = IsAdminAccess;
        }

        public override async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/DELETE")).Count() > 0 ? "true" : "false";
            }

            OrgGroup = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(ID);

            //ViewData["RegionSelectList"] = await new Common_Region(_iconfiguration, _globalCurrentUser).GetRegionDropDown(OrgGroup.RegionID);

            var result = await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.ORGGROUPTYPE.ToString());
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
                    await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
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
        }
    }
}