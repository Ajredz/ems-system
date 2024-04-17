using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Manpower;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.Manpower.Transfer.DataDuplication.Position;
using EMS.Manpower.Transfer.MRFSignatories;
using EMS.Plantilla.Transfer.OrgGroup;
using EMS.Plantilla.Transfer.PositionLevel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Manpower.MRF
{
    public class AddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Manpower.Transfer.MRF.Form MRF { get; set; }

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
        }

        public async Task OnGet()
        {
            if (_globalCurrentUser != null)
            {
                MRF = new EMS.Manpower.Transfer.MRF.Form
                {
                    Status = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _manpowerBaseURL, _env)
                    .GetReferenceValueByRefCodeAndValue(
                        ReferenceCodes_Manpower.MRF_STATUS.ToString(), MRF_STATUS.FOR_APPROVAL.ToString())).Description,
                        MRFTransactionID = "",
                    Vacancy = 1
                };



                ViewData["OrgGroupSelectList"] = 
                    await new Common_Synced_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                    .GetOrgGroupDropDown(new EMS.Manpower.Transfer.DataDuplication.OrgGroup.GetDropDownInput
                    { 
                        ID = _globalCurrentUser.OrgGroupID,
                        AdminAccess = new EMS.Manpower.Transfer.Shared.AdminAccess { 
                            CurrentUserOrgGroupID = _globalCurrentUser.OrgGroupID,
                            IsAdminAccess = _IsAdminAccess,
                            OrgGroupDescendantsDelimited =
                            _globalCurrentUser.OrgGroupRovingDescendants?.Count > 0 ?
                         string.Join(",", _globalCurrentUser.OrgGroupDescendants
                            .Union(_globalCurrentUser.OrgGroupRovingDescendants))
                        : string.Join(",", _globalCurrentUser.OrgGroupDescendants ?? new List<int>())
                        }
                    });


                //ViewData["PositionSelectList"] = await new Common_Synced_Position(_iconfiguration, _globalCurrentUser, _env)
                //    .GetDropDownByParentPositionID(
                //new GetDropDownByParentPositionIDInput
                //{
                //    ParentPositionID = _globalCurrentUser.PositionID,
                //    SelectedValue = 0
                //});


                var natureOfEmployment = await new Common_Reference(_iconfiguration, _globalCurrentUser, _manpowerBaseURL, _env).GetReferenceValueByRefCode(
                    ReferenceCodes_Manpower.NATURE_OF_EMPLOYMENT.ToString());

                ViewData["NatureOfEmploymentSelectList"] = natureOfEmployment.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Description
                    }).ToList();

                var purpose = await new Common_Reference(_iconfiguration, _globalCurrentUser, _manpowerBaseURL, _env).GetReferenceValueByRefCode(
                    ReferenceCodes_Manpower.MRF_PURPOSE.ToString());

                ViewData["PurposeSelectList"] = purpose.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Description
                    }).ToList();

            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            EMS.Plantilla.Transfer.OrgGroup.GetByOrgGroupIDAndPositionIDOutput manpowerCount =
               await new SharedClasses.Common_Plantilla.Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetByOrgGroupAndPosition(
                   new EMS.Plantilla.Transfer.OrgGroup.GetByOrgGroupIDAndPositionIDInput
                   {
                       OrgGroupID = MRF.OrgGroupID,
                       PositionID = MRF.PositionID
                   });

            _resultView = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env)
                .PostValidateExistingActual(new EMS.Manpower.Transfer.MRF.ValidateMRFExistingActualInput
                {
                    OrgGroupID = MRF.OrgGroupID,
                    PositionID = MRF.PositionID,
                    PlannedCount = manpowerCount.PlannedCount,
                    ActiveCount = manpowerCount.ActiveCount,
                    InactiveCount = manpowerCount.InactiveCount,
                });
            
            if (_resultView.IsSuccess)
            {
                MRF.CreatedBy = _globalCurrentUser.UserID;
                MRF.Vacancy = 1;

                var URL = string.Concat(_manpowerBaseURL,
                        _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("Add").Value, "?",
                        "userid=", _globalCurrentUser.UserID);

                var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(MRF, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "MRF",
                            TableID = 0, // New Record, no ID yet
                            Remarks = string.Concat(MRF.MRFTransactionID, " added"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionDropDownWithCount([FromQuery] EMS.Plantilla.Transfer.Position.GetDropdownByOrgGroupInput param)
        {
            _resultView.Result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env)
                .GetDropdownWithCountByOrgGroup(
                new EMS.Plantilla.Transfer.Position.GetDropdownByOrgGroupInput
                {
                    OrgGroupID = param.OrgGroupID,
                    SelectedValue = 0
                });
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetOrgGroupRollupPositionDropdown([FromQuery] int OrgGroupID)
        {
            _resultView.Result = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupRollupPositionDropdown(OrgGroupID)).Select(x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.Position
                });
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetOrgGroupType([FromQuery] int OrgGroupID)
        {
            _resultView.Result = (await new Common_OrgGroup(_iconfiguration,_globalCurrentUser,_env)
                .GetOrgGroup(OrgGroupID)).OrgType;
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

    }
}