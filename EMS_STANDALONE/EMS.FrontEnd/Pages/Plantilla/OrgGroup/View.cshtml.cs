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

namespace EMS.FrontEnd.Pages.Plantilla.OrgGroup
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.OrgGroup.Form OrgGroup { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
        }

        public virtual async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/DELETE")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/EDIT")).Count() > 0 ? "true" : "false";
                //ViewData["HasEditDetailsFeature"] = ViewData["HasEditFeature"] + "" == "false" ? "true" : "false";
                ViewData["HasEditDetailsFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/EDITDETAILS")).Count() > 0 ? "true" : "false";
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

        public async Task<JsonResult> OnGetOrgGroup(int ID)
        {
            _resultView.Result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(ID);
            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionDropDownByID(int ID)
        {
            _resultView.Result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionDropdown(ID);
            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionDropDown(int PositionLevelID)
        {
            _resultView.Result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetDropdownByPositionLevel(PositionLevelID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetOrgGroupDropDown()
        {
            _resultView.Result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupDropDown();
            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionLevelDropDown()
        {
            _resultView.Result = await new Common_PositionLevel(_iconfiguration, _globalCurrentUser, _env).GetPositionLevelDropdown();
            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetOrgGroupEmployeeList(GetEmployeeListInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                      _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetOrgGroupEmployeeList").Value, "?",
                       "userid=", _globalCurrentUser.UserID,
                         "&sidx=", param.sidx,
                          "&sord=", param.sord,
                          "&pageNumber=", param.pageNumber,
                          "&rows=", param.rows,
                          "&ID=", param.ID,
                          "&PositionDelimited=", param.PositionDelimited,
                          "&EmployeeName=", param.EmployeeName
                       );

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetOrgGroupEmployeeOutput>(), URL);
            if (IsSuccess)
            {
                var jsonData = new
                {
                    total = Result.Count > 0 ? Result.FirstOrDefault().NoOfPages : 0,
                    param.pageNumber,
                    sort = param.sidx,
                    records = Result.Count > 0 ? Result.Last().TotalNoOfRecord : 0,
                    rows = Result
                };
                return new JsonResult(jsonData);
            }
            else
            {
                return new BadRequestObjectResult(ErrorMessage);
            }
        }

        public async Task<JsonResult> OnGetPositionAutoCompleteAsync(EMS.Plantilla.Transfer.Position.GetAutoCompleteInput param)
        {
            var result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionAutoComplete(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetChildrenOrgDropDown(GetByIDInput param)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetChildrenOrgDropDown(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }  
        
        public async Task<JsonResult> OnGetOrgGroupPosition(GetByIDInput param)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupPosition(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetOrgGroupNPRF(int OrgGroupID)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupNPRF(OrgGroupID);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

    }
}