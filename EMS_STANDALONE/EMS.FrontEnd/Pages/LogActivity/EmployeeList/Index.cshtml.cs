using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Workflow.Transfer.Accountability;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI.Common;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Utilities.API;

namespace EMS.FrontEnd.Pages.LogActivity.EmployeeList
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] EMS.Plantilla.Transfer.Employee.GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataList(param, false);

            var finalStatus = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeAccountabilityStatusPercentage(new GetEmployeeAccountabilityStatusPercentageInput() { EmployeeIDs = (Result.Where(x => x.ID != 0).Select(x => (int)x.ID).Distinct().ToList()), ClearingOrgIDDelimited = param.Percentage })).Item1;

            Result = (from left in Result
                          join right in finalStatus on left.ID equals right.EmployeeID into righTable
                          from subright in righTable.DefaultIfEmpty()
                          select new EMS.Plantilla.Transfer.Employee.GetListOutput()
                          {
                              ID = left.ID,
                              Percent = (subright == null ? "0 / 0" : string.Concat(subright.OverDone," / ", subright.OverAll)),
                              Code = left.Code,
                              Name = left.Name,
                              OrgGroup = left.OrgGroup,
                              Position = left.Position,
                              EmploymentStatus = left.EmploymentStatus,
                              DateStatus = left.DateStatus,
                              DateHired = left.DateHired,
                              MovementDate = left.MovementDate,
                              OldEmployeeID = left.OldEmployeeID,
                              Cluster = left.Cluster,
                              Area = left.Area,
                              Region = left.Region,
                              Zone = left.Zone,

                              NoOfPages = left.NoOfPages,
                              Row = left.Row,
                              TotalNoOfRecord = left.TotalNoOfRecord
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

        private async Task<(List<EMS.Plantilla.Transfer.Employee.GetListOutput>, bool, string)> GetDataList([FromQuery] EMS.Plantilla.Transfer.Employee.GetListInput param, bool IsExport)
        {
            /*var OrgIDListDescendants = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupDescendants(_globalCurrentUser.OrgGroupID)).ToList();
            var OrgGroup = string.Concat(string.Join(',', OrgIDListDescendants), (string.IsNullOrEmpty(param.OrgGroupDelimited)? "" : "," + param.OrgGroupDelimited));
            var EmploymentStatus = string.Concat("51,52,490,497", (string.IsNullOrEmpty(param.EmploymentStatusDelimited) ? "" : "," + param.EmploymentStatusDelimited));*/

            var Org="";
            if (_systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/EMPLOYEELIST/VIEWALL")).Count() > 0)
                Org = param.OrgGroupDelimited;
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
                  "MovementDateFrom=", param.MovementDateFrom, "&",
                  "MovementDateTo=", param.MovementDateTo, "&",
                  "OldEmployeeID=", param.OldEmployeeID, "&",
                  "OrgGroupDelimitedClus=", param.OrgGroupDelimitedClus, "&",
                  "OrgGroupDelimitedArea=", param.OrgGroupDelimitedArea, "&",
                  "OrgGroupDelimitedReg=", param.OrgGroupDelimitedReg, "&",
                  "OrgGroupDelimitedZone=", param.OrgGroupDelimitedZone, "&",
                  "ShowActiveEmployee=", param.ShowActiveEmployee, "&",
                  "IsExport=", IsExport);

            return await SharedUtilities.GetFromAPI(new List<EMS.Plantilla.Transfer.Employee.GetListOutput>(), URL);
        }

        public async Task<JsonResult> OnGetOrgTypeAutoCompleteAsync(string term, int TopResults)
        {
            if (_systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEELIST/VIEWALL")).Count() > 0)
            {
                var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupAutoComplete(term, TopResults);
                _resultView.IsSuccess = true;
                _resultView.Result = result;
                return new JsonResult(_resultView);
            }
            else
            {
                var Org = string.Concat(string.Join(",", _globalCurrentUser.OrgGroupID),
                    (_globalCurrentUser.OrgGroupDescendants == null ? "" : "," + string.Join(",", _globalCurrentUser.OrgGroupDescendants)),
                    (_globalCurrentUser.OrgGroupRovingDescendants == null ? "" : "," + string.Join(",", _globalCurrentUser.OrgGroupRovingDescendants)),
                    (_globalCurrentUser.Roving == null ? "" : "," + string.Join(",", _globalCurrentUser.Roving.Select(x => x.OrgGroupID).ToList())));

                var ConvertToList = (Org.Split(",")).Select(int.Parse).ToList();

                var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupAutoComplete(term, TopResults);
                _resultView.IsSuccess = true;
                _resultView.Result = result.Where(x => ConvertToList.Contains(x.ID)).ToList();
                return new JsonResult(_resultView);
            }
        }
        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
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
        public async Task<IActionResult> OnGetDownloadEmployeeExportListAsync([FromQuery] EMS.Plantilla.Transfer.Employee.GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataList(param, true);
            var finalStatus = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeAccountabilityStatusPercentage(new GetEmployeeAccountabilityStatusPercentageInput() { EmployeeIDs = (Result.Where(x => x.ID != 0).Select(x => (int)x.ID).Distinct().ToList()), ClearingOrgIDDelimited = param.Percentage })).Item1;

            Result = (from left in Result
                      join right in finalStatus on left.ID equals right.EmployeeID into righTable
                      from subright in righTable.DefaultIfEmpty()
                      select new EMS.Plantilla.Transfer.Employee.GetListOutput()
                      {
                          ID = left.ID,
                          Percent = (subright == null ? "0 / 0" : string.Concat(subright.OverDone, " / ", subright.OverAll)),
                          Code = left.Code,
                          Name = left.Name,
                          OrgGroup = left.OrgGroup,
                          Position = left.Position,
                          EmploymentStatus = left.EmploymentStatus,
                          DateStatus = left.DateStatus,
                          MovementDate = left.MovementDate,
                          DateHired = left.DateHired,
                          OldEmployeeID = left.OldEmployeeID,
                          Cluster = left.Cluster,
                          Area = left.Area,
                          Region = left.Region,
                          Zone = left.Zone,

                          NoOfPages = left.NoOfPages,
                          Row = left.Row,
                          TotalNoOfRecord = left.TotalNoOfRecord
                      }).ToList();

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
                    row.CreateCell(1).SetCellValue("Clearance");
                    row.CreateCell(2).SetCellValue("New Employee ID");
                    row.CreateCell(3).SetCellValue("Name");
                    row.CreateCell(4).SetCellValue("Organizational Group");
                    row.CreateCell(5).SetCellValue("Position");
                    row.CreateCell(6).SetCellValue("Employment Status");
                    row.CreateCell(7).SetCellValue("Date Separated");
                    row.CreateCell(8).SetCellValue("Date Hired");
                    row.CreateCell(9).SetCellValue("Movement Updated Date");
                    row.CreateCell(10).SetCellValue("Old Employee ID");
                    row.CreateCell(11).SetCellValue("Company");
                    row.CreateCell(12).SetCellValue("Cluster");
                    row.CreateCell(13).SetCellValue("Area");
                    row.CreateCell(14).SetCellValue("Region");
                    row.CreateCell(15).SetCellValue("Zone");

                    excelSheet.SetColumnWidth(0, 3500);
                    excelSheet.SetColumnWidth(1, 5500);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 10000);
                    excelSheet.SetColumnWidth(4, 10000);
                    excelSheet.SetColumnWidth(5, 6500);
                    excelSheet.SetColumnWidth(6, 4500);
                    excelSheet.SetColumnWidth(7, 4500);
                    excelSheet.SetColumnWidth(8, 5500);
                    excelSheet.SetColumnWidth(9, 5500);
                    excelSheet.SetColumnWidth(10, 5500);
                    excelSheet.SetColumnWidth(11, 5500);
                    excelSheet.SetColumnWidth(12, 5500);
                    excelSheet.SetColumnWidth(13, 5500);
                    excelSheet.SetColumnWidth(14, 5500);
                    excelSheet.SetColumnWidth(15, 5500);

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
                    row.Cells[14].CellStyle = colHeaderStyle;
                    row.Cells[15].CellStyle = colHeaderStyle;
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(Convert.ToDouble(item.ID));
                        row.CreateCell(1).SetCellValue(item.Percent);
                        row.CreateCell(2).SetCellValue(item.Code);
                        row.CreateCell(3).SetCellValue(item.Name);
                        row.CreateCell(4).SetCellValue(item.OrgGroup);
                        row.CreateCell(5).SetCellValue(item.Position);
                        row.CreateCell(6).SetCellValue(item.EmploymentStatus);
                        //row.CreateCell(6).SetCellValue(item.CurrentStep);
                        //row.CreateCell(7).SetCellValue(item.DateScheduled);
                        //row.CreateCell(8).SetCellValue(item.DateCompleted);
                        //row.CreateCell(9).SetCellValue(item.Remarks);
                        row.CreateCell(7).SetCellValue(item.DateStatus);
                        row.CreateCell(8).SetCellValue(item.DateHired);
                        row.CreateCell(9).SetCellValue(item.MovementDate);
                        row.CreateCell(10).SetCellValue(item.OldEmployeeID);
                        row.CreateCell(11).SetCellValue(item.Company);
                        row.CreateCell(12).SetCellValue(item.Cluster);
                        row.CreateCell(13).SetCellValue(item.Area);
                        row.CreateCell(14).SetCellValue(item.Region);
                        row.CreateCell(15).SetCellValue(item.Zone);

                        row.Cells[0].CellStyle = alignCenter;
                        row.Cells[1].CellStyle = alignCenter;
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
    }
}
