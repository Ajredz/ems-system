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

namespace EMS_FrontEnd.Pages.Plantilla.OrgGroup
{
    public class EditModel : EMS.FrontEnd.SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.OrgGroup.Form OrgGroup { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
        }

        public virtual async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/DELETE")).Count() > 0 ? "true" : "false";
            }

            OrgGroup = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(ID);

            if (OrgGroup.ParentOrgID != 0)
            {
                var ParentOrgGroup = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(Convert.ToInt32(OrgGroup.ParentOrgID));

                OrgGroup.ParentOrgDescription = ParentOrgGroup.Code + " - " + ParentOrgGroup.Description;
            }

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

        public async Task<JsonResult> OnPostAsync()
        {
            OrgGroup.CreatedBy = _globalCurrentUser.UserID;

            if (OrgGroup.OrgGroupNPRFList != null)
            {
                OrgGroup.OrgGroupNPRFList = OrgGroup.OrgGroupNPRFList.Select(x =>
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
            

            var URL = string.Concat(_plantillaBaseURL,
                _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("Edit").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var oldValue = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(OrgGroup.ID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(OrgGroup, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                StringBuilder Remarks = new StringBuilder();

                if (OrgGroup.ParentOrgID != oldValue.ParentOrgID)
                {
                    Remarks.Append(string.Concat("ParentOrgID changed from ", oldValue.ParentOrgID, " to ", OrgGroup.ParentOrgID, ". "));
                }

                if (OrgGroup.Code != oldValue.Code)
                {
                    Remarks.Append(string.Concat("Code changed from ", oldValue.Code, " to ", OrgGroup.Code, ". "));
                }

                if (OrgGroup.Description != oldValue.Description)
                {
                    Remarks.Append(string.Concat("Description changed from ", oldValue.Description, " to ", OrgGroup.Description, ". "));
                }

                if (OrgGroup.OrgType != oldValue.OrgType)
                {
                    Remarks.Append(string.Concat("OrgType changed from ", oldValue.OrgType, " to ", OrgGroup.OrgType, ". "));
                }

                if (OrgGroup.Address != oldValue.Address)
                {
                    Remarks.Append(string.Concat("Code changed from ", oldValue.Address, " to ", OrgGroup.Address, ". "));
                }

                if (OrgGroup.IsBranchActive != oldValue.IsBranchActive)
                {
                    Remarks.Append(string.Concat("Changed from ", oldValue.IsBranchActive == true ? "Active" : "Inactive", " to ", OrgGroup.IsBranchActive == true ? "Active" : "Inactive", ". "));
                }

                if (OrgGroup.ServiceBayCount != oldValue.ServiceBayCount)
                {
                    Remarks.Append(string.Concat("ServiceBayCount changed from ", oldValue.ServiceBayCount, " to ", OrgGroup.ServiceBayCount, ". "));
                }


                if (Remarks.Length > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new EMS.Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "OrgGroup",
                            TableID = OrgGroup.ID,
                            Remarks = Remarks.ToString(),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }

                if (OrgGroup.OrgGroupPositionList != null && OrgGroup.OrgGroupPositionList.Count() > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new EMS.Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "OrgGroupPosition",
                            TableID = OrgGroup.ID,
                            Remarks = string.Concat(OrgGroup.Code, " OrgGroup Position updated"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }

                if (OrgGroup.OrgGroupTagList != null && OrgGroup.OrgGroupTagList.Count() > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new EMS.Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "OrgGroupTag",
                            TableID = OrgGroup.ID,
                            Remarks = string.Concat(OrgGroup.Code, " OrgGroup Tag updated"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }

                if (OrgGroup.OrgGroupNPRFList != null && OrgGroup.OrgGroupNPRFList.Count() > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new EMS.Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "OrgGroupNPRF",
                            TableID = OrgGroup.ID,
                            Remarks = string.Concat(OrgGroup.Code, " OrgGroup NPRF updated"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });

                    foreach (var item in OrgGroup.OrgGroupNPRFList)
                    {
                        if (item.File != null)
                        {
                            await CopyToServerPath(Path.Combine(_env.WebRootPath,
                           _iconfiguration.GetSection("PlantillaService_OrgGroup_Attachment_Path").Value), item.File, item.ServerFile);
                        }
                    }
                }
            }

            return new JsonResult(_resultView);
        }

        //public async Task<JsonResult> OnGetRegionDropDown()
        //{
        //    _resultView.Result = await new Common_Region(_iconfiguration, _globalCurrentUser).GetRegionDropDown();
        //    _resultView.IsSuccess = true;

        //    return new JsonResult(_resultView);
        //}

        //public async Task<JsonResult> OnGetOrgGroupDropDown()
        //{
        //    _resultView.Result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupDropDown();
        //    _resultView.IsSuccess = true;

        //    return new JsonResult(_resultView);
        //}

        //public async Task<JsonResult> OnGetPositionLevelDropDown()
        //{
        //    _resultView.Result = await new Common_PositionLevel(_iconfiguration, _globalCurrentUser, _env).GetPositionLevelDropdown();
        //    _resultView.IsSuccess = true;

        //    return new JsonResult(_resultView);
        //}

        //public async Task<JsonResult> OnGetPositionDropDown(int PositionLevelID)
        //{
        //    _resultView.Result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetDropdownByPositionLevel(PositionLevelID);
        //    _resultView.IsSuccess = true;
        //    return new JsonResult(_resultView);
        //}

        //public async Task<JsonResult> OnGetPositionDropDownByID(int PositionLevelID, int ID)
        //{
        //    _resultView.Result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetDropdownByPositionLevel(PositionLevelID, ID);
        //    _resultView.IsSuccess = true;

        //    return new JsonResult(_resultView);
        //}
    }
}