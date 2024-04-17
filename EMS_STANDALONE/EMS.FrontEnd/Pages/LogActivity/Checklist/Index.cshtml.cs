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
using NPOI.SS.Formula.Atp;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;
using System;

namespace EMS.FrontEnd.Pages.LogActivity.Checklist
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnGetList([FromQuery] GetChecklistListInput param)
        {

            var (Result, IsSuccess, ErrorMessage) = await GetExportData(param, false);

            // Get Employee description by System User IDs
            List<EMS.Plantilla.Transfer.Employee.Form> assignedBy =
                (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeByUserIDs(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy)
                 .Distinct().ToList())).Item1;

            // Get Employee Names by Assigned User ID
            List<EMS.Plantilla.Transfer.Employee.Form> assignedTo =
               (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeByUserIDs(Result.Where(x => x.AssignedUserID != 0).Select(x => x.AssignedUserID)
                 .Distinct().ToList())).Item1;

            if (assignedBy.Count > 0)
            {

                Result = Result
                       .GroupJoin(assignedBy,
                       x => new { x.CreatedBy },
                       y => new { CreatedBy = y.SystemUserID },
                       (x, y) => new { activities = x, employees = y })
                       .SelectMany(x => x.employees.DefaultIfEmpty(),
                       (x, y) => new { activities = x, employees = y })
                       .Select(x => new GetChecklistListOutput
                       {
                           
                           ID = x.activities.activities.ID,
                           Title = x.activities.activities.Title,
                           Type = x.activities.activities.Type,
                           SubType = x.activities.activities.SubType,
                           Description = x.activities.activities.Description,
                           CurrentStatus = x.activities.activities.CurrentStatus,
                           CurrentTimestamp = x.activities.activities.CurrentTimestamp,
                           AssignedBy = x.employees == null ? "" : x.employees.PersonalInformation == null ? "" :
                                string.Concat((x.employees.PersonalInformation.LastName ?? "").Trim(),
                                     string.IsNullOrEmpty((x.employees.PersonalInformation.FirstName ?? "").Trim()) ? ""
                                         : string.Concat(", ", x.employees.PersonalInformation.FirstName),
                                     string.IsNullOrEmpty((x.employees.PersonalInformation.MiddleName ?? "").Trim()) ? ""
                                         : string.Concat(" ", x.employees.PersonalInformation.MiddleName), " (", x.employees.Code, ") "),
                           NoOfPages = x.activities.activities.NoOfPages,
                           Row = x.activities.activities.Row,
                           TotalNoOfRecord = x.activities.activities.TotalNoOfRecord,
                           IsPass = x.activities.activities.IsPass,
                           CreatedBy = x.activities.activities.CreatedBy,
                           EmployeeID = x.activities.activities.EmployeeID,
                           Remarks = x.activities.activities.Remarks,
                           DueDate = x.activities.activities.DueDate,
                           AssignedUserID = x.activities.activities.AssignedUserID
                       }).ToList();
            }

            if (assignedTo.Count > 0)
            {

                Result = Result
                       .GroupJoin(assignedTo,
                       x => new { x.AssignedUserID },
                       y => new { AssignedUserID = y.SystemUserID },
                       (x, y) => new { activities = x, employees = y })
                       .SelectMany(x => x.employees.DefaultIfEmpty(),
                       (x, y) => new { activities = x, employees = y })
                       .Select(x => new GetChecklistListOutput
                       {

                           ID = x.activities.activities.ID,
                           Title = x.activities.activities.Title,
                           Type = x.activities.activities.Type,
                           SubType = x.activities.activities.SubType,
                           Description = x.activities.activities.Description,
                           CurrentStatus = x.activities.activities.CurrentStatus,
                           CurrentTimestamp = x.activities.activities.CurrentTimestamp,
                           AssignedBy = x.activities.activities.AssignedBy,
                           NoOfPages = x.activities.activities.NoOfPages,
                           Row = x.activities.activities.Row,
                           TotalNoOfRecord = x.activities.activities.TotalNoOfRecord,
                           IsPass = x.activities.activities.IsPass,
                           CreatedBy = x.activities.activities.CreatedBy,
                           EmployeeID = x.activities.activities.EmployeeID,
                           Remarks = x.activities.activities.Remarks,
                           DueDate = x.activities.activities.DueDate,
                           AssignedUserID = x.activities.activities.AssignedUserID,
                           AssignedTo = x.employees == null ? "" : x.employees.PersonalInformation == null ? "" :
                                string.Concat((x.employees.PersonalInformation.LastName ?? "").Trim(),
                                     string.IsNullOrEmpty((x.employees.PersonalInformation.FirstName ?? "").Trim()) ? ""
                                         : string.Concat(", ", x.employees.PersonalInformation.FirstName),
                                     string.IsNullOrEmpty((x.employees.PersonalInformation.MiddleName ?? "").Trim()) ? ""
                                         : string.Concat(" ", x.employees.PersonalInformation.MiddleName), " (", x.employees.Code, ") ")
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

        public async Task<(List<GetChecklistListOutput>, bool, string)> GetExportData([FromQuery] GetChecklistListInput param, bool IsExport)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetChecklistList").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "EmployeeID=", _globalCurrentUser.EmployeeID, "&",
                  "TypeDelimited=", param.TypeDelimited, "&",
                  "SubTypeDelimited=", param.SubTypeDelimited, "&",
                  "Title=", param.Title, "&",
                  "Description=", param.Description, "&",
                  "CurrentStatusDelimited=", param.CurrentStatusDelimited, "&",
                  "AssignedByDelimited=", param.AssignedByDelimited, "&",
                  "CurrentTimestampFrom=", param.CurrentTimestampFrom, "&",
                  "CurrentTimestampTo=", param.CurrentTimestampTo, "&",
                  "AssignedToDelimited=", param.AssignedToDelimited, "&",
                  "Remarks=", param.Remarks, "&",
                  "IsExport=", IsExport);

            return await SharedUtilities.GetFromAPI(new List<GetChecklistListOutput>(), URL);

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

        public async Task<JsonResult> OnGetApplicantLogActivityStatusHistory(int ID)
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
        
        public async Task<JsonResult> OnGetEmployeeLogActivityStatusHistory(int ID)
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

        public async Task<JsonResult> OnGetReferredBy(string Term, int TopResults)
        {
            //var result = await new Common_Synced_SystemUser(_iconfiguration, _globalCurrentUser, _env).GetSystemUserAutoComplete(Term, TopResults);
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployeeWithSystemUserAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetCheckExportList([FromQuery] GetChecklistListInput param)
        {

            var (Result, IsSuccess, ErrorMessage) = await GetExportData(param, true);

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

        public async Task<IActionResult> OnGetDownloadExportList([FromQuery] GetChecklistListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetExportData(param, true);

            // Get Employee description by System User IDs
            List<EMS.Plantilla.Transfer.Employee.Form> assignedBy =
                (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeByUserIDs(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy)
                 .Distinct().ToList())).Item1;

            // Get Employee Names by Assigned User ID
            List<EMS.Plantilla.Transfer.Employee.Form> assignedTo =
               (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeByUserIDs(Result.Where(x => x.AssignedUserID != 0).Select(x => x.AssignedUserID)
                 .Distinct().ToList())).Item1;

            if (assignedBy.Count > 0)
            {

                Result = Result
                       .GroupJoin(assignedBy,
                       x => new { x.CreatedBy },
                       y => new { CreatedBy = y.SystemUserID },
                       (x, y) => new { activities = x, employees = y })
                       .SelectMany(x => x.employees.DefaultIfEmpty(),
                       (x, y) => new { activities = x, employees = y })
                       .Select(x => new GetChecklistListOutput
                       {

                           ID = x.activities.activities.ID,
                           Title = x.activities.activities.Title,
                           Type = x.activities.activities.Type,
                           SubType = x.activities.activities.SubType,
                           Description = x.activities.activities.Description,
                           CurrentStatus = x.activities.activities.CurrentStatus,
                           CurrentTimestamp = x.activities.activities.CurrentTimestamp,
                           AssignedBy = x.employees == null ? "" : x.employees.PersonalInformation == null ? "" :
                                string.Concat((x.employees.PersonalInformation.LastName ?? "").Trim(),
                                     string.IsNullOrEmpty((x.employees.PersonalInformation.FirstName ?? "").Trim()) ? ""
                                         : string.Concat(", ", x.employees.PersonalInformation.FirstName),
                                     string.IsNullOrEmpty((x.employees.PersonalInformation.MiddleName ?? "").Trim()) ? ""
                                         : string.Concat(" ", x.employees.PersonalInformation.MiddleName), " (", x.employees.Code, ") "),
                           NoOfPages = x.activities.activities.NoOfPages,
                           Row = x.activities.activities.Row,
                           TotalNoOfRecord = x.activities.activities.TotalNoOfRecord,
                           IsPass = x.activities.activities.IsPass,
                           CreatedBy = x.activities.activities.CreatedBy,
                           EmployeeID = x.activities.activities.EmployeeID,
                           Remarks = x.activities.activities.Remarks,
                           DueDate = x.activities.activities.DueDate,
                           AssignedUserID = x.activities.activities.AssignedUserID
                       }).ToList();
            }

            if (assignedTo.Count > 0)
            {

                Result = Result
                       .GroupJoin(assignedTo,
                       x => new { x.AssignedUserID },
                       y => new { AssignedUserID = y.SystemUserID },
                       (x, y) => new { activities = x, employees = y })
                       .SelectMany(x => x.employees.DefaultIfEmpty(),
                       (x, y) => new { activities = x, employees = y })
                       .Select(x => new GetChecklistListOutput
                       {

                           ID = x.activities.activities.ID,
                           Title = x.activities.activities.Title,
                           Type = x.activities.activities.Type,
                           SubType = x.activities.activities.SubType,
                           Description = x.activities.activities.Description,
                           CurrentStatus = x.activities.activities.CurrentStatus,
                           CurrentTimestamp = x.activities.activities.CurrentTimestamp,
                           AssignedBy = x.activities.activities.AssignedBy,
                           NoOfPages = x.activities.activities.NoOfPages,
                           Row = x.activities.activities.Row,
                           TotalNoOfRecord = x.activities.activities.TotalNoOfRecord,
                           IsPass = x.activities.activities.IsPass,
                           CreatedBy = x.activities.activities.CreatedBy,
                           EmployeeID = x.activities.activities.EmployeeID,
                           Remarks = x.activities.activities.Remarks,
                           DueDate = x.activities.activities.DueDate,
                           AssignedUserID = x.activities.activities.AssignedUserID,
                           AssignedTo = x.employees == null ? "" : x.employees.PersonalInformation == null ? "" :
                                string.Concat((x.employees.PersonalInformation.LastName ?? "").Trim(),
                                     string.IsNullOrEmpty((x.employees.PersonalInformation.FirstName ?? "").Trim()) ? ""
                                         : string.Concat(", ", x.employees.PersonalInformation.FirstName),
                                     string.IsNullOrEmpty((x.employees.PersonalInformation.MiddleName ?? "").Trim()) ? ""
                                         : string.Concat(" ", x.employees.PersonalInformation.MiddleName), " (", x.employees.Code, ") ")
                       }).ToList();
            }

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "Employee Checklist.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Employee Checklist");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("ID");
                    row.CreateCell(1).SetCellValue("Type");
                    row.CreateCell(2).SetCellValue("Title");
                    row.CreateCell(3).SetCellValue("Description");
                    row.CreateCell(4).SetCellValue("Assigned By");
                    row.CreateCell(5).SetCellValue("Assigned To");
                    row.CreateCell(6).SetCellValue("Due Date");
                    row.CreateCell(7).SetCellValue("Current Status");
                    row.CreateCell(8).SetCellValue("Timestamp");
                    row.CreateCell(9).SetCellValue("Remarks");

                    excelSheet.SetColumnWidth(0, 3000);
                    excelSheet.SetColumnWidth(1, 7000);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 10000);
                    excelSheet.SetColumnWidth(4, 10000);
                    excelSheet.SetColumnWidth(5, 10000);
                    excelSheet.SetColumnWidth(6, 4000);
                    excelSheet.SetColumnWidth(7, 4500);
                    excelSheet.SetColumnWidth(8, 6500);
                    excelSheet.SetColumnWidth(9, 10000);

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
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(Convert.ToInt32(item.ID));
                        row.CreateCell(1).SetCellValue(item.Type);
                        row.CreateCell(2).SetCellValue(item.Title);
                        row.CreateCell(3).SetCellValue(item.Description);
                        row.CreateCell(4).SetCellValue(item.AssignedBy);
                        row.CreateCell(5).SetCellValue(item.AssignedTo);
                        row.CreateCell(6).SetCellValue(item.DueDate);
                        row.CreateCell(7).SetCellValue(item.CurrentStatus);
                        row.CreateCell(8).SetCellValue(item.CurrentTimestamp);
                        row.CreateCell(9).SetCellValue(item.Remarks);

                        row.Cells[0].CellStyle = alignCenter;
                        row.Cells[6].CellStyle = alignCenter;
                        row.Cells[8].CellStyle = alignCenter;

                        row.Cells[2].CellStyle = wrapText;
                        row.Cells[3].CellStyle = wrapText;
                        row.Cells[9].CellStyle = wrapText;

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
                        TableName = "EmployeeLogActivity",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Employee Checklist Exported",
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

    }
}