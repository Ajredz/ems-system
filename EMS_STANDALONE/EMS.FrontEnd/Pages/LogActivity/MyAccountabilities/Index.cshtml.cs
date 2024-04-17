using EMS.Plantilla.Transfer.Position;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using Microsoft.AspNetCore.Hosting;
using EMS.Workflow.Transfer.LogActivity;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.Workflow.Transfer.Accountability;
using EMS.Workflow.Transfer.Workflow;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.FrontEnd.Pages.Plantilla.Employee;
using EMS.Workflow.Transfer.EmailServerCredential;
using EMS.IPM.Data.DataDuplication.Position;
using EMS.FrontEnd.Pages.Administrator.SystemRole;
using EMS.IPM.Data.DataDuplication.Employee;
using NPOI.POIFS.Storage;
using EMS.Plantilla.Transfer.OrgGroup;
using EMS.Plantilla.Transfer.Employee;
using NPOI.OpenXmlFormats;
using EMS.Workflow.Transfer.Question;
using NPOI.SS.Formula.Functions;

namespace EMS.FrontEnd.Pages.LogActivity.MyAccountabilities
{
    public class IndexModel : SharedClasses.Utilities
    {
        public bool _IsClearance = false;

        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false, bool IsClearance = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
            _IsClearance = IsClearance;
        }

        public void OnGet()
        {
            if (_systemURL != null)
            {
                if (_IsAdminAccess & !_IsClearance)
                {
                    ViewData["HasUploadFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/ALLACCOUNTABILITIES/UPLOAD")).Count() > 0 ? "true" : "false";
                    ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/ALLACCOUNTABILITIES/DELETE")).Count() > 0 ? "true" : "false";
                }
                ViewData["HasChangeStatus"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARANCE/CHANGESTATUS")).Count() > 0 ? "true" : "false";
                ViewData["ClearingHRBP"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARANCE/ALLACCOUNTABILITY")).Count() > 0 ? "true" : "false";
                ViewData["HasExportExitInterview"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARANCE/EXPORTEXITINTERVIEW")).Count() > 0 ? "true" : "false";
            }
        }

        public async Task<IActionResult> OnGetList([FromQuery] GetMyAccountabilitiesListInput param)
        {
            var StatusColor = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(4)).WorkflowStepList;
            
            param.IsAdminAccess = _IsAdminAccess;
            param.IsClearance = _IsClearance;
            param.OrgGroupDescendantAccess = _globalCurrentUser.OrgGroupID.ToString();
            param.MyEmployeeID = _globalCurrentUser.EmployeeID;

            if (param.IsClearance)
            {
                List<int> GetOrg = new List<int>();
                var GetClearingDept = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                    .GetOrgGroupHierarchy(_globalCurrentUser.OrgGroupID)).Where(x => x.OrgType.Equals("DEPT") || x.OrgType.Equals("SECTION")).ToList();
                if (_globalCurrentUser.OrgGroupDescendants != null)
                    GetOrg.AddRange(_globalCurrentUser.OrgGroupDescendants);
                if (_globalCurrentUser.OrgGroupRovingDescendants != null)
                    GetOrg.AddRange(_globalCurrentUser.OrgGroupRovingDescendants);
                if (GetClearingDept != null)
                    GetOrg.AddRange(GetClearingDept.Select(x=>x.ID).Distinct().ToList());
                if (GetOrg.Count() > 0)
                    param.OrgGroupDescendantAccess = string.Join(",", ((await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => GetOrg.Contains(x.OrgGroupID)).Select(x => x.OrgGroupID).ToList()));

                var EmployeeAccess = ((await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                    .GetEmployeeIDDescendant(_globalCurrentUser.EmployeeID)).Select(x => x.ID).ToList());
                EmployeeAccess.Add(_globalCurrentUser.EmployeeID);
                param.EmployeeIDDescendantAccess = string.Join(",", EmployeeAccess);
            }
            if (!param.IsAdminAccess && !param.IsClearance)
                param.EmployeeIDDelimited = _globalCurrentUser.EmployeeID.ToString();


            List<int> EmployeeID = new List<int>();

            if (!string.IsNullOrEmpty(param.EmployeeOrgDelimited))
            {
                var Employee = ((await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeByOrgGroup(param.EmployeeOrgDelimited.Split(',').Select(x => int.Parse(x)).ToList())).Item1).Select(x => x.ID).ToList();
                var GetActiveEmployee = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => Employee.Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList();
                EmployeeID.AddRange(GetActiveEmployee);
            }
            if (!string.IsNullOrEmpty(param.EmployeeOrgRegionDelimited))
            {
                var GetBranch = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                    .GetOrgGroupDescendantsList(param.EmployeeOrgRegionDelimited.Split(',').Select(x => int.Parse(x)).ToList())).ToList();
                var Employee = ((await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeByOrgGroup(GetBranch)).Item1).Select(x => x.ID).ToList();
                var GetActiveEmployee = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => Employee.Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList();
                EmployeeID.AddRange(GetActiveEmployee);
            }
            if (!string.IsNullOrEmpty(param.EmployeePosDelimited))
            {
                var Employee = ((await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeByPosition(param.EmployeePosDelimited.Split(',').Select(x => int.Parse(x)).ToList())).Item1).Select(x => x.ID).ToList();
                var GetActiveEmployee = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => Employee.Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList();
                EmployeeID.AddRange(GetActiveEmployee);
            }
            if (!string.IsNullOrEmpty(param.EmployeeOrgDelimited) || !string.IsNullOrEmpty(param.EmployeeOrgRegionDelimited) || !string.IsNullOrEmpty(param.EmployeePosDelimited))
            {
                if (EmployeeID.Count() > 0)
                    param.EmployeeIDDelimited = string.Join(',', EmployeeID);
                else
                    param.EmployeeIDDelimited = "0";
            }

            if (!string.IsNullOrEmpty(param.SeparationDateFrom) && !string.IsNullOrEmpty(param.SeparationDateTo))
            {
                var GetEmployeeByDate = (await new Common_Employee(_iconfiguration,_globalCurrentUser,_env)
                    .GetEmployeeLastEmploymentStatusByDate(new GetEmployeeLastEmploymentStatusByDateInput { DateFrom = param.SeparationDateFrom, DateTo = param.SeparationDateTo })).Item1;
                var GetActiveEmployee = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => (GetEmployeeByDate.Select(x=>x.ID)).Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList();
                param.EmployeeIDDelimited = GetActiveEmployee.Count() > 0 ? string.Join(",", GetActiveEmployee) : "0";
            }

            if (!string.IsNullOrEmpty(param.HiredDateFrom) && !string.IsNullOrEmpty(param.HiredDateTo))
            {
                var GetEmployeeByDate = (await new Common_Employee(_iconfiguration,_globalCurrentUser,_env)
                    .GetEmployeeByDateHired(new GetEmployeeLastEmploymentStatusByDateInput { DateFrom = param.HiredDateFrom, DateTo = param.HiredDateTo })).Item1;
                var GetActiveEmployee = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => (GetEmployeeByDate.Select(x => x.ID)).Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList();
                param.EmployeeIDDelimited = GetActiveEmployee.Count() > 0 ? string.Join(",", GetActiveEmployee) : "0";
            }

            if (_systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARANCE/ALLACCOUNTABILITY")).Count() > 0)
            {
                List<int> GetOrg = new List<int>();
                if (_globalCurrentUser.OrgGroupDescendants != null)
                    GetOrg.AddRange(_globalCurrentUser.OrgGroupDescendants);
                if (_globalCurrentUser.OrgGroupRovingDescendants != null)
                    GetOrg.AddRange(_globalCurrentUser.OrgGroupRovingDescendants);
                if (GetOrg.Count() > 0)
                {
                    var GetEmployee = ((await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                        .GetEmployeeByOrgGroup(GetOrg)).Item1).Select(x => x.ID).ToList();
                    param.EmployeeIDDescendantAccess = string.Join(",",(await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                        .GetAllEmployeeAccountability()).Where(x => GetEmployee.Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList());
                }
                param.IsAdminAccess = true;
            }

            var (Result, IsSuccess, ErrorMessage) = await new Common_Accountability(_iconfiguration,_globalCurrentUser,_env)
                .GetList(param);

            var employeeName = (await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeByIDs(Result.Where(x => x.EmployeeID != 0).Select(x => x.EmployeeID).Distinct().ToList())).Item1;

            var employeeLastEmploymentStatus = (await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeLastEmploymentStatus(Result.Where(x => x.EmployeeID != 0).Select(x => x.EmployeeID).Distinct().ToList())).Item1;

            var employeeOrg = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupFormatByID(employeeName.Select(x => x.OrgGroupID).Distinct().ToList())).Item1;

            var employeePos = (await new Common_Position(_iconfiguration, _globalCurrentUser, _env)
                        .GetAll());

            var clearingOrg = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupFormatByID(Result.Select(x => x.OrgGroupID).Distinct().ToList())).Item1;

            var statsUpdatedBy = await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(Result.Where(x => x.StatusUpdatedBy != 0).Select(x => x.StatusUpdatedBy).Distinct().ToList());

            var orgRegion = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupParent(new GetOrgGroupParentInput() { OrgGroupIDs = employeeOrg.Select(x=>x.ID).ToList(),OrgType = "REG" })).Item1;

            Result = (from main in Result
                        join emp in employeeName on main.EmployeeID equals emp.ID
                        join employeeStatus in employeeLastEmploymentStatus on main.EmployeeID equals employeeStatus.ID
                        join status in StatusColor on main.Status equals status.StepCode
                        join empOrg in employeeOrg on emp.OrgGroupID equals empOrg.ID
                        join empOrgRegion in orgRegion on emp.OrgGroupID equals empOrgRegion.ID
                        join empPos in employeePos on emp.PositionID equals empPos.ID 
                        join clearOrg in clearingOrg on main.OrgGroupID equals clearOrg.ID
                        join statUpdatedBy in statsUpdatedBy on main.StatusUpdatedBy equals statUpdatedBy.ID
                        select new GetMyAccountabilitiesListOutput()
                        {
                            TotalNoOfRecord = main.TotalNoOfRecord,
                            NoOfPages = main.NoOfPages,
                            Row = main.Row,

                            ID = main.ID,
                            CreatedDate = main.CreatedDate,
                            EmployeeName = string.Concat("(", emp.Code, ") ", emp.PersonalInformation.LastName, ", ", emp.PersonalInformation.FirstName, " ", ((emp.PersonalInformation.MiddleName == null || emp.PersonalInformation.MiddleName == "") ? "" : emp.PersonalInformation.MiddleName.Substring(0, 1))),
                            EmployeeStatusUpdatedDate = employeeStatus.StatusUpdatedDate.ToString("dd-MMM-yyyy"),
                            EmployeeDatehired = emp.DateHired?.ToString("dd-MMM-yyyy"),
                            Title = main.Title,
                            StatusDescription = status.StepDescription,
                            StatusColor = status.StatusColor,
                            StatusUpdateDate = main.StatusUpdateDate,
                            EmployeeOrg = empOrg.Result,
                            EmployeeOrgRegion = empOrgRegion.Result,
                            EmployeePos = string.Concat(empPos.Code," - ",empPos.Title," | ",emp.EmploymentStatus),
                            ClearingOrg = clearOrg.Result,
                            StatusUpdatedByName = string.Concat("(", statUpdatedBy.Username, ") ", statUpdatedBy.LastName, ", ", statUpdatedBy.FirstName, " ", ((statUpdatedBy.MiddleName == null || statUpdatedBy.MiddleName == "") ? "" : statUpdatedBy.MiddleName.Substring(0, 1))),
                            StatusUpdatedBy = main.StatusUpdatedBy,
                            StatusRemarks = main .StatusRemarks,
                            LastComment = main.LastComment,
                            LastCommentDate = main.LastCommentDate,

                            EmployeeID = main.EmployeeID,
                            Status = main.Status,
                            OrgGroupID = main.OrgGroupID
                        }).ToList();


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

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetLogActivitySubType()
        {
            _resultView.Result = await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                .GetLogActivitySubType();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetReferredBy(string Term, int TopResults)
        {
            //var result = await new Common_Synced_SystemUser(_iconfiguration, _globalCurrentUser, _env).GetSystemUserAutoComplete(Term, TopResults);
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployeeWithSystemUserAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionAutoCompleteAsync(EMS.Plantilla.Transfer.Position.GetAutoCompleteInput param)
        {
            var result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionAutoComplete(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetOrgTypeAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetOrgGroupByOrgTypeAutoCompleteAsync(EMS.Plantilla.Transfer.OrgGroup.GetByOrgTypeAutoCompleteInput param)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupByOrgTypeAutoComplete(param);
            result.Add(new GetIDByOrgTypeAutoCompleteOutput() { ID = 1993, Description = "HEAD OFFICE" });
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetAccountabilityStatusHistory(int ID)
        {
            List<GetEmployeeAccountabilityStatusHistoryOutput> statusHistory =
                   await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                   .GetEmployeeAccountabilityStatusHistory(ID);

            // Get Employee Names by User IDs
            List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
               await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
               .GetSystemUserByIDs(statusHistory.Where(x => x.UserID != 0).Select(x => x.UserID).Distinct().ToList());

            // Set employee names
            if (systemUsers.Count > 0)
            {
                statusHistory = statusHistory
                        .GroupJoin(systemUsers,
                        x => new { x.UserID },
                        y => new { UserID = y.ID },
                        (x, y) => new { activities = x, employees = y })
                        .SelectMany(x => x.employees.DefaultIfEmpty(),
                        (x, y) => new { activities = x, employees = y })
                        .Select(x => new GetEmployeeAccountabilityStatusHistoryOutput
                        {
                            Status = x.activities.activities.Status,
                            Timestamp = x.activities.activities.Timestamp,
                            Remarks = x.activities.activities.Remarks,
                            User = x.employees == null ? "" : string.Concat((x.employees.LastName ?? "").Trim(),
                                string.IsNullOrEmpty((x.employees.FirstName ?? "").Trim()) ? "" : string.Concat(", ", x.employees.FirstName),
                                string.IsNullOrEmpty((x.employees.MiddleName ?? "").Trim()) ? "" : string.Concat(" ", x.employees.MiddleName))

                        }).ToList();
            }

            var jsonData = new
            {
                rows = statusHistory
            };
            return new JsonResult(jsonData);
        }

        public async Task<JsonResult> OnGetEmployeeAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetOldEmployeeIDAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetOldEmployeeIDAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetCheckExportList([FromQuery] GetMyAccountabilitiesListInput param)
        {
            var StatusColor = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(4)).WorkflowStepList;

            param.IsAdminAccess = _IsAdminAccess;
            param.IsClearance = _IsClearance;
            param.OrgGroupDescendantAccess = _globalCurrentUser.OrgGroupID.ToString();

            if (param.IsClearance)
            {
                List<int> GetOrg = new List<int>();
                var GetClearingDept = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                    .GetOrgGroupHierarchy(_globalCurrentUser.OrgGroupID)).Where(x => x.OrgType.Equals("DEPT") || x.OrgType.Equals("SECTION")).ToList();
                if (_globalCurrentUser.OrgGroupDescendants != null)
                    GetOrg.AddRange(_globalCurrentUser.OrgGroupDescendants);
                if (_globalCurrentUser.OrgGroupRovingDescendants != null)
                    GetOrg.AddRange(_globalCurrentUser.OrgGroupRovingDescendants);
                if (GetClearingDept != null)
                    GetOrg.AddRange(GetClearingDept.Select(x => x.ID).Distinct().ToList());
                if (GetOrg.Count() > 0)
                    param.OrgGroupDescendantAccess = string.Join(",", ((await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => GetOrg.Contains(x.OrgGroupID)).Select(x => x.OrgGroupID).ToList()));

                var EmployeeAccess = ((await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                    .GetEmployeeIDDescendant(_globalCurrentUser.EmployeeID)).Select(x => x.ID).ToList());
                EmployeeAccess.Add(_globalCurrentUser.EmployeeID);
                param.EmployeeIDDescendantAccess = string.Join(",", EmployeeAccess);
            }
            if (!param.IsAdminAccess && !param.IsClearance)
                param.EmployeeIDDelimited = _globalCurrentUser.EmployeeID.ToString();


            List<int> EmployeeID = new List<int>();

            if (!string.IsNullOrEmpty(param.EmployeeOrgDelimited))
            {
                var Employee = ((await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeByOrgGroup(param.EmployeeOrgDelimited.Split(',').Select(x => int.Parse(x)).ToList())).Item1).Select(x => x.ID).ToList();
                var GetActiveEmployee = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => Employee.Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList();
                EmployeeID.AddRange(GetActiveEmployee);
            }
            if (!string.IsNullOrEmpty(param.EmployeeOrgRegionDelimited))
            {
                var GetBranch = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                    .GetOrgGroupDescendantsList(param.EmployeeOrgRegionDelimited.Split(',').Select(x => int.Parse(x)).ToList())).ToList();
                var Employee = ((await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeByOrgGroup(GetBranch)).Item1).Select(x => x.ID).ToList();
                var GetActiveEmployee = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => Employee.Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList();
                EmployeeID.AddRange(GetActiveEmployee);
            }
            if (!string.IsNullOrEmpty(param.EmployeePosDelimited))
            {
                var Employee = ((await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeByPosition(param.EmployeePosDelimited.Split(',').Select(x => int.Parse(x)).ToList())).Item1).Select(x => x.ID).ToList();
                var GetActiveEmployee = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => Employee.Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList();
                EmployeeID.AddRange(GetActiveEmployee);
            }
            if (!string.IsNullOrEmpty(param.EmployeeOrgDelimited) || !string.IsNullOrEmpty(param.EmployeeOrgRegionDelimited) || !string.IsNullOrEmpty(param.EmployeePosDelimited))
            {
                if (EmployeeID.Count() > 0)
                    param.EmployeeIDDelimited = string.Join(',', EmployeeID);
                else
                    param.EmployeeIDDelimited = "0";
            }

            if (!string.IsNullOrEmpty(param.SeparationDateFrom) && !string.IsNullOrEmpty(param.SeparationDateTo))
            {
                var GetEmployeeByDate = (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeLastEmploymentStatusByDate(new GetEmployeeLastEmploymentStatusByDateInput { DateFrom = param.SeparationDateFrom, DateTo = param.SeparationDateTo })).Item1;
                var GetActiveEmployee = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => (GetEmployeeByDate.Select(x => x.ID)).Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList();
                param.EmployeeIDDelimited = string.Join(",", GetActiveEmployee);
            }

            if (!string.IsNullOrEmpty(param.HiredDateFrom) && !string.IsNullOrEmpty(param.HiredDateTo))
            {
                var GetEmployeeByDate = (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeByDateHired(new GetEmployeeLastEmploymentStatusByDateInput { DateFrom = param.HiredDateFrom, DateTo = param.HiredDateTo })).Item1;
                var GetActiveEmployee = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => (GetEmployeeByDate.Select(x => x.ID)).Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList();
                param.EmployeeIDDelimited = GetActiveEmployee.Count() > 0 ? string.Join(",", GetActiveEmployee) : "0";
            }

            if (_systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARANCE/ALLACCOUNTABILITY")).Count() > 0)
            {
                List<int> GetOrg = new List<int>();
                if (_globalCurrentUser.OrgGroupDescendants != null)
                    GetOrg.AddRange(_globalCurrentUser.OrgGroupDescendants);
                if (_globalCurrentUser.OrgGroupRovingDescendants != null)
                    GetOrg.AddRange(_globalCurrentUser.OrgGroupRovingDescendants);
                if (GetOrg.Count() > 0)
                {
                    var GetEmployee = ((await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                        .GetEmployeeByOrgGroup(GetOrg)).Item1).Select(x => x.ID).ToList();
                    param.EmployeeIDDescendantAccess = string.Join(",", (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                        .GetAllEmployeeAccountability()).Where(x => GetEmployee.Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList());
                }
                param.IsAdminAccess = true;
            }

            var (Result, IsSuccess, ErrorMessage) = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetList(param);

            var employeeName = (await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeByIDs(Result.Where(x => x.EmployeeID != 0).Select(x => x.EmployeeID).Distinct().ToList())).Item1;

            var employeeOrg = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupFormatByID(employeeName.Select(x => x.OrgGroupID).Distinct().ToList())).Item1;

            var employeePos = (await new Common_Position(_iconfiguration, _globalCurrentUser, _env)
                        .GetAll());

            var clearingOrg = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupFormatByID(Result.Select(x => x.OrgGroupID).Distinct().ToList())).Item1;

            var statsUpdatedBy = await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(Result.Where(x => x.StatusUpdatedBy != 0).Select(x => x.StatusUpdatedBy).Distinct().ToList());

            var orgRegion = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupParent(new GetOrgGroupParentInput() { OrgGroupIDs = employeeOrg.Select(x => x.ID).ToList(), OrgType = "REG" })).Item1;

            Result = (from main in Result
                      join emp in employeeName on main.EmployeeID equals emp.ID
                      join status in StatusColor on main.Status equals status.StepCode
                      join empOrg in employeeOrg on emp.OrgGroupID equals empOrg.ID
                      join empOrgRegion in orgRegion on emp.OrgGroupID equals empOrgRegion.ID
                      join empPos in employeePos on emp.PositionID equals empPos.ID
                      join clearOrg in clearingOrg on main.OrgGroupID equals clearOrg.ID
                      join statUpdatedBy in statsUpdatedBy on main.StatusUpdatedBy equals statUpdatedBy.ID
                      select new GetMyAccountabilitiesListOutput()
                      {
                          TotalNoOfRecord = main.TotalNoOfRecord,
                          NoOfPages = main.NoOfPages,
                          Row = main.Row,

                          ID = main.ID,
                          CreatedDate = main.CreatedDate,
                          EmployeeName = string.Concat("(", emp.Code, ") ", emp.PersonalInformation.LastName, ", ", emp.PersonalInformation.FirstName, " ", ((emp.PersonalInformation.MiddleName == null || emp.PersonalInformation.MiddleName == "") ? "" : emp.PersonalInformation.MiddleName.Substring(0, 1))),
                          Title = main.Title,
                          StatusDescription = status.StepDescription,
                          StatusColor = status.StatusColor,
                          StatusUpdateDate = main.StatusUpdateDate,
                          EmployeeDatehired = emp.DateHired?.ToString("dd-MMM-yyyy"),
                          EmployeeOrg = empOrg.Result,
                          EmployeeOrgRegion = empOrgRegion.Result,
                          EmployeePos = string.Concat(empPos.Code, " - ", empPos.Title, " | ", emp.EmploymentStatus),
                          ClearingOrg = clearOrg.Result,
                          StatusUpdatedByName = string.Concat("(", statUpdatedBy.Username, ") ", statUpdatedBy.LastName, ", ", statUpdatedBy.FirstName, " ", ((statUpdatedBy.MiddleName == null || statUpdatedBy.MiddleName == "") ? "" : statUpdatedBy.MiddleName.Substring(0, 1))),
                          StatusUpdatedBy = main.StatusUpdatedBy,
                          StatusRemarks = main.StatusRemarks,
                          LastComment = main.LastComment,
                          LastCommentDate = main.LastCommentDate,


                          EmployeeID = main.EmployeeID,
                          Status = main.Status,
                          OrgGroupID = main.OrgGroupID
                      }).ToList();

            if (IsSuccess)
            {
                if (Result.Count > 0)
                {
                    _resultView.IsSuccess = true;
                }
                else
                {
                    _resultView.IsSuccess = false;
                    _resultView.Result = MessageUtilities.ERRMSG_NO_RECORDS;
                }

                return new JsonResult(_resultView);
            }
            else
            {
                return new BadRequestObjectResult(ErrorMessage);
            }
        }

        public async Task<IActionResult> OnGetExportMyAccountabilitiesList([FromQuery] GetMyAccountabilitiesListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetList(param);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "Accountabilities.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Accountabilities");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Status");
                    row.CreateCell(1).SetCellValue("Status Update Date");
                    row.CreateCell(2).SetCellValue("Title");
                    row.CreateCell(3).SetCellValue("Employee Branch / Department");
                    row.CreateCell(4).SetCellValue("Position");
                    row.CreateCell(5).SetCellValue("Last Update By");
                    row.CreateCell(6).SetCellValue("Last Update Date");

                    excelSheet.SetColumnWidth(0, 7000);
                    excelSheet.SetColumnWidth(1, 7000);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 7000);
                    excelSheet.SetColumnWidth(4, 7000);
                    excelSheet.SetColumnWidth(5, 7000);
                    excelSheet.SetColumnWidth(6, 7000);

                    XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle wrapText = (XSSFCellStyle)workbook.CreateCellStyle();

                    var colHeaderFont = workbook.CreateFont();
                    colHeaderFont.IsBold = true;
                    colHeaderStyle.SetFont(colHeaderFont);
                    colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                    colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                    colHeaderStyle.Alignment = HorizontalAlignment.Center;
                    alignCenter.Alignment = HorizontalAlignment.Center;
                    wrapText.WrapText = true;

                    row.Cells[0].CellStyle = colHeaderStyle;
                    row.Cells[1].CellStyle = colHeaderStyle;
                    row.Cells[2].CellStyle = colHeaderStyle;
                    row.Cells[3].CellStyle = colHeaderStyle;
                    row.Cells[4].CellStyle = colHeaderStyle;
                    row.Cells[5].CellStyle = colHeaderStyle;
                    row.Cells[6].CellStyle = colHeaderStyle;
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.Status);
                        row.CreateCell(1).SetCellValue(item.StatusUpdateDate);
                        row.CreateCell(2).SetCellValue(item.Title);

                        irow++;
                    }
                    #endregion

                    workbook.Write(fs);
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EXPORT.ToString(),
                        TableName = "Accountability",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Accountability Exported",
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
            }
            else
            {
                return new BadRequestObjectResult(ErrorMessage);
            }
        }

        public async Task<IActionResult> OnGetExportAllAccountabilitiesList([FromQuery] GetMyAccountabilitiesListInput param)
        {
            var StatusColor = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(4)).WorkflowStepList;

            param.IsAdminAccess = _IsAdminAccess;
            param.IsClearance = _IsClearance;
            param.OrgGroupDescendantAccess = _globalCurrentUser.OrgGroupID.ToString();

            if (param.IsClearance)
            {
                List<int> GetOrg = new List<int>();
                var GetClearingDept = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                    .GetOrgGroupHierarchy(_globalCurrentUser.OrgGroupID)).Where(x => x.OrgType.Equals("DEPT") || x.OrgType.Equals("SECTION")).ToList();
                if (_globalCurrentUser.OrgGroupDescendants != null)
                    GetOrg.AddRange(_globalCurrentUser.OrgGroupDescendants);
                if (_globalCurrentUser.OrgGroupRovingDescendants != null)
                    GetOrg.AddRange(_globalCurrentUser.OrgGroupRovingDescendants);
                if (GetClearingDept != null)
                    GetOrg.AddRange(GetClearingDept.Select(x => x.ID).Distinct().ToList());
                if (GetOrg.Count() > 0)
                    param.OrgGroupDescendantAccess = string.Join(",", ((await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => GetOrg.Contains(x.OrgGroupID)).Select(x => x.OrgGroupID).ToList()));

                var EmployeeAccess = ((await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                    .GetEmployeeIDDescendant(_globalCurrentUser.EmployeeID)).Select(x => x.ID).ToList());
                EmployeeAccess.Add(_globalCurrentUser.EmployeeID);
                param.EmployeeIDDescendantAccess = string.Join(",", EmployeeAccess);
            }
            if (!param.IsAdminAccess && !param.IsClearance)
                param.EmployeeIDDelimited = _globalCurrentUser.EmployeeID.ToString();


            List<int> EmployeeID = new List<int>();

            if (!string.IsNullOrEmpty(param.EmployeeOrgDelimited))
            {
                var Employee = ((await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeByOrgGroup(param.EmployeeOrgDelimited.Split(',').Select(x => int.Parse(x)).ToList())).Item1).Select(x => x.ID).ToList();
                var GetActiveEmployee = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => Employee.Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList();
                EmployeeID.AddRange(GetActiveEmployee);
            }
            if (!string.IsNullOrEmpty(param.EmployeeOrgRegionDelimited))
            {
                var GetBranch = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                    .GetOrgGroupDescendantsList(param.EmployeeOrgRegionDelimited.Split(',').Select(x => int.Parse(x)).ToList())).ToList();
                var Employee = ((await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeByOrgGroup(GetBranch)).Item1).Select(x => x.ID).ToList();
                var GetActiveEmployee = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => Employee.Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList();
                EmployeeID.AddRange(GetActiveEmployee);
            }
            if (!string.IsNullOrEmpty(param.EmployeePosDelimited))
            {
                var Employee = ((await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeByPosition(param.EmployeePosDelimited.Split(',').Select(x => int.Parse(x)).ToList())).Item1).Select(x => x.ID).ToList();
                var GetActiveEmployee = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => Employee.Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList();
                EmployeeID.AddRange(GetActiveEmployee);
            }
            if (!string.IsNullOrEmpty(param.EmployeeOrgDelimited) || !string.IsNullOrEmpty(param.EmployeeOrgRegionDelimited) || !string.IsNullOrEmpty(param.EmployeePosDelimited))
            {
                if (EmployeeID.Count() > 0)
                    param.EmployeeIDDelimited = string.Join(',', EmployeeID);
                else
                    param.EmployeeIDDelimited = "0";
            }

            if (!string.IsNullOrEmpty(param.SeparationDateFrom) && !string.IsNullOrEmpty(param.SeparationDateTo))
            {
                var GetEmployeeByDate = (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeLastEmploymentStatusByDate(new GetEmployeeLastEmploymentStatusByDateInput { DateFrom = param.SeparationDateFrom, DateTo = param.SeparationDateTo })).Item1;
                var GetActiveEmployee = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => (GetEmployeeByDate.Select(x => x.ID)).Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList();
                param.EmployeeIDDelimited = string.Join(",", GetActiveEmployee);
            }

            if (!string.IsNullOrEmpty(param.HiredDateFrom) && !string.IsNullOrEmpty(param.HiredDateTo))
            {
                var GetEmployeeByDate = (await new Common_Employee(_iconfiguration,_globalCurrentUser,_env)
                    .GetEmployeeByDateHired(new GetEmployeeLastEmploymentStatusByDateInput { DateFrom = param.HiredDateFrom, DateTo = param.HiredDateTo })).Item1;
                var GetActiveEmployee = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetAllEmployeeAccountability()).Where(x => (GetEmployeeByDate.Select(x => x.ID)).Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList();
                param.EmployeeIDDelimited = GetActiveEmployee.Count() > 0 ? string.Join(",", GetActiveEmployee) : "0";
            }

            if (_systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARANCE/ALLACCOUNTABILITY")).Count() > 0)
            {
                List<int> GetOrg = new List<int>();
                if (_globalCurrentUser.OrgGroupDescendants != null)
                    GetOrg.AddRange(_globalCurrentUser.OrgGroupDescendants);
                if (_globalCurrentUser.OrgGroupRovingDescendants != null)
                    GetOrg.AddRange(_globalCurrentUser.OrgGroupRovingDescendants);
                if (GetOrg.Count() > 0)
                {
                    var GetEmployee = ((await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                        .GetEmployeeByOrgGroup(GetOrg)).Item1).Select(x => x.ID).ToList();
                    param.EmployeeIDDescendantAccess = string.Join(",", (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                        .GetAllEmployeeAccountability()).Where(x => GetEmployee.Contains(x.EmployeeID)).Select(x => x.EmployeeID).ToList());
                }
                param.IsAdminAccess = true;
            }

            var (Result, IsSuccess, ErrorMessage) = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetList(param);

            var employeeName = (await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeByIDs(Result.Where(x => x.EmployeeID != 0).Select(x => x.EmployeeID).Distinct().ToList())).Item1;

            var employeeLastEmploymentStatus = (await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeLastEmploymentStatus(Result.Where(x => x.EmployeeID != 0).Select(x => x.EmployeeID).Distinct().ToList())).Item1;

            var employeeOrg = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupFormatByID(employeeName.Select(x => x.OrgGroupID).Distinct().ToList())).Item1;

            var employeePos = (await new Common_Position(_iconfiguration, _globalCurrentUser, _env)
                        .GetAll());

            var clearingOrg = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupFormatByID(Result.Select(x => x.OrgGroupID).Distinct().ToList())).Item1;

            var statsUpdatedBy = await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(Result.Where(x => x.StatusUpdatedBy != 0).Select(x => x.StatusUpdatedBy).Distinct().ToList());

            var finalStatus = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeAccountabilityStatusPercentage(new GetEmployeeAccountabilityStatusPercentageInput() { EmployeeIDs = (Result.Where(x => x.EmployeeID != 0).Select(x => x.EmployeeID).Distinct().ToList() )})).Item1;

            var orgRegion = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupParent(new GetOrgGroupParentInput() { OrgGroupIDs = employeeOrg.Select(x => x.ID).ToList(), OrgType = "REG" })).Item1;

            Result = (from main in Result
                      join emp in employeeName on main.EmployeeID equals emp.ID
                      join employeeStatus in employeeLastEmploymentStatus on main.EmployeeID equals employeeStatus.ID
                      join status in StatusColor on main.Status equals status.StepCode
                      join empOrg in employeeOrg on emp.OrgGroupID equals empOrg.ID
                      join empOrgRegion in orgRegion on emp.OrgGroupID equals empOrgRegion.ID
                      join empPos in employeePos on emp.PositionID equals empPos.ID
                      join clearOrg in clearingOrg on main.OrgGroupID equals clearOrg.ID
                      join statUpdatedBy in statsUpdatedBy on main.StatusUpdatedBy equals statUpdatedBy.ID
                      join finalStats in finalStatus on main.EmployeeID equals finalStats.EmployeeID
                      select new GetMyAccountabilitiesListOutput()
                      {
                          TotalNoOfRecord = main.TotalNoOfRecord,
                          NoOfPages = main.NoOfPages,
                          Row = main.Row,

                          ID = main.ID,
                          CreatedDate = main.CreatedDate,
                          EmployeeName = string.Concat("(", emp.Code, ") ", emp.PersonalInformation.LastName, ", ", emp.PersonalInformation.FirstName, " ", ((emp.PersonalInformation.MiddleName == null || emp.PersonalInformation.MiddleName == "") ? "" : emp.PersonalInformation.MiddleName.Substring(0, 1))),
                          EmployeeStatusUpdatedDate = employeeStatus.StatusUpdatedDate.ToString("dd-MMM-yyyy"),
                          EmployeeDatehired = emp.DateHired?.ToString("dd-MMM-yyyy"),
                          Title = main.Title,
                          StatusDescription = status.StepDescription,
                          StatusColor = status.StatusColor,
                          StatusUpdateDate = main.StatusUpdateDate,
                          EmployeeOrg = empOrg.Result,
                          EmployeeOrgRegion = empOrgRegion.Result,
                          EmployeePos = string.Concat(empPos.Code, " - ", empPos.Title, " | ", emp.EmploymentStatus),
                          ClearingOrg = clearOrg.Result,
                          StatusUpdatedByName = string.Concat("(", statUpdatedBy.Username, ") ", statUpdatedBy.LastName, ", ", statUpdatedBy.FirstName, " ", ((statUpdatedBy.MiddleName == null || statUpdatedBy.MiddleName == "") ? "" : statUpdatedBy.MiddleName.Substring(0, 1))),
                          StatusUpdatedBy = main.StatusUpdatedBy,
                          StatusRemarks = main.StatusRemarks,
                          LastComment = main.LastComment,
                          LastCommentDate = main.LastCommentDate,

                          EmployeeID = main.EmployeeID,
                          Status = main.Status,
                          OrgGroupID = main.OrgGroupID,
                          OldEmployeeID = emp.OldEmployeeID,

                          Over = string.Concat(finalStats.OverDone," / ", finalStats.OverAll),
                          FinalStatus = finalStats.Status
                      }).ToList();

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "Accountabilities.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Accountabilities");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("ID");
                    row.CreateCell(1).SetCellValue("Employee");
                    row.CreateCell(2).SetCellValue("Date Separated");
                    row.CreateCell(3).SetCellValue("Date Hired");
                    row.CreateCell(4).SetCellValue("Title");
                    row.CreateCell(5).SetCellValue("Status");
                    row.CreateCell(6).SetCellValue("Status Updated Date");
                    row.CreateCell(7).SetCellValue("Employee Branch / Department");
                    row.CreateCell(8).SetCellValue("Employee Region");
                    row.CreateCell(9).SetCellValue("Employee Position");
                    row.CreateCell(10).SetCellValue("Clearing Department");
                    row.CreateCell(11).SetCellValue("Status Updated By");
                    row.CreateCell(12).SetCellValue("Status Remarks");
                    row.CreateCell(13).SetCellValue("Last Comment");
                    row.CreateCell(14).SetCellValue("Last Comment Date");
                    row.CreateCell(15).SetCellValue("Created Date");
                    row.CreateCell(16).SetCellValue("Old Employee ID");
                    row.CreateCell(17).SetCellValue("Clearance Percentage");
                    row.CreateCell(18).SetCellValue("Final Status");

                    excelSheet.SetColumnWidth(0, 3000);
                    excelSheet.SetColumnWidth(1, 10000);
                    excelSheet.SetColumnWidth(2, 5000);
                    excelSheet.SetColumnWidth(3, 5000);
                    excelSheet.SetColumnWidth(4, 10000);
                    excelSheet.SetColumnWidth(5, 7000);
                    excelSheet.SetColumnWidth(6, 5000);
                    excelSheet.SetColumnWidth(7, 10000);
                    excelSheet.SetColumnWidth(8, 7000);
                    excelSheet.SetColumnWidth(9, 10000);
                    excelSheet.SetColumnWidth(10, 10000);
                    excelSheet.SetColumnWidth(11, 5000);
                    excelSheet.SetColumnWidth(12, 5000);
                    excelSheet.SetColumnWidth(13, 5000);
                    excelSheet.SetColumnWidth(14, 5000);
                    excelSheet.SetColumnWidth(15, 5000);
                    excelSheet.SetColumnWidth(16, 3000);
                    excelSheet.SetColumnWidth(17, 5000);
                    excelSheet.SetColumnWidth(18, 7000);

                    XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle wrapText = (XSSFCellStyle)workbook.CreateCellStyle();

                    var colHeaderFont = workbook.CreateFont();
                    colHeaderFont.IsBold = true;
                    colHeaderStyle.SetFont(colHeaderFont);
                    colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                    colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                    colHeaderStyle.Alignment = HorizontalAlignment.Center;
                    alignCenter.Alignment = HorizontalAlignment.Center;
                    wrapText.WrapText = true;

                    row.Cells[0].CellStyle = colHeaderStyle;
                    row.Cells[1].CellStyle = colHeaderStyle;
                    row.Cells[2].CellStyle = colHeaderStyle;
                    row.Cells[3].CellStyle = colHeaderStyle;
                    row.Cells[4].CellStyle = colHeaderStyle;
                    row.Cells[5].CellStyle = colHeaderStyle;
                    row.Cells[6].CellStyle = colHeaderStyle;
                    row.Cells[7].CellStyle = colHeaderStyle;
                    row.Cells[8].CellStyle = colHeaderStyle;
                    row.Cells[9].CellStyle = colHeaderStyle;
                    row.Cells[10].CellStyle = colHeaderStyle;
                    row.Cells[11].CellStyle = colHeaderStyle;
                    row.Cells[12].CellStyle = colHeaderStyle;
                    row.Cells[13].CellStyle = colHeaderStyle;
                    row.Cells[14].CellStyle = colHeaderStyle;
                    row.Cells[15].CellStyle = colHeaderStyle;
                    row.Cells[16].CellStyle = colHeaderStyle;
                    row.Cells[17].CellStyle = colHeaderStyle;
                    row.Cells[18].CellStyle = colHeaderStyle;
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.ID);
                        row.CreateCell(1).SetCellValue(item.EmployeeName);
                        row.CreateCell(2).SetCellValue(item.EmployeeStatusUpdatedDate);
                        row.CreateCell(3).SetCellValue(item.EmployeeDatehired);
                        row.CreateCell(4).SetCellValue(item.Title);
                        row.CreateCell(5).SetCellValue(item.StatusDescription);
                        row.CreateCell(6).SetCellValue(item.StatusUpdateDate);
                        row.CreateCell(7).SetCellValue(item.EmployeeOrg);
                        row.CreateCell(8).SetCellValue(item.EmployeeOrgRegion);
                        row.CreateCell(9).SetCellValue(item.EmployeePos);
                        row.CreateCell(10).SetCellValue(item.ClearingOrg);
                        row.CreateCell(11).SetCellValue(item.StatusUpdatedByName);
                        row.CreateCell(12).SetCellValue(item.StatusRemarks);
                        row.CreateCell(13).SetCellValue(item.LastComment);
                        row.CreateCell(14).SetCellValue(item.LastCommentDate);
                        row.CreateCell(15).SetCellValue(item.CreatedDate);
                        row.CreateCell(16).SetCellValue(item.OldEmployeeID);
                        row.CreateCell(17).SetCellValue(item.Over);
                        row.CreateCell(18).SetCellValue(item.FinalStatus);

                        irow++;
                    }
                    #endregion

                    workbook.Write(fs);
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EXPORT.ToString(),
                        TableName = "Accountability",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Accountabilities Exported",
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
            }
            else
            {
                return new BadRequestObjectResult(ErrorMessage);
            }
        }

        public async Task<IActionResult> OnGetExportClearanceList([FromQuery] GetMyAccountabilitiesListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetList(param);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "Clearance.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Clearance");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Employee Name");
                    row.CreateCell(1).SetCellValue("Status");
                    row.CreateCell(2).SetCellValue("Status Update Date");
                    row.CreateCell(3).SetCellValue("Title");
                    row.CreateCell(4).SetCellValue("Employee Branch / Department");
                    row.CreateCell(5).SetCellValue("Position");
                    row.CreateCell(6).SetCellValue("Last Update By");
                    row.CreateCell(7).SetCellValue("Last Update Date");
                    row.CreateCell(8).SetCellValue("Old Employee ID");
                    row.CreateCell(9).SetCellValue("Remarks");
                    row.CreateCell(10).SetCellValue("Last Comment");

                    excelSheet.SetColumnWidth(0, 10000);
                    excelSheet.SetColumnWidth(1, 7000);
                    excelSheet.SetColumnWidth(2, 7000);
                    excelSheet.SetColumnWidth(3, 10000);
                    excelSheet.SetColumnWidth(4, 7000);
                    excelSheet.SetColumnWidth(5, 7000);
                    excelSheet.SetColumnWidth(6, 7000);
                    excelSheet.SetColumnWidth(7, 7000);
                    excelSheet.SetColumnWidth(8, 7000);
                    excelSheet.SetColumnWidth(9, 7000);
                    excelSheet.SetColumnWidth(10, 7000);

                    XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle wrapText = (XSSFCellStyle)workbook.CreateCellStyle();

                    var colHeaderFont = workbook.CreateFont();
                    colHeaderFont.IsBold = true;
                    colHeaderStyle.SetFont(colHeaderFont);
                    colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                    colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                    colHeaderStyle.Alignment = HorizontalAlignment.Center;
                    alignCenter.Alignment = HorizontalAlignment.Center;
                    wrapText.WrapText = true;

                    row.Cells[0].CellStyle = colHeaderStyle;
                    row.Cells[1].CellStyle = colHeaderStyle;
                    row.Cells[2].CellStyle = colHeaderStyle;
                    row.Cells[3].CellStyle = colHeaderStyle;
                    row.Cells[4].CellStyle = colHeaderStyle;
                    row.Cells[5].CellStyle = colHeaderStyle;
                    row.Cells[6].CellStyle = colHeaderStyle;
                    row.Cells[7].CellStyle = colHeaderStyle;
                    row.Cells[8].CellStyle = colHeaderStyle;
                    row.Cells[9].CellStyle = colHeaderStyle;
                    row.Cells[10].CellStyle = colHeaderStyle;
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(1).SetCellValue(item.Status);
                        row.CreateCell(2).SetCellValue(item.StatusUpdateDate);
                        row.CreateCell(3).SetCellValue(item.Title);
                        row.CreateCell(10).SetCellValue(item.LastComment);

                        irow++;
                    }
                    #endregion

                    workbook.Write(fs);
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EXPORT.ToString(),
                        TableName = "Accountability",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Clearance Exported",
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
            }
            else
            {
                return new BadRequestObjectResult(ErrorMessage);
            }
        }

        public async Task<JsonResult> OnGetAccountabilityChangeStatus(string CurrentStatus, string Form)
        {
            var roles = (await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                   .GetSystemUserRoleDropDownByUserID(_globalCurrentUser.UserID)).ToList();

            var status = (await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetNextWorkflowStep(new GetNextWorkflowStepInput
                {
                    WorkflowCode = "ACCOUNTABILITY",
                    CurrentStepCode = CurrentStatus,
                    RoleIDDelimited = string.Join(",", roles.Select(x => x.Value).ToArray())
                })).Where(x=> (CurrentStatus!="NEW" ? !x.Code.Equals("ACCEPTED") : true) && (Form.Equals("Clearance") ? !x.Code.Equals("REPLIED") : true) && !x.Code.Equals(CurrentStatus)).ToList();

            if (Form.Equals("Employee"))
            {
                if (CurrentStatus.Equals("NEW"))
                    status = status.Where(x => x.Code.Equals("ACCEPTED")).ToList();
                else if (CurrentStatus.Equals("PENDING"))
                    status = status.Where(x => x.Code.Equals("REPLIED")).ToList();
                else
                    status = new List<GetNextWorkflowStepOutput>();
            }


            _resultView.Result = status.OrderByDescending(x => x.Order).Select(x => new SelectListItem
            {
                Value = x.Code,
                Text = x.Description,
                Selected = x.Code.Equals(CurrentStatus),
                Disabled = x.Code.Equals(CurrentStatus) ? true : false
            }).ToList();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostAccountabilityChangeStatus(string ID, string NewStatus, string Remarks)
        {
            ChangeStatus changeStatus = new ChangeStatus();
            changeStatus.ID = (ID.Split(",")).Select(x => long.Parse(x)).ToList();
            changeStatus.Status = NewStatus;
            changeStatus.Remarks = Remarks;

            var (IsSuccess, Message) = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env).PostChangeStatus(changeStatus);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = "CHANGE STATUS",
                        TableName = "EmployeeAccountabilityStatusHistory",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat("Change Status ID: ", ID),
                        IsSuccess = IsSuccess,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                var AllStatus = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetWorkflow(4)).WorkflowStepList;
                var CurrentStatus = AllStatus.Where(x => x.StepCode.Equals(changeStatus.Status)).FirstOrDefault();
                if (CurrentStatus.SendEmailToApprover || CurrentStatus.SendEmailToRequester)
                {
                    var NextStatus = AllStatus.Where(x => x.Order.Equals(CurrentStatus.Order + 1)).ToList();

                    var GetInactiveEmploymentStatus = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                        .GetReferenceValueByRefCode("ESTATUS")).Where(x=>x.Value.Equals("INACTIVE")).Select(y=>y.Description).FirstOrDefault();

                    var EmployeeAccountability = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                        .GetEmployeeByEmployeeAccountabilityIDs(changeStatus.ID)).Item1;
                    var Employee = ((await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                        .GetEmployeeByIDs(EmployeeAccountability.Select(x => x.EmployeeID).Distinct().ToList())).Item1).ToList();
                    var Position = (await new Common_Position(_iconfiguration, _globalCurrentUser, _env)
                        .GetAll());

                    if (CurrentStatus.SendEmailToRequester)
                    {
                        var OrgGroup = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                            .GetOrgGroupByIDs(Employee.Select(x => x.OrgGroupID).ToList())).Item1;
                        var emails = (from left in Employee.Where(x => (GetInactiveEmploymentStatus.Contains(x.EmploymentStatus) ? !string.IsNullOrEmpty(x.PersonalInformation.Email) : !string.IsNullOrEmpty(x.CorporateEmail)))
                                      join right in EmployeeAccountability on left.ID equals right.EmployeeID
                                      join rightPosition in Position on left.PositionID equals rightPosition.ID
                                      join rightOrgGroup in OrgGroup on left.OrgGroupID equals rightOrgGroup.ID
                                      select new EmailLogsInput()
                                      {
                                          PositionTitle = String.Concat(rightPosition.Code, " - ", rightPosition.Title),
                                          Status = CurrentStatus.StepDescription,
                                          SystemCode = "EMS ACCOUNTABILITY",
                                          SenderName = "EMS ACCOUNTABILITY",
                                          FromEmailAddress = "noreply@motortrade.com.ph",
                                          Name = String.Concat("(", left.Code, ") ", left.PersonalInformation.LastName, ", ", left.PersonalInformation.FirstName, " ", left.PersonalInformation.MiddleName.Substring(0, 1)),
                                          ToEmailAddress = (GetInactiveEmploymentStatus.Contains(left.EmploymentStatus) ? left.PersonalInformation.Email : left.CorporateEmail),
                                          Subject = String.Concat("CHANGE STATUS: ACCOUNTABILITY | ", right.Title, " | ", CurrentStatus.StepDescription),
                                          Body = MessageUtilities.EMAIL_BODY_TRAINING
                                                    .Replace("&lt;FormName&gt;", "ACCOUNTABILITY")
                                                    .Replace("&lt;RecordID&gt;", right.ID.ToString().PadLeft(7, '0'))
                                                    .Replace("&lt;EmployeeName&gt;", String.Concat(left.PersonalInformation.LastName, ", ", left.PersonalInformation.FirstName, " ", left.PersonalInformation.MiddleName.Substring(0, 1)))
                                                    .Replace("&lt;Remarks&gt;", changeStatus.Remarks)
                                                    .Replace("&lt;CurrentStatus&gt;", CurrentStatus.StepDescription)
                                                    .Replace("&lt;UpdatedBy&gt;", String.Concat("(", _globalCurrentUser.Username, ") ", _globalCurrentUser.LastName ?? "", ", ", _globalCurrentUser.FirstName ?? "", " ", string.IsNullOrEmpty(_globalCurrentUser.MiddleName ?? "").ToString().Substring(0, 1)))
                                                    .Replace("&lt;UpdatedDate&gt;", DateTime.Now.ToString("yyyy-MM-dd HH:mm")),
                                      }).ToList();

                        var (IsSuccessEmail, MessageEmail) = await new Common_EmailServerCredential(_iconfiguration, _globalCurrentUser, _env)
                            .AddMultipleEmailLogs(emails);
                    }
                    if (CurrentStatus.SendEmailToApprover)
                    {
                        List<int> UserRoleID = new List<int>();
                        UserRoleID.Add(163);
                        //UserRoleID.Add(154); //PROD
                        var SystemUserIDApprover = (await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                            .GetUserByRoleIDs(UserRoleID.Distinct().ToList())).Select(x => x.ID).ToList();
                        var ApproverEmployee = ((await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                            .GetEmployeeByUserIDs(SystemUserIDApprover)).Item1).Where(x=>!string.IsNullOrEmpty(x.CorporateEmail)).ToList();
                        var OrgGroup = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                            .GetOrgGroupByIDs(ApproverEmployee.Select(x => x.OrgGroupID).ToList())).Item1;

                        var emails = (from left in ApproverEmployee
                                      join right in EmployeeAccountability on left.OrgGroupID equals right.OrgGroupID
                                      join rightEmployee in Employee on right.EmployeeID equals rightEmployee.ID
                                      join rightPosition in Position on left.PositionID equals rightPosition.ID
                                      join rightOrgGroup in OrgGroup on left.OrgGroupID equals rightOrgGroup.ID
                                      select new EmailLogsInput()
                                      {
                                          PositionTitle = String.Concat(rightPosition.Code, " - ", rightPosition.Title),
                                          Status = CurrentStatus.StepDescription,
                                          SystemCode = "EMS ACCOUNTABILITY",
                                          SenderName = "EMS ACCOUNTABILITY",
                                          FromEmailAddress = "noreply@motortrade.com.ph",
                                          Name = String.Concat("(", left.Code,") ", left.PersonalInformation.LastName, ", ", left.PersonalInformation.FirstName, " ", ((left.PersonalInformation.MiddleName == null || left.PersonalInformation.MiddleName == "") ? "" : left.PersonalInformation.MiddleName.Substring(0, 1))),
                                          ToEmailAddress = left.CorporateEmail,
                                          Subject = String.Concat("CHANGE STATUS: ACCOUNTABILITY | ", right.Title, " | ", CurrentStatus.StepDescription),
                                          Body = MessageUtilities.EMAIL_BODY_TRAINING
                                                    .Replace("&lt;FormName&gt;", "ACCOUNTABILITY")
                                                    .Replace("&lt;RecordID&gt;", right.ID.ToString().PadLeft(7, '0'))
                                                    .Replace("&lt;EmployeeName&gt;", String.Concat("(", rightEmployee.Code, ") ", rightEmployee.PersonalInformation.LastName, ", ", rightEmployee.PersonalInformation.FirstName, " ", ((rightEmployee.PersonalInformation.MiddleName == null || rightEmployee.PersonalInformation.MiddleName == "") ? "" : rightEmployee.PersonalInformation.MiddleName.Substring(0, 1))))
                                                    .Replace("&lt;Remarks&gt;", changeStatus.Remarks)
                                                    .Replace("&lt;CurrentStatus&gt;", CurrentStatus.StepDescription)
                                                    .Replace("&lt;UpdatedBy&gt;", String.Concat("(", _globalCurrentUser.Username, ") ", _globalCurrentUser.LastName ?? "", ", ", _globalCurrentUser.FirstName ?? "", " ", ((_globalCurrentUser.MiddleName == null || _globalCurrentUser.MiddleName == "") ? "" : _globalCurrentUser.MiddleName.Substring(0, 1))))
                                                    .Replace("&lt;UpdatedDate&gt;", DateTime.Now.ToString("yyyy-MM-dd HH:mm")),
                                      }).ToList();
                        
                        var (IsSuccessEmail, MessageEmail) = await new Common_EmailServerCredential(_iconfiguration, _globalCurrentUser, _env)
                            .AddMultipleEmailLogs(emails);
                    }
                }
            }

            /*var AccountabilityWorkflow = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(4);
            var SendEmailToRequestor = AccountabilityWorkflow.WorkflowStepList.Where(y => y.StepCode.Equals(NewStatus)).Select(x => x.SendEmailToRequester).FirstOrDefault();
            var SendEmailToApprover = AccountabilityWorkflow.WorkflowStepList.Where(y => y.StepCode.Equals(NewStatus)).Select(x => x.SendEmailToApprover).FirstOrDefault();

            List<int> AccountabilityID = (ID.Split(",").Select(int.Parse)).ToList();
            foreach (var item in AccountabilityID)
            {
                EmployeeAccountabilityForm employeeAccountabilityForm = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeAccountabilityByID(item);

                employeeAccountabilityForm.CreatedBy = _globalCurrentUser.UserID;
                employeeAccountabilityForm.Remarks = Remarks;
                employeeAccountabilityForm.Status = NewStatus;

                var URL = string.Concat(_workflowBaseURL,
                        _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("AddEmployeeAccountabilityStatusHistory").Value, "?",
                        "userid=", _globalCurrentUser.UserID);

                var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(employeeAccountabilityForm, URL);

                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {
                    var Employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployee(employeeAccountabilityForm.EmployeeID);
                    var Position = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPosition(Employee.PositionID);

                    if (SendEmailToApprover)
                    {
                        var Approver = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmail(employeeAccountabilityForm.OrgGroupID, "APPROVER");
                        if (Approver.Count() > 0)
                        {
                            foreach (var ApproverDetails in Approver)
                            {
                                *//*var EmailSenderEmail = await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                                    .GetReferenceValueByRefCodeAndValue(ReferenceCodes_Workflow.EMAIL.ToString(), ReferenceCodes_Email.SENDER_EMAIL.ToString());
                                var EmailChangeStatus = await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                                    .GetReferenceValueByRefCodeAndValue(ReferenceCodes_Workflow.EMAIL.ToString(), ReferenceCodes_Email.CHANGE_STATUS.ToString());
                                var EmailBody = await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                                    .GetReferenceValueByRefCodeAndValue(ReferenceCodes_Workflow.EMAIL.ToString(), ReferenceCodes_Email.CLEARANCE_BODY.ToString());
                                var EmailForm = await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                                    .GetReferenceValueByRefCodeAndValue(ReferenceCodes_Workflow.EMAIL.ToString(), ReferenceCodes_Email.CLEARANCE_FORM.ToString());
                                var EmpName = String.Concat(Employee.PersonalInformation.LastName, ", ", Employee.PersonalInformation.FirstName, " ", Employee.PersonalInformation.MiddleName.Substring(0, 1));
                                var Subject = String.Concat(EmailChangeStatus.Description, " ", EmailForm.Description, " | ", employeeAccountabilityForm.Title, " | ", NewStatus);
                                var UpdatedBy = String.Concat("(", _globalCurrentUser.Username, ") ", _globalCurrentUser.LastName ?? "", ", ", _globalCurrentUser.FirstName ?? "", " ", string.IsNullOrEmpty(_globalCurrentUser.MiddleName ?? "").ToString().Substring(0, 1));
                                var RecordID = item.ToString().PadLeft(7, '0');*//*

                                var EmailBody = "The record has been added/updated. Please login to EMS for the complete details. <br><br><strong>Form Name:</strong> &lt;FormName&gt;<br><strong>Record ID:</strong> &lt;RecordID&gt;<br><strong>Employee:</strong> &lt;EmployeeName&gt;<br><strong>Remarks:</strong>  &lt;Remarks&gt;<br><strong>Current Status:</strong> &lt;CurrentStatus&gt;<br><strong>Updated By:</strong> &lt;UpdatedBy&gt;<br><strong>Updated Date:</strong> &lt;UpdatedDate&gt;<br><br><i>Note: This is a system-generated email only. Please do not reply.</i>";
                                var EmpName = String.Concat(Employee.PersonalInformation.LastName, ", ", Employee.PersonalInformation.FirstName, " ", Employee.PersonalInformation.MiddleName.Substring(0, 1));
                                var Subject = String.Concat("CHANGE STATUS: ACCOUNTABILITY | ", employeeAccountabilityForm.Title, " | ", NewStatus);
                                var UpdatedBy = String.Concat("(", _globalCurrentUser.Username, ") ", _globalCurrentUser.LastName ?? "", ", ", _globalCurrentUser.FirstName ?? "", " ", string.IsNullOrEmpty(_globalCurrentUser.MiddleName ?? "").ToString().Substring(0, 1));
                                var RecordID = item.ToString().PadLeft(7, '0');

                                EmailLogsInput emailLogs = new EmailLogsInput()
                                {
                                    PositionTitle = String.Concat(Position.Code, " - ", Position.Title),
                                    Status = NewStatus,
                                    SystemCode = "EMS Clearance",
                                    SenderName = "EMS Clearance",
                                    FromEmailAddress = "noreply@motortrade.com.ph",
                                    Name = EmpName,
                                    ToEmailAddress = ApproverDetails.Email,
                                    Subject = Subject,
                                    Body = EmailBody
                                        .Replace("&lt;FormName&gt;", "ACCOUNTABILITY")
                                        .Replace("&lt;RecordID&gt;", RecordID)
                                        .Replace("&lt;EmployeeName&gt;", EmpName)
                                        .Replace("&lt;Remarks&gt;", Remarks)
                                        .Replace("&lt;CurrentStatus&gt;", NewStatus)
                                        .Replace("&lt;UpdatedBy&gt;", UpdatedBy)
                                        .Replace("&lt;UpdatedDate&gt;", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                };

                                var (IsSuccessEmail, MessageEmail) = await new Common_EmailServerCredential(_iconfiguration, _globalCurrentUser, _env)
                                    .AddEmailLogs(emailLogs);
                            }
                        }
                    }

                    if (SendEmailToRequestor)
                    {
                        var EmployeeDetails = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmail(employeeAccountabilityForm.EmployeeID, "EMPLOYEE");
                        if (EmployeeDetails.Count() > 0)
                        {
                            var EmailBody = "The record has been added/updated. Please login to EMS for the complete details. <br><br><strong>Form Name:</strong> &lt;FormName&gt;<br><strong>Record ID:</strong> &lt;RecordID&gt;<br><strong>Employee:</strong> &lt;EmployeeName&gt;<br><strong>Remarks:</strong>  &lt;Remarks&gt;<br><strong>Current Status:</strong> &lt;CurrentStatus&gt;<br><strong>Updated By:</strong> &lt;UpdatedBy&gt;<br><strong>Updated Date:</strong> &lt;UpdatedDate&gt;<br><br><i>Note: This is a system-generated email only. Please do not reply.</i>";
                            var EmpName = String.Concat(Employee.PersonalInformation.LastName, ", ", Employee.PersonalInformation.FirstName, " ", Employee.PersonalInformation.MiddleName.Substring(0, 1));
                            var Subject = String.Concat("CHANGE STATUS: ACCOUNTABILITY | ", employeeAccountabilityForm.Title, " | ", NewStatus);
                            var UpdatedBy = String.Concat("(", _globalCurrentUser.Username, ") ", _globalCurrentUser.LastName ?? "", ", ", _globalCurrentUser.FirstName ?? "", " ", string.IsNullOrEmpty(_globalCurrentUser.MiddleName ?? "").ToString().Substring(0, 1));
                            var RecordID = item.ToString().PadLeft(7, '0');

                            EmailLogsInput emailLogs = new EmailLogsInput()
                            {
                                PositionTitle = String.Concat(Position.Code, " - ", Position.Title),
                                Status = NewStatus,
                                SystemCode = "EMS Clearance",
                                SenderName = "EMS Clearance",
                                FromEmailAddress = "noreply@motortrade.com.ph",
                                Name = EmpName,
                                ToEmailAddress = EmployeeDetails.Select(x => x.Email).FirstOrDefault(),
                                Subject = Subject,
                                Body = EmailBody
                                    .Replace("&lt;FormName&gt;", "ACCOUNTABILITY")
                                    .Replace("&lt;RecordID&gt;", RecordID)
                                    .Replace("&lt;EmployeeName&gt;", EmpName)
                                    .Replace("&lt;Remarks&gt;", Remarks)
                                    .Replace("&lt;CurrentStatus&gt;", NewStatus)
                                    .Replace("&lt;UpdatedBy&gt;", UpdatedBy)
                                    .Replace("&lt;UpdatedDate&gt;", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                            };

                            var (IsSuccessEmail, MessageEmail) = await new Common_EmailServerCredential(_iconfiguration, _globalCurrentUser, _env)
                                .AddEmailLogs(emailLogs);
                        }
                    }


                    //Add AuditLog
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EDIT.ToString(),
                        TableName = "EmployeeAccountabilityStatusHistory",
                        TableID = employeeAccountabilityForm.ID,
                        Remarks = string.Concat(employeeAccountabilityForm.ID, " Employee Accountability Status Changed to ", employeeAccountabilityForm.Status),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
                }
            }*/

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostDeleteAccountability([FromQuery] string ID)
        {
            var (IsSuccess, Message) = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env).BulkEmployeeAccountabilityDelete(ID);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetStatusFilter()
        {
            var StatusFilter = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(4)).WorkflowStepList;
            _resultView.Result = StatusFilter.Select(x => new Utilities.API.ReferenceMaintenance.MultiSelectedFilter()
            {
                ID = x.StepCode,
                Description = x.StepDescription
            });
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetQuestionByCategory(string Category,int EmployeeID)
        {
            var (Result,IsSuccess,Message) = await new Common_Question(_iconfiguration, _globalCurrentUser, _env)
                .GetQuestionEmployeeAnswer(Category, EmployeeID);
            _resultView.Result = Result;
            _resultView.IsSuccess = IsSuccess;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetEmployeeMovementByEmployeeIDs(int EmployeeID)
        {
            var (Result, IsSuccess, Message) = await new Common_EmployeeMovement(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeMovementByEmployeeIDs(new List<int>() { EmployeeID });
            var jsonData = new
            {
                rows = Result
            };
            return new JsonResult(jsonData);
        }
        public async Task<IActionResult> OnGetCheckQuestionEmployeeAnswersExport()
        {
            var (Result, IsSuccess, Message) = await new Common_Question(_iconfiguration, _globalCurrentUser, _env)
                .GetQuestionEmployeeAnswersExport(new List<QuestionEmployeeAnswerInput>());

            if (IsSuccess)
            {
                if (Result.Count > 0)
                {
                    _resultView.IsSuccess = true;
                }
                else
                {
                    _resultView.IsSuccess = false;
                    _resultView.Result = MessageUtilities.ERRMSG_NO_RECORDS;
                }

                return new JsonResult(_resultView);
            }
            else
            {
                return new BadRequestObjectResult(Message);
            }
        }
        public async Task<IActionResult> OnGetQuestionEmployeeAnswersExport()
        {
            var (ResultQuestion, IsSuccessQuestion, MessageQuestion) = await new Common_Question(_iconfiguration, _globalCurrentUser, _env)
                .GetQuestion();
            ResultQuestion = ResultQuestion.Where(x=>x.Category.Equals("EXITINTERVIEW")).ToList();
            var (Result, IsSuccess, Message) = await new Common_Question(_iconfiguration, _globalCurrentUser, _env)
                .GetQuestionEmployeeAnswersExport(new List<QuestionEmployeeAnswerInput>());

            var employeeName = (await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeByIDs(Result.Where(x => x.EmployeeID != 0).Select(x => x.EmployeeID).Distinct().ToList())).Item1;

            var employeeLastEmploymentStatus = (await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeLastEmploymentStatus(Result.Where(x => x.EmployeeID != 0).Select(x => x.EmployeeID).Distinct().ToList())).Item1;

            var employeeOrg = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupFormatByID(employeeName.Select(x => x.OrgGroupID).Distinct().ToList())).Item1;

            var employeePos = (await new Common_Position(_iconfiguration, _globalCurrentUser, _env)
                        .GetAll());
            Result = (from main in Result
                      join emp in employeeName on main.EmployeeID equals emp.ID
                      join employeeStatus in employeeLastEmploymentStatus on main.EmployeeID equals employeeStatus.ID
                      join empOrg in employeeOrg on emp.OrgGroupID equals empOrg.ID
                      join empPos in employeePos on emp.PositionID equals empPos.ID
                      select new SPGetQuestionEmployeeAnswerExport()
                      {
                          ID = main.ID,
                          EmployeeID = main.EmployeeID,
                          ColumnG = main.ColumnG,
                          ColumnH = main.ColumnH,
                          ColumnI = main.ColumnI,
                          ColumnJ = main.ColumnJ,
                          ColumnK = main.ColumnK,
                          ColumnL = main.ColumnL,
                          ColumnM = main.ColumnM,
                          ColumnN = main.ColumnN,
                          ColumnO = main.ColumnO,
                          ColumnP = main.ColumnP,

                          OrgGroup = empOrg.Result,
                          EmployeeName = string.Concat("(", emp.Code, ") ", emp.PersonalInformation.LastName, ", ", emp.PersonalInformation.FirstName, " ", ((emp.PersonalInformation.MiddleName == null || emp.PersonalInformation.MiddleName == "") ? "" : emp.PersonalInformation.MiddleName.Substring(0, 1))),
                          Position = string.Concat(empPos.Code, " - ", empPos.Title, " | ", emp.EmploymentStatus),
                          DateHired = emp.DateHired?.ToString("dd-MMM-yyyy"),
                          DateSeparated = employeeStatus.StatusUpdatedDate.ToString("dd-MMM-yyyy")
                      }).ToList();

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "QuestionEmployeeAnswer.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Question Employee Answer");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Count");
                    row.CreateCell(1).SetCellValue("Branch / Department");
                    row.CreateCell(2).SetCellValue("Employee Name");
                    row.CreateCell(3).SetCellValue("Position");
                    row.CreateCell(4).SetCellValue("Date Hired");
                    row.CreateCell(5).SetCellValue("Date Separated");
                    row.CreateCell(6).SetCellValue(ResultQuestion.Where(y=>y.Code.Equals("Q2")).Select(x=>x.Question).FirstOrDefault());
                    row.CreateCell(7).SetCellValue(ResultQuestion.Where(y => y.Code.Equals("Q3")).Select(x => x.Question).FirstOrDefault());
                    row.CreateCell(8).SetCellValue(ResultQuestion.Where(y => y.Code.Equals("Q4")).Select(x => x.Question).FirstOrDefault());
                    row.CreateCell(9).SetCellValue(ResultQuestion.Where(y => y.Code.Equals("Q5")).Select(x => x.Question).FirstOrDefault());
                    row.CreateCell(10).SetCellValue(ResultQuestion.Where(y => y.Code.Equals("Q6")).Select(x => x.Question).FirstOrDefault());
                    row.CreateCell(11).SetCellValue(ResultQuestion.Where(y => y.Code.Equals("Q7")).Select(x => x.Question).FirstOrDefault());
                    row.CreateCell(12).SetCellValue(ResultQuestion.Where(y => y.Code.Equals("Q8")).Select(x => x.Question).FirstOrDefault());
                    row.CreateCell(13).SetCellValue(ResultQuestion.Where(y => y.Code.Equals("Q9")).Select(x => x.Question).FirstOrDefault());
                    row.CreateCell(14).SetCellValue(ResultQuestion.Where(y => y.Code.Equals("Q10")).Select(x => x.Question).FirstOrDefault());
                    row.CreateCell(15).SetCellValue(ResultQuestion.Where(y => y.Code.Equals("Q11")).Select(x => x.Question).FirstOrDefault());

                    excelSheet.SetColumnWidth(0, 3000);
                    excelSheet.SetColumnWidth(1, 5000);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 5000);
                    excelSheet.SetColumnWidth(4, 5000);
                    excelSheet.SetColumnWidth(5, 5000);
                    excelSheet.SetColumnWidth(6, 10000);
                    excelSheet.SetColumnWidth(7, 10000);
                    excelSheet.SetColumnWidth(8, 10000);
                    excelSheet.SetColumnWidth(9, 10000);
                    excelSheet.SetColumnWidth(10, 10000);
                    excelSheet.SetColumnWidth(11, 10000);
                    excelSheet.SetColumnWidth(12, 10000);
                    excelSheet.SetColumnWidth(13, 10000);
                    excelSheet.SetColumnWidth(14, 10000);
                    excelSheet.SetColumnWidth(15, 10000);

                    XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle wrapText = (XSSFCellStyle)workbook.CreateCellStyle();

                    var colHeaderFont = workbook.CreateFont();
                    colHeaderFont.IsBold = true;
                    colHeaderStyle.SetFont(colHeaderFont);
                    colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                    colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                    colHeaderStyle.Alignment = HorizontalAlignment.Center;
                    alignCenter.Alignment = HorizontalAlignment.Center;
                    wrapText.WrapText = true;

                    row.Cells[0].CellStyle = colHeaderStyle;
                    row.Cells[1].CellStyle = colHeaderStyle;
                    row.Cells[2].CellStyle = colHeaderStyle;
                    row.Cells[3].CellStyle = colHeaderStyle;
                    row.Cells[4].CellStyle = colHeaderStyle;
                    row.Cells[5].CellStyle = colHeaderStyle;
                    row.Cells[6].CellStyle = colHeaderStyle;
                    row.Cells[7].CellStyle = colHeaderStyle;
                    row.Cells[8].CellStyle = colHeaderStyle;
                    row.Cells[9].CellStyle = colHeaderStyle;
                    row.Cells[10].CellStyle = colHeaderStyle;
                    row.Cells[11].CellStyle = colHeaderStyle;
                    row.Cells[12].CellStyle = colHeaderStyle;
                    row.Cells[13].CellStyle = colHeaderStyle;
                    row.Cells[14].CellStyle = colHeaderStyle;
                    row.Cells[15].CellStyle = colHeaderStyle;
                    #endregion

                    var rows = 0;
                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(rows += 1);
                        row.CreateCell(1).SetCellValue(item.OrgGroup);
                        row.CreateCell(2).SetCellValue(item.EmployeeName);
                        row.CreateCell(3).SetCellValue(item.Position);
                        row.CreateCell(4).SetCellValue(item.DateHired);
                        row.CreateCell(5).SetCellValue(item.DateSeparated);
                        row.CreateCell(6).SetCellValue(item.ColumnG);
                        row.CreateCell(7).SetCellValue(item.ColumnH);
                        row.CreateCell(8).SetCellValue(item.ColumnI);
                        row.CreateCell(9).SetCellValue(item.ColumnJ);
                        row.CreateCell(10).SetCellValue(item.ColumnK);
                        row.CreateCell(11).SetCellValue(item.ColumnL);
                        row.CreateCell(12).SetCellValue(item.ColumnM);
                        row.CreateCell(13).SetCellValue(item.ColumnN);
                        row.CreateCell(14).SetCellValue(item.ColumnO);
                        row.CreateCell(15).SetCellValue(item.ColumnP);

                        irow++;
                    }
                    #endregion

                    workbook.Write(fs);
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EXPORT.ToString(),
                        TableName = "QuestionEmployeeAnswer",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Question Employee Answer Exported",
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
            }
            else
            {
                return new BadRequestObjectResult(Message);
            }
        }
    }
}