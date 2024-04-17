using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.Recruitment.Transfer.Applicant;
using EMS.Recruitment.Transfer.DataDuplication.OrgGroup;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using EMS.Workflow.Transfer.LogActivity;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_PSGC;
using EMS.Recruitment.Transfer.PSGC;
using EMS.FrontEnd.SharedClasses.Common_Manpower;
using EMS.FrontEnd.SharedClasses.Common_Security;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Util;
using NPOI.HSSF.UserModel;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMS.FrontEnd.Pages.Recruitment.Applicant
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/RECRUITMENT/APPLICANT/ADD")).Count() > 0 ? "true" : "false";
                ViewData["HasExportFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/RECRUITMENT/APPLICANT/EXPORT")).Count() > 0 ? "true" : "false";
            
            }

            if (_globalCurrentUser != null)
            {
                ViewData["OrgListFilter"] = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                        .GetReferenceValueByRefCode(ReferenceCodes_Recruitment.ORGLIST_FILTER.ToString())).FirstOrDefault().Value; 
            }

        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] EMS.Recruitment.Transfer.Applicant.GetListInput param)
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

        public async Task<IActionResult> OnGetCheckApplicantExportListAsync([FromQuery] EMS.Recruitment.Transfer.Applicant.GetListInput param)
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

        public async Task<IActionResult> OnGetDownloadApplicantExportListAsync([FromQuery] EMS.Recruitment.Transfer.Applicant.GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataList(param, true);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "Applicant List Export.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Applicant List");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Last Name");
                    row.CreateCell(1).SetCellValue("First Name");
                    row.CreateCell(2).SetCellValue("Middle Name");
                    row.CreateCell(3).SetCellValue("Suffix");
                    row.CreateCell(4).SetCellValue("Date Applied");
                    row.CreateCell(5).SetCellValue("Application Source");
                    row.CreateCell(6).SetCellValue("Referred By");
                    row.CreateCell(7).SetCellValue("MRF ID");
                    row.CreateCell(8).SetCellValue("Application Status");
		            row.CreateCell(9).SetCellValue("Date Scheduled");
                    row.CreateCell(10).SetCellValue("Date Completed");
                    row.CreateCell(11).SetCellValue("Approver Remarks");
                    row.CreateCell(12).SetCellValue("Desired Position (Remarks)");
                    row.CreateCell(13).SetCellValue("Course");
                    row.CreateCell(14).SetCellValue("Current Position Title");
                    row.CreateCell(15).SetCellValue("Expected Salary");
                    row.CreateCell(16).SetCellValue("BirthDate");
                    row.CreateCell(17).SetCellValue("Address Line 1");
                    row.CreateCell(18).SetCellValue("Address Line 2");
                    row.CreateCell(19).SetCellValue("Region");
                    row.CreateCell(20).SetCellValue("City");
                    row.CreateCell(21).SetCellValue("Email Address");
                    row.CreateCell(22).SetCellValue("Cellphone Number");

                    excelSheet.SetColumnWidth(0, 7000);
                    excelSheet.SetColumnWidth(1, 7000);
                    excelSheet.SetColumnWidth(2, 7000);
                    excelSheet.SetColumnWidth(3, 4000);
                    excelSheet.SetColumnWidth(4, 3500);
                    excelSheet.SetColumnWidth(5, 5500);
                    excelSheet.SetColumnWidth(6, 7500);
                    excelSheet.SetColumnWidth(7, 4500);
                    excelSheet.SetColumnWidth(8, 6500);
                    excelSheet.SetColumnWidth(9, 4500);
                    excelSheet.SetColumnWidth(10, 4500);
                    excelSheet.SetColumnWidth(11, 5500);
                    excelSheet.SetColumnWidth(12, 7000);
                    excelSheet.SetColumnWidth(13, 7000);
		            excelSheet.SetColumnWidth(14, 8000);
		            excelSheet.SetColumnWidth(15, 5500);
		            excelSheet.SetColumnWidth(16, 3500);
                    excelSheet.SetColumnWidth(17, 12000);
                    excelSheet.SetColumnWidth(18, 7000);
                    excelSheet.SetColumnWidth(19, 10500);
                    excelSheet.SetColumnWidth(20, 10500);
                    excelSheet.SetColumnWidth(21, 8500);
                    excelSheet.SetColumnWidth(22, 7500);

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
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.LastName);
                        row.CreateCell(1).SetCellValue(item.FirstName);
                        row.CreateCell(2).SetCellValue(item.MiddleName);
                        row.CreateCell(3).SetCellValue(item.Suffix);
                        row.CreateCell(4).SetCellValue(item.DateApplied);
                        row.CreateCell(5).SetCellValue(item.ApplicationSource);
                        row.CreateCell(6).SetCellValue(item.ReferredBy);
                        row.CreateCell(7).SetCellValue(item.MRFTransactionID);
                        row.CreateCell(8).SetCellValue(item.CurrentStep);
                        row.CreateCell(9).SetCellValue(item.DateScheduled);
                        row.CreateCell(10).SetCellValue(item.DateCompleted);
                        row.CreateCell(11).SetCellValue(item.ApproverRemarks);
                        row.CreateCell(12).SetCellValue(item.PositionRemarks);
                        row.CreateCell(13).SetCellValue(item.Course);
                        row.CreateCell(14).SetCellValue(item.CurrentPositionTitle);
                        row.CreateCell(15).SetCellValue(Convert.ToDouble(item.ExpectedSalary));
                        row.CreateCell(16).SetCellValue(item.BirthDate);
                        row.CreateCell(17).SetCellValue(item.AddressLine1);
                        row.CreateCell(18).SetCellValue(item.AddressLine2);
                        row.CreateCell(19).SetCellValue(item.ScopeOrgGroup);
                        row.CreateCell(20).SetCellValue(item.City);
                        row.CreateCell(21).SetCellValue(item.Email);
                        row.CreateCell(22).SetCellValue(item.CellphoneNumber);

                        //row.Cells[1].CellStyle = alignCenter;
                        row.Cells[5].CellStyle = alignCenter;
                        row.Cells[6].CellStyle = alignCenter;
                        row.Cells[7].CellStyle = alignCenter;
                        row.Cells[12].CellStyle = alignCenter;
                        row.Cells[13].CellStyle = alignCenter;

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
                        TableName = "Applicant",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Applicant List exported",
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

        private async Task<(List<EMS.Recruitment.Transfer.Applicant.GetListOutput>, bool, string)> GetDataList([FromQuery] EMS.Recruitment.Transfer.Applicant.GetListInput param, bool IsExport)
        {

            var URL = string.Concat(_recruitmentBaseURL,
                  _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ID=", param.ID, "&",
                  "LastName=", param.LastName, "&",
                  "FirstName=", param.FirstName, "&",
                  "MiddleName=", param.MiddleName, "&",
                  "Suffix=", param.Suffix, "&",
                  "ApplicationSourceDelimited=", param.ApplicationSourceDelimited, "&",
                  "MRFTransactionID=", param.MRFTransactionID, "&",
                  "CurrentStepDelimited=", param.CurrentStepDelimited, "&",
                  "DateScheduledFrom=", param.DateScheduledFrom, "&",
                  "DateScheduledTo=", param.DateScheduledTo, "&",
                  "DateCompletedFrom=", param.DateCompletedFrom, "&",
                  "DateCompletedTo=", param.DateCompletedTo, "&",
                  "ApproverRemarks=", param.ApproverRemarks, "&",
                  //"CurrentStepDelimited=", param.CurrentStepDelimited, "&",
                  //"WorkflowDelimited=", param.WorkflowDelimited, "&",
                  //"OrgGroupRemarks=", param.OrgGroupRemarks, "&",
                  //"OrgGroupDelimited=", param.OrgGroupDelimited, "&",
                  "PositionRemarks=", param.PositionRemarks, "&",
                  //"PositionDelimited=", param.PositionDelimited, "&",
                  "Course=", param.Course, "&",
                  "CurrentPositionTitle=", param.CurrentPositionTitle, "&",
                  "ExpectedSalaryFrom=", param.ExpectedSalaryFrom, "&",
                  "ExpectedSalaryTo=", param.ExpectedSalaryTo, "&",
                  "DateAppliedFrom=", param.DateAppliedFrom, "&",
                  "DateAppliedTo=", param.DateAppliedTo, "&",
                  "ScopeOrgType=", param.ScopeOrgType, "&",
                  "ScopeOrgGroupDelimited=", param.ScopeOrgGroupDelimited, "&",
                  //"WorkflowStatusDelimited=", param.WorkflowStatusDelimited, "&",
                  "IsExport=", IsExport);

            return await SharedUtilities.GetFromAPI(new List<EMS.Recruitment.Transfer.Applicant.GetListOutput>(), URL);
        }

        public async Task<JsonResult> OnGetOrgTypeAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new SharedClasses.Common_Recruitment.Common_Synced_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new SharedClasses.Common_Recruitment.Common_Synced_Position(_iconfiguration, _globalCurrentUser, _env)
                .GetPositionAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
        
        public async Task<JsonResult> OnGetCurrentStepAutoCompleteAsync(GetWorkflowStepAutoCompleteInput param)
        {
            _resultView.Result = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflowStepAutoComplete(param);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
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

        public async Task<JsonResult> OnGetOrgGroupByOrgTypeAutoComplete(GetByOrgTypeAutoCompleteInput param)
        {
            var result = await new SharedClasses.Common_Recruitment.Common_Synced_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupByOrgTypeAutoComplete(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetReferenceValueByCodes(List<string> RefCodes)
        {
            var result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                .GetReferenceValueByRefCodes(RefCodes);

            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        //public async Task<JsonResult> OnGetLogActivities(int ApplicantID)
        //{
        //    List<GetApplicantLogActivityByApplicantIDOutput> activities =
        //           await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
        //           .GetApplicantLogActivityByApplicantID(ApplicantID);

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
        //                .Select(x => new GetApplicantLogActivityByApplicantIDOutput
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
        //                .Select(x => new GetApplicantLogActivityByApplicantIDOutput
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
        //                .Select(x => new GetApplicantLogActivityByApplicantIDOutput
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

        public async Task<IActionResult> OnGetLogActivities([FromQuery] GetApplicantLogActivityListInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetApplicantLogActivityList").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ApplicantID=", param.ApplicantID, "&",
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

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetApplicantLogActivityListOutput>(), URL);

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
                       .Select(x => new GetApplicantLogActivityListOutput
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
                           ApplicantID = x.activities.activities.activities.activities.ApplicantID,
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
            List<GetApplicantLogActivityStatusHistoryOutput> statusHistory =
                   await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env)
                   .GetApplicantLogActivityStatusHistory(ID);

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
                        .Select(x => new GetApplicantLogActivityStatusHistoryOutput
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

        public async Task<JsonResult> OnGetApplicantInfo(int ID)
        {
            _resultView.Result = await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env).GetApplicant(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        //public async Task<JsonResult> OnGetCityDropDownByRegion(int RegionID)
        //{
        //    _resultView.Result = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env).GetCityDropdownByRegion(RegionID);
        //    _resultView.IsSuccess = true;
        //    return new JsonResult(_resultView);
        //}

        public async Task<JsonResult> OnGetRegionAutoComplete(GetRegionAutoCompleteInput param)
        {
            var result = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env)
                .GetRegionAutoComplete(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostAddPreloadedActivities(int ApplicantID, int LogActivityPreloadedID)
        {
            var URL = string.Concat(_workflowBaseURL,
                 _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("AddApplicantPreLoadedActivities").Value, "?",
                 "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(new AddApplicantPreLoadedActivitiesInput
            { 
                ApplicantID =  ApplicantID,
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

        public async Task<JsonResult> OnGetMRFIDDropdown(int ApplicantID)
        {
            _resultView.Result = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env)
                .GetMRFIDDropdownByApplicantID(ApplicantID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetWorkflowTransaction(int WorkflowID, int MRFApplicantID)
        {
            AddWorkflowTransaction result = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetTransactionByRecordID(new GetTransactionByRecordIDInput { WorkflowID = WorkflowID, RecordID = MRFApplicantID });

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

        public async Task<IActionResult> OnGetDownloadApplicantTemplate()
        {
            string sWebRootFolder = _env.WebRootPath;
            string sFileName = "Applicant.xlsx";
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Applicant");

                int irow = 0;

                #region Column Headers
                IRow row = excelSheet.CreateRow(irow); irow++;

                
                var ApplicationSourceArray = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                        .GetReferenceValueByRefCode(ReferenceCodes_Recruitment.APPLICATION_SOURCE.ToString()))
                        .OrderBy(y => y.Value).Select(x => x.Value).ToArray();

                var CourseArray = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                .GetReferenceValueByRefCode(ReferenceCodes_Recruitment.COURSE.ToString())).OrderBy(y => y.Value).Select(x => x.Value).ToArray();

                var regionPSGCList = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env).GetAllRegion();
                var provincePSGCList = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env).GetAllProvince();
                var cityMunicipalityPSGCList = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env).GetAllCityMunicipality();
                var barangayPSGCList = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env).GetAllBarangay();

                //var regionPSGCArray = regionPSGCList.OrderBy(y => y.Code).Select(x => x.Code).ToArray();

                IDataValidation getDropdownValidation(CellRangeAddressList addressList, string[] stringArray) {
                    IDataValidationHelper validationHelper = new XSSFDataValidationHelper((XSSFSheet)excelSheet);
                    IDataValidationConstraint constraint = validationHelper.CreateExplicitListConstraint(stringArray);
                    IDataValidation dataValidation = validationHelper.CreateValidation(constraint, addressList);
                    dataValidation.SuppressDropDownArrow = true;
                    dataValidation.ShowErrorBox = true;
                    dataValidation.CreateErrorBox("", "Please select from Dropdown values.");
                    return dataValidation;
                }
                
                IDataValidation getDateValidation(CellRangeAddressList addressList) {
                    IDataValidationHelper validationHelper = new XSSFDataValidationHelper((XSSFSheet)excelSheet);
                    IDataValidationConstraint constraint = validationHelper.CreateDateConstraint(OperatorType.LESS_OR_EQUAL, "=TODAY()", "", "");
                    IDataValidation dataValidation = validationHelper.CreateValidation(constraint, addressList);
                    dataValidation.SuppressDropDownArrow = true;
                    dataValidation.ShowErrorBox = true;
                    dataValidation.CreateErrorBox("", "Cell Format must be 'Text'. Must not be greater than date today.");
                    return dataValidation;
                }

                //excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 2, 2), ApplicationSourceArray));
                //excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 11, 11), regionPSGCArray));
                //excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 14, 14), CourseArray));
                //excelSheet.AddValidationData(getDateValidation(new CellRangeAddressList(1, 50, 1, 1)));
                //excelSheet.AddValidationData(getDateValidation(new CellRangeAddressList(1, 50, 8, 8)));


                row.CreateCell(0).SetCellValue("* Desired Position (remarks)");
                row.CreateCell(1).SetCellValue("* Date Applied (MM/DD/YYYY)");
                row.CreateCell(2).SetCellValue("* Application Source");
                row.CreateCell(3).SetCellValue("Expected Salary");
                row.CreateCell(4).SetCellValue("* Last Name");
                row.CreateCell(5).SetCellValue("* First Name");
                row.CreateCell(6).SetCellValue("* Middle Name");
                row.CreateCell(7).SetCellValue("Suffix");
                row.CreateCell(8).SetCellValue("* Birth Date (MM/DD/YYYY)");
                row.CreateCell(9).SetCellValue("* Address Line 1");
                row.CreateCell(10).SetCellValue("Address Line 2");
                row.CreateCell(11).SetCellValue("* Region (PSGC)");
                row.CreateCell(12).SetCellValue("* Province (PSGC)");
                row.CreateCell(13).SetCellValue("* City / Municipality (PSGC)");
                row.CreateCell(14).SetCellValue("* Barangay (PSGC)");
                row.CreateCell(15).SetCellValue("* Current Position");
                row.CreateCell(16).SetCellValue("* Course");
                row.CreateCell(17).SetCellValue("* Email Address");
                row.CreateCell(18).SetCellValue("* Cellphone Number (09xxxxxxxxx)");
                row.CreateCell(19).SetCellValue("Referred By (Employee Code)");


                excelSheet.SetColumnWidth(0, 8000);
                excelSheet.SetColumnWidth(1, 8000);
                excelSheet.SetColumnWidth(2, 5000);
                excelSheet.SetColumnWidth(3, 5000);
                excelSheet.SetColumnWidth(4, 5000);
                excelSheet.SetColumnWidth(5, 5000);
                excelSheet.SetColumnWidth(6, 5000);
                excelSheet.SetColumnWidth(7, 5000);
                excelSheet.SetColumnWidth(8, 8000);
                excelSheet.SetColumnWidth(9, 5000);
                excelSheet.SetColumnWidth(10, 5000);
                excelSheet.SetColumnWidth(11, 5000);
                excelSheet.SetColumnWidth(12, 5000);
                excelSheet.SetColumnWidth(13, 5000);
                excelSheet.SetColumnWidth(14, 5000);
                excelSheet.SetColumnWidth(15, 5000);
                excelSheet.SetColumnWidth(16, 5000);
                excelSheet.SetColumnWidth(17, 5000);
                excelSheet.SetColumnWidth(18, 10000);
                excelSheet.SetColumnWidth(19, 8000);
                excelSheet.SetColumnWidth(20, 10000); // Legend

                XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();
                XSSFCellStyle legendStyle = (XSSFCellStyle)workbook.CreateCellStyle();

                string legendsMessage = string.Concat("Legends:", Environment.NewLine
                            , "*       - Required, Case Sensitive.", Environment.NewLine
                            , "**     - see Reference Lookup Worksheet, Required, Case Sensitive.");

                var format = workbook.CreateDataFormat().GetFormat("text");
                var colHeaderFont = workbook.CreateFont();
                var legendFont = workbook.CreateFont();
                legendFont.IsBold = true;
                legendFont.IsItalic = true;
                colHeaderFont.IsBold = true;
                colHeaderStyle.SetFont(colHeaderFont);
                colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                colHeaderStyle.Alignment = HorizontalAlignment.Center;
                colHeaderStyle.DataFormat = format;
                legendStyle.SetFont(legendFont);
                legendStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                legendStyle.FillPattern = FillPattern.SolidForeground;
                legendStyle.Alignment = HorizontalAlignment.Left;
                legendStyle.DataFormat = format;
                legendStyle.WrapText = true;
                alignCenter.Alignment = HorizontalAlignment.Center;

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
                #endregion

                for (int x = irow; x < 100; x++)
                {
                    IRow rowDate = excelSheet.CreateRow(x);
                    XSSFCellStyle dateCS = (XSSFCellStyle)workbook.CreateCellStyle();
                    var dateFormat = workbook.CreateDataFormat().GetFormat("text");
                    dateCS.DataFormat = dateFormat;
                    rowDate.CreateCell(0, CellType.String).SetCellValue("");
                    rowDate.CreateCell(1, CellType.String).SetCellValue("");
                    rowDate.CreateCell(2, CellType.String).SetCellValue("");
                    rowDate.CreateCell(3, CellType.String).SetCellValue("");
                    rowDate.CreateCell(4, CellType.String).SetCellValue("");
                    rowDate.CreateCell(5, CellType.String).SetCellValue("");
                    rowDate.CreateCell(6, CellType.String).SetCellValue("");
                    rowDate.CreateCell(7, CellType.String).SetCellValue("");
                    rowDate.CreateCell(8, CellType.String).SetCellValue("");
                    rowDate.CreateCell(9, CellType.String).SetCellValue("");
                    rowDate.CreateCell(10, CellType.String).SetCellValue("");
                    rowDate.CreateCell(11, CellType.String).SetCellValue("");
                    rowDate.CreateCell(12, CellType.String).SetCellValue("");
                    rowDate.CreateCell(13, CellType.String).SetCellValue("");
                    rowDate.CreateCell(14, CellType.String).SetCellValue("");
                    rowDate.CreateCell(15, CellType.String).SetCellValue("");
                    rowDate.CreateCell(16, CellType.String).SetCellValue("");
                    rowDate.CreateCell(17, CellType.String).SetCellValue("");
                    rowDate.CreateCell(18, CellType.String).SetCellValue("");
                    rowDate.CreateCell(19, CellType.String).SetCellValue("");

                    rowDate.Cells[0].CellStyle = dateCS;
                    rowDate.Cells[1].CellStyle = dateCS;
                    rowDate.Cells[2].CellStyle = dateCS;
                    rowDate.Cells[3].CellStyle = dateCS;
                    rowDate.Cells[4].CellStyle = dateCS;
                    rowDate.Cells[5].CellStyle = dateCS;
                    rowDate.Cells[6].CellStyle = dateCS;
                    rowDate.Cells[7].CellStyle = dateCS;
                    rowDate.Cells[8].CellStyle = dateCS;
                    rowDate.Cells[9].CellStyle = dateCS;
                    rowDate.Cells[10].CellStyle = dateCS;
                    rowDate.Cells[11].CellStyle = dateCS;
                    rowDate.Cells[12].CellStyle = dateCS;
                    rowDate.Cells[13].CellStyle = dateCS;
                    rowDate.Cells[14].CellStyle = dateCS;
                    rowDate.Cells[15].CellStyle = dateCS;
                    rowDate.Cells[16].CellStyle = dateCS;
                    rowDate.Cells[17].CellStyle = dateCS;
                    rowDate.Cells[18].CellStyle = dateCS;
                    rowDate.Cells[19].CellStyle = dateCS;
                }

                #region PSGC Lookup
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

                #endregion




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
            string filePath = Path.Combine(_env.WebRootPath, "\\Recruitment\\Uploads");
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
                                    DesiredPosition = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    DateApplied = row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    ApplicationSource = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    ExpectedSalary = row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    LastName = row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    FirstName = row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    MiddleName = row.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Suffix = row.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    BirthDate = row.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    AddressLine1 = row.GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    AddressLine2 = row.GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    PSGCRegionCode = row.GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    PSGCProvinceCode = row.GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    PSGCCityMunicipalityCode = row.GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    PSGCBarangayCode = row.GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    CurrentPosition = row.GetCell(15, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Course = row.GetCell(16, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    EmailAddress = row.GetCell(17, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    CellphoneNumber = row.GetCell(18, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    ReferredByCode = row.GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),

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

                    var ReferredByCodesDelimited = string.Join(",",uploadList.Select(x => x.ReferredByCode).Distinct());
                    var ReferredByEmployees = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                        .GetByCodes(ReferredByCodesDelimited);

                    uploadList = uploadList.GroupJoin(ReferredByEmployees,
                        x => new { x.ReferredByCode },
                        y => new { ReferredByCode = y.Code },
                        (x, y) => (x, y))
                        .SelectMany(x => x.y.DefaultIfEmpty(),
                        (x, y) => new { x, y })
                        .Select(x => new UploadFile { 
                            RowNum = x.x.x.RowNum,
                            DesiredPosition = x.x.x.DesiredPosition,
                            DateApplied = x.x.x.DateApplied,
                            ApplicationSource = x.x.x.ApplicationSource,
                            ExpectedSalary = x.x.x.ExpectedSalary,
                            LastName = x.x.x.LastName,
                            FirstName = x.x.x.FirstName,
                            MiddleName = x.x.x.MiddleName,
                            Suffix = x.x.x.Suffix,
                            BirthDate = x.x.x.BirthDate,
                            AddressLine1 = x.x.x.AddressLine1,
                            AddressLine2 = x.x.x.AddressLine2,
                            PSGCRegionCode = x.x.x.PSGCRegionCode,
                            PSGCProvinceCode = x.x.x.PSGCProvinceCode,
                            PSGCCityMunicipalityCode = x.x.x.PSGCCityMunicipalityCode,
                            PSGCBarangayCode = x.x.x.PSGCBarangayCode,
                            CurrentPosition = x.x.x.CurrentPosition,
                            Course = x.x.x.Course,
                            EmailAddress = x.x.x.EmailAddress,
                            CellphoneNumber = x.x.x.CellphoneNumber,
                            ReferredByCode = x.x.x.ReferredByCode,
                            ReferredByID = string.IsNullOrEmpty(x.x.x.ReferredByCode) ? default(int?) : x.y == null ? 0 : x.y.ID
                        } ).ToList();

                    var URL = string.Concat(_recruitmentBaseURL,
                               _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("UploadInsert").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

                    var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(uploadList, URL);
                    _resultView.IsSuccess = IsSuccess;
                    _resultView.Result = Message;
                }
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetWorkflowReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetAssignedUser(string Term, int TopResults)
        {
            //var result = await new Common_Synced_SystemUser(_iconfiguration, _globalCurrentUser, _env).GetSystemUserAutoComplete(Term, TopResults);
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployeeWithSystemUserAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetMRFIDByMRFTransactionID(string MRFTransactionID)
        {
            _resultView.Result = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env).GetByMRFTransactionID(MRFTransactionID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetProvinceDropDownByRegion(string Code)
        {
            var res = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env).GetProvinceDropdownByRegion(Code);

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
            var res = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env).GetCityMunicipalityDropdownByProvince(Code);

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
            var res = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env).GetBarangayDropdownByCityMunicipality(Code);

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
        public async Task<JsonResult> OnGetApplicantLegalProfile(int ApplicantId)
        {
            _resultView.Result = await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env).GetApplicantLegalProfile(ApplicantId);

            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }
    }
}