using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Manpower;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.Manpower.Transfer.DataDuplication.Position;
using EMS.Manpower.Transfer.DataDuplication.PositionLevel;
using EMS.Manpower.Transfer.MRFApproval;
using EMS.Manpower.Transfer.MRFSignatories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Manpower.MRFApproval
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Manpower.Transfer.MRFApproval.Form MRFApproval { get; set; }

        [BindProperty]
        public EMS.Manpower.Transfer.MRF.MRFCommentsForm CommentsForm { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID, string RecordStatus)
        {
            if (_globalCurrentUser != null)
            {

                MRFApproval = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env).GetApprovalMRF(ID);

                /* Roving Employees */
                List<EMS.Plantilla.Transfer.Employee.GetRovingByPositionIDOrgGroupIDOutput> employeeRoving =
                (new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetRovingByPositionIDOrgGroupID(
                    new EMS.Plantilla.Transfer.Employee.GetRovingByPositionIDOrgGroupIDInput
                    {
                        PositionID = MRFApproval.ApproverPositionID,
                        OrgGroupID = MRFApproval.ApproverOrgGroupID
                    }
                    )).Result;

                if (_systemURL != null & 
                    (
                        (
                            RecordStatus.Equals("For Approval") || 
                            (
                                // Revised MRF 
                                RecordStatus.Equals("Rejected") & MRFApproval.Status.Equals("FOR APPROVAL")
                            )
                        ) 
                        & 
                        (
                            (
                                _globalCurrentUser.OrgGroupID == MRFApproval.ApproverOrgGroupID &
                                _globalCurrentUser.PositionID == MRFApproval.ApproverPositionID
                            )
                            ||
                            (
                                _globalCurrentUser.OrgGroupID == MRFApproval.AltApproverOrgGroupID &
                                _globalCurrentUser.PositionID == MRFApproval.AltApproverPositionID
                            )
                            ||
                            (
                                /*_globalCurrentUser.Roving?.Where(x => x.OrgGroupID == MRFApproval.ApproverOrgGroupID & 
                                x.PositionID == MRFApproval.ApproverPositionID).Count() > 0*/
                                employeeRoving.Where(x =>
                                x.EmployeeName.Equals(
                                    string.Concat(_globalCurrentUser.LastName, ", ", _globalCurrentUser.FirstName, " ", _globalCurrentUser.MiddleName))
                                ).Count() > 0
                            )
                        )
                    ))
                {
                    ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/MANPOWER/MRFAPPROVAL/EDIT")).Count() > 0 ? "true" : "false";
                }

                EMS.Security.Transfer.SystemUser.Form systemUser = await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env).GetSystemUserByID(MRFApproval.CreatedBy);

                MRFApproval.Requester = string.Concat(systemUser.FirstName, " ", 
                    string.IsNullOrEmpty(systemUser.MiddleName) ? systemUser.MiddleName + " " : "", 
                    systemUser.LastName);

                var orgGroup = await new Common_Synced_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupByID(MRFApproval.OrgGroupID);


                ViewData["OrgGroupSelectList"] =
                 new List<SelectListItem> {
                        new SelectListItem
                        {
                            Value = orgGroup.ID.ToString(),
                            Text = string.Concat(orgGroup.Code, " - ", orgGroup.Description),
                            Selected = true
                        }
                    };

                //ViewData["PositionLevelSelectList"] = await new Common_Synced_PositionLevel(_iconfiguration, _globalCurrentUser, _env).GetPositionLevelDropdownByOrgGroupID(new GetByOrgGroupIDInput
                //{
                //    OrgGroupID = MRFApproval.OrgGroupID,
                //    SelectedValue = MRFApproval.PositionLevelID
                //});

                EMS.Manpower.Transfer.DataDuplication.Position.Form position = (await new Common_Synced_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionByID(MRFApproval.PositionID));
                ViewData["PositionTitle"] = string.Concat(position.Code, " - ", position.Title);

                //// Get requester Position ID and OrgGroup ID
                //EMS.Plantilla.Transfer.Employee.GetByIDOutput requester =
                //    await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                //    .GetEmployeeByUserID(MRFApproval.CreatedBy);

                ViewData["RequestingPositionID"] = MRFApproval.PositionID;
                ViewData["RequestingOrgGroupID"] = MRFApproval.OrgGroupID;

                var natureOfEmployment = await new Common_Reference(_iconfiguration, _globalCurrentUser, _manpowerBaseURL, _env).GetReferenceValueByRefCodeAndValue(
                    ReferenceCodes_Manpower.NATURE_OF_EMPLOYMENT.ToString(), MRFApproval.NatureOfEmploymentValue);

                ViewData["NatureOfEmploymentSelectList"] = 
                    new List<SelectListItem> {
                        new SelectListItem
                        {
                            Value = natureOfEmployment.Value,
                            Text = natureOfEmployment.Description,
                            Selected = true
                        }
                    };

                var purpose = await new Common_Reference(_iconfiguration, _globalCurrentUser, _manpowerBaseURL, _env).GetReferenceValueByRefCodeAndValue(
                    ReferenceCodes_Manpower.MRF_PURPOSE.ToString(), MRFApproval.PurposeValue);

                ViewData["PurposeSelectList"] = new List<SelectListItem> {
                        new SelectListItem
                        {
                            Value = purpose.Value,
                            Text = purpose.Description,
                            Selected = true
                        }
                    };
            }
        }

        public async Task<JsonResult> OnPostAsync(ApproverResponse param)
        {
            MRFApproval.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_manpowerBaseURL,
                _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRFApproval").GetSection("Edit").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(
                new ApproverResponse {
                    RecordID = param.RecordID, 
                    Result = param.Result,
                    LevelOfApproval = param.LevelOfApproval,
                    NextApproverPositionID = param.NextApproverPositionID,
                    NextApproverOrgGroupID = param.NextApproverOrgGroupID,
                    Remarks = param.Remarks
                }, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionLevelDropDownByOrgGroupID([FromQuery] GetByOrgGroupIDInput param)
        {
            var result = await new Common_Synced_PositionLevel(_iconfiguration, _globalCurrentUser, _env).GetPositionLevelDropdownByOrgGroupID(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionDropDown(int PositionLevelID)
        {
            _resultView.Result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetDropdownDetailedByPositionLevel(PositionLevelID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetSignatories(int PositionID, int RecordID)
        {
            var GetSignatoriesURL = string.Concat(_manpowerBaseURL,
               _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRFSignatories").GetSection("GetSignatoriesAdd").Value, "?",
                "UserID=", _globalCurrentUser.UserID,
                "&PositionID=", PositionID,
                "&RequesterID=", 0,
                "&RecordID=", RecordID);

            var (Result, IsSuccess, _) = await SharedUtilities.GetFromAPI(new List<GetMRFSignatoriesAddOutput>(), GetSignatoriesURL);

            _resultView.Result = Result;
            _resultView.IsSuccess = IsSuccess;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetRegionByID(int RegionID)
        {
            _resultView.Result = await new Common_Region(_iconfiguration, _globalCurrentUser, _env).GetRegion(RegionID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetMRFComments(int MRFID)
        {
            _resultView.Result = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env).GetMRFComments(MRFID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostSaveComments()
        {
            CommentsForm.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_manpowerBaseURL,
                _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("AddComments").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(CommentsForm, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "MRFComments",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(CommentsForm.MRFID, " Comment added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }
    }
}