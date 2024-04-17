using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using Utilities.API.ReferenceMaintenance;

namespace EMS_FrontEnd.Pages.Plantilla.OrgGroup.Admin
{
    public class UpdatePlantillaModel : OrgGroup.UpdatePlantillaModel
    {
        public UpdatePlantillaModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = true) : base(iconfiguration, env, IsAdminAccess)
        {
        }

        public override async Task OnGetAsync(int OrgGroupID)
        {
            if (_systemURL != null)
            {
                ViewData["HasUpdatePlannedFeature"] =
                    _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/ADMIN/UPDATEPLANTILLA")).Count() > 0 ? "true" : "false";
                ViewData["HasUpdateActiveFeature"] =
                    _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/ADMIN/UPDATEPLANTILLA?HANDLER=ACTIVE")).Count() > 0 ? "true" : "false";
                ViewData["HasUpdateInactiveFeature"] =
                    _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/ADMIN/UPDATEPLANTILLA?HANDLER=INACTIVE")).Count() > 0 ? "true" : "false";
            }

            if (
                _IsAdminAccess |
                (
                    !_IsAdminAccess & _globalCurrentUser.OrgGroupDescendants.Contains(OrgGroupID)
                )
            )
            {
                PlantillaCount = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupPositionWithDescription(OrgGroupID);
            }
        }

        public override async Task<JsonResult> OnPostAsync()
        {
            bool HasUpdatePlanned =
                _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/ADMIN/UPDATEPLANTILLA?HANDLER=PLANNED")).Count() > 0 ? true : false;
            bool HasUpdateActive =
                _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/ADMIN/UPDATEPLANTILLA?HANDLER=ACTIVE")).Count() > 0 ? true : false;
            bool HasUpdateInactive =
                _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/ADMIN/UPDATEPLANTILLA?HANDLER=INACTIVE")).Count() > 0 ? true : false;

            List<EMS.Plantilla.Transfer.OrgGroup.PlantillaCountUpdateForm> originalPlantillaCount =
                await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupPositionWithDescription(
                    PlantillaCount != null ? PlantillaCount.First().OrgGroupID : 0
                );

            PlantillaCount = PlantillaCount.Select(x => {
                x.ModifiedBy = _globalCurrentUser.UserID;
                if (!HasUpdatePlanned)
                    x.PlannedCount = originalPlantillaCount.Where(y => y.PositionID == x.PositionID).First().PlannedCount;
                if (!HasUpdateActive)
                    x.ActiveCount = originalPlantillaCount.Where(y => y.PositionID == x.PositionID).First().ActiveCount;
                if (!HasUpdateInactive)
                    x.InactiveCount = originalPlantillaCount.Where(y => y.PositionID == x.PositionID).First().InactiveCount;

                return x;
            }).ToList();

            var URL = string.Concat(_plantillaBaseURL,
                _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("UpdatePlantillaCount").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(PlantillaCount, URL);

            if (IsSuccess)
            {
                if (NPRFList != null)
                {
                    NPRFList = NPRFList.Select(x =>
                    {
                        if (x.File != null)
                        {
                            string dateTimePreFix = DateTime.Now.ToString("yyyyMMddHHmmss") + "_";
                            x.ServerFile = string.Concat(dateTimePreFix, Guid.NewGuid().ToString("N").Substring(0, 4), "_", x.File.FileName);
                            x.SourceFile = x.File.FileName;
                        }
                        return x;
                    }).ToList();
                }

                var NPRFURL = string.Concat(_plantillaBaseURL,
                                 _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("AddNPRF").Value, "?",
                                 "userid=", _globalCurrentUser.UserID);

                var (NPRFIsSuccess, NPRFMessage) = await SharedUtilities.PostFromAPI(NPRFList, NPRFURL);

                if (NPRFIsSuccess)
                {
                    if (NPRFList != null)
                    {
                        foreach (var item in NPRFList)
                        {
                            if (item.File != null)
                            {
                                await CopyToServerPath(Path.Combine(_env.WebRootPath,
                               _iconfiguration.GetSection("PlantillaService_OrgGroup_Attachment_Path").Value), item.File, item.ServerFile);
                            }
                        }
                    }
                }
            }

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}