using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.Plantilla.Transfer.Dashboard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Plantilla.Dashboard
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync()
        {
            var reportType = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCode(
                      EMS.Plantilla.Transfer.Enums.ReferenceCodes.REPORT_TYPE.ToString());

            ViewData["ReportTypeSelectList"] = reportType.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description
                }).ToList();
        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] GetEmployeeDashboardInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataList(param, false);

            if (IsSuccess)
            {
                int totalResult = 0;
                long recordResult = 0;

                if (param.ReportType == ReferenceCodes_ReportType.PRO_EMP_PER_GROUP.ToString())
                {
                    totalResult = Result.Report1Output.Count > 0 ? Result.NoOfPages : 0;
                    recordResult = Result.Report1Output.Count > 0 ? Result.TotalNoOfRecord : 0;
                }
                else if (param.ReportType == ReferenceCodes_ReportType.PRO_EMP_PER_POSITION.ToString())
                {
                    totalResult = Result.Report2Output.Count > 0 ? Result.NoOfPages : 0;
                    recordResult = Result.Report2Output.Count > 0 ? Result.TotalNoOfRecord : 0;
                }
                else if (param.ReportType == ReferenceCodes_ReportType.PRO_STATUS_BEYOND_6.ToString())
                {
                    totalResult = Result.Report3Output.Count > 0 ? Result.NoOfPages : 0;
                    recordResult = Result.Report3Output.Count > 0 ? Result.TotalNoOfRecord : 0;
                }
                else if (param.ReportType == ReferenceCodes_ReportType.PRO_STATUS_EXPIRING.ToString())
                {
                    totalResult = Result.Report4Output.Count > 0 ? Result.NoOfPages : 0;
                    recordResult = Result.Report4Output.Count > 0 ? Result.TotalNoOfRecord : 0;
                }
                else if (param.ReportType == ReferenceCodes_ReportType.BIRTHDAY_PER_MONTH.ToString())
                {
                    totalResult = Result.Report5Output.Count > 0 ? Result.NoOfPages : 0;
                    recordResult = Result.Report5Output.Count > 0 ? Result.TotalNoOfRecord : 0;
                }
                else if (param.ReportType == ReferenceCodes_ReportType.ACT_EMP_PER_GROUP.ToString())
                {
                    totalResult = Result.Report6Output.Count > 0 ? Result.NoOfPages : 0;
                    recordResult = Result.Report6Output.Count > 0 ? Result.TotalNoOfRecord : 0;
                }
                else if (param.ReportType == ReferenceCodes_ReportType.ACT_EMP_PER_POSITION.ToString())
                {
                    totalResult = Result.Report7Output.Count > 0 ? Result.NoOfPages : 0;
                    recordResult = Result.Report7Output.Count > 0 ? Result.TotalNoOfRecord : 0;
                }
                else if (param.ReportType == ReferenceCodes_ReportType.INACTIVE_EMPLOYEE.ToString())
                {
                    totalResult = Result.Report8Output.Count > 0 ? Result.NoOfPages : 0;
                    recordResult = Result.Report8Output.Count > 0 ? Result.TotalNoOfRecord : 0;
                }

                var jsonData = new
                {
                    total = totalResult,
                    param.pageNumber,
                    sort = param.sidx,
                    records = recordResult,
                    rows = Result
                };
                return new JsonResult(jsonData);
            }
            else
            {
                return new BadRequestObjectResult(ErrorMessage);
            }
        }

        private async Task<(GetEmployeeDashboardOutput, bool, string)> GetDataList(GetEmployeeDashboardInput param, bool IsExport)
        {
            var URL = "";

            if (param.ReportType == ReferenceCodes_ReportType.PRO_EMP_PER_GROUP.ToString())
            {
                URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Dashboard").GetSection("GetEmployeeDashboardByReportType").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ReportType=", param.ReportType, "&",
                  "Report1Input.OrgGroup=", param.Report1Input.OrgGroup, "&",
                  "Report1Input.CountMin=", param.Report1Input.CountMin, "&",
                  "Report1Input.CountMax=", param.Report1Input.CountMax, "&",
                  "Report1Input.IsExport=", IsExport
                  );
            }
            else if (param.ReportType == ReferenceCodes_ReportType.PRO_EMP_PER_POSITION.ToString())
            {
                URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Dashboard").GetSection("GetEmployeeDashboardByReportType").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ReportType=", param.ReportType, "&",
                  "Report2Input.Position=", param.Report2Input.Position, "&",
                  "Report2Input.CountMin=", param.Report2Input.CountMin, "&",
                  "Report2Input.CountMax=", param.Report2Input.CountMax, "&",
                  "Report2Input.IsExport=", IsExport
                  );
            }
            else if (param.ReportType == ReferenceCodes_ReportType.PRO_STATUS_BEYOND_6.ToString())
            {
                URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Dashboard").GetSection("GetEmployeeDashboardByReportType").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ReportType=", param.ReportType, "&",
                  "Report3Input.Status=", param.Report3Input.Status, "&",
                  "Report3Input.CountMin=", param.Report3Input.CountMin, "&",
                  "Report3Input.CountMax=", param.Report3Input.CountMax, "&",
                  "Report3Input.IsExport=", IsExport
                  );
            }
            else if (param.ReportType == ReferenceCodes_ReportType.PRO_STATUS_EXPIRING.ToString())
            {
                URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Dashboard").GetSection("GetEmployeeDashboardByReportType").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ReportType=", param.ReportType, "&",
                  "Report4Input.Status=", param.Report4Input.Status, "&",
                  "Report4Input.CountMin=", param.Report4Input.CountMin, "&",
                  "Report4Input.CountMax=", param.Report4Input.CountMax, "&",
                  "Report4Input.IsExport=", IsExport
                  );
            }
            else if (param.ReportType == ReferenceCodes_ReportType.BIRTHDAY_PER_MONTH.ToString())
            {
                URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Dashboard").GetSection("GetEmployeeDashboardByReportType").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ReportType=", param.ReportType, "&",
                  "Report5Input.Month=", param.Report5Input.Month, "&",
                  "Report5Input.CountMin=", param.Report5Input.CountMin, "&",
                  "Report5Input.CountMax=", param.Report5Input.CountMax, "&",
                  "Report5Input.IsExport=", IsExport
                  );
            }
            else if (param.ReportType == ReferenceCodes_ReportType.ACT_EMP_PER_GROUP.ToString())
            {
                URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Dashboard").GetSection("GetEmployeeDashboardByReportType").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ReportType=", param.ReportType, "&",
                  "Report6Input.OrgGroup=", param.Report6Input.OrgGroup, "&",
                  "Report6Input.CountMin=", param.Report6Input.CountMin, "&",
                  "Report6Input.CountMax=", param.Report6Input.CountMax, "&",
                  "Report6Input.IsExport=", IsExport
                  );
            }
            else if (param.ReportType == ReferenceCodes_ReportType.ACT_EMP_PER_POSITION.ToString())
            {
                URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Dashboard").GetSection("GetEmployeeDashboardByReportType").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ReportType=", param.ReportType, "&",
                  "Report7Input.Position=", param.Report7Input.Position, "&",
                  "Report7Input.CountMin=", param.Report7Input.CountMin, "&",
                  "Report7Input.CountMax=", param.Report7Input.CountMax, "&",
                  "Report7Input.IsExport=", IsExport
                  );
            }
            else if (param.ReportType == ReferenceCodes_ReportType.INACTIVE_EMPLOYEE.ToString())
            {
                URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Dashboard").GetSection("GetEmployeeDashboardByReportType").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ReportType=", param.ReportType, "&",
                  "Report8Input.Status=", param.Report8Input.Status, "&",
                  "Report8Input.CountMin=", param.Report8Input.CountMin, "&",
                  "Report8Input.CountMax=", param.Report8Input.CountMax, "&",
                  "Report8Input.IsExport=", IsExport
                  );
            }

            return await SharedUtilities.GetFromAPI(new GetEmployeeDashboardOutput(), URL);
        }

        public async Task<IActionResult> OnGetCheckEmployeeDashboard([FromQuery] GetEmployeeDashboardInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataList(param, true);

            if (IsSuccess)
            {
                int res = 0;

                if (param.ReportType == ReferenceCodes_ReportType.PRO_EMP_PER_GROUP.ToString())
                {
                    res = Result.Report1Output.Count();
                }
                else if (param.ReportType == ReferenceCodes_ReportType.PRO_EMP_PER_POSITION.ToString())
                {
                    res = Result.Report2Output.Count();
                }
                else if (param.ReportType == ReferenceCodes_ReportType.PRO_STATUS_BEYOND_6.ToString())
                {
                    res = Result.Report3Output.Count();
                }
                else if (param.ReportType == ReferenceCodes_ReportType.PRO_STATUS_EXPIRING.ToString())
                {
                    res = Result.Report4Output.Count();
                }
                else if (param.ReportType == ReferenceCodes_ReportType.BIRTHDAY_PER_MONTH.ToString())
                {
                    res = Result.Report5Output.Count();
                }
                else if (param.ReportType == ReferenceCodes_ReportType.ACT_EMP_PER_GROUP.ToString())
                {
                    res = Result.Report6Output.Count();
                }
                else if (param.ReportType == ReferenceCodes_ReportType.ACT_EMP_PER_POSITION.ToString())
                {
                    res = Result.Report7Output.Count();
                }
                else if (param.ReportType == ReferenceCodes_ReportType.INACTIVE_EMPLOYEE.ToString())
                {
                    res = Result.Report8Output.Count();
                }

                if (res > 0)
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
                throw new Exception(ErrorMessage);
        }

        public async Task<IActionResult> OnGetDownloadEmployeeDashboardAsync([FromQuery] GetEmployeeDashboardInput param)
        {

            var (Result, IsSuccess, ErrorMessage) = await GetDataList(param, true);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string fName = "";
                if (param.ReportType == ReferenceCodes_ReportType.PRO_EMP_PER_GROUP.ToString())
                {
                    fName = "Count of Probationary Employees (By Group)";
                }
                else if (param.ReportType == ReferenceCodes_ReportType.PRO_EMP_PER_POSITION.ToString())
                {
                    fName = "Count of Probationary Employees (By Position)";
                }
                else if (param.ReportType == ReferenceCodes_ReportType.PRO_STATUS_BEYOND_6.ToString())
                {
                    fName = "Count of Probationary Status Beyond 6 Months";
                }
                else if (param.ReportType == ReferenceCodes_ReportType.PRO_STATUS_EXPIRING.ToString())
                {
                    fName = "Count of Probationary Status Expiring in 1 Month";
                }
                else if (param.ReportType == ReferenceCodes_ReportType.BIRTHDAY_PER_MONTH.ToString())
                {
                    fName = "Count of Birthdays This Month and Next Month";
                }
                else if (param.ReportType == ReferenceCodes_ReportType.ACT_EMP_PER_GROUP.ToString())
                {
                    fName = "Count of Active Employees (Per Group)";
                }
                else if (param.ReportType == ReferenceCodes_ReportType.ACT_EMP_PER_POSITION.ToString())
                {
                    fName = "Count of Active Employees (Per Position)";
                }
                else if (param.ReportType == ReferenceCodes_ReportType.INACTIVE_EMPLOYEE.ToString())
                {
                    fName = "Count of Inactive Employees";
                }
                string sFileName = string.Concat(fName, ".xlsx");

                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Sheet1");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    if (param.ReportType == ReferenceCodes_ReportType.PRO_EMP_PER_GROUP.ToString())
                    {
                        row.CreateCell(0).SetCellValue("Org Group");
                        row.CreateCell(1).SetCellValue("Count");

                        excelSheet.SetColumnWidth(0, 9000);
                        excelSheet.SetColumnWidth(1, 3500);
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.PRO_EMP_PER_POSITION.ToString())
                    {
                        row.CreateCell(0).SetCellValue("Position");
                        row.CreateCell(1).SetCellValue("Count");

                        excelSheet.SetColumnWidth(0, 9000);
                        excelSheet.SetColumnWidth(1, 3500);
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.PRO_STATUS_BEYOND_6.ToString())
                    {
                        row.CreateCell(0).SetCellValue("Status");
                        row.CreateCell(1).SetCellValue("Count");

                        excelSheet.SetColumnWidth(0, 9000);
                        excelSheet.SetColumnWidth(1, 3500);
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.PRO_STATUS_EXPIRING.ToString())
                    {
                        row.CreateCell(0).SetCellValue("Status");
                        row.CreateCell(1).SetCellValue("Count");

                        excelSheet.SetColumnWidth(0, 9000);
                        excelSheet.SetColumnWidth(1, 3500);
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.BIRTHDAY_PER_MONTH.ToString())
                    {
                        row.CreateCell(0).SetCellValue("Birth Month");
                        row.CreateCell(1).SetCellValue("Count");

                        excelSheet.SetColumnWidth(0, 9000);
                        excelSheet.SetColumnWidth(1, 3500);
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.ACT_EMP_PER_GROUP.ToString())
                    {
                        row.CreateCell(0).SetCellValue("Group");
                        row.CreateCell(1).SetCellValue("Count");

                        excelSheet.SetColumnWidth(0, 9000);
                        excelSheet.SetColumnWidth(1, 3500);
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.ACT_EMP_PER_POSITION.ToString())
                    {
                        row.CreateCell(0).SetCellValue("Position");
                        row.CreateCell(1).SetCellValue("Count");

                        excelSheet.SetColumnWidth(0, 9000);
                        excelSheet.SetColumnWidth(1, 3500);
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.INACTIVE_EMPLOYEE.ToString())
                    {
                        row.CreateCell(0).SetCellValue("Status");
                        row.CreateCell(1).SetCellValue("Count");

                        excelSheet.SetColumnWidth(0, 9000);
                        excelSheet.SetColumnWidth(1, 3500);
                    }

                    XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle negativeVariace = (XSSFCellStyle)workbook.CreateCellStyle();
                    var colHeaderFont = workbook.CreateFont();
                    var negativeVariaceFont = workbook.CreateFont();
                    colHeaderFont.IsBold = true;
                    colHeaderStyle.SetFont(colHeaderFont);
                    colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                    colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                    colHeaderStyle.Alignment = HorizontalAlignment.Center;
                    alignCenter.Alignment = HorizontalAlignment.Center;
                    negativeVariaceFont.Color = HSSFColor.Red.Index;
                    negativeVariace.SetFont(negativeVariaceFont);
                    negativeVariace.Alignment = HorizontalAlignment.Center;

                    if (param.ReportType == ReferenceCodes_ReportType.PRO_EMP_PER_GROUP.ToString())
                    {
                        row.Cells[0].CellStyle = colHeaderStyle;
                        row.Cells[1].CellStyle = colHeaderStyle;
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.PRO_EMP_PER_POSITION.ToString())
                    {
                        row.Cells[0].CellStyle = colHeaderStyle;
                        row.Cells[1].CellStyle = colHeaderStyle;
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.PRO_STATUS_BEYOND_6.ToString())
                    {
                        row.Cells[0].CellStyle = colHeaderStyle;
                        row.Cells[1].CellStyle = colHeaderStyle;
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.PRO_STATUS_EXPIRING.ToString())
                    {
                        row.Cells[0].CellStyle = colHeaderStyle;
                        row.Cells[1].CellStyle = colHeaderStyle;
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.BIRTHDAY_PER_MONTH.ToString())
                    {
                        row.Cells[0].CellStyle = colHeaderStyle;
                        row.Cells[1].CellStyle = colHeaderStyle;
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.ACT_EMP_PER_GROUP.ToString())
                    {
                        row.Cells[0].CellStyle = colHeaderStyle;
                        row.Cells[1].CellStyle = colHeaderStyle;
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.ACT_EMP_PER_POSITION.ToString())
                    {
                        row.Cells[0].CellStyle = colHeaderStyle;
                        row.Cells[1].CellStyle = colHeaderStyle;
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.INACTIVE_EMPLOYEE.ToString())
                    {
                        row.Cells[0].CellStyle = colHeaderStyle;
                        row.Cells[1].CellStyle = colHeaderStyle;
                    }
                    #endregion

                    #region Column Details
                    if (param.ReportType == ReferenceCodes_ReportType.PRO_EMP_PER_GROUP.ToString())
                    {
                        foreach (var item in Result.Report1Output)
                        {
                            row = excelSheet.CreateRow(Convert.ToInt32(irow));
                            row.CreateCell(0).SetCellValue(item.OrgGroup);
                            row.CreateCell(1).SetCellValue(item.Count);

                            row.Cells[1].CellStyle = alignCenter;

                            irow++;
                        }
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.PRO_EMP_PER_POSITION.ToString())
                    {
                        foreach (var item in Result.Report2Output)
                        {
                            row = excelSheet.CreateRow(Convert.ToInt32(irow));
                            row.CreateCell(0).SetCellValue(item.Position);
                            row.CreateCell(1).SetCellValue(item.Count);

                            row.Cells[1].CellStyle = alignCenter;

                            irow++;
                        }
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.PRO_STATUS_BEYOND_6.ToString())
                    {
                        foreach (var item in Result.Report3Output)
                        {
                            row = excelSheet.CreateRow(Convert.ToInt32(irow));
                            row.CreateCell(0).SetCellValue(item.Status);
                            row.CreateCell(1).SetCellValue(item.Count);

                            row.Cells[1].CellStyle = alignCenter;

                            irow++;
                        }
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.PRO_STATUS_EXPIRING.ToString())
                    {
                        foreach (var item in Result.Report4Output)
                        {
                            row = excelSheet.CreateRow(Convert.ToInt32(irow));
                            row.CreateCell(0).SetCellValue(item.Status);
                            row.CreateCell(1).SetCellValue(item.Count);

                            row.Cells[1].CellStyle = alignCenter;

                            irow++;
                        }
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.BIRTHDAY_PER_MONTH.ToString())
                    {
                        foreach (var item in Result.Report5Output)
                        {
                            row = excelSheet.CreateRow(Convert.ToInt32(irow));
                            row.CreateCell(0).SetCellValue(item.Month);
                            row.CreateCell(1).SetCellValue(item.Count);

                            row.Cells[1].CellStyle = alignCenter;

                            irow++;
                        }
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.ACT_EMP_PER_GROUP.ToString())
                    {
                        foreach (var item in Result.Report6Output)
                        {
                            row = excelSheet.CreateRow(Convert.ToInt32(irow));
                            row.CreateCell(0).SetCellValue(item.OrgGroup);
                            row.CreateCell(1).SetCellValue(item.Count);

                            row.Cells[1].CellStyle = alignCenter;

                            irow++;
                        }
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.ACT_EMP_PER_POSITION.ToString())
                    {
                        foreach (var item in Result.Report7Output)
                        {
                            row = excelSheet.CreateRow(Convert.ToInt32(irow));
                            row.CreateCell(0).SetCellValue(item.Position);
                            row.CreateCell(1).SetCellValue(item.Count);

                            row.Cells[1].CellStyle = alignCenter;

                            irow++;
                        }
                    }
                    else if (param.ReportType == ReferenceCodes_ReportType.INACTIVE_EMPLOYEE.ToString())
                    {
                        foreach (var item in Result.Report8Output)
                        {
                            row = excelSheet.CreateRow(Convert.ToInt32(irow));
                            row.CreateCell(0).SetCellValue(item.Status);
                            row.CreateCell(1).SetCellValue(item.Count);

                            row.Cells[1].CellStyle = alignCenter;

                            irow++;
                        }
                    }

                    #endregion

                    workbook.Write(fs);
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EXPORT.ToString(),
                        TableName = "Dashboard",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(param.ReportType, " exported"),
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