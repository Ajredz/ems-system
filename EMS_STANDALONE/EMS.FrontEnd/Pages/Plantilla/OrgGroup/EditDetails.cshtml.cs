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
    public class EditDetailsModel : EMS.FrontEnd.SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.OrgGroup.Form OrgGroup { get; set; }

        public EditDetailsModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
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
        }

        public async Task<JsonResult> OnPostAsync()
        {
            OrgGroup.CreatedBy = _globalCurrentUser.UserID;            

            var URL = string.Concat(_plantillaBaseURL,
                _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("Edit").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var oldValue = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(OrgGroup.ID);

            OrgGroup.Code = oldValue.Code;
            OrgGroup.Description = oldValue.Description;
            OrgGroup.OrgType = oldValue.OrgType;
            OrgGroup.ParentOrgID = oldValue.ParentOrgID;

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(OrgGroup, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                StringBuilder Remarks = new StringBuilder();

                if (OrgGroup.Address != oldValue.Address)
                {
                    Remarks.Append(string.Concat("Code changed from ", oldValue.Address, " to ", OrgGroup.Address, ". "));
                }

                if (OrgGroup.IsBranchActive != oldValue.IsBranchActive)
                {
                    Remarks.Append(string.Concat("Changed from ", oldValue.IsBranchActive == true ? "Active" : "Inactive", " to ", OrgGroup.IsBranchActive == true ? "Active" : "Inactive", ". "));
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
            }

            return new JsonResult(_resultView);
        }
    }
}