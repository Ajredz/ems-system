using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Security.Transfer.SystemErrorLog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Utilities.API;
using EMS.FrontEnd.SharedClasses.Common_Security;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;

namespace EMS.FrontEnd.Pages.Administrator.SystemErrorLog
{
    public class IndexModel : SharedClasses.Utilities
    {

        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetExportData(param, false);

            if (IsSuccess)
            {
                // Get Employee description by System User IDs
                List<EMS.Plantilla.Transfer.Employee.Form> systemUsers =
                    (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeByUserIDs(Result.Where(x => x.UserID != 0).Select(x => x.UserID)
                     .Distinct().ToList())).Item1;

                if (systemUsers.Count > 0)
                {
                    Result = Result
                      .GroupJoin(systemUsers,
                      x => new { x.UserID },
                      y => new { UserID = y.SystemUserID },
                      (x, y) => new { activities = x, employees = y })
                      .SelectMany(x => x.employees.DefaultIfEmpty(),
                      (x, y) => new { activities = x, employees = y })
                      .Select(x => new GetListOutput
                      {
                          ID = x.activities.activities.ID,
                          Class = x.activities.activities.Class,
                          ErrorMessage = x.activities.activities.ErrorMessage,
                          CreatedDate = x.activities.activities.CreatedDate,
                          User = x.employees == null ? "" : x.employees.PersonalInformation == null ? "" :
                               string.Concat((x.employees.PersonalInformation.LastName ?? "").Trim(),
                                    string.IsNullOrEmpty((x.employees.PersonalInformation.FirstName ?? "").Trim()) ? ""
                                        : string.Concat(", ", x.employees.PersonalInformation.FirstName),
                                    string.IsNullOrEmpty((x.employees.PersonalInformation.MiddleName ?? "").Trim()) ? ""
                                        : string.Concat(" ", x.employees.PersonalInformation.MiddleName), " (", x.employees.Code, ") "),
                          UserID = x.activities.activities.NoOfPages,
                          Row = x.activities.activities.Row,
                          TotalNoOfRecord = x.activities.activities.TotalNoOfRecord,
                          NoOfPages = x.activities.activities.NoOfPages
                      }).ToList();
                }

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

        public async Task<(List<GetListOutput>, bool, string)> GetExportData([FromQuery] GetListInput param, bool IsExport)
        {
            var Database =
                (param.ReportType ?? "").Equals("FRONT_END") ?
                    string.Concat(_securityBaseURL,
                  _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemErrorLog").GetSection("List").Value, "?") :
                (param.ReportType ?? "").Equals("PLANTILLA") ?
                    string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("SystemErrorLog").GetSection("List").Value, "?") :
                (param.ReportType ?? "").Equals("RECRUITMENT") ?
                    string.Concat(_recruitmentBaseURL,
                  _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("SystemErrorLog").GetSection("List").Value, "?") :
                (param.ReportType ?? "").Equals("MANPOWER") ?
                    string.Concat(_manpowerBaseURL,
                  _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("SystemErrorLog").GetSection("List").Value, "?") :
                (param.ReportType ?? "").Equals("IPM") ?
                    string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("SystemErrorLog").GetSection("List").Value, "?") :
                  string.Concat(_securityBaseURL,
                  _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemErrorLog").GetSection("List").Value, "?");

            var URL = string.Concat(
                Database,
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ID=", param.ID, "&",
                  "Class=", param.Class, "&",
                  "UserIDDelimited=", param.UserIDDelimited, "&",
                  "ErrorMessage=", param.ErrorMessage, "&",
                  "DateCreatedFrom=", param.DateCreatedFrom, "&",
                  "DateCreatedTo=", param.DateCreatedTo, "&",
                  "IsExport=", IsExport);

            return await SharedUtilities.GetFromAPI(new List<GetListOutput>(), URL);
        }

        public async Task<IActionResult> OnGetCheckExportList([FromQuery] GetListInput param)
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

        public async Task<IActionResult> OnGetDownloadExportList([FromQuery] GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetExportData(param, true);

            if (IsSuccess)
            {
                // Get Employee description by System User IDs
                List<EMS.Plantilla.Transfer.Employee.Form> systemUsers =
                    (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeByUserIDs(Result.Where(x => x.UserID != 0).Select(x => x.UserID)
                     .Distinct().ToList())).Item1;

                if (systemUsers.Count > 0)
                {
                    Result = Result
                      .GroupJoin(systemUsers,
                      x => new { x.UserID },
                      y => new { UserID = y.SystemUserID },
                      (x, y) => new { activities = x, employees = y })
                      .SelectMany(x => x.employees.DefaultIfEmpty(),
                      (x, y) => new { activities = x, employees = y })
                      .Select(x => new GetListOutput
                      {
                          ID = x.activities.activities.ID,
                          Class = x.activities.activities.Class,
                          ErrorMessage = x.activities.activities.ErrorMessage,
                          CreatedDate = x.activities.activities.CreatedDate,
                          User = x.employees == null ? "" : x.employees.PersonalInformation == null ? "" :
                               string.Concat((x.employees.PersonalInformation.LastName ?? "").Trim(),
                                    string.IsNullOrEmpty((x.employees.PersonalInformation.FirstName ?? "").Trim()) ? ""
                                        : string.Concat(", ", x.employees.PersonalInformation.FirstName),
                                    string.IsNullOrEmpty((x.employees.PersonalInformation.MiddleName ?? "").Trim()) ? ""
                                        : string.Concat(" ", x.employees.PersonalInformation.MiddleName), " (", x.employees.Code, ") "),
                          UserID = x.activities.activities.NoOfPages,
                          Row = x.activities.activities.Row,
                          TotalNoOfRecord = x.activities.activities.TotalNoOfRecord,
                          NoOfPages = x.activities.activities.NoOfPages
                      }).ToList();
                }

                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "System User.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("System User");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("ID");
                    row.CreateCell(1).SetCellValue("Class");
                    row.CreateCell(2).SetCellValue("Error Message");
                    row.CreateCell(3).SetCellValue("User");
                    row.CreateCell(4).SetCellValue("Created Date");

                    excelSheet.SetColumnWidth(0, 1000);
                    excelSheet.SetColumnWidth(1, 3000);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 5000);
                    excelSheet.SetColumnWidth(4, 5000);

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
                        row.CreateCell(0).SetCellValue(item.ID + "");
                        row.CreateCell(1).SetCellValue(item.Class);
                        row.CreateCell(2).SetCellValue(item.ErrorMessage);
                        row.CreateCell(3).SetCellValue(item.User);
                        row.CreateCell(4).SetCellValue(item.CreatedDate);

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
                        TableName = "SystemErrorLog",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Error Log Exported",
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

        public async Task<JsonResult> OnGetUserAutoComplete(string Term, int TopResults)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeWithSystemUserAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

    }
}