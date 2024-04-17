using EMS.Plantilla.Transfer.Employee;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using Microsoft.AspNetCore.Hosting;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Workflow.Transfer.LogActivity;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.FrontEnd.SharedClasses;
using EMS.Workflow.Transfer.Accountability;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.Workflow.Transfer.Workflow;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;
using System;
using NPOI.SS.Util;
using EMS.FrontEnd.SharedClasses.Common_PSGC;
using Microsoft.AspNetCore.Http;
using System.Text;
using NPOI.HSSF.UserModel;
using EMS.Security.Transfer.SystemUser;
using System.Composition;
using NPOI.SS.Formula.Functions;
using EMS.Workflow.Transfer.EmailServerCredential;
using MySqlX.XDevAPI.Common;
using Microsoft.AspNetCore.SignalR;

namespace EMS.FrontEnd.Pages.Plantilla.Employee
{
    public class IndexModel : SharedClasses.Utilities
    {

        [BindProperty]
        public EMS.Plantilla.Transfer.Employee.EmployeeAttachmentForm EmployeeAttachmentForm { get; set; }

        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/ADD")).Count() > 0 ? "true" : "false";
                ViewData["HasExportFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/EXPORT")).Count() > 0 ? "true" : "false";
                ViewData["HasExportETFFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/EXPORTETF")).Count() > 0 ? "true" : "false";
                ViewData["HasUploadFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE?HANDLER=UPLOAD")).Count() > 0 ? "true" : "false";
                ViewData["HasChangeToDraftFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/CHANGETODRAFT")).Count() > 0 ? "true" : "false";
                ViewData["HasExportDraftFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/EXPORTDRAFT")).Count() > 0 ? "true" : "false";
                ViewData["HasuploadDraftFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/UPLOADDRAFT")).Count() > 0 ? "true" : "false";
            }
        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] EMS.Plantilla.Transfer.Employee.GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataList(param, false);

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

        public async Task<JsonResult> OnGetOrgTypeAutoCompleteAsync(string term, int TopResults)
        {
            /*var Org = string.Concat(string.Join(",", _globalCurrentUser.OrgGroupID),
                (_globalCurrentUser.OrgGroupDescendants == null ? "" : "," + string.Join(",", _globalCurrentUser.OrgGroupDescendants)),
                (_globalCurrentUser.OrgGroupRovingDescendants == null ? "" : "," + string.Join(",", _globalCurrentUser.OrgGroupRovingDescendants)),
                (_globalCurrentUser.Roving == null ? "" : "," + string.Join(",", _globalCurrentUser.Roving.Select(x => x.OrgGroupID).ToList())));*/

            //var ConvertToList = (Org.Split(",")).Select(int.Parse).ToList();

            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            //_resultView.Result = result.Where(x => ConvertToList.Contains(x.ID)).ToList();
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetOrgGroupByOrgTypeAutoCompleteAsync(EMS.Plantilla.Transfer.OrgGroup.GetByOrgTypeAutoCompleteInput param)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupByOrgTypeAutoComplete(param);
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

        public async Task<JsonResult> OnGetEmployeeAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployeeAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }


        public async Task<JsonResult> OnGetUserNameAutoComplete(string Term, int TopResults)
        {
            var result = await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env).GetSystemUserAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionDropDown()
        {
            _resultView.Result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionDropdown(0);
            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetOrgGroupDropDown()
        {
            _resultView.Result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupDropDown();
            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }

        //public async Task<JsonResult> OnGetLogActivities(int EmployeeID)
        //{
        //    List<GetEmployeeLogActivityByEmployeeIDOutput> activities =
        //           await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
        //           .GetEmployeeLogActivityByEmployeeID(EmployeeID);

        //    // Get Employee Names by User IDs
        //    List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
        //       await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
        //       .GetSystemUserByIDs(activities.Where(x => x.AssignedUserID != 0).Select(x => x.AssignedUserID)
        //       .Union(activities.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy))
        //       .Distinct().ToList());

        //    // Get OrgGroup descriptions by IDs
        //    List<EMS.Plantilla.Transfer.OrgGroup.Form> orgGroups = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
        //        .GetOrgGroupByIDs(activities.Where(x => x.AssignedOrgGroupID != 0).Select(x => x.AssignedOrgGroupID).Distinct().ToList())).Item1.ToList();

        //    // Set employee names
        //    if (systemUsers.Count > 0)
        //    {
        //        activities = activities
        //                .GroupJoin(systemUsers,
        //                x => new { x.AssignedUserID },
        //                y => new { AssignedUserID = y.ID },
        //                (x, y) => new { activities = x, employees = y })
        //                .SelectMany(x => x.employees.DefaultIfEmpty(),
        //                (x, y) => new { activities = x, employees = y })
        //                .Select(x => new GetEmployeeLogActivityByEmployeeIDOutput
        //                {
        //                    ID = x.activities.activities.ID,
        //                    Title = x.activities.activities.Title,
        //                    Type = x.activities.activities.Type,
        //                    SubType = x.activities.activities.SubType,
        //                    Description = x.activities.activities.Description,
        //                    CurrentStatus = x.activities.activities.CurrentStatus,
        //                    CurrentTimestamp = x.activities.activities.CurrentTimestamp,
        //                    AssignedUser = x.employees == null ? "" : string.Concat(x.employees.LastName,
        //                        string.IsNullOrEmpty(x.employees.FirstName) ? "" : string.Concat(", ", x.employees.FirstName),
        //                        string.IsNullOrEmpty(x.employees.MiddleName) ? "" : string.Concat(" ", x.employees.MiddleName)),
        //                    AssignedUserID = x.activities.activities.AssignedUserID,
        //                    AssignedOrgGroupID = x.activities.activities.AssignedOrgGroupID,
        //                    IsPass = x.activities.activities.IsPass,
        //                    CreatedBy = x.activities.activities.CreatedBy
        //                }).ToList();

        //        activities = activities
        //                .GroupJoin(systemUsers,
        //                x => new { x.CreatedBy },
        //                y => new { CreatedBy = y.ID },
        //                (x, y) => new { activities = x, employees = y })
        //                .SelectMany(x => x.employees.DefaultIfEmpty(),
        //                (x, y) => new { activities = x, employees = y })
        //                .Select(x => new GetEmployeeLogActivityByEmployeeIDOutput
        //                {
        //                    ID = x.activities.activities.ID,
        //                    Title = x.activities.activities.Title,
        //                    Type = x.activities.activities.Type,
        //                    SubType = x.activities.activities.SubType,
        //                    Description = x.activities.activities.Description,
        //                    CurrentStatus = x.activities.activities.CurrentStatus,
        //                    CurrentTimestamp = x.activities.activities.CurrentTimestamp,
        //                    AssignedUser = x.activities.activities.AssignedUser,
        //                    AssignedUserID = x.activities.activities.AssignedUserID,
        //                    AssignedOrgGroupID = x.activities.activities.AssignedOrgGroupID,
        //                    IsPass = x.activities.activities.IsPass,
        //                    AssignedBy = x.employees == null ? "" : string.Concat(x.employees.LastName,
        //                        string.IsNullOrEmpty(x.employees.FirstName) ? "" : string.Concat(", ", x.employees.FirstName),
        //                        string.IsNullOrEmpty(x.employees.MiddleName) ? "" : string.Concat(" ", x.employees.MiddleName)),
        //                }).ToList();
        //    }

        //    // Set OrgGroup descriptions
        //    if (orgGroups.Count > 0)
        //    {
        //        activities = activities
        //                .GroupJoin(orgGroups,
        //                x => new { x.AssignedOrgGroupID },
        //                y => new { AssignedOrgGroupID = y.ID },
        //                (x, y) => new { activities = x, orgGroup = y })
        //                .SelectMany(x => x.orgGroup.DefaultIfEmpty(),
        //                (x, y) => new { activities = x, orgGroup = y })
        //                .Select(x => new GetEmployeeLogActivityByEmployeeIDOutput
        //                {
        //                    ID = x.activities.activities.ID,
        //                    Title = x.activities.activities.Title,
        //                    Type = x.activities.activities.Type,
        //                    SubType = x.activities.activities.SubType,
        //                    Description = x.activities.activities.Description,
        //                    CurrentStatus = x.activities.activities.CurrentStatus,
        //                    CurrentTimestamp = x.activities.activities.CurrentTimestamp,
        //                    AssignedUser = x.activities.activities.AssignedUser,
        //                    AssignedBy = x.activities.activities.AssignedBy,
        //                    AssignedOrgGroup = x.orgGroup == null ? "" : string.Concat(x.orgGroup.Code, " - ", x.orgGroup.Description),
        //                    IsPass = x.activities.activities.IsPass
        //                }).ToList();
        //    }

        //    var jsonData = new
        //    {
        //        total = activities.Count,
        //        pageNumber = 1,
        //        sort = "CurrentTimestamp",
        //        records = activities.Count,
        //        rows = activities
        //    };
        //    return new JsonResult(jsonData);
        //}

        public async Task<IActionResult> OnGetLogActivities([FromQuery] GetChecklistListInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetEmployeeLogActivityList").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "EmployeeID=", param.EmployeeID, "&",
                  "TypeDelimited=", param.TypeDelimited, "&",
                  "SubTypeDelimited=", param.SubTypeDelimited, "&",
                  "Title=", param.Title, "&",
                  "Description=", param.Description, "&",
                  "CurrentStatusDelimited=", param.CurrentStatusDelimited, "&",
                  "AssignedByDelimited=", param.AssignedByDelimited, "&",
                  "CurrentTimestampFrom=", param.CurrentTimestampFrom, "&",
                  "CurrentTimestampTo=", param.CurrentTimestampTo, "&",
                  "DueDateFrom=", param.DueDateFrom, "&",
                  "DueDateTo=", param.DueDateTo, "&",
                  "AssignedToDelimited=", param.AssignedToDelimited, "&",
                  "Remarks=", param.Remarks);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetChecklistListOutput>(), URL);

            // Get Employee Names by User IDs
            List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
               await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
               .GetSystemUserByIDs(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy)
               .Distinct().ToList());

            // Get Employee Names by Assigned User ID
            List<EMS.Security.Transfer.SystemUser.Form> assignedUsers =
               await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
               .GetSystemUserByIDs(Result.Where(x => x.AssignedUserID != 0).Select(x => x.AssignedUserID)
               .Distinct().ToList());

            if (systemUsers.Count > 0)
            {

                Result = Result
                       .GroupJoin(systemUsers,
                       x => new { x.CreatedBy },
                       y => new { CreatedBy = y.ID },
                       (x, y) => new { activities = x, employees = y })
                       .SelectMany(x => x.employees.DefaultIfEmpty(),
                       (x, y) => new { activities = x, employees = y })
                       .GroupJoin(assignedUsers,
                       x => new { x.activities.activities.AssignedUserID },
                       y => new { AssignedUserID = y.ID },
                       (x, y) => new { activities = x, employees = y })
                       .SelectMany(x => x.employees.DefaultIfEmpty(),
                       (x, y) => new { activities = x, employees = y })
                       .Select(x => new GetChecklistListOutput
                       {

                           ID = x.activities.activities.activities.activities.ID,
                           Title = x.activities.activities.activities.activities.Title,
                           Type = x.activities.activities.activities.activities.Type,
                           SubType = x.activities.activities.activities.activities.SubType,
                           Description = x.activities.activities.activities.activities.Description,
                           CurrentStatus = x.activities.activities.activities.activities.CurrentStatus,
                           CurrentTimestamp = x.activities.activities.activities.activities.CurrentTimestamp,
                           AssignedBy = x.activities.activities.employees == null ? "" : string.Concat(x.activities.activities.employees.LastName,
                               string.IsNullOrEmpty(x.activities.activities.employees.FirstName) ? "" : string.Concat(", ", x.activities.activities.employees.FirstName),
                               string.IsNullOrEmpty(x.activities.activities.employees.MiddleName) ? "" : string.Concat(" ", x.activities.activities.employees.MiddleName)),
                           NoOfPages = x.activities.activities.activities.activities.NoOfPages,
                           Row = x.activities.activities.activities.activities.Row,
                           TotalNoOfRecord = x.activities.activities.activities.activities.TotalNoOfRecord,
                           IsPass = x.activities.activities.activities.activities.IsPass,
                           CreatedBy = x.activities.activities.activities.activities.CreatedBy,
                           EmployeeID = x.activities.activities.activities.activities.EmployeeID,
                           Remarks = x.activities.activities.activities.activities.Remarks,
                           AssignedTo = x.employees == null ? "" : string.Concat(x.employees.LastName,
                               string.IsNullOrEmpty(x.employees.FirstName) ? "" : string.Concat(", ", x.employees.FirstName),
                               string.IsNullOrEmpty(x.employees.MiddleName) ? "" : string.Concat(" ", x.employees.MiddleName)),
                           IsAssignment = x.activities.activities.activities.activities.IsAssignment,
                           DueDate = x.activities.activities.activities.activities.DueDate
                       }).ToList();
            }

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

        public async Task<JsonResult> OnGetLogActivityStatusHistory(int ID)
        {
            List<GetEmployeeLogActivityStatusHistoryOutput> statusHistory =
                   await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                   .GetEmployeeLogActivityStatusHistory(ID);

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
                        .Select(x => new GetEmployeeLogActivityStatusHistoryOutput
                        {
                            Status = x.activities.activities.Status,
                            Timestamp = x.activities.activities.Timestamp,
                            Remarks = x.activities.activities.Remarks,
                            User = x.employees == null ? "" : string.Concat((x.employees.LastName ?? "").Trim(),
                                string.IsNullOrEmpty((x.employees.FirstName ?? "").Trim()) ? "" : string.Concat(", ", x.employees.FirstName),
                                string.IsNullOrEmpty((x.employees.MiddleName ?? "").Trim()) ? "" : string.Concat(" ", x.employees.MiddleName)),
                            IsPass = x.activities.activities.IsPass

                        }).ToList();
            }

            var jsonData = new
            {
                rows = statusHistory
            };
            return new JsonResult(jsonData);
        }

        public async Task<JsonResult> OnPostAddPreloadedActivities(int EmployeeID, int LogActivityPreloadedID)
        {
            var URL = string.Concat(_workflowBaseURL,
                 _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("AddEmployeePreLoadedActivities").Value, "?",
                 "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(new AddEmployeePreLoadedActivitiesInput
            {
                EmployeeID = EmployeeID,
                LogActivityPreloadedIDs = new List<int> { LogActivityPreloadedID }
            }, URL);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetLogActivityPreloadedDropDown()
        {
            _resultView.Result = await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                .GetLogActivityPreloadedDropdown();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetLogActivityByPreloadedID(int LogActivityPreloadedID)
        {
            List<GetLogActivityByPreloadedIDOutput> activities =
                   await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                   .GetLogActivityByPreloadedID(LogActivityPreloadedID);

            var jsonData = new
            {
                total = activities.Count,
                pageNumber = 1,
                sort = "Title",
                records = activities.Count,
                rows = activities
            };
            return new JsonResult(jsonData);
        }

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
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

        public async Task<JsonResult> OnGetAccountabilityDropDown()
        {
            _resultView.Result = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetAccountabilityDropdown();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetAccountability(int EmployeeID)
        {
            var StatusColor = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(4)).WorkflowStepList;

            List<GetEmployeeAccountabilityByEmployeeIDOutput> result =
                   await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                   .GetEmployeeAccountabilityByEmployeeID(EmployeeID);
            /*
                        // Get OrgGroup description by OrgGroup IDs
                        List<EMS.Plantilla.Transfer.OrgGroup.Form> orgGroup =
                            (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                            .GetOrgGroupByIDs(result.Where(x => x.OrgGroupID != 0).Select(x => x.OrgGroupID)
                             .Distinct().ToList())).Item1;

                        // Get Employee Names by User IDs
                        List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
                           await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                           .GetSystemUserByIDs(result.Where(x => x.ModifiedBy != 0).Select(x => x.ModifiedBy).Distinct().ToList());

                        if (orgGroup.Count > 0)
                        {

                            result = result
                                   .GroupJoin(orgGroup,
                                   x => new { x.OrgGroupID },
                                   y => new { OrgGroupID = y.ID },
                                   (x, y) => new { activities = x, employees = y })
                                   .SelectMany(x => x.employees.DefaultIfEmpty(),
                                   (x, y) => new { activities = x, orgGroup = y })
                                   .Select(x => new GetEmployeeAccountabilityByEmployeeIDOutput
                                   {

                                       ID = x.activities.activities.ID,
                                       Title = x.activities.activities.Title,
                                       Type = x.activities.activities.Type,
                                       Description = x.activities.activities.Description,
                                       OrgGroupDescription = x.orgGroup == null ? "" :
                                        string.Concat(x.orgGroup.Code, " - ", x.orgGroup.Description),
                                       OrgGroupID = x.activities.activities.OrgGroupID,
                                       CreatedBy = x.activities.activities.CreatedBy,
                                       CurrentStatus = x.activities.activities.CurrentStatus,
                                       CurrentStatusCode = x.activities.activities.CurrentStatusCode,
                                       CurrentTimestamp = x.activities.activities.CurrentTimestamp,
                                       ModifiedBy = x.activities.activities.ModifiedBy,
                                       Remarks = x.activities.activities.Remarks
                                   }).ToList();
                        }

                        // Set employee names
                        if (systemUsers.Count > 0)
                        {
                            result = result
                                    .GroupJoin(systemUsers,
                                    x => new { x.ModifiedBy },
                                    y => new { ModifiedBy = y.ID },
                                    (x, y) => new { activities = x, employees = y })
                                    .SelectMany(x => x.employees.DefaultIfEmpty(),
                                    (x, y) => new { activities = x, employees = y })
                                    .Select(x => new GetEmployeeAccountabilityByEmployeeIDOutput
                                    {
                                        ID = x.activities.activities.ID,
                                        Title = x.activities.activities.Title,
                                        Type = x.activities.activities.Type,
                                        Description = x.activities.activities.Description,
                                        OrgGroupDescription = x.activities.activities.OrgGroupDescription,
                                        OrgGroupID = x.activities.activities.OrgGroupID,
                                        CreatedBy = x.activities.activities.CreatedBy,
                                        CurrentStatus = x.activities.activities.CurrentStatus,
                                        CurrentStatusCode = x.activities.activities.CurrentStatusCode,
                                        CurrentTimestamp = x.activities.activities.CurrentTimestamp,
                                        ModifiedBy = x.activities.activities.ModifiedBy,
                                        Remarks = x.activities.activities.Remarks,
                                        ModifiedName = x.employees == null ? "" : string.Concat(x.employees.LastName,
                                            string.IsNullOrEmpty(x.employees.FirstName) ? "" : string.Concat(", ", x.employees.FirstName),
                                            string.IsNullOrEmpty(x.employees.MiddleName) ? "" : string.Concat(" ", x.employees.MiddleName))
                                    }).ToList();

                        }*/


            var clearingOrg = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupFormatByID(result.Select(x => x.OrgGroupID).Distinct().ToList())).Item1;

            var statsUpdatedBy = await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(result.Where(x => x.StatusUpdatedBy != 0).Select(x => x.StatusUpdatedBy).Distinct().ToList());

            result = (from left in result
                      join right in StatusColor on left.Status equals right.StepCode
                      join rightOrg in clearingOrg on left.OrgGroupID equals rightOrg.ID
                      join statUpdatedBy in statsUpdatedBy on left.StatusUpdatedBy equals statUpdatedBy.ID
                      select new GetEmployeeAccountabilityByEmployeeIDOutput()
                      {
                          ID = left.ID,
                          Status = left.Status,
                          StatusDescription = right.StepDescription,
                          StatusColor = right.StatusColor,
                          StatusUpdatedByName = string.Concat("(", statUpdatedBy.Username, ") ", statUpdatedBy.LastName, ", ", statUpdatedBy.FirstName, " ", ((statUpdatedBy.MiddleName == null || statUpdatedBy.MiddleName == "") ? "" : statUpdatedBy.MiddleName.Substring(0, 1))),
                          StatusUpdatedByNameNoCode = string.Concat(statUpdatedBy.LastName, ", ", statUpdatedBy.FirstName, " ", ((statUpdatedBy.MiddleName == null || statUpdatedBy.MiddleName == "") ? "" : statUpdatedBy.MiddleName.Substring(0, 1))),
                          StatusRemarks = left.StatusRemarks,
                          Title = left.Title,
                          Type = left.Type,
                          Description = left.Description,
                          StatusUpdatedDate = left.StatusUpdatedDate,
                          OrgGroupID = left.OrgGroupID,
                          OrgGroupDescription = rightOrg.Result,
                          EmployeeID = left.EmployeeID,
                          LastComment = left.LastComment,
                          LastCommentDate = left.LastCommentDate
                      }).ToList();

            /*var URL = string.Concat(_plantillaBaseURL,
              _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("List").Value, "?",
              "userid=", _globalCurrentUser.UserID, "&",
              "EmployeeID=", EmployeeID, "&",
              "IsShowActiveOnly=false&",
              "IsExport=true");
            var (ResultMovement, IsSuccessMovement, ErrorMessageMovement) = await SharedUtilities.GetFromAPI(new List<EMS.Plantilla.Transfer.EmployeeMovement.GetListOutput>(), URL);
            
            var Resignation = ResultMovement.OrderByDescending(y => y.CreatedDate).Where(x => x.EmployeeField.Equals("EMPLOYMENT STATUS") && x.To.Equals("RESIGNED")).FirstOrDefault();*/

            var employeeLastEmploymentStatus = (await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeLastEmploymentStatus(new List<int> { EmployeeID })).Item1;

            var finalStatus = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeAccountabilityStatusPercentage(new GetEmployeeAccountabilityStatusPercentageInput() { EmployeeIDs = (new List<int> { EmployeeID }) })).Item1;

            var jsonData = new
            {
                total = result.Count,
                pageNumber = 1,
                sort = "CurrentTimestamp",
                records = result.Count,
                rows = result.OrderBy(x => x.Title).ToList(),
                //Supervisor = "ako", // immediate supervisor am or reporting position
                Resignation = employeeLastEmploymentStatus,
                FinalStatus = finalStatus
            };
            return new JsonResult(jsonData);
        }

        public async Task<JsonResult> OnGetOnboardingWorkflowTransaction(int WorkflowID, int EmployeeID)
        {
            AddWorkflowTransaction result = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetTransactionByRecordID(new GetTransactionByRecordIDInput { WorkflowCode = "ONBOARDING", WorkflowID = WorkflowID, RecordID = EmployeeID });

            var jsonData = new
            {
                total = result.History.Count,
                pageNumber = 1,
                sort = "Timestamp",
                records = result.History.Count,
                rows = result.History
            };
            return new JsonResult(jsonData);
        }

        public async Task<JsonResult> OnGetAccountabilityDetailsByID(int AccountabilityID)
        {
            List<GetDetailsByIDOutput> activities =
                   await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                   .GetAccountabilityDetailsByID(AccountabilityID);

            // Get OrgGroup description by OrgGroup IDs
            List<EMS.Plantilla.Transfer.OrgGroup.Form> orgGroup =
                (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupByIDs(activities.Where(x => x.OrgGroupID != 0).Select(x => x.OrgGroupID)
                 .Distinct().ToList())).Item1;

            if (orgGroup.Count > 0)
            {

                activities = activities
                       .GroupJoin(orgGroup,
                       x => new { x.OrgGroupID },
                       y => new { OrgGroupID = y.ID },
                       (x, y) => new { activities = x, employees = y })
                       .SelectMany(x => x.employees.DefaultIfEmpty(),
                       (x, y) => new { activities = x, orgGroup = y })
                       .Select(x => new GetDetailsByIDOutput
                       {

                           Title = x.activities.activities.Title,
                           Type = x.activities.activities.Type,
                           Description = x.activities.activities.Description,
                           OrgGroupDescription = x.orgGroup == null ? "" :
                            string.Concat(x.orgGroup.Code, " - ", x.orgGroup.Description),
                           OrgGroupID = x.activities.activities.OrgGroupID
                       }).ToList();
            }

            var jsonData = new
            {
                total = activities.Count,
                pageNumber = 1,
                sort = "Title",
                records = activities.Count,
                rows = activities
            };
            return new JsonResult(jsonData);
        }

        public async Task<JsonResult> OnPostAddPreLoadedAccountability(int EmployeeID, int AccountabilityPreloadedID, int OrgGroupID, int PositionID)
        {
            var ReportingPosition = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupPositionByID(OrgGroupID))
                .Where(y => y.PositionID.Equals(PositionID)).Select(x => x.ReportingPositionID).FirstOrDefault();

            var URL = string.Concat(_workflowBaseURL,
                 _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("AddEmployeePreLoadedAccountability").Value, "?",
                 "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(new AddEmployeePreLoadedAccountabilityInput
            {
                EmployeeID = EmployeeID,
                AccountabilityPreloadedIDs = new List<int> { AccountabilityPreloadedID },
                PositionID = ReportingPosition
            }, URL);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

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

        public async Task<JsonResult> OnGetProvinceDropDownByRegion(string Code)
        {
            var res = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env).GetProvinceDropdownByRegion(Code);

            _resultView.Result =
                res.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Text
                    }).ToList();

            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetCityMunicipalityDropDownByProvince(string Code)
        {
            var res = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env).GetCityMunicipalityDropdownByProvince(Code);

            _resultView.Result =
                res.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Text
                    }).ToList();

            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetBarangayDropDownByCityMunicipality(string Code)
        {
            var res = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env).GetBarangayDropdownByCityMunicipality(Code);

            _resultView.Result =
                res.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Text
                    }).ToList();

            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetRelationshipDropDown()
        {
            var res = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.FAMILY_RELATIONSHIP.ToString());

            _resultView.Result =
                res.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Description
                    }).ToList();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetFamilyByEmployeeID(int EmployeeID)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetFamilyByEmployeeID(EmployeeID);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetEducationByEmployeeID(int EmployeeID)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEducationByEmployeeID(EmployeeID);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetWorkingHistoryByEmployeeID(int EmployeeID)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetWorkingHistoryByEmployeeID(EmployeeID);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetEmploymentStatusByEmployeeID(int EmployeeID)
        {
            //var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetStatusHistoryByEmployeeID(EmployeeID);
            //_resultView.IsSuccess = true;
            //_resultView.Result = result;
            //return new JsonResult(_resultView);

            List<GetEmploymentStatusOutput> statusHistory =
                   await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                   .GetEmploymentStatusByEmployeeID(EmployeeID);

            var jsonData = new
            {
                rows = statusHistory
            };
            return new JsonResult(jsonData);
        }


        public async Task<JsonResult> OnGetWorkflowStepDropdown(string WorkflowCode)
        {
            _resultView.Result = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetWorkflowStepDropDown(WorkflowCode);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        private async Task<(List<EMS.Plantilla.Transfer.Employee.GetListOutput>, bool, string)> GetDataList([FromQuery] EMS.Plantilla.Transfer.Employee.GetListInput param, bool IsExport)
        {
            /*var Org = (string.IsNullOrEmpty(param.OrgGroupDelimited) ? string.Concat(string.Join(",", _globalCurrentUser.OrgGroupID),
                (_globalCurrentUser.OrgGroupDescendants == null ? "" : "," + string.Join(",", _globalCurrentUser.OrgGroupDescendants)),
                (_globalCurrentUser.OrgGroupRovingDescendants == null ? "" : "," + string.Join(",", _globalCurrentUser.OrgGroupRovingDescendants)),
                (_globalCurrentUser.Roving == null ? "" : "," + string.Join(",", _globalCurrentUser.Roving.Select(x => x.OrgGroupID).ToList())))
                 : param.OrgGroupDelimited);*/


            var Org = "";
            if (_systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/VIEWALL")).Count() > 0)
            {
                Org = param.OrgGroupDelimited;
            }
            else
                Org = (string.IsNullOrEmpty(param.OrgGroupDelimited) ? string.Concat(string.Join(",", _globalCurrentUser.OrgGroupID),
                (_globalCurrentUser.OrgGroupDescendants == null ? "" : "," + string.Join(",", _globalCurrentUser.OrgGroupDescendants)),
                (_globalCurrentUser.OrgGroupRovingDescendants == null ? "" : "," + string.Join(",", _globalCurrentUser.OrgGroupRovingDescendants)),
                (_globalCurrentUser.Roving == null ? "" : "," + string.Join(",", _globalCurrentUser.Roving.Select(x => x.OrgGroupID).ToList())))
                    : param.OrgGroupDelimited);

            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ID=", param.ID, "&",
                  "Code=", param.Code, "&",
                  "Name=", param.Name, "&",
                  "OrgGroupDelimited=", Org, "&",
                  "PositionDelimited=", param.PositionDelimited, "&",
                  "EmploymentStatusDelimited=", param.EmploymentStatusDelimited, "&",
                  "CurrentStepDelimited=", param.CurrentStepDelimited, "&",
                  "DateScheduledFrom=", param.DateScheduledFrom, "&",
                  "DateScheduledTo=", param.DateScheduledTo, "&",
                  "DateCompletedFrom=", param.DateCompletedFrom, "&",
                  "DateCompletedTo=", param.DateCompletedTo, "&",
                  "Remarks=", param.Remarks, "&",
                  "DateHiredFrom=", param.DateHiredFrom, "&",
                  "DateHiredTo=", param.DateHiredTo, "&",
                  "DateStatusFrom=", param.DateStatusFrom, "&",
                  "DateStatusTo=", param.DateStatusTo, "&",
                  "BirthDateFrom=", param.BirthDateFrom, "&",
                  "BirthDateTo=", param.BirthDateTo, "&",
                  "OldEmployeeID=", param.OldEmployeeID, "&",
                  "OrgGroupDelimitedClus=", param.OrgGroupDelimitedClus, "&",
                  "OrgGroupDelimitedArea=", param.OrgGroupDelimitedArea, "&",
                  "OrgGroupDelimitedReg=", param.OrgGroupDelimitedReg, "&",
                  "OrgGroupDelimitedZone=", param.OrgGroupDelimitedZone, "&",
                  "ShowActiveEmployee=", param.ShowActiveEmployee, "&",
                  "IsExport=", IsExport);

            return await SharedUtilities.GetFromAPI(new List<EMS.Plantilla.Transfer.Employee.GetListOutput>(), URL);
        }

        public async Task<IActionResult> OnGetCheckEmployeeExportListAsync([FromQuery] EMS.Plantilla.Transfer.Employee.GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataList(param, true);

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

        public async Task<IActionResult> OnGetDownloadEmployeeExportListAsync([FromQuery] EMS.Plantilla.Transfer.Employee.GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataList(param, true);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "EmployeeList.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Employee List");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("ID");
                    row.CreateCell(1).SetCellValue("New Employee ID");
                    row.CreateCell(2).SetCellValue("Name");
                    row.CreateCell(3).SetCellValue("Organizational Group");
                    row.CreateCell(4).SetCellValue("Position");
                    row.CreateCell(5).SetCellValue("Employment Status");
                    //row.CreateCell(6).SetCellValue("Onboarding Status");
                    //row.CreateCell(7).SetCellValue("Date Scheduled");
                    //row.CreateCell(8).SetCellValue("Date Completed");
                    //row.CreateCell(9).SetCellValue("Onboarding Remarks");
                    row.CreateCell(6).SetCellValue("Status Update Date");
                    row.CreateCell(7).SetCellValue("Date Hired");
                    row.CreateCell(8).SetCellValue("Old Employee ID");
                    row.CreateCell(9).SetCellValue("Company");
                    row.CreateCell(10).SetCellValue("Cluster");
                    row.CreateCell(11).SetCellValue("Area");
                    row.CreateCell(12).SetCellValue("Region");
                    row.CreateCell(13).SetCellValue("Zone");

                    excelSheet.SetColumnWidth(0, 3500);
                    excelSheet.SetColumnWidth(1, 5500);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 10000);
                    excelSheet.SetColumnWidth(4, 10000);
                    excelSheet.SetColumnWidth(5, 6500);
                    //excelSheet.SetColumnWidth(6, 5500);
                    //excelSheet.SetColumnWidth(7, 4500);
                    //excelSheet.SetColumnWidth(8, 4500);
                    //excelSheet.SetColumnWidth(9, 8000);
                    excelSheet.SetColumnWidth(6, 4500);
                    excelSheet.SetColumnWidth(7, 4500);
                    excelSheet.SetColumnWidth(8, 5500);
                    excelSheet.SetColumnWidth(9, 5500);
                    excelSheet.SetColumnWidth(10, 5500);
                    excelSheet.SetColumnWidth(11, 5500);
                    excelSheet.SetColumnWidth(12, 5500);
                    excelSheet.SetColumnWidth(13, 5500);

                    XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();

                    var colHeaderFont = workbook.CreateFont();
                    colHeaderFont.IsBold = true;
                    colHeaderStyle.SetFont(colHeaderFont);
                    colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                    colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                    colHeaderStyle.Alignment = HorizontalAlignment.Center;
                    alignCenter.Alignment = HorizontalAlignment.Center;

                    row.Cells[0].CellStyle = colHeaderStyle;
                    row.Cells[1].CellStyle = colHeaderStyle;
                    row.Cells[2].CellStyle = colHeaderStyle;
                    row.Cells[3].CellStyle = colHeaderStyle;
                    row.Cells[4].CellStyle = colHeaderStyle;
                    row.Cells[5].CellStyle = colHeaderStyle;
                    //row.Cells[6].CellStyle = colHeaderStyle;
                    //row.Cells[7].CellStyle = colHeaderStyle;
                    //row.Cells[8].CellStyle = colHeaderStyle;
                    //row.Cells[9].CellStyle = colHeaderStyle;
                    row.Cells[6].CellStyle = colHeaderStyle;
                    row.Cells[7].CellStyle = colHeaderStyle;
                    row.Cells[8].CellStyle = colHeaderStyle;
                    row.Cells[9].CellStyle = colHeaderStyle;
                    row.Cells[10].CellStyle = colHeaderStyle;
                    row.Cells[11].CellStyle = colHeaderStyle;
                    row.Cells[12].CellStyle = colHeaderStyle;
                    row.Cells[13].CellStyle = colHeaderStyle;
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(Convert.ToDouble(item.ID));
                        row.CreateCell(1).SetCellValue(item.Code);
                        row.CreateCell(2).SetCellValue(item.Name);
                        row.CreateCell(3).SetCellValue(item.OrgGroup);
                        row.CreateCell(4).SetCellValue(item.Position);
                        row.CreateCell(5).SetCellValue(item.EmploymentStatus);
                        //row.CreateCell(6).SetCellValue(item.CurrentStep);
                        //row.CreateCell(7).SetCellValue(item.DateScheduled);
                        //row.CreateCell(8).SetCellValue(item.DateCompleted);
                        //row.CreateCell(9).SetCellValue(item.Remarks);
                        row.CreateCell(6).SetCellValue(item.DateStatus);
                        row.CreateCell(7).SetCellValue(item.DateHired);
                        row.CreateCell(8).SetCellValue(item.OldEmployeeID);
                        row.CreateCell(9).SetCellValue(item.Company);
                        row.CreateCell(10).SetCellValue(item.Cluster);
                        row.CreateCell(11).SetCellValue(item.Area);
                        row.CreateCell(12).SetCellValue(item.Region);
                        row.CreateCell(13).SetCellValue(item.Zone);

                        row.Cells[0].CellStyle = alignCenter;
                        //row.Cells[5].CellStyle = alignCenter;
                        //row.Cells[6].CellStyle = alignCenter;
                        //row.Cells[7].CellStyle = alignCenter;
                        //row.Cells[8].CellStyle = alignCenter;
                        row.Cells[6].CellStyle = alignCenter;

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
                        TableName = "Employee",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Employee List exported",
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

        public async Task<IActionResult> OnGetDownloadEmployeeTemplate()
        {
            string sWebRootFolder = _env.WebRootPath;
            string sFileName = "Employee.xlsx";
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                
                // Header style
                XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();
                var format = workbook.CreateDataFormat().GetFormat("text");
                var colHeaderFont = workbook.CreateFont();
                colHeaderFont.IsBold = true;
                colHeaderStyle.SetFont(colHeaderFont);
                colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                colHeaderStyle.Alignment = HorizontalAlignment.Center;
                colHeaderStyle.DataFormat = format;
                alignCenter.Alignment = HorizontalAlignment.Center;

                // Dropdown values
                var regionPSGCList = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env).GetAllRegion();
                var provincePSGCList = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env).GetAllProvince();
                var cityMunicipalityPSGCList = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env).GetAllCityMunicipality();
                var barangayPSGCList = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env).GetAllBarangay();

                //var regionPSGCArray = regionPSGCList.OrderBy(y => y.Value).Select(x => x.Value).ToArray();

                var nationalityList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_NATIONALITY.ToString()))
                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();
                var citizenshipList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_CITIZENSHIP.ToString()))
                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();

                var genderList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_GENDER.ToString()))
                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();
                var civilStatusList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_CIVIL_STATUS.ToString()))
                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();
                var religionList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_RELIGION.ToString()))
                    .OrderBy(y => y.Value).ToList();
                var religionArray = religionList.OrderBy(y => y.Value != "ROMCAT" /*Move 'ROMCAT' on top */).Select(x => x.Value).ToArray();

                var sssStatusList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_SSS_STAT.ToString()))
                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();
                var exemptStatusList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_EXEMPT_STAT.ToString()))
                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();
                var familyRelationshipList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.FAMILY_RELATIONSHIP.ToString()))
                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();
                var schoolLevelList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_SCHOOL_LEVEL.ToString()))
                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();
                var edAttDegList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_ED_ATT_DEG.ToString()))
                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();
                var edAttStatList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_ED_ATT_STAT.ToString()))
                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();
                var companyTagList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.COMPANY_TAG.ToString()))
                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();
                var employmentStatusList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMPLOYMENT_STATUS.ToString()))
                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();

                //var orgGroupList = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupCodeDropDown(0))
                //                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();
                //var positionList = (await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionCodeDropdown(0))
                //                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();


                IDataValidation getDropdownValidation(CellRangeAddressList addressList, string[] stringArray, ISheet excelSheet)
                {
                    IDataValidationHelper validationHelper = new XSSFDataValidationHelper((XSSFSheet)excelSheet);
                    IDataValidationConstraint constraint = validationHelper.CreateExplicitListConstraint(stringArray);
                    IDataValidation dataValidation = validationHelper.CreateValidation(constraint, addressList);
                    dataValidation.SuppressDropDownArrow = true;
                    dataValidation.ShowErrorBox = true;
                    dataValidation.CreateErrorBox("", "Please select from Dropdown values.");
                    return dataValidation;
                }


                void AddETFSheet()
                {
                    ISheet excelSheet = workbook.CreateSheet("ETF");
                    int rowCtr = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(rowCtr); rowCtr++;

                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 10, 10), genderList, excelSheet));
                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 13, 13), civilStatusList, excelSheet));
                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 14, 14), religionArray, excelSheet));
                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 15, 15), sssStatusList, excelSheet));
                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 16, 16), exemptStatusList, excelSheet));
                    //excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 20, 20), regionPSGCArray, excelSheet));
                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 28, 28), familyRelationshipList, excelSheet));
                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 41, 41), schoolLevelList, excelSheet));
                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 45, 45), edAttDegList, excelSheet));
                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 46, 46), edAttStatList, excelSheet));
                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 47, 47), companyTagList, excelSheet));
                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 64, 64), employmentStatusList, excelSheet));

                    row.CreateCell(0).SetCellValue("* Old Employee Code");
                    row.CreateCell(1).SetCellValue("* Last Name");
                    row.CreateCell(2).SetCellValue("* First Name");
                    row.CreateCell(3).SetCellValue("* Middle Name");
                    row.CreateCell(4).SetCellValue("Suffix");
                    row.CreateCell(5).SetCellValue("* Nickname");
                    row.CreateCell(6).SetCellValue("* Nationality");
                    row.CreateCell(7).SetCellValue("* Citizenship");
                    row.CreateCell(8).SetCellValue("* BirthDate (MM/DD/YYYY)");
                    row.CreateCell(9).SetCellValue("* BirthPlace");
                    row.CreateCell(10).SetCellValue("* Gender");
                    row.CreateCell(11).SetCellValue("* Height (cm)");
                    row.CreateCell(12).SetCellValue("* Weight (lbs)");
                    row.CreateCell(13).SetCellValue("* Civil Status");
                    row.CreateCell(14).SetCellValue("* Religion");
                    row.CreateCell(15).SetCellValue("* SSS Status");
                    row.CreateCell(16).SetCellValue("* Exemption Status");
                    row.CreateCell(17).SetCellValue("* Home Address");
                    row.CreateCell(18).SetCellValue("* Mobile No (09xxxxxxxxx)");
                    row.CreateCell(19).SetCellValue("* Region (PSGC Code)");
                    row.CreateCell(20).SetCellValue("* Province (PSGC Code)");
                    row.CreateCell(21).SetCellValue("* City / Municipality (PSGC Code)");
                    row.CreateCell(22).SetCellValue("* Barangay (PSGC Code)");
                    row.CreateCell(23).SetCellValue("* HDMF No");
                    row.CreateCell(24).SetCellValue("* SSS No");
                    row.CreateCell(25).SetCellValue("* TIN");
                    row.CreateCell(26).SetCellValue("* PhilHealth No");
                    row.CreateCell(27).SetCellValue("* Contact Person");
                    row.CreateCell(28).SetCellValue("* Contact Person Relationship");
                    row.CreateCell(29).SetCellValue("Spouse");
                    row.CreateCell(30).SetCellValue("Spouse Birthdate (MM/DD/YYYY)");
                    row.CreateCell(31).SetCellValue("Spouse Employer");
                    row.CreateCell(32).SetCellValue("Spouse Occupation");
                    row.CreateCell(33).SetCellValue("* Father");
                    row.CreateCell(34).SetCellValue("* Father Birthdate (MM/DD/YYYY)");
                    row.CreateCell(35).SetCellValue("* Father Occupation");
                    row.CreateCell(36).SetCellValue("* Mother");
                    row.CreateCell(37).SetCellValue("* Mother Birthdate (MM/DD/YYYY)");
                    row.CreateCell(38).SetCellValue("* Mother Occupation");
                    row.CreateCell(39).SetCellValue("* School");
                    row.CreateCell(40).SetCellValue("* School Address");
                    row.CreateCell(41).SetCellValue("* School Level");
                    row.CreateCell(42).SetCellValue("* Course");
                    row.CreateCell(43).SetCellValue("* Year From (YYYY)");
                    row.CreateCell(44).SetCellValue("* Year To (YYYY)");
                    row.CreateCell(45).SetCellValue("* Educational Attainment Degree");
                    row.CreateCell(46).SetCellValue("* Educational Attainment Status");
                    row.CreateCell(47).SetCellValue("* Company Code");
                    row.CreateCell(48).SetCellValue("* Branch Code");
                    row.CreateCell(49).SetCellValue("Branch Name");
                    row.CreateCell(50).SetCellValue("Branch Region");
                    row.CreateCell(51).SetCellValue("* Monthly Salary");
                    row.CreateCell(52).SetCellValue("* Daily Salary");
                    row.CreateCell(53).SetCellValue("* Hourly Salary");
                    row.CreateCell(54).SetCellValue("* Department Code");
                    row.CreateCell(55).SetCellValue("Department");
                    row.CreateCell(56).SetCellValue("* Designation Code");
                    row.CreateCell(57).SetCellValue("Designation");
                    row.CreateCell(58).SetCellValue("Job Class");
                    row.CreateCell(59).SetCellValue("* Date Hired (MM/DD/YYYY)");
                    row.CreateCell(60).SetCellValue("Previous Employer");
                    row.CreateCell(61).SetCellValue("Previous Designation");
                    row.CreateCell(62).SetCellValue("Year Started (YYYY)");
                    row.CreateCell(63).SetCellValue("Year Ended (YYYY)");
                    row.CreateCell(64).SetCellValue("* Employment Status");

                    excelSheet.SetColumnWidth(0, 6500);
                    excelSheet.SetColumnWidth(1, 4500);
                    excelSheet.SetColumnWidth(2, 4500);
                    excelSheet.SetColumnWidth(3, 4500);
                    excelSheet.SetColumnWidth(4, 3500);
                    excelSheet.SetColumnWidth(5, 3500);
                    excelSheet.SetColumnWidth(6, 4500);
                    excelSheet.SetColumnWidth(7, 4500);
                    excelSheet.SetColumnWidth(8, 6500);
                    excelSheet.SetColumnWidth(9, 4500);
                    excelSheet.SetColumnWidth(10, 3500);
                    excelSheet.SetColumnWidth(11, 3500);
                    excelSheet.SetColumnWidth(12, 3500);
                    excelSheet.SetColumnWidth(13, 3500);
                    excelSheet.SetColumnWidth(14, 4500);
                    excelSheet.SetColumnWidth(15, 4500);
                    excelSheet.SetColumnWidth(16, 5500);
                    excelSheet.SetColumnWidth(17, 20000);
                    excelSheet.SetColumnWidth(18, 7500);
                    excelSheet.SetColumnWidth(19, 5500);
                    excelSheet.SetColumnWidth(20, 5500);
                    excelSheet.SetColumnWidth(21, 5500);
                    excelSheet.SetColumnWidth(22, 5500);
                    excelSheet.SetColumnWidth(23, 5500);
                    excelSheet.SetColumnWidth(24, 5500);
                    excelSheet.SetColumnWidth(25, 5500);
                    excelSheet.SetColumnWidth(26, 5500);
                    excelSheet.SetColumnWidth(27, 7000);
                    excelSheet.SetColumnWidth(28, 7000);
                    excelSheet.SetColumnWidth(29, 5500);
                    excelSheet.SetColumnWidth(30, 8500);
                    excelSheet.SetColumnWidth(31, 6500);
                    excelSheet.SetColumnWidth(32, 6500);
                    excelSheet.SetColumnWidth(33, 5500);
                    excelSheet.SetColumnWidth(34, 8500);
                    excelSheet.SetColumnWidth(35, 5500);
                    excelSheet.SetColumnWidth(36, 5500);
                    excelSheet.SetColumnWidth(37, 8500);
                    excelSheet.SetColumnWidth(38, 5500);
                    excelSheet.SetColumnWidth(39, 8500);
                    excelSheet.SetColumnWidth(40, 8500);
                    excelSheet.SetColumnWidth(41, 5500);
                    excelSheet.SetColumnWidth(42, 5500);
                    excelSheet.SetColumnWidth(43, 6500);
                    excelSheet.SetColumnWidth(44, 6500);
                    excelSheet.SetColumnWidth(45, 8500);
                    excelSheet.SetColumnWidth(46, 8500);
                    excelSheet.SetColumnWidth(47, 6000);
                    excelSheet.SetColumnWidth(48, 5500);
                    excelSheet.SetColumnWidth(49, 6500);
                    excelSheet.SetColumnWidth(50, 7000);
                    excelSheet.SetColumnWidth(51, 4500);
                    excelSheet.SetColumnWidth(52, 4500);
                    excelSheet.SetColumnWidth(53, 4500);
                    excelSheet.SetColumnWidth(54, 6500);
                    excelSheet.SetColumnWidth(55, 7000);
                    excelSheet.SetColumnWidth(56, 5500);
                    excelSheet.SetColumnWidth(57, 8500);
                    excelSheet.SetColumnWidth(58, 3500);
                    excelSheet.SetColumnWidth(59, 7500);
                    excelSheet.SetColumnWidth(60, 8500);
                    excelSheet.SetColumnWidth(61, 5500);
                    excelSheet.SetColumnWidth(62, 6500);
                    excelSheet.SetColumnWidth(63, 6500);
                    excelSheet.SetColumnWidth(64, 6500);

                    for (int i = 0; i <= 64; i++)
                        row.Cells[i].CellStyle = colHeaderStyle; 
                    
                    #endregion

                    for (int x = rowCtr; x <= 1000; x++)
                    {
                        IRow rowDate = excelSheet.CreateRow(x);
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;

                        for (int i = 0; i <= 64; i++)
                            rowDate.CreateCell(i, CellType.String).SetCellValue("");

                        for (int i = 0; i <= 64; i++)
                            rowDate.Cells[i].CellStyle = textCS;
                    }

                }

                void AddReferenceLookupSheet()
                { 
                    ISheet ReferenceLookupSheet = workbook.CreateSheet("Reference Lookup");
                    int rowCtr = 0;
                    IRow pRow = ReferenceLookupSheet.CreateRow(rowCtr); rowCtr++;
                    pRow.CreateCell(0).SetCellValue("Nationality");
                    pRow.CreateCell(1).SetCellValue("Citizenship");
                    pRow.CreateCell(2).SetCellValue("Region");
                    ReferenceLookupSheet.SetColumnWidth(0, 10000);
                    ReferenceLookupSheet.SetColumnWidth(1, 10000);
                    ReferenceLookupSheet.SetColumnWidth(2, 12000);
                    pRow.Cells[0].CellStyle = colHeaderStyle;
                    pRow.Cells[1].CellStyle = colHeaderStyle;
                    pRow.Cells[2].CellStyle = colHeaderStyle;

                    foreach (var item in nationalityList.OrderBy(x => x != "FILIPINO" /*Move 'FILIPINO' on top */).ToList())
                    {
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;
                        IRow rowLookup = ReferenceLookupSheet.CreateRow(rowCtr);
                        rowLookup.CreateCell(0, CellType.String).SetCellValue(item);
                        rowCtr++;
                    }
                    rowCtr = 1;
                    foreach (var item in citizenshipList.OrderBy(x => x != "FILIPINO" /*Move 'FILIPINO' on top */).ToList())
                    {
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;
                        IRow rowLookup = ReferenceLookupSheet.GetRow(rowCtr);
                        if (rowLookup != null)
                            rowLookup.CreateCell(1, CellType.String).SetCellValue(item);
                        else
                        { 
                            IRow newRow = ReferenceLookupSheet.CreateRow(rowCtr);
                            newRow.CreateCell(1, CellType.String).SetCellValue(item);
                        }

                        rowCtr++;
                    }
                    rowCtr = 1;
                    foreach (var item in religionList.OrderBy(x => x.Value != "ROMCAT" /*Move 'ROMCAT' on top */).ToList())
                    {
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;
                        IRow rowLookup = ReferenceLookupSheet.GetRow(rowCtr);
                        if (rowLookup != null)
                            rowLookup.CreateCell(2, CellType.String).SetCellValue(string.Concat(item.Value, " - ", item.Description));
                        else
                        { 
                            IRow newRow = ReferenceLookupSheet.CreateRow(rowCtr);
                            newRow.CreateCell(2, CellType.String).SetCellValue(string.Concat(item.Value, " - ", item.Description));
                        }

                        rowCtr++;
                    }
                }

                async Task AddPSGCLookupSheet()
                { 
                    ISheet PSGCLookupSheet = workbook.CreateSheet("PSGC Lookup");
                    int rowCtr = 0;
                    IRow pRow = PSGCLookupSheet.CreateRow(rowCtr); rowCtr++;
                    pRow.CreateCell(0).SetCellValue("Region");
                    pRow.CreateCell(1).SetCellValue("Code");
                    pRow.CreateCell(2).SetCellValue("Province");
                    pRow.CreateCell(3).SetCellValue("Code");
                    pRow.CreateCell(4).SetCellValue("City / Municipality");
                    pRow.CreateCell(5).SetCellValue("Code");
                    pRow.CreateCell(6).SetCellValue("Barangay");
                    pRow.CreateCell(7).SetCellValue("Code");
                    PSGCLookupSheet.SetColumnWidth(0, 8000);
                    PSGCLookupSheet.SetColumnWidth(1, 3000);
                    PSGCLookupSheet.SetColumnWidth(2, 6000);
                    PSGCLookupSheet.SetColumnWidth(3, 3000);
                    PSGCLookupSheet.SetColumnWidth(4, 6000);
                    PSGCLookupSheet.SetColumnWidth(5, 3000);
                    PSGCLookupSheet.SetColumnWidth(6, 6000);
                    PSGCLookupSheet.SetColumnWidth(7, 3000);
                    pRow.Cells[0].CellStyle = colHeaderStyle;
                    pRow.Cells[1].CellStyle = colHeaderStyle;
                    pRow.Cells[2].CellStyle = colHeaderStyle;
                    pRow.Cells[3].CellStyle = colHeaderStyle;
                    pRow.Cells[4].CellStyle = colHeaderStyle;
                    pRow.Cells[5].CellStyle = colHeaderStyle;
                    pRow.Cells[6].CellStyle = colHeaderStyle;
                    pRow.Cells[7].CellStyle = colHeaderStyle;

                    foreach (var region in regionPSGCList.OrderBy(x => x.Code).ToList())
                    {
                        XSSFCellStyle dateCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var dateFormat = workbook.CreateDataFormat().GetFormat("text");
                        dateCS.DataFormat = dateFormat;

                        int regionCtr = 0;
                        foreach (var province in provincePSGCList.Where(x => x.ParentPrefix == region.Prefix))
                        {
                            int provinceCtr = 0;
                            foreach (var cityMunicipality in cityMunicipalityPSGCList.Where(x => x.ParentPrefix == province.Prefix))
                            {
                                int cityMunicipalityCtr = 0;
                                foreach (var barangay in barangayPSGCList.Where(x => x.ParentPrefix == cityMunicipality.Prefix))
                                {

                                    IRow rowLookup = PSGCLookupSheet.CreateRow(rowCtr);
                                    rowLookup.CreateCell(0, CellType.String).SetCellValue(regionCtr == 0 ? region.Description : "");
                                    rowLookup.CreateCell(1, CellType.String).SetCellValue(regionCtr == 0 ? region.Code : "");
                                    rowLookup.CreateCell(2, CellType.String).SetCellValue(provinceCtr == 0 ? province.Description : "");
                                    rowLookup.CreateCell(3, CellType.String).SetCellValue(provinceCtr == 0 ? province.Code : "");
                                    rowLookup.CreateCell(4, CellType.String).SetCellValue(cityMunicipalityCtr == 0 ? cityMunicipality.Description : "");
                                    rowLookup.CreateCell(5, CellType.String).SetCellValue(cityMunicipalityCtr == 0 ? cityMunicipality.Code : "");
                                    rowLookup.CreateCell(6, CellType.String).SetCellValue(barangay.Description);
                                    rowLookup.CreateCell(7, CellType.String).SetCellValue(barangay.Code);
                                    cityMunicipalityCtr++;
                                    provinceCtr++;
                                    regionCtr++;
                                    rowCtr++;
                                }
                            }
                        }
                        rowCtr++;
                    }
                }

                AddETFSheet();
                AddReferenceLookupSheet();
                await AddPSGCLookupSheet();

                workbook.Write(fs);
            }

            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        public async Task<IActionResult> OnPostUploadInsert()
        {
            IFormFile file = Request.Form.Files[0];
            string filePath = Path.Combine(_env.WebRootPath, "\\Employee\\Uploads");
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(filePath, file.FileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;

                    if (sFileExtension == ".xls")//This will read the Excel 97-2000 formats    
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                    else //This will read 2007 Excel format    
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }

                    IRow headerRow = sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;

                    List<UploadFile> uploadList = new List<UploadFile>();
                    int blankRows = 0;

                    var OnboardingWorkflow = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                        .GetLastStepByWorkflowCode("ONBOARDING");

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        if (row != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()))
                            {
                                UploadFile obj = new UploadFile
                                {
                                    RowNum = (i + 1).ToString(),
                                    // Assign Onboarding Workflow ID from Workflow API
                                    OnboardingWorkflowID = OnboardingWorkflow.WorkflowID,
                                    OldEmployeeCode = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    LastName = row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    FirstName = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    MiddleName = row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Suffix = row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Nickname = row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Nationality = row.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Citizenship = row.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    BirthDate = row.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    BirthPlace = row.GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Gender = row.GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    HeightCM = row.GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    WeightLBS = row.GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    CivilStatus = row.GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Religion = row.GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    SSSStatus = row.GetCell(15, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    ExemptionStatus = row.GetCell(16, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    HomeAddress = row.GetCell(17, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    MobileNo = row.GetCell(18, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    PSGCRegionCode = row.GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    PSGCProvinceCode = row.GetCell(20, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    PSGCCityMunicipalityCode = row.GetCell(21, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    PSGCBarangayCode = row.GetCell(22, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    HDMFNo = row.GetCell(23, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    SSSNo = row.GetCell(24, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    TIN = row.GetCell(25, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    PhilHealthNo = row.GetCell(26, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    ContactPerson = row.GetCell(27, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    ContactPersonRelationship = row.GetCell(28, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Spouse = row.GetCell(29, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    SpouseBirthDate = row.GetCell(30, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    SpouseEmployer = row.GetCell(31, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    SpouseOccupation = row.GetCell(32, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Father = row.GetCell(33, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    FatherBirthDate = row.GetCell(34, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    FatherOccupation = row.GetCell(35, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Mother = row.GetCell(36, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    MotherBirthDate = row.GetCell(37, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    MotherOccupation = row.GetCell(38, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    School = row.GetCell(39, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    SchoolAddress = row.GetCell(40, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    SchoolLevel = row.GetCell(41, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Course = row.GetCell(42, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    SchoolYearFrom = row.GetCell(43, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    SchoolYearTo = row.GetCell(44, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    EducationalAttainmentDegree = row.GetCell(45, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    EducationalAttainmentStatus = row.GetCell(46, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    CompanyCode = row.GetCell(47, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    BranchCode = row.GetCell(48, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    MonthlySalary = row.GetCell(51, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    DailySalary = row.GetCell(52, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    HourlySalary = row.GetCell(53, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    DepartmentCode = row.GetCell(54, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    DesignationCode = row.GetCell(56, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    DateHired = row.GetCell(59, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    PreviousEmployer = row.GetCell(60, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    PreviousDesignation = row.GetCell(61, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    YearStarted = row.GetCell(62, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    YearEnded = row.GetCell(63, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    EmploymentStatus = row.GetCell(64, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                };
                                uploadList.Add(obj);
                            }
                        }
                        else
                        {
                            blankRows++;
                            if (blankRows > 3)
                                break;
                        }

                    }
                 
                    var URL = string.Concat(_plantillaBaseURL,
                               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("UploadInsert").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

                    var (Result, IsSuccess, Message) = await SharedUtilities.PostFromAPI(new UploadInsertOutput(), uploadList, URL);

                    if (IsSuccess)
                    {

                        /*Add AuditLog*/
                        await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                            .AddAuditLog(new Security.Transfer.AuditLog.Form
                            {
                                EventType = Common_AuditLog.EventType.UPLOAD.ToString(),
                                TableName = "Employee",
                                TableID = 0, // New Record, no ID yet
                                Remarks = "Employee uploaded",
                                IsSuccess = true,
                                CreatedBy = _globalCurrentUser.UserID
                            });

                        #region Create System Users for the newly uploaded employees
                        var securityURL = string.Concat(_securityBaseURL,
                                         _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("EmployeeUploadInsert").Value, "?",
                                         "userid=", _globalCurrentUser.UserID);

                        var (ResultSystemUsers, IsSuccess1, Message1) =
                            await SharedUtilities.PostFromAPI(new List<EMS.Security.Transfer.SystemUser.EmployeeUploadInsertOutput>(),
                            Result.NewSystemUsers.Select(x => new EmployeeUploadInsertInput
                            {
                                NewEmployeeCode = x.Username,
                                FirstName = x.FirstName,
                                MiddleName = x.MiddleName,
                                LastName = x.LastName,
                                CreatedBy = _globalCurrentUser.UserID
                            }).ToList(), securityURL);
                        #endregion


                        #region Update system user ID on Employee
                        if (IsSuccess1)
                        {
                            var UploadInsertUpdateSystemUser = string.Concat(_plantillaBaseURL,
                                             _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("UploadInsertUpdateSystemUser").Value, "?",
                                             "userid=", _globalCurrentUser.UserID);

                            var (IsSuccess2, Message2) = await SharedUtilities.PostFromAPI(
                                ResultSystemUsers.Select(x => new UploadInsertUpdateSystemUserInput
                                {
                                    SystemUserID = x.SystemUserID,
                                    NewEmployeeCode = x.NewEmployeeCode
                                }), UploadInsertUpdateSystemUser);

                            
                        }
                        #endregion





                    }


                    _resultView.IsSuccess = IsSuccess;
                    _resultView.Result = IsSuccess ? Result.Message : Message;
                }
            }

            return new JsonResult(_resultView);
        }


        public async Task<JsonResult> OnGetCurrentStepAutoCompleteAsync(GetWorkflowStepAutoCompleteInput param)
        {
            _resultView.Result = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflowStepAutoComplete(param);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetWorkflowReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
		
        public async Task<JsonResult> OnGetRegionByOrgGroupID(int OrgGroupID)
        {
            _resultView.Result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetRegionByOrgGroupID(OrgGroupID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetJobClassByPositionID(int PositionID)
        {
            _resultView.Result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env)
                .GetPosition(PositionID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        private async Task<(List<EMS.Plantilla.Transfer.Employee.GetETFListOutput>, bool, string)> GetETFDataList([FromQuery] EMS.Plantilla.Transfer.Employee.GetListInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetETFList").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ID=", param.ID, "&",
                  "Code=", param.Code, "&",
                  "Name=", param.Name, "&",
                  "OrgGroupDelimited=", param.OrgGroupDelimited, "&",
                  "PositionDelimited=", param.PositionDelimited, "&",
                  "EmploymentStatusDelimited=", param.EmploymentStatusDelimited, "&",
                  "CurrentStepDelimited=", param.CurrentStepDelimited, "&",
                  "DateScheduledFrom=", param.DateScheduledFrom, "&",
                  "DateScheduledTo=", param.DateScheduledTo, "&",
                  "DateCompletedFrom=", param.DateCompletedFrom, "&",
                  "DateCompletedTo=", param.DateCompletedTo, "&",
                  "Remarks=", param.Remarks, "&",
                  "DateHiredFrom=", param.DateHiredFrom, "&",
                  "DateHiredTo=", param.DateHiredTo, "&",
                  "OldEmployeeID=", param.OldEmployeeID, "&",
                  "OrgGroupDelimitedClus=", param.OrgGroupDelimitedClus, "&",
                  "OrgGroupDelimitedArea=", param.OrgGroupDelimitedArea, "&",
                  "OrgGroupDelimitedReg=", param.OrgGroupDelimitedReg, "&",
                  "OrgGroupDelimitedZone=", param.OrgGroupDelimitedZone, "&",
                  "IsExport=", true);

            return await SharedUtilities.GetFromAPI(new List<EMS.Plantilla.Transfer.Employee.GetETFListOutput>(), URL);
        }

        public async Task<IActionResult> OnGetCheckETFExportListAsync([FromQuery] EMS.Plantilla.Transfer.Employee.GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetETFDataList(param);

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

        public async Task<IActionResult> OnGetDownloadETFExportListAsync([FromQuery] EMS.Plantilla.Transfer.Employee.GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetETFDataList(param);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "EMS ETF.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("ETF");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Employee Code");
                    row.CreateCell(1).SetCellValue("Last Name");
                    row.CreateCell(2).SetCellValue("First Name");
                    row.CreateCell(3).SetCellValue("Middle Name");
                    row.CreateCell(4).SetCellValue("Suffix");
                    row.CreateCell(5).SetCellValue("Nickname");
                    row.CreateCell(6).SetCellValue("Nationality");
                    row.CreateCell(7).SetCellValue("Citizenship");
                    row.CreateCell(8).SetCellValue("BirthDate");
                    row.CreateCell(9).SetCellValue("BirthPlace");
                    row.CreateCell(10).SetCellValue("Gender");
                    row.CreateCell(11).SetCellValue("Height (cm)");
                    row.CreateCell(12).SetCellValue("Weight (lbs)");
                    row.CreateCell(13).SetCellValue("Civil Status");
                    row.CreateCell(14).SetCellValue("Religion");
                    row.CreateCell(15).SetCellValue("SSS Status");
                    row.CreateCell(16).SetCellValue("Exemption Status");
                    row.CreateCell(17).SetCellValue("Home Address");
                    row.CreateCell(18).SetCellValue("Mobile No");
                    row.CreateCell(19).SetCellValue("Home Address City");
                    row.CreateCell(20).SetCellValue("Home Address Region");
                    row.CreateCell(21).SetCellValue("HDMF No");
                    row.CreateCell(22).SetCellValue("SSS No");
                    row.CreateCell(23).SetCellValue("Tin No");
                    row.CreateCell(24).SetCellValue("PhilHealth No");
                    row.CreateCell(25).SetCellValue("Contact Person");
                    row.CreateCell(26).SetCellValue("Contact Person Relationship");
                    row.CreateCell(27).SetCellValue("Spouse");
                    row.CreateCell(28).SetCellValue("Spouse Birthdate");
                    row.CreateCell(29).SetCellValue("Spouse Employer");
                    row.CreateCell(30).SetCellValue("Spouse Occupation");
                    row.CreateCell(31).SetCellValue("Father");
                    row.CreateCell(32).SetCellValue("Father Birthdate");
                    row.CreateCell(33).SetCellValue("Father Occupation");
                    row.CreateCell(34).SetCellValue("Mother");
                    row.CreateCell(35).SetCellValue("Mother Birthdate");
                    row.CreateCell(36).SetCellValue("Mother Occupation");
                    row.CreateCell(37).SetCellValue("School");
                    row.CreateCell(38).SetCellValue("School Address");
                    row.CreateCell(39).SetCellValue("School Level");
                    row.CreateCell(40).SetCellValue("Course");
                    row.CreateCell(41).SetCellValue("Year From");
                    row.CreateCell(42).SetCellValue("Year To");
                    row.CreateCell(43).SetCellValue("Educational Attainment Degree");
                    row.CreateCell(44).SetCellValue("Educational Attainment Status");
                    row.CreateCell(45).SetCellValue("Company Code");
                    row.CreateCell(46).SetCellValue("Home Branch");
                    row.CreateCell(47).SetCellValue("Branch Code");
                    row.CreateCell(48).SetCellValue("Branch Name");
                    row.CreateCell(49).SetCellValue("Cluster");
                    row.CreateCell(50).SetCellValue("Area");
                    row.CreateCell(51).SetCellValue("Region");
                    row.CreateCell(52).SetCellValue("Zone");
                    row.CreateCell(53).SetCellValue("Monthly Salary");
                    row.CreateCell(54).SetCellValue("Daily Salary");
                    row.CreateCell(55).SetCellValue("Hourly Salary");
                    row.CreateCell(56).SetCellValue("Department Code");
                    row.CreateCell(57).SetCellValue("Department");
                    row.CreateCell(58).SetCellValue("Designation Code");
                    row.CreateCell(59).SetCellValue("Designation");
                    row.CreateCell(60).SetCellValue("Position Level");
                    row.CreateCell(61).SetCellValue("Job Class");
                    row.CreateCell(62).SetCellValue("Date Hired");
                    row.CreateCell(63).SetCellValue("Previous Employer");
                    row.CreateCell(64).SetCellValue("Previous Designation");
                    row.CreateCell(65).SetCellValue("Year Started");
                    row.CreateCell(66).SetCellValue("Year Ended");
                    row.CreateCell(67).SetCellValue("Employment Status");
                    row.CreateCell(68).SetCellValue("New Employee ID");

                    excelSheet.SetColumnWidth(0, 5500);
                    excelSheet.SetColumnWidth(1, 4500);
                    excelSheet.SetColumnWidth(2, 4500);
                    excelSheet.SetColumnWidth(3, 4500);
                    excelSheet.SetColumnWidth(4, 3500);
                    excelSheet.SetColumnWidth(5, 3500);
                    excelSheet.SetColumnWidth(6, 4500);
                    excelSheet.SetColumnWidth(7, 4500);
                    excelSheet.SetColumnWidth(8, 4500);
                    excelSheet.SetColumnWidth(9, 4500);
                    excelSheet.SetColumnWidth(10, 3500);
                    excelSheet.SetColumnWidth(11, 3500);
                    excelSheet.SetColumnWidth(12, 3500);
                    excelSheet.SetColumnWidth(13, 3500);
                    excelSheet.SetColumnWidth(14, 4500);
                    excelSheet.SetColumnWidth(15, 4500);
                    excelSheet.SetColumnWidth(16, 5500);
                    excelSheet.SetColumnWidth(17, 20000);
                    excelSheet.SetColumnWidth(18, 5500);
                    excelSheet.SetColumnWidth(19, 5500);
                    excelSheet.SetColumnWidth(20, 6500);
                    excelSheet.SetColumnWidth(21, 5500);
                    excelSheet.SetColumnWidth(22, 5500);
                    excelSheet.SetColumnWidth(23, 5500);
                    excelSheet.SetColumnWidth(24, 5500);
                    excelSheet.SetColumnWidth(25, 7000);
                    excelSheet.SetColumnWidth(26, 7000);
                    excelSheet.SetColumnWidth(27, 5500);
                    excelSheet.SetColumnWidth(28, 4500);
                    excelSheet.SetColumnWidth(29, 6500);
                    excelSheet.SetColumnWidth(30, 6500);
                    excelSheet.SetColumnWidth(31, 5500);
                    excelSheet.SetColumnWidth(32, 4500);
                    excelSheet.SetColumnWidth(33, 5500);
                    excelSheet.SetColumnWidth(34, 5500);
                    excelSheet.SetColumnWidth(35, 4500);
                    excelSheet.SetColumnWidth(36, 5500);
                    excelSheet.SetColumnWidth(37, 8500);
                    excelSheet.SetColumnWidth(38, 8500);
                    excelSheet.SetColumnWidth(39, 5500);
                    excelSheet.SetColumnWidth(40, 5500);
                    excelSheet.SetColumnWidth(41, 3500);
                    excelSheet.SetColumnWidth(42, 3500);
                    excelSheet.SetColumnWidth(43, 7000);
                    excelSheet.SetColumnWidth(44, 7000);
                    excelSheet.SetColumnWidth(45, 6000);
                    excelSheet.SetColumnWidth(46, 5500);
                    excelSheet.SetColumnWidth(47, 5500);
                    excelSheet.SetColumnWidth(48, 6500);
                    excelSheet.SetColumnWidth(49, 7000);
                    excelSheet.SetColumnWidth(50, 4500);
                    excelSheet.SetColumnWidth(51, 4500);
                    excelSheet.SetColumnWidth(52, 4500);
                    excelSheet.SetColumnWidth(53, 6500);
                    excelSheet.SetColumnWidth(54, 7000);
                    excelSheet.SetColumnWidth(55, 5500);
                    excelSheet.SetColumnWidth(56, 8500);
                    excelSheet.SetColumnWidth(57, 8500);
                    excelSheet.SetColumnWidth(58, 4500);
                    excelSheet.SetColumnWidth(59, 8500);
                    excelSheet.SetColumnWidth(60, 5500);
                    excelSheet.SetColumnWidth(61, 5500);
                    excelSheet.SetColumnWidth(62, 3500);
                    excelSheet.SetColumnWidth(63, 3500);
                    excelSheet.SetColumnWidth(64, 6500);
                    excelSheet.SetColumnWidth(65, 5500);
                    excelSheet.SetColumnWidth(66, 5500);
                    excelSheet.SetColumnWidth(67, 5500);
                    excelSheet.SetColumnWidth(68, 5500);

                    XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle alignRight = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle numberStyle = (XSSFCellStyle)workbook.CreateCellStyle();

                    var colHeaderFont = workbook.CreateFont();
                    colHeaderFont.IsBold = true;
                    colHeaderStyle.SetFont(colHeaderFont);
                    colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                    colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                    colHeaderStyle.Alignment = HorizontalAlignment.Center;
                    alignCenter.Alignment = HorizontalAlignment.Center;
                    alignRight.Alignment = HorizontalAlignment.Right;
                    numberStyle.DataFormat = workbook.CreateDataFormat().GetFormat("#,###");

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
                    row.Cells[19].CellStyle = colHeaderStyle;
                    row.Cells[20].CellStyle = colHeaderStyle;
                    row.Cells[21].CellStyle = colHeaderStyle;
                    row.Cells[22].CellStyle = colHeaderStyle;
                    row.Cells[23].CellStyle = colHeaderStyle;
                    row.Cells[24].CellStyle = colHeaderStyle;
                    row.Cells[25].CellStyle = colHeaderStyle;
                    row.Cells[26].CellStyle = colHeaderStyle;
                    row.Cells[27].CellStyle = colHeaderStyle;
                    row.Cells[28].CellStyle = colHeaderStyle;
                    row.Cells[29].CellStyle = colHeaderStyle;
                    row.Cells[30].CellStyle = colHeaderStyle;
                    row.Cells[31].CellStyle = colHeaderStyle;
                    row.Cells[32].CellStyle = colHeaderStyle;
                    row.Cells[33].CellStyle = colHeaderStyle;
                    row.Cells[34].CellStyle = colHeaderStyle;
                    row.Cells[35].CellStyle = colHeaderStyle;
                    row.Cells[36].CellStyle = colHeaderStyle;
                    row.Cells[37].CellStyle = colHeaderStyle;
                    row.Cells[38].CellStyle = colHeaderStyle;
                    row.Cells[39].CellStyle = colHeaderStyle;
                    row.Cells[40].CellStyle = colHeaderStyle;
                    row.Cells[41].CellStyle = colHeaderStyle;
                    row.Cells[42].CellStyle = colHeaderStyle;
                    row.Cells[43].CellStyle = colHeaderStyle;
                    row.Cells[44].CellStyle = colHeaderStyle;
                    row.Cells[45].CellStyle = colHeaderStyle;
                    row.Cells[46].CellStyle = colHeaderStyle;
                    row.Cells[47].CellStyle = colHeaderStyle;
                    row.Cells[48].CellStyle = colHeaderStyle;
                    row.Cells[49].CellStyle = colHeaderStyle;
                    row.Cells[50].CellStyle = colHeaderStyle;
                    row.Cells[51].CellStyle = colHeaderStyle;
                    row.Cells[52].CellStyle = colHeaderStyle;
                    row.Cells[53].CellStyle = colHeaderStyle;
                    row.Cells[54].CellStyle = colHeaderStyle;
                    row.Cells[55].CellStyle = colHeaderStyle;
                    row.Cells[56].CellStyle = colHeaderStyle;
                    row.Cells[57].CellStyle = colHeaderStyle;
                    row.Cells[58].CellStyle = colHeaderStyle;
                    row.Cells[59].CellStyle = colHeaderStyle;
                    row.Cells[60].CellStyle = colHeaderStyle;
                    row.Cells[61].CellStyle = colHeaderStyle;
                    row.Cells[62].CellStyle = colHeaderStyle;
                    row.Cells[63].CellStyle = colHeaderStyle;
                    row.Cells[64].CellStyle = colHeaderStyle;
                    row.Cells[65].CellStyle = colHeaderStyle;
                    row.Cells[66].CellStyle = colHeaderStyle;
                    row.Cells[67].CellStyle = colHeaderStyle;
                    row.Cells[68].CellStyle = colHeaderStyle;
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.OldEmployeeID);
                        row.CreateCell(1).SetCellValue(item.LastName);
                        row.CreateCell(2).SetCellValue(item.FirstName);
                        row.CreateCell(3).SetCellValue(item.MiddleName);
                        row.CreateCell(4).SetCellValue(item.Suffix);
                        row.CreateCell(5).SetCellValue(item.Nickname);
                        row.CreateCell(6).SetCellValue(item.Nationality);
                        row.CreateCell(7).SetCellValue(item.Citizenship);
                        row.CreateCell(8).SetCellValue(item.BirthDate);
                        row.CreateCell(9).SetCellValue(item.BirthPlace);
                        row.CreateCell(10).SetCellValue(item.Gender);
                        row.CreateCell(11).SetCellValue(item.Height);
                        row.CreateCell(12).SetCellValue(item.Weight);
                        row.CreateCell(13).SetCellValue(item.CivilStatus);
                        row.CreateCell(14).SetCellValue(item.Religion);
                        row.CreateCell(15).SetCellValue(item.SSSStatus);
                        row.CreateCell(16).SetCellValue(item.ExemptionStatus);
                        row.CreateCell(17).SetCellValue(item.HomeAddress);
                        row.CreateCell(18).SetCellValue(item.CellphoneNumber);
                        row.CreateCell(19).SetCellValue(item.HomeCity);
                        row.CreateCell(20).SetCellValue(item.HomeRegion);
                        row.CreateCell(21).SetCellValue(item.PagibigNumber);
                        row.CreateCell(22).SetCellValue(item.SSSNumber);
                        row.CreateCell(23).SetCellValue(item.TIN);
                        row.CreateCell(24).SetCellValue(item.PhilhealthNumber);
                        row.CreateCell(25).SetCellValue(item.ContactPersonName);
                        row.CreateCell(26).SetCellValue(item.ContactPersonRelationship);
                        row.CreateCell(27).SetCellValue(item.SpouseName);
                        row.CreateCell(28).SetCellValue(item.SpouseBirthdate);
                        row.CreateCell(29).SetCellValue(item.SpouseEmployer);
                        row.CreateCell(30).SetCellValue(item.SpouseOccupation);
                        row.CreateCell(31).SetCellValue(item.FatherName);
                        row.CreateCell(32).SetCellValue(item.FatherBirthdate);
                        row.CreateCell(33).SetCellValue(item.FatherOccupation);
                        row.CreateCell(34).SetCellValue(item.MotherName);
                        row.CreateCell(35).SetCellValue(item.MotherBirthdate);
                        row.CreateCell(36).SetCellValue(item.MotherOccupation);
                        row.CreateCell(37).SetCellValue(item.School);
                        row.CreateCell(38).SetCellValue(item.SchoolAddress);
                        row.CreateCell(39).SetCellValue(item.SchoolLevel);
                        row.CreateCell(40).SetCellValue(item.Course);
                        row.CreateCell(41).SetCellValue(item.YearFrom);
                        row.CreateCell(42).SetCellValue(item.YearTo);
                        row.CreateCell(43).SetCellValue(item.AttainmentDegree);
                        row.CreateCell(44).SetCellValue(item.AttainmentStatus);
                        row.CreateCell(45).SetCellValue(item.CompanyCode);
                        row.CreateCell(46).SetCellValue(item.HomeBranch);
                        row.CreateCell(47).SetCellValue(item.BranchCode);
                        row.CreateCell(48).SetCellValue(item.BranchName);
                        row.CreateCell(49).SetCellValue(item.Cluster);
                        row.CreateCell(50).SetCellValue(item.Area);
                        row.CreateCell(51).SetCellValue(item.Region);
                        row.CreateCell(52).SetCellValue(item.Zone);
                        row.CreateCell(53).SetCellValue(Convert.ToDouble(item.MonthlySalary) > 0 ? Convert.ToDouble(item.MonthlySalary) : 0.00);
                        row.CreateCell(54).SetCellValue(Convert.ToDouble(item.DailySalary) > 0 ? Convert.ToDouble(item.DailySalary) : 0.00);
                        row.CreateCell(55).SetCellValue(Convert.ToDouble(item.HourlySalary) > 0 ? Convert.ToDouble(item.HourlySalary) : 0.00);
                        row.CreateCell(56).SetCellValue(item.DeptCode);
                        row.CreateCell(57).SetCellValue(item.DeptName);
                        row.CreateCell(58).SetCellValue(item.PositionCode);
                        row.CreateCell(59).SetCellValue(item.PositionTitle);
                        row.CreateCell(60).SetCellValue(item.PositionLevel);
                        row.CreateCell(61).SetCellValue(item.JobClass);
                        row.CreateCell(62).SetCellValue(item.DateHired);
                        row.CreateCell(63).SetCellValue(item.PreviousEmployer);
                        row.CreateCell(64).SetCellValue(item.PreviousDesignation);
                        row.CreateCell(65).SetCellValue(item.YearStarted);
                        row.CreateCell(66).SetCellValue(item.YearEnded);
                        row.CreateCell(67).SetCellValue(item.EmploymentStatus);
                        row.CreateCell(68).SetCellValue(item.Code);

                        row.Cells[0].CellStyle = alignCenter;
                        row.Cells[4].CellStyle = alignCenter;
                        //row.Cells[5].CellStyle = alignCenter;
                        row.Cells[6].CellStyle = alignCenter;
                        row.Cells[7].CellStyle = alignCenter;
                        row.Cells[8].CellStyle = alignCenter;
                        row.Cells[11].CellStyle = alignCenter;
                        row.Cells[12].CellStyle = alignCenter;
                        row.Cells[16].CellStyle = alignCenter;
                        row.Cells[18].CellStyle = alignCenter;
                        row.Cells[21].CellStyle = alignCenter;
                        row.Cells[22].CellStyle = alignCenter;
                        row.Cells[23].CellStyle = alignCenter;
                        row.Cells[24].CellStyle = alignCenter;
                        row.Cells[28].CellStyle = alignCenter;
                        row.Cells[32].CellStyle = alignCenter;
                        row.Cells[35].CellStyle = alignCenter;
                        row.Cells[41].CellStyle = alignCenter;
                        row.Cells[42].CellStyle = alignCenter;
                        row.Cells[45].CellStyle = alignCenter;
                        row.Cells[52].CellStyle = alignRight;
                        row.Cells[53].CellStyle = alignRight;
                        row.Cells[54].CellStyle = alignRight;
                        row.Cells[59].CellStyle = alignCenter;
                        row.Cells[61].CellStyle = alignCenter;
                        row.Cells[64].CellStyle = alignCenter;
                        row.Cells[65].CellStyle = alignCenter;
                        row.Cells[52].CellStyle = numberStyle;
                        row.Cells[53].CellStyle = numberStyle;
                        row.Cells[54].CellStyle = numberStyle;
                        row.Cells[67].CellStyle = alignCenter;

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
                        TableName = "Employee",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "ETF List exported",
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


        public async Task<JsonResult> OnGetMainAttachment(int ID)
        {
            _resultView.Result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeAttachment(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostSaveMainAttachment()
        {
            var (IsSuccess, Message) =
                await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .SaveEmployeeAttachment(EmployeeAttachmentForm);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            StringBuilder message = new StringBuilder();
            if (EmployeeAttachmentForm.AddAttachmentForm != null)
            {
                if (EmployeeAttachmentForm.AddAttachmentForm.Count > 0)
                {
                    foreach (var item in EmployeeAttachmentForm.AddAttachmentForm)
                    {
                        if (item.File != null)
                        {
                            message.Append(string.Concat(item.SourceFile, " Attachment added. ")); 
                        }
                    }
                }
            }
            
            if (EmployeeAttachmentForm.DeleteAttachmentForm != null)
            {
                if (EmployeeAttachmentForm.DeleteAttachmentForm.Count > 0)
                {
                    foreach (var item in EmployeeAttachmentForm.DeleteAttachmentForm)
                    {
                        if (item.ServerFile != null)
                        {
                            message.Append(string.Concat(item.ServerFile, " Attachment removed. ")); 
                        }
                    }
                }
            }

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "EmployeeAttachment",
                        TableID = EmployeeAttachmentForm.EmployeeID,
                        Remarks = message.ToString(),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetSkillsList([FromQuery] EmployeeSkillsFormInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmployeeSkillsByEmployeeId").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ID=", param.ID, "&",
                  "EmployeeId=",param.EmployeeID,"&",/*, param.EmployeeID, "&",*/
                  "SkillsCode=", param.SkillsCode, "&",
                  "SkillsDescription=", param.SkillsDescription, "&",
                  "Rate=", param.Rate, "&",
                  "Remarks=", param.Remarks, "&",
                  "CreatedBy=", param.CreatedBy, "&",
                  "CreatedDate=", param.CreatedDate, "&",
                  "ModifiedBy=", param.ModifiedBy, "&",
                  "ModifiedDate=", param.ModifiedDate, "&",
                  "IsExport=", param.IsExport);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<EmployeeSkillsFormOutput>(), URL);

            //// Get Employee Names by User IDs
            //List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
            //   await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
            //   .GetSystemUserByIDs(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy)
            //   .Distinct().ToList());

            //// Get Employee Names by Assigned User ID
            //List<EMS.Security.Transfer.SystemUser.Form> assignedUsers =
            //   await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
            //   .GetSystemUserByIDs(Result.Where(x => x.AssignedUserID != 0).Select(x => x.AssignedUserID)
            //   .Distinct().ToList());

            //if (Result.Count > 0)
            //{

            //    Result = Result
            //           .GroupJoin(systemUsers,
            //           x => new { x.CreatedBy },
            //           y => new { CreatedBy = y.ID },
            //           (x, y) => new { activities = x, employees = y })
            //           .SelectMany(x => x.employees.DefaultIfEmpty(),
            //           (x, y) => new { activities = x, employees = y })
            //           .GroupJoin(assignedUsers,
            //           x => new { x.activities.activities.AssignedUserID },
            //           y => new { AssignedUserID = y.ID },
            //           (x, y) => new { activities = x, employees = y })
            //           .SelectMany(x => x.employees.DefaultIfEmpty(),
            //           (x, y) => new { activities = x, employees = y })
            //           .Select(x => new GetChecklistListOutput
            //           {

            //               ID = x.activities.activities.activities.activities.ID,
            //               Title = x.activities.activities.activities.activities.Title,
            //               Type = x.activities.activities.activities.activities.Type,
            //               SubType = x.activities.activities.activities.activities.SubType,
            //               Description = x.activities.activities.activities.activities.Description,
            //               CurrentStatus = x.activities.activities.activities.activities.CurrentStatus,
            //               CurrentTimestamp = x.activities.activities.activities.activities.CurrentTimestamp,
            //               AssignedBy = x.activities.activities.employees == null ? "" : string.Concat(x.activities.activities.employees.LastName,
            //                   string.IsNullOrEmpty(x.activities.activities.employees.FirstName) ? "" : string.Concat(", ", x.activities.activities.employees.FirstName),
            //                   string.IsNullOrEmpty(x.activities.activities.employees.MiddleName) ? "" : string.Concat(" ", x.activities.activities.employees.MiddleName)),
            //               NoOfPages = x.activities.activities.activities.activities.NoOfPages,
            //               Row = x.activities.activities.activities.activities.Row,
            //               TotalNoOfRecord = x.activities.activities.activities.activities.TotalNoOfRecord,
            //               IsPass = x.activities.activities.activities.activities.IsPass,
            //               CreatedBy = x.activities.activities.activities.activities.CreatedBy,
            //               EmployeeID = x.activities.activities.activities.activities.EmployeeID,
            //               Remarks = x.activities.activities.activities.activities.Remarks,
            //               AssignedTo = x.employees == null ? "" : string.Concat(x.employees.LastName,
            //                   string.IsNullOrEmpty(x.employees.FirstName) ? "" : string.Concat(", ", x.employees.FirstName),
            //                   string.IsNullOrEmpty(x.employees.MiddleName) ? "" : string.Concat(" ", x.employees.MiddleName)),
            //               IsAssignment = x.activities.activities.activities.activities.IsAssignment,
            //               DueDate = x.activities.activities.activities.activities.DueDate
            //           }).ToList();
            //}

            if (IsSuccess)
            {
                var jsonData = new
                {
                    total = 0,
                    param.pageNumber,
                    sort = param.sidx,
                    records = 0,
                    rows = Result
                };
                return new JsonResult(jsonData);
            }
            else
            {
                return new BadRequestObjectResult(ErrorMessage);
            }
        }
        public async Task<IActionResult> OnPostUploadLogActivity(int ID)
        {
            var EmployeeList = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployee(ID);
            IFormFile file = Request.Form.Files[0];
            string filePath = Path.Combine(_env.WebRootPath, "\\LogActivity\\Uploads");
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(filePath, file.FileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;

                    if (sFileExtension == ".xls")//This will read the Excel 97-2000 formats    
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                    else //This will read 2007 Excel format    
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }

                    IRow headerRow = sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;

                    List<UploadLogActivityFile> uploadLogActivityList = new List<UploadLogActivityFile>();
                    int blankRows = 0;

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        if (row != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()))
                            {
                                UploadLogActivityFile obj = new UploadLogActivityFile
                                {
                                    RowNum = (i + 1).ToString(),
                                    Type = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Title = row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Description = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    DueDate = Convert.ToDateTime(row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()),
                                    Remarks = row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    OrgGroupCode = row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                };
                                uploadLogActivityList.Add(obj);
                            }
                        }
                        else
                        {
                            blankRows++;
                            if (blankRows > 3)
                                break;
                        }

                    }

                    var OrgGroupDelimited = string.Join(",", uploadLogActivityList.Select(x => x.OrgGroupCode).Distinct());
                    var OrgGroupList = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                        .GetByCodes(OrgGroupDelimited);

                    if (OrgGroupList.Count > 0)
                    {
                        uploadLogActivityList = uploadLogActivityList.GroupJoin(OrgGroupList,
                        x => new { x.OrgGroupCode },
                        y => new { OrgGroupCode = y.Code },
                        (x, y) => (x, y))
                        .SelectMany(x => x.y.DefaultIfEmpty(),
                        (x, y) => new { x, y })
                        .Select(x => new UploadLogActivityFile
                        {
                            RowNum = x.x.x.RowNum,
                            EmployeeID = ID,
                            EmployeeName = EmployeeList.PersonalInformation.LastName + ", " + EmployeeList.PersonalInformation.FirstName + " " + EmployeeList.PersonalInformation.MiddleName,
                            Email = EmployeeList.CorporateEmail,
                            AssignedUserId = EmployeeList.SystemUserID,
                            Type = x.x.x.Type,
                            Title = x.x.x.Title,
                            Description = x.x.x.Description,
                            DueDate = x.x.x.DueDate,
                            OrgGroupCode = x.x.x.OrgGroupCode,
                            Remarks = x.x.x.Remarks,
                            CreatedBy = _globalCurrentUser.UserID,
                            AssignedOrgGroupID = x.y == null ? 0 : x.y.ID
                        }).ToList();
                    }

                    var URL = string.Concat(_workflowBaseURL,
                               _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("UploadLogActivityInsert").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

                    var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(uploadLogActivityList, URL);

                    if (IsSuccess)
                    {
                        /*Add AuditLog*/
                        await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                            .AddAuditLog(new Security.Transfer.AuditLog.Form
                            {
                                EventType = Common_AuditLog.EventType.UPLOAD.ToString(),
                                TableName = "EmployeeLogActivity",
                                TableID = 0, // New Record, no ID yet
                                Remarks = "Employee Log Activity uploaded",
                                IsSuccess = true,
                                CreatedBy = _globalCurrentUser.UserID
                            });

                    }

                    _resultView.IsSuccess = IsSuccess;
                    _resultView.Result = Message;
                }
            }

            return new JsonResult(_resultView);
        }
        public async Task<IActionResult> OnGetDownloadLogActivityTemplate()
        {
            string sWebRootFolder = _env.WebRootPath;
            string sFileName = "LogActivity.xlsx";
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();

                // Header style
                XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();
                var format = workbook.CreateDataFormat().GetFormat("text");
                var colHeaderFont = workbook.CreateFont();
                colHeaderFont.IsBold = true;
                colHeaderStyle.SetFont(colHeaderFont);
                colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                colHeaderStyle.Alignment = HorizontalAlignment.Center;
                colHeaderStyle.DataFormat = format;
                alignCenter.Alignment = HorizontalAlignment.Center;

                // Dropdown values
                var typeList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Workflow.Transfer.Enums.ReferenceCodes.ACTIVITY_TYPE.ToString()))
                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();


                IDataValidation getDropdownValidation(CellRangeAddressList addressList, string[] stringArray, ISheet excelSheet)
                {
                    IDataValidationHelper validationHelper = new XSSFDataValidationHelper((XSSFSheet)excelSheet);
                    IDataValidationConstraint constraint = validationHelper.CreateExplicitListConstraint(stringArray);
                    IDataValidation dataValidation = validationHelper.CreateValidation(constraint, addressList);
                    dataValidation.SuppressDropDownArrow = true;
                    dataValidation.ShowErrorBox = true;
                    dataValidation.CreateErrorBox("", "Please select from Dropdown values.");
                    return dataValidation;
                }


                void AddETFSheet()
                {
                    ISheet excelSheet = workbook.CreateSheet("Accountability");
                    int rowCtr = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(rowCtr); rowCtr++;

                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 0, 0), typeList, excelSheet));

                    row.CreateCell(0).SetCellValue("* Type");
                    row.CreateCell(1).SetCellValue("* Title");
                    row.CreateCell(2).SetCellValue("Description");
                    row.CreateCell(3).SetCellValue("* Due Date (YYYY-MM-DD)");
                    row.CreateCell(4).SetCellValue("Remarks");
                    row.CreateCell(5).SetCellValue("* Org Group Code");

                    excelSheet.SetColumnWidth(0, 6500);
                    excelSheet.SetColumnWidth(1, 6500);
                    excelSheet.SetColumnWidth(2, 6500);
                    excelSheet.SetColumnWidth(3, 6500);
                    excelSheet.SetColumnWidth(4, 6500);
                    excelSheet.SetColumnWidth(5, 6500);

                    for (int i = 0; i <= 5; i++)
                        row.Cells[i].CellStyle = colHeaderStyle;

                    #endregion

                    for (int x = rowCtr; x <= 10000; x++)
                    {
                        IRow rowDate = excelSheet.CreateRow(x);
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;

                        for (int i = 0; i <= 5; i++)
                            rowDate.CreateCell(i, CellType.String).SetCellValue("");

                        for (int i = 0; i <= 5; i++)
                            rowDate.Cells[i].CellStyle = textCS;
                    }

                }

                AddETFSheet();

                workbook.Write(fs);
            }

            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        public async Task<IActionResult> OnGetDownloadAccountabilityTemplate()
        {
            string sWebRootFolder = _env.WebRootPath;
            string sFileName = "Accountability.xlsx";
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();

                // Header style
                XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();
                var format = workbook.CreateDataFormat().GetFormat("text");
                var colHeaderFont = workbook.CreateFont();
                colHeaderFont.IsBold = true;
                colHeaderStyle.SetFont(colHeaderFont);
                colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                colHeaderStyle.Alignment = HorizontalAlignment.Center;
                colHeaderStyle.DataFormat = format;
                alignCenter.Alignment = HorizontalAlignment.Center;

                // Dropdown values
                var typeList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Workflow.Transfer.Enums.ReferenceCodes.ACCOUNTABILITY_TYPE.ToString()))
                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();


                IDataValidation getDropdownValidation(CellRangeAddressList addressList, string[] stringArray, ISheet excelSheet)
                {
                    IDataValidationHelper validationHelper = new XSSFDataValidationHelper((XSSFSheet)excelSheet);
                    IDataValidationConstraint constraint = validationHelper.CreateExplicitListConstraint(stringArray);
                    IDataValidation dataValidation = validationHelper.CreateValidation(constraint, addressList);
                    dataValidation.SuppressDropDownArrow = true;
                    dataValidation.ShowErrorBox = true;
                    dataValidation.CreateErrorBox("", "Please select from Dropdown values.");
                    return dataValidation;
                }


                void AddETFSheet()
                {
                    ISheet excelSheet = workbook.CreateSheet("Accountability");
                    int rowCtr = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(rowCtr); rowCtr++;

                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 0, 0), typeList, excelSheet));

                    row.CreateCell(0).SetCellValue("* Type");
                    row.CreateCell(1).SetCellValue("* Title");
                    row.CreateCell(2).SetCellValue("Description");
                    row.CreateCell(3).SetCellValue("Remarks");
                    row.CreateCell(4).SetCellValue("* Org Group Code");

                    excelSheet.SetColumnWidth(0, 6500);
                    excelSheet.SetColumnWidth(1, 6500);
                    excelSheet.SetColumnWidth(2, 6500);
                    excelSheet.SetColumnWidth(3, 6500);
                    excelSheet.SetColumnWidth(4, 6500);

                    for (int i = 0; i <= 4; i++)
                        row.Cells[i].CellStyle = colHeaderStyle;

                    #endregion

                    for (int x = rowCtr; x <= 10000; x++)
                    {
                        IRow rowDate = excelSheet.CreateRow(x);
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;

                        for (int i = 0; i <= 4; i++)
                            rowDate.CreateCell(i, CellType.String).SetCellValue("");

                        for (int i = 0; i <= 4; i++)
                            rowDate.Cells[i].CellStyle = textCS;
                    }

                }

                AddETFSheet();

                workbook.Write(fs);
            }

            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        public async Task<IActionResult> OnPostUploadAccountability(int ID)
        {
            IFormFile file = Request.Form.Files[0];
            string filePath = Path.Combine(_env.WebRootPath, "\\Accountability\\Uploads");
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(filePath, file.FileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;

                    if (sFileExtension == ".xls")//This will read the Excel 97-2000 formats    
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                    else //This will read 2007 Excel format    
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }

                    IRow headerRow = sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;

                    List<AccountabilityUploadFile> uploadList = new List<AccountabilityUploadFile>();
                    int blankRows = 0;

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        if (row != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()))
                            {
                                AccountabilityUploadFile obj = new AccountabilityUploadFile
                                {
                                    RowNum = (i + 1).ToString(),
                                    Type = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Title = row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Description = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Remarks = row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    OrgGroupCode = row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()
                                };
                                uploadList.Add(obj);
                            }
                        }
                        else
                        {
                            blankRows++;
                            if (blankRows > 3)
                                break;
                        }

                    }

                    var EmployeeList = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                        .GetEmployee(ID);

                    var OrgGroupDelimited = string.Join(",", uploadList.Select(x => x.OrgGroupCode).Distinct());
                    var OrgGroupList = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                        .GetByCodes(OrgGroupDelimited);

                    if (OrgGroupList.Count > 0)
                    {
                        uploadList = uploadList.GroupJoin(OrgGroupList,
                        x => new { x.OrgGroupCode },
                        y => new { OrgGroupCode = y.Code },
                        (x, y) => (x, y))
                        .SelectMany(x => x.y.DefaultIfEmpty(),
                        (x, y) => new { x, y })
                        .Select(x => new AccountabilityUploadFile
                        {
                            RowNum = x.x.x.RowNum,
                            OldEmployeeID = EmployeeList.OldEmployeeID,
                            Type = x.x.x.Type,
                            Title = x.x.x.Title,
                            Description = x.x.x.Description,
                            Remarks = x.x.x.Remarks,
                            OrgGroupCode = x.x.x.OrgGroupCode,
                            EmployeeID = EmployeeList.ID,
                            OrgGroupID = x.y == null ? 0 : x.y.ID
                        }).ToList();
                    }


                    var URL = string.Concat(_workflowBaseURL,
                               _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("UploadInsert").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

                    var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(uploadList, URL);


                    if (IsSuccess)
                    {

                        /*Add AuditLog*/
                        await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                            .AddAuditLog(new Security.Transfer.AuditLog.Form
                            {
                                EventType = Common_AuditLog.EventType.UPLOAD.ToString(),
                                TableName = "EmployeeAccountability",
                                TableID = 0, // New Record, no ID yet
                                Remarks = "Employee Accountability uploaded",
                                IsSuccess = true,
                                CreatedBy = _globalCurrentUser.UserID
                            });
                    }


                    _resultView.IsSuccess = IsSuccess;
                    _resultView.Result = Message;
                }
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetOrgGroupPositionDropDown(int ID = 0)
        {
            return new JsonResult(await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupPositionByID(ID));
        }

        public async Task<JsonResult> OnPostDeleteAccountability([FromQuery] string ID)
        {
            var (IsSuccess, Message) = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env).BulkEmployeeAccountabilityDelete(ID);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetAccountabilityChangeStatus(string CurrentStatus)
        {
            var roles = (await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                   .GetSystemUserRoleDropDownByUserID(_globalCurrentUser.UserID)).ToList();

            var status = (await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetNextWorkflowStep(new GetNextWorkflowStepInput
                {
                    WorkflowCode = "ACCOUNTABILITY",
                    CurrentStepCode = CurrentStatus,
                    RoleIDDelimited = string.Join(",", roles.Select(x => x.Value).ToArray())
                })).Where(x=> !x.Code.Equals(CurrentStatus)).ToList();

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

        public async Task<JsonResult> OnPostAccountabilityChangeStatus(string ID,string NewStatus,string Remarks)
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
                        .GetReferenceValueByRefCode("ESTATUS")).Where(x => x.Value.Equals("INACTIVE")).Select(y => y.Description).FirstOrDefault();

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
                            .GetEmployeeByUserIDs(SystemUserIDApprover)).Item1).Where(x => !string.IsNullOrEmpty(x.CorporateEmail)).ToList();
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
                                          Name = String.Concat("(", left.Code, ") ", left.PersonalInformation.LastName, ", ", left.PersonalInformation.FirstName, " ", ((left.PersonalInformation.MiddleName == null || left.PersonalInformation.MiddleName == "") ? "" : left.PersonalInformation.MiddleName.Substring(0, 1))),
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

        public async Task<JsonResult> OnGetStatusColor([FromQuery] string Color)
        {
            _resultView.Result = await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _securityBaseURL, _env)
                .GetReferenceValueBySecurityRefCode(Color);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetCorporateEmailAutomate([FromQuery] string BranchID)
        {
            _resultView.Result = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                                            .GetEmployeeEmailByOrgID(Convert.ToInt32(BranchID)));
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnPostEditDraftToProbationary([FromQuery] string ID)
        {
            List<int> IDs = (ID.Split(",")).Select(x => int.Parse(x)).ToList();
            var (IsSuccess, Messages) = (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                            .EditDraftToProbationary(IDs));
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Messages;
            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetCheckExportDraftAsync([FromQuery] EMS.Plantilla.Transfer.Employee.GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataList(param,true);

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

        public async Task<IActionResult> OnGetDownloadExportDraftAsync([FromQuery] EMS.Plantilla.Transfer.Employee.GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataList(param, true);
            Result = Result.Where(x=>string.IsNullOrEmpty(x.Code)).ToList();
            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "EmployeeList.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    var Company = (await new SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                        .GetReferenceValueByRefCode("COMPANY_TAG")).Select(x=>x.Value).ToArray();

                    IDataValidation getDropdownValidation(CellRangeAddressList addressList, string[] stringArray, ISheet excelSheet)
                    {
                        IDataValidationHelper validationHelper = new XSSFDataValidationHelper((XSSFSheet)excelSheet);
                        IDataValidationConstraint constraint = validationHelper.CreateExplicitListConstraint(stringArray);
                        IDataValidation dataValidation = validationHelper.CreateValidation(constraint, addressList);
                        dataValidation.SuppressDropDownArrow = true;
                        dataValidation.ShowErrorBox = true;
                        dataValidation.CreateErrorBox("", "Please select from Dropdown values.");
                        return dataValidation;
                    }

                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Employee List");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 2, 2), Company, excelSheet));

                    row.CreateCell(0).SetCellValue("ID");
                    row.CreateCell(1).SetCellValue("Employee Name");
                    row.CreateCell(2).SetCellValue("Company");
                    row.CreateCell(3).SetCellValue("Old Employee ID");
                    row.CreateCell(4).SetCellValue("Email");

                    excelSheet.SetColumnWidth(0, 3000);
                    excelSheet.SetColumnWidth(1, 10000);
                    excelSheet.SetColumnWidth(2, 5500);
                    excelSheet.SetColumnWidth(3, 10000);
                    excelSheet.SetColumnWidth(4, 10000);

                    XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();

                    var colHeaderFont = workbook.CreateFont();
                    colHeaderFont.IsBold = true;
                    colHeaderStyle.SetFont(colHeaderFont);
                    colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                    colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                    colHeaderStyle.Alignment = HorizontalAlignment.Center;
                    alignCenter.Alignment = HorizontalAlignment.Center;

                    row.Cells[0].CellStyle = colHeaderStyle;
                    row.Cells[1].CellStyle = colHeaderStyle;
                    row.Cells[2].CellStyle = colHeaderStyle;
                    row.Cells[3].CellStyle = colHeaderStyle;
                    row.Cells[4].CellStyle = colHeaderStyle;
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(Convert.ToDouble(item.ID));
                        row.CreateCell(1).SetCellValue(item.Name);

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
                        TableName = "Employee",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Draft Employee exported",
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

        public async Task<IActionResult> OnPostUploadEditDraft()
        {
            IFormFile file = Request.Form.Files[0];
            string filePath = Path.Combine(_env.WebRootPath, "\\Employee\\Uploads");
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(filePath, file.FileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;

                    if (sFileExtension == ".xls")//This will read the Excel 97-2000 formats    
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                    else //This will read 2007 Excel format    
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }

                    IRow headerRow = sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;

                    List<NewEmployeeForm> uploadList = new List<NewEmployeeForm>();
                    int blankRows = 0;

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        if (row != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()))
                            {
                                NewEmployeeForm obj = new NewEmployeeForm
                                {
                                    RowNum = (i + 1).ToString(),
                                    ID = int.Parse(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()),
                                    CompanyTag = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    OldEmployeeID = row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Email = row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                };
                                uploadList.Add(obj);
                            }
                        }
                        else
                        {
                            blankRows++;
                            if (blankRows > 3)
                                break;
                        }
                    }

                    var(Result,IsSuccess,Message) = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                        .ConvertNewEmployees(uploadList);

                    if (IsSuccess)
                    {
                        /*Add AuditLog*/
                        await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                            .AddAuditLog(new Security.Transfer.AuditLog.Form
                            {
                                EventType = Common_AuditLog.EventType.UPLOAD.ToString(),
                                TableName = "Employee",
                                TableID = 0, // New Record, no ID yet
                                Remarks = "Employee draft updated",
                                IsSuccess = true,
                                CreatedBy = _globalCurrentUser.UserID
                            });

                        #region Create System Users for the newly uploaded employees
                        var securityURL = string.Concat(_securityBaseURL,
                                         _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("EmployeeUploadInsert").Value, "?",
                                         "userid=", _globalCurrentUser.UserID);

                        var (ResultSystemUsers, IsSuccess1, Message1) =
                            await SharedUtilities.PostFromAPI(new List<EMS.Security.Transfer.SystemUser.EmployeeUploadInsertOutput>(),
                            Result.Select(x => new EmployeeUploadInsertInput
                            {
                                NewEmployeeCode = x.Code,
                                FirstName = x.FirstName,
                                MiddleName = x.MiddleName,
                                LastName = x.LastName,
                                CreatedBy = _globalCurrentUser.UserID
                            }).ToList(), securityURL);
                        #endregion

                        #region Update system user ID on Employee
                        if (IsSuccess1)
                        {
                            var UploadInsertUpdateSystemUser = string.Concat(_plantillaBaseURL,
                                             _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("UploadInsertUpdateSystemUser").Value, "?",
                                             "userid=", _globalCurrentUser.UserID);

                            var (IsSuccess2, Message2) = await SharedUtilities.PostFromAPI(
                                ResultSystemUsers.Select(x => new UploadInsertUpdateSystemUserInput
                                {
                                    SystemUserID = x.SystemUserID,
                                    NewEmployeeCode = x.NewEmployeeCode
                                }), UploadInsertUpdateSystemUser);
                        }
                        #endregion


                        var OrgGroup = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                            .GetOrgGroupByIDs(Result.Select(x => x.OrgGroupID).ToList())).Item1;
                        var Position = (await new Common_Position(_iconfiguration, _globalCurrentUser, _env)
                        .GetAll());

                        var emails = (from left in Result
                                      join rightPosition in Position on left.PositionID equals rightPosition.ID
                                      join rightOrgGroup in OrgGroup on left.OrgGroupID equals rightOrgGroup.ID
                                      select new EmailLogsInput()
                                      {
                                          PositionTitle = String.Concat(rightPosition.Code, " - ", rightPosition.Title),
                                          Status = "DRAFT",
                                          SystemCode = "EMS PLANTILLA",
                                          SenderName = "EMS PLANTILLA",
                                          FromEmailAddress = "noreply@motortrade.com.ph",
                                          Name = string.Concat("(", left.Code, ") ", left.LastName, ", ", left.FirstName, " ", string.IsNullOrEmpty(left.MiddleName) ? "" : left.MiddleName.Substring(0, 1)),
                                          ToEmailAddress = left.Email,
                                          Subject = String.Concat("STATUS: DRAFT | ", string.Concat("(", left.Code, ") ", left.LastName, ", ", left.FirstName, " ", string.IsNullOrEmpty(left.MiddleName) ? "" : left.MiddleName.Substring(0, 1))),
                                          Body = MessageUtilities.EMAIL_BODY_DRAFT_EMPLOYEE
                                                    .Replace("&lt;EmployeeName&gt;", string.Concat(left.LastName, ", ", left.FirstName, " ", string.IsNullOrEmpty(left.MiddleName) ? "" : left.MiddleName.Substring(0, 1)))
                                                    .Replace("&lt;Username&gt;", left.Code),
                                      }).ToList();

                        var (IsSuccessEmail, MessageEmail) = await new Common_EmailServerCredential(_iconfiguration, _globalCurrentUser, _env)
                            .AddMultipleEmailLogs(emails);
                    }

                    _resultView.IsSuccess = IsSuccess;
                    _resultView.Result = MessageUtilities.SCSSMSG_REC_SAVE;

                }
            }

            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetQuestionByCategory(string Category, int EmployeeID)
        {
            var (Result, IsSuccess, Message) = await new Common_Question(_iconfiguration, _globalCurrentUser, _env)
                .GetQuestionEmployeeAnswer(Category, EmployeeID);
            _resultView.Result = Result;
            _resultView.IsSuccess = IsSuccess;
            return new JsonResult(_resultView);
        }
    }
}