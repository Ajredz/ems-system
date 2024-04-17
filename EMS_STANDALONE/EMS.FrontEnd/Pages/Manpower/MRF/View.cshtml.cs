using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Manpower;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.Manpower.Transfer.DataDuplication.Position;
using EMS.Manpower.Transfer.DataDuplication.PositionLevel;
using EMS.Manpower.Transfer.MRF;
using EMS.Manpower.Transfer.MRFSignatories;
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
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Manpower.Transfer.MRF.Form MRF { get; set; }

        [BindProperty]
        public EMS.Manpower.Transfer.MRF.MRFCommentsForm CommentsForm { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
        }

        public async Task OnGetAsync(int ID)
        {
            if (_globalCurrentUser != null)
            {
                if (_systemURL != null)
                {
                    ViewData["HasCancelFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/MANPOWER/MRF/CANCEL")).Count() > 0 || _IsAdminAccess ? "true" : "false";
                    ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/MANPOWER/MRF/DELETE")).Count() > 0 || _IsAdminAccess ? "true" : "false";
                    ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/MANPOWER/MRF/EDIT")).Count() > 0 || _IsAdminAccess ? "true" : "false";
                    ViewData["HasHRCancelFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/MANPOWER/MRF/ADMIN/CANCELMRF")).Count() > 0 ? "true" : "false";
                }

                MRF = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env).GetMRF(ID);

                EMS.Security.Transfer.SystemUser.Form systemUser = await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env).GetSystemUserByID(MRF.CreatedBy);

                MRF.Requester = string.Concat(systemUser.FirstName, " ",
                    string.IsNullOrEmpty(systemUser.MiddleName) ? systemUser.MiddleName + " " : "",
                    systemUser.LastName);

                // Get PositionID and OrgGroupID if the user logged in is not the creator
                EMS.Plantilla.Transfer.Employee.GetByIDOutput employeeDetails = null;
                if (_globalCurrentUser.UserID != MRF.CreatedBy)
                { 
                    employeeDetails =
                    await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeByUserID(MRF.CreatedBy); 
                }

                MRFApprovalHistoryForm param = new MRFApprovalHistoryForm
                {
                    RequestingPositionID = MRF.PositionID,
                    RequestingOrgGroupID = MRF.OrgGroupID,
                    PositionID = MRF.PositionID,
                    MRFID = MRF.ID,
                };

                List<MRFApprovalHistoryOutput> approvalHistory = 
                    await new Common_MRF(_iconfiguration, _globalCurrentUser, _env)
                    .GetApprovalHistory(param);

                if (MRF.StatusCode.Equals(MRF_STATUS.CANCELLED.ToString()) ||
                    MRF.StatusCode.Equals(MRF_STATUS.CLOSED.ToString()) ||
                    MRF.StatusCode.Equals(MRF_STATUS.HR_CANCELLED.ToString()) ||
                    MRF.StatusCode.Equals(MRF_STATUS.AUTO_CANCELLED.ToString()) ||
                    MRF.StatusCode.Equals(MRF_STATUS.REJECTED.ToString())
                    )
                { 
                    ViewData["HasDeleteFeature"] = "false";
                    ViewData["HasEditFeature"] = "false";
                    if(!MRF.StatusCode.Equals(MRF_STATUS.CLOSED.ToString())) 
                        ViewData["HasHRCancelFeature"] = "false";
                    ViewData["HasCancelFeature"] = "false";
                    ViewData["ShowReviseButton"] = MRF.StatusCode.Equals(MRF_STATUS.REJECTED.ToString()) ? "true" : "false";
                }

                if (approvalHistory?.Where(x => x.ApprovalStatusCode.Contains(MRF_APPROVER_STATUS.APPROVED.ToString()) | 
                x.ApprovalStatusCode.Contains(MRF_APPROVER_STATUS.REJECTED.ToString()) |
                x.ApprovalStatusCode.Contains(MRF_APPROVER_STATUS.CANCELLED.ToString())).Count() > 0)
                {
                    ViewData["HasDeleteFeature"] = "false";
                    //ViewData["HasEditFeature"] = "false";
                }
                
                if (
                    approvalHistory?.Where(x => x.ApprovalStatusCode.Contains(MRF_APPROVER_STATUS.REJECTED.ToString())).Count() > 0 
                    |
                    approvalHistory?.Where(x => x.ApprovalStatusCode.Contains(MRF_APPROVER_STATUS.CANCELLED.ToString())).Count() > 0 
                    |
                    approvalHistory?.Where(x => x.ApprovalStatusCode.Contains(MRF_APPROVER_STATUS.APPROVED.ToString())).Count() == approvalHistory?.Count()
                    |
                    approvalHistory?.Where(x => x.ApprovalStatusCode.Contains(MRF_APPROVER_STATUS.APPROVED.ToString())).Count() == 0
                    )
                {
                    ViewData["HasCancelFeature"] = "false";
                }

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

                EMS.Manpower.Transfer.DataDuplication.Position.Form position = (await new Common_Synced_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionByID(MRF.PositionID));
                ViewData["PositionTitle"] = string.Concat(position.Code, " - ", position.Title);

                var natureOfEmployment = await new Common_Reference(_iconfiguration, _globalCurrentUser, _manpowerBaseURL, _env).GetReferenceValueByRefCodeAndValue(
                       ReferenceCodes_Manpower.NATURE_OF_EMPLOYMENT.ToString(), MRF.NatureOfEmploymentValue);

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
                    ReferenceCodes_Manpower.MRF_PURPOSE.ToString(), MRF.PurposeValue);

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

        public async Task<JsonResult> OnGetPositionDropDown([FromQuery] GetDropdownByOrgGroupInput param)
        {
            _resultView.Result = await new Common_Synced_Position(_iconfiguration, _globalCurrentUser, _env)
                .GetDropdownByOrgGroup(
                new GetDropdownByOrgGroupInput
                {
                    OrgGroupID = param.OrgGroupID,
                    SelectedValue = 0
                });
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetMRFApprovalHistory(MRFApprovalHistoryForm param, int CreatedBy)
        {
            List<MRFApprovalHistoryOutput> approvalHistory =
                   await new Common_MRF(_iconfiguration, _globalCurrentUser, _env)
                   .GetApprovalHistory(param);

            approvalHistory =
            approvalHistory
            .Select(x => {
                // Populate Approver Name if ApproverID exists
                if (x.ApproverID > 0)
                {
                    EMS.Plantilla.Transfer.Employee.GetByIDOutput employee =
                    (new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeByUserID(x.ApproverID)).Result;

                    x.ApproverName = string.Concat(employee.LastName,
                        string.IsNullOrEmpty(employee.FirstName) ? "" : string.Concat(", ", employee.FirstName),
                        string.IsNullOrEmpty(employee.MiddleName) ? "" : string.Concat(" ", employee.MiddleName));
                }
                else
                {
                    List<EMS.Plantilla.Transfer.Employee.GetByPositionIDOrgGroupIDOutput> employee =
                    (new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetByPositionIDOrgGroupID(
                        new EMS.Plantilla.Transfer.Employee.GetByPositionIDOrgGroupIDInput 
                        { 
                            PositionID = x.PositionID,
                            OrgGroupID = x.OrgGroupID
                        }
                        )).Result;
                    
                    List<EMS.Plantilla.Transfer.Employee.GetByPositionIDOrgGroupIDOutput> employeeAlternate =
                    (new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetByPositionIDOrgGroupID(
                        new EMS.Plantilla.Transfer.Employee.GetByPositionIDOrgGroupIDInput 
                        { 
                            PositionID = x.AltPositionID,
                            OrgGroupID = x.AltOrgGroupID
                        }
                        )).Result;
                    
                    /* Roving Employees */
                    List<EMS.Plantilla.Transfer.Employee.GetRovingByPositionIDOrgGroupIDOutput> employeeRoving =
                    (new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetRovingByPositionIDOrgGroupID(
                        new EMS.Plantilla.Transfer.Employee.GetRovingByPositionIDOrgGroupIDInput 
                        { 
                            PositionID = x.PositionID,
                            OrgGroupID = x.OrgGroupID
                        }
                        )).Result;

                    x.ApproverName = string.Join(" | ", 
                        employee.Select(x => x.EmployeeName).ToArray()
                        .Union(employeeRoving.Select(x => string.Concat(x.EmployeeName, " (Roving)")).ToArray())
                        .Union(employeeAlternate.Select(x => string.Concat(x.EmployeeName, " (Alternate)")).ToArray())
                        );
                }
                return x;
            }).ToList();

            var jsonData = new
            {
                total = approvalHistory.Count,
                pageNumber = 1,
                sort = "HierarchyLevel",
                records = approvalHistory.Count,
                rows = approvalHistory
            };
            return new JsonResult(jsonData);
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

            return new JsonResult(_resultView);
        }

    }
}