
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

namespace EMS.FrontEnd.Pages.Plantilla.OrgGroup
{
    public class ChartModel : SharedClasses.Utilities
    {
      
        public ChartModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
        }

        public virtual async Task OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasOrgGroupAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/ADD")).Count() > 0 ? "true" : "false";
                ViewData["HasOrgGroupEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/EDIT")).Count() > 0 ? "true" : "false";
                ViewData["HasPositionAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/POSITION/ADD")).Count() > 0 ? "true" : "false";
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

        public async Task<JsonResult> OnGetOrgGroupByOrgType([FromQuery] string OrgType)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupByOrgType(OrgType);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetChart(GetChartInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetChart").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "OrgGroupID=", param.OrgGroupID, "&",
                  "Depth=", param.Depth, "&",
                  "ShowClosedBranches=", param.ShowClosedBranches, "&",
                  "AdminAccess.OrgGroupDescendantsDelimited=", string.Join(",", _globalCurrentUser.OrgGroupDescendants ?? new List<int>()), "&",
                  "AdminAccess.IsAdminAccess=", _IsAdminAccess
                  );

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetChartOutput>(), URL);

            if (IsSuccess)
            {
                return new JsonResult(Result);
            }
            else
            {
                return new BadRequestObjectResult(ErrorMessage);
            }

        }
        
        public async Task<IActionResult> OnGetChartPosition(int OrgGroupID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetChartPosition").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "orggroupid=", OrgGroupID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetChartPositionOutput>(), URL);

            if (IsSuccess)
            {
                return new JsonResult(Result);
            }
            else
            {
                return new BadRequestObjectResult(ErrorMessage);
            }

        }

        public async Task<JsonResult> OnGetUserOrgGroup()
        {
            _resultView.Result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(_globalCurrentUser.OrgGroupID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
    }
}