using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Manpower;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.Manpower.Transfer.MRF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Manpower.MRFApproval
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] EMS.Manpower.Transfer.MRFApproval.GetApprovalListInput param)
        {
            //List<int> roles = 
            //    (await new Common_SystemRole(_iconfiguration, _globalCurrentUser, _env).GetSystemRoleByUserID()).Select(x => x.RoleID).ToList();

            var sample = string.Join(",", _globalCurrentUser.Roving == null ? System.Array.Empty<int>() : _globalCurrentUser.Roving.Select(x => x.PositionID).ToArray());

            var URL = string.Concat(_manpowerBaseURL,
                  _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRFApproval").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",
                  "ApproverPositionID=", _globalCurrentUser.PositionID, "&",
                  "ApproverOrgGroupID=", _globalCurrentUser.OrgGroupID, "&",
                  "RovingPositionDelimited=", string.Join(",", _globalCurrentUser.Roving == null ? System.Array.Empty<int>() : _globalCurrentUser.Roving.Select(x => x.PositionID).ToArray()), "&",
                  "RovingOrgGroupDelimited=", string.Join(",", _globalCurrentUser.Roving == null ? System.Array.Empty<int>() : _globalCurrentUser.Roving.Select(x => x.OrgGroupID).ToArray()), "&",
                  "ApproverID=", _globalCurrentUser.UserID, "&",

                  "ID=", param.ID, "&",
                  "MRFTransactionID=", param.MRFTransactionID, "&",
                  "OrgGroupDelimited=", param.OrgGroupDelimited, "&",
                  "PositionLevelDelimited=", param.PositionLevelDelimited, "&",
                  "PositionDelimited=", param.PositionDelimited, "&",
                  "NatureOfEmploymentDelimited=", param.NatureOfEmploymentDelimited, "&",
                  "NoOfApplicantMin=", param.NoOfApplicantMin, "&",
                  "NoOfApplicantMax=", param.NoOfApplicantMax, "&",
                  "StatusDelimited=", param.StatusDelimited, "&",
                  "DateCreatedFrom=", param.DateCreatedFrom, "&",
                  "DateCreatedTo=", param.DateCreatedTo, "&",
                  "DateApprovedFrom=", param.DateApprovedFrom, "&",
                  "DateApprovedTo=", param.DateApprovedTo, "&",
                  "AgeMin=", param.AgeMin, "&",
                  "AgeMax=", param.AgeMax);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetListOutput>(), URL);

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

        public async Task<JsonResult> OnGetPositionLevelAutoCompleteAsync(string Term, int TopResults)
        {
            var result = await new Common_Synced_PositionLevel(_iconfiguration, _globalCurrentUser, _env).GetPositionLevelAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetOrgTypeAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_Synced_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_Synced_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            var result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _manpowerBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetMRFApprovalHistory(MRFApprovalHistoryForm param)
        {
            //param.RequesterPositionID = _globalCurrentUser.PositionID;
            //param.RequesterOrgGroupID = _globalCurrentUser.OrgGroupID;

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


    }
}