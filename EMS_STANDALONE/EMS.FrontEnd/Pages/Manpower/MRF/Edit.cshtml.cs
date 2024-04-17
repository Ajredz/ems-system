using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Manpower;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.Manpower.Transfer.DataDuplication.Position;
using EMS.Manpower.Transfer.DataDuplication.PositionLevel;
using EMS.Manpower.Transfer.MRFSignatories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Manpower.MRF
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Manpower.Transfer.MRF.Form MRF { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
        }

        public async Task OnGetAsync(int ID)
        {
            if (_globalCurrentUser != null)
            {
                if (_systemURL != null)
                {
                    ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/MANPOWER/MRF/DELETE")).Count() > 0 ? "true" : "false";
                }

                MRF = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env).GetMRF(ID);

                var orgGroup = await new Common_Synced_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupByID(MRF.OrgGroupID);

                ViewData["OrgGroupSelectList"] =
               new List<SelectListItem> {
                        new SelectListItem
                        {
                            Value = orgGroup.ID.ToString(),
                            Text = string.Concat(orgGroup.Code, " - ", orgGroup.Description),
                            Selected = true
                        }
                  };

                //ViewData["OrgGroupSelectList"] = 
                //    await new Common_Synced_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                //    .GetOrgGroupDropDown(new EMS.Manpower.Transfer.DataDuplication.OrgGroup.GetDropDownInput
                //    {
                //        ID = MRF.OrgGroupID,
                //        AdminAccess = new EMS.Manpower.Transfer.Shared.AdminAccess
                //        {
                //            CurrentUserOrgGroupID = _globalCurrentUser.OrgGroupID,
                //            IsAdminAccess = _IsAdminAccess,
                //            OrgGroupDescendantsDelimited = string.Join(",", _globalCurrentUser.OrgGroupDescendants)
                //        }
                //    });

                var position = await new Common_Synced_Position(_iconfiguration, _globalCurrentUser, _env)
                .GetPositionByID(MRF.PositionID);

                ViewData["PositionSelectList"] =
                     new List<SelectListItem> {
                        new SelectListItem
                        {
                            Value = MRF.PositionID.ToString(),
                            Text = string.Concat(position.Code, " - ", position.Title),
                            Selected = true
                        }
                  };

                //ViewData["PositionSelectList"] = 
                //    await new Common_Synced_Position(_iconfiguration, _globalCurrentUser, _env)
                //    .GetDropdownByOrgGroup(
                //new GetDropdownByOrgGroupInput
                //{
                //    OrgGroupID = MRF.OrgGroupID,
                //    SelectedValue = MRF.PositionID
                //});

                var natureOfEmployment = await new Common_Reference(_iconfiguration, _globalCurrentUser, _manpowerBaseURL, _env).GetReferenceValueByRefCode(
                    ReferenceCodes_Manpower.NATURE_OF_EMPLOYMENT.ToString());

                ViewData["NatureOfEmploymentSelectList"] = natureOfEmployment.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Description,
                        Selected = MRF.NatureOfEmploymentValue == x.Value
                    }).ToList();

                var purpose = await new Common_Reference(_iconfiguration, _globalCurrentUser, _manpowerBaseURL, _env).GetReferenceValueByRefCode(
                    ReferenceCodes_Manpower.MRF_PURPOSE.ToString());

                ViewData["PurposeSelectList"] = purpose.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Description,
                        Selected = MRF.PurposeValue == x.Value
                    }).ToList();

                ViewData["ShowReviseButton"] = MRF.StatusCode.Equals(MRF_STATUS.REJECTED.ToString()) ? "true" : "false";
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            MRF.CreatedBy = _globalCurrentUser.UserID;
            MRF.Vacancy = 1;

            var URL = string.Concat(_manpowerBaseURL,
                _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("Edit").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var oldValue = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env).GetMRF(MRF.ID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(MRF, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                StringBuilder Remarks = new StringBuilder();

                if (MRF.OrgGroupID != oldValue.OrgGroupID)
                {
                    Remarks.Append(string.Concat("OrgGroupID changed from ", oldValue.OrgGroupID, " to ", MRF.OrgGroupID, ". "));
                }

                if (MRF.PositionID != oldValue.PositionID)
                {
                    Remarks.Append(string.Concat("PositionID changed from ", oldValue.PositionID, " to ", MRF.PositionID, ". "));
                }

                if (MRF.IsConfidential != oldValue.IsConfidential)
                {
                    Remarks.Append(string.Concat("IsConfidential changed from ", oldValue.IsConfidential, " to ", MRF.IsConfidential, ". "));
                }

                if (MRF.NatureOfEmploymentValue != oldValue.NatureOfEmploymentValue)
                {
                    Remarks.Append(string.Concat("Nature of Employment changed from ", oldValue.NatureOfEmploymentValue, " to ", MRF.NatureOfEmploymentValue, ". "));
                }

                if (MRF.PurposeValue != oldValue.PurposeValue)
                {
                    Remarks.Append(string.Concat("Purpose changed from ", oldValue.PurposeValue, " to ", MRF.PurposeValue, ". "));
                }

                if (MRF.Vacancy != oldValue.Vacancy)
                {
                    Remarks.Append(string.Concat("Vacancy changed from ", oldValue.Vacancy, " to ", MRF.Vacancy, ". "));
                }

                if (MRF.Remarks != oldValue.Remarks)
                {
                    Remarks.Append(string.Concat("Remarks changed from ", oldValue.Remarks, " to ", MRF.Remarks, ". "));
                }

                if (MRF.ReasonForCancellation != oldValue.ReasonForCancellation)
                {
                    Remarks.Append(string.Concat("Reason For Cancellation changed from ", oldValue.ReasonForCancellation, " to ", MRF.ReasonForCancellation, ". "));
                }

                if (MRF.Status != oldValue.Status)
                {
                    Remarks.Append(string.Concat("Status changed from ", oldValue.Status, " to ", MRF.Status, ". "));
                }

                if (Remarks.Length > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new EMS.Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "MRF",
                            TableID = MRF.ID,
                            Remarks = Remarks.ToString(),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostRevise()
        {
            MRF.CreatedBy = _globalCurrentUser.UserID;
            MRF.Vacancy = 1;

            var URL = string.Concat(_manpowerBaseURL,
                _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("Revise").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var oldValue = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env).GetMRF(MRF.ID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(MRF, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                StringBuilder Remarks = new StringBuilder();

                if (MRF.StatusCode != oldValue.StatusCode)
                {
                    Remarks.Append(string.Concat("StatusCode changed from ", oldValue.StatusCode, " to ", MRF.StatusCode, ". "));
                }

                if (MRF.OrgGroupID != oldValue.OrgGroupID)
                {
                    Remarks.Append(string.Concat("OrgGroupID changed from ", oldValue.OrgGroupID, " to ", MRF.OrgGroupID, ". "));
                }

                if (MRF.PositionID != oldValue.PositionID)
                {
                    Remarks.Append(string.Concat("PositionID changed from ", oldValue.PositionID, " to ", MRF.PositionID, ". "));
                }

                if (MRF.IsConfidential != oldValue.IsConfidential)
                {
                    Remarks.Append(string.Concat("IsConfidential changed from ", oldValue.IsConfidential, " to ", MRF.IsConfidential, ". "));
                }

                if (MRF.NatureOfEmploymentValue != oldValue.NatureOfEmploymentValue)
                {
                    Remarks.Append(string.Concat("Nature of Employment changed from ", oldValue.NatureOfEmploymentValue, " to ", MRF.NatureOfEmploymentValue, ". "));
                }

                if (MRF.PurposeValue != oldValue.PurposeValue)
                {
                    Remarks.Append(string.Concat("Purpose changed from ", oldValue.PurposeValue, " to ", MRF.PurposeValue, ". "));
                }

                if (MRF.Vacancy != oldValue.Vacancy)
                {
                    Remarks.Append(string.Concat("Vacancy changed from ", oldValue.Vacancy, " to ", MRF.Vacancy, ". "));
                }

                if (MRF.Remarks != oldValue.Remarks)
                {
                    Remarks.Append(string.Concat("Remarks changed from ", oldValue.Remarks, " to ", MRF.Remarks, ". "));
                }

                if (MRF.ReasonForCancellation != oldValue.ReasonForCancellation)
                {
                    Remarks.Append(string.Concat("Reason For Cancellation changed from ", oldValue.ReasonForCancellation, " to ", MRF.ReasonForCancellation, ". "));
                }

                if (MRF.Status != oldValue.Status)
                {
                    Remarks.Append(string.Concat("Status changed from ", oldValue.Status, " to ", MRF.Status, ". "));
                }

                if (Remarks.Length > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new EMS.Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.REVISE.ToString(),
                            TableName = "MRF",
                            TableID = MRF.ID,
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