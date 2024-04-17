
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using EMS.FrontEnd.SharedClasses;
using EMS.Plantilla.Transfer.OrgGroup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using NPOI.HSSF.Util;

namespace EMS.FrontEnd.Pages.Plantilla.OrgGroup.Admin
{
    public class ChartModel : OrgGroup.ChartModel
    {
        public ChartModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = true) : base(iconfiguration, env, IsAdminAccess)
        {
        }

        public override async Task OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasOrgGroupAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/ADMIN/ADD")).Count() > 0 ? "true" : "false";
                ViewData["HasOrgGroupEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/ADMIN/EDIT")).Count() > 0 ? "true" : "false";
                ViewData["HasPositionAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/POSITION/ADMIN/ADD")).Count() > 0 ? "true" : "false";
            }

            if (_globalCurrentUser != null)
            {
                var orgTypes = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCode(
                      EMS.Plantilla.Transfer.Enums.ReferenceCodes.ORGGROUPTYPE.ToString());

                ViewData["OrgTypeSelectList"] = orgTypes.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Description
                    }).ToList();


                var URL = string.Concat(_plantillaBaseURL,
                 _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetOrgGroupHierarchyLevelsDropDown").Value, "?",
                 "userid=", _globalCurrentUser.UserID);

                ViewData["OrgGroupLevels"] = (await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL)).APIResult;

            }
        }


    }
}