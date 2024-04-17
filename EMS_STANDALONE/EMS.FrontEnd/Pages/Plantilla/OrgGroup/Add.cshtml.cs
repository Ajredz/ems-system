using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using EMS.FrontEnd.SharedClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using EMS.FrontEnd.SharedClasses.Common_Security;

namespace EMS.FrontEnd.Pages.Plantilla.OrgGroup
{
    public class AddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.OrgGroup.Form OrgGroup { get; set; }

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
        }

        public async Task OnGet()
        {
            if (_globalCurrentUser != null)
            {
                OrgGroup = new EMS.Plantilla.Transfer.OrgGroup.Form();

                //ViewData["RegionSelectList"] = await new Common_Region(_iconfiguration, _globalCurrentUser).GetRegionDropDown();

                var result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.ORGGROUPTYPE.ToString());
                ViewData["OrgTypeSelectList"] = result.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Description
                    }).ToList();

                ViewData["OrgGroupSelectList"] = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupDropDown();

                //var result1 = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL).GetReferenceValueByRefCode(ReferenceCodes.COMPANY_TAG.ToString());
                //ViewData["CompanyTagSelectList"] = result1.Select(
                //    x => new SelectListItem
                //    {
                //        Value = x.Value,
                //        Text = x.Description
                //    }).ToList();

                var CategorySelectList = await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.ORGGROUPCATEGORY.ToString());
                ViewData["CategorySelectList"] = CategorySelectList.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Description
                    }).ToList();
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            OrgGroup.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_plantillaBaseURL,
                               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("Add").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(OrgGroup, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "OrgGroup",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(OrgGroup.Code, " added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                if (OrgGroup.OrgGroupPositionList != null)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "OrgGroupPosition",
                            TableID = 0, // New Record, no ID yet
                            Remarks = string.Concat(OrgGroup.Code, " OrgGroup Position added"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }

                if (OrgGroup.OrgGroupTagList != null)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "OrgGroupTag",
                            TableID = 0, // New Record, no ID yet
                            Remarks = string.Concat(OrgGroup.Code, " OrgGroup Tag added"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }

                if (OrgGroup.OrgGroupNPRFList != null)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "OrgGroupNPRF",
                            TableID = 0, // New Record, no ID yet
                            Remarks = string.Concat(OrgGroup.Code, " OrgGroup NPRF added"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
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
    }
}