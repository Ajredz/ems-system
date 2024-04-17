using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;
using System;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.IPM.Transfer.KPIPosition;
using EMS.FrontEnd.SharedClasses.Common_IPM;

namespace EMS.FrontEnd.Pages.IPM.KPIPosition
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/KPIPOSITION/ADD")).Count() > 0 ? "true" : "false";
                ViewData["HasUploadInsertFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/KPIPOSITION/UPLOADINSERT")).Count() > 0 ? "true" : "false";
            }
        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetListData(param, false);

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

        public async Task<JsonResult> OnGetKPIDropdown()
        {
            _resultView.Result = await new Common_KPIPosition(_iconfiguration, _globalCurrentUser, _env).GetKPIDropdown();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetKPIList()
        {
            _resultView.Result = await new Common_KPIPosition(_iconfiguration, _globalCurrentUser, _env).GetAllKPI();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetKPIPositionList()
        {
            _resultView.Result = await new Common_KPIPosition(_iconfiguration, _globalCurrentUser, _env).GetAllKPIPosition();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetKPIAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetKPIAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        private async Task<(List<GetListOutput>, bool, string)> GetListData(GetListInput param, bool IsExport)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPIPosition").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "IsShowRecentOnly=", param.IsShowRecentOnly, "&",
                  "IsExport=", IsExport, "&",

                  "ID=", param.ID, "&",
                  "PositionDelimited=", param.PositionDelimited, "&",
                  "Weight=", param.Weight, "&",
                  "DateEffectiveFrom=", param.DateEffectiveFrom, "&",
                  "DateEffectiveTo=", param.DateEffectiveTo);

            return await SharedUtilities.GetFromAPI(new List<GetListOutput>(), URL);
        }

        public async Task<IActionResult> OnGetCheckExportList([FromQuery] GetListInput param)
        {

            var (Result, IsSuccess, ErrorMessage) = await GetListData(param, true);

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

        private async Task<(List<GetExportListOutput>, bool, string)> GetExportListData(GetListInput param, bool IsExport)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPIPosition").GetSection("ExportList").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "IsShowRecentOnly=", param.IsShowRecentOnly, "&",
                  "IsExport=", IsExport, "&",

                  "ID=", param.ID, "&",
                  "PositionDelimited=", param.PositionDelimited, "&",
                  "Weight=", param.Weight, "&",
                  "DateEffectiveFrom=", param.DateEffectiveFrom, "&",
                  "DateEffectiveTo=", param.DateEffectiveTo);

            return await SharedUtilities.GetFromAPI(new List<GetExportListOutput>(), URL);
        }

        public async Task<IActionResult> OnGetDownloadExportList([FromQuery] GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetExportListData(param, true);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "KPI Position List.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("KPI PositionList");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Position");
                    row.CreateCell(1).SetCellValue("Effective Date");
                    row.CreateCell(2).SetCellValue("KPI Name");
                    row.CreateCell(3).SetCellValue("Weight");


                    excelSheet.SetColumnWidth(0, 11000);
                    excelSheet.SetColumnWidth(1, 9000);
                    excelSheet.SetColumnWidth(2, 11000);
                    excelSheet.SetColumnWidth(3, 7000);

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
                    #endregion

                    XSSFCellStyle weightScoreStyle = (XSSFCellStyle)workbook.CreateCellStyle();

                    weightScoreStyle.Alignment = HorizontalAlignment.Right;

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.Position);
                        row.CreateCell(1).SetCellValue(item.EffectiveDate);
                        row.CreateCell(2).SetCellValue(item.KPI);
                        row.CreateCell(3).SetCellValue(item.Weight);
                        irow++;

                        row.Cells[3].CellStyle = weightScoreStyle;
                    }
                    #endregion

                    workbook.Write(fs);
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EXPORT.ToString(),
                        TableName = "KPI Position",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "KPI Position exported",
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

        public async Task<IActionResult> OnGetDownloadKPIPositionTemplate()
        {
            string sWebRootFolder = _env.WebRootPath;
            string sFileName = "KPI Position.xlsx";
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();

                // Header style
                XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();
                XSSFCellStyle legendStyle = (XSSFCellStyle)workbook.CreateCellStyle();

                string legendsMessage = string.Concat("Legends:", Environment.NewLine
                            , "*       - Required, Case Sensitive.", Environment.NewLine
                            , "**     - see Reference Lookup Worksheet, Required, Case Sensitive.");

                var format = workbook.CreateDataFormat().GetFormat("text");
                var colHeaderFont = workbook.CreateFont();
                var legendFont = workbook.CreateFont();
                colHeaderFont.IsBold = true;
                legendFont.IsBold = true;
                legendFont.IsItalic = true;
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

                var kpiList = (await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetAllKPIDetails()).ToList();
                var positionList = (await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionCodeDropDown(0)).ToList();

                // Dropdown values
                var kpiCodeList = kpiList.OrderBy(y => y.KPICode).Select(x => x.KPICode).ToArray();

                void AddKPIPositionSheet()
                {
                    ISheet excelSheet = workbook.CreateSheet("KPI Position");
                    int rowCtr = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(rowCtr); rowCtr++;

                    row.CreateCell(0).SetCellValue("* Effective Date (MM/DD/YYYY)");
                    row.CreateCell(1).SetCellValue("** KPI Code");
                    row.CreateCell(2).SetCellValue("** Position Code");
                    row.CreateCell(3).SetCellValue("Position");
                    row.CreateCell(4).SetCellValue("* Weight");

                    excelSheet.SetColumnWidth(0, 9000);
                    excelSheet.SetColumnWidth(1, 6500);
                    excelSheet.SetColumnWidth(2, 6500);
                    excelSheet.SetColumnWidth(3, 10000);
                    excelSheet.SetColumnWidth(4, 4500);
                    excelSheet.SetColumnWidth(5, 10000); // Legend

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

                        if (x == 1)
                        {
                            rowDate.CreateCell(4 + 1).SetCellValue(legendsMessage);
                            rowDate.Cells[4 + 1].CellStyle = legendStyle;
                        }
                    }

                }

                void AddReferenceLookupSheet()
                {
                    ISheet ReferenceLookupSheet = workbook.CreateSheet("Reference Lookup");
                    int rowCtr = 0;
                    IRow pRow = ReferenceLookupSheet.CreateRow(rowCtr); rowCtr++;
                    pRow.CreateCell(0).SetCellValue("KPI Codes");
                    pRow.CreateCell(1).SetCellValue("Old KPI Codes");
                    pRow.CreateCell(2).SetCellValue("KPI Description");
                    pRow.CreateCell(3).SetCellValue("");
                    pRow.CreateCell(4).SetCellValue("Position Codes");
                    pRow.CreateCell(5).SetCellValue("Position Descsription");
                    ReferenceLookupSheet.SetColumnWidth(0, 5000);
                    ReferenceLookupSheet.SetColumnWidth(1, 5000);
                    ReferenceLookupSheet.SetColumnWidth(2, 25000);
                    ReferenceLookupSheet.SetColumnWidth(3, 1000);
                    ReferenceLookupSheet.SetColumnWidth(4, 8000);
                    ReferenceLookupSheet.SetColumnWidth(5, 12000);
                    pRow.Cells[0].CellStyle = colHeaderStyle;
                    pRow.Cells[1].CellStyle = colHeaderStyle;
                    pRow.Cells[2].CellStyle = colHeaderStyle;
                    pRow.Cells[4].CellStyle = colHeaderStyle;
                    pRow.Cells[5].CellStyle = colHeaderStyle;

                    foreach (var item in kpiList.ToList())
                    {
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;
                        IRow rowLookup = ReferenceLookupSheet.GetRow(rowCtr);
                        if (rowLookup != null)
                        {
                            rowLookup.CreateCell(0, CellType.String).SetCellValue(item.KPICode);
                            rowLookup.CreateCell(1, CellType.String).SetCellValue(item.OldKPICode);
                            rowLookup.CreateCell(2, CellType.String).SetCellValue(item.KPIDescription);
                        }
                        else
                        {
                            IRow newRow = ReferenceLookupSheet.CreateRow(rowCtr);
                            newRow.CreateCell(0, CellType.String).SetCellValue(item.KPICode);
                            newRow.CreateCell(1, CellType.String).SetCellValue(item.OldKPICode);
                            newRow.CreateCell(2, CellType.String).SetCellValue(item.KPIDescription);
                        }
                        rowCtr++;
                    }
                    rowCtr = 1;
                    foreach (var item in positionList.ToList())
                    {
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;
                        IRow rowLookup = ReferenceLookupSheet.GetRow(rowCtr);
                        if (rowLookup != null)
                        {
                            rowLookup.CreateCell(4, CellType.String).SetCellValue(item.Value);
                            rowLookup.CreateCell(5, CellType.String).SetCellValue(item.Text);
                        }
                        else
                        {
                            IRow newRow = ReferenceLookupSheet.CreateRow(rowCtr);
                            newRow.CreateCell(4, CellType.String).SetCellValue(item.Value);
                            newRow.CreateCell(5, CellType.String).SetCellValue(item.Text);
                        }

                        rowCtr++;
                    }
                }

                AddKPIPositionSheet();
                AddReferenceLookupSheet();

                workbook.Write(fs);
            }

            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        public async Task<JsonResult> OnGetCopyPositionAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_KPIPosition(_iconfiguration, _globalCurrentUser, _env).GetCopyPosition(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetCopyPositionResultAutoCompleteAsync(int ID, string Date)
        {
            return new JsonResult(await new Common_KPIPosition(_iconfiguration, _globalCurrentUser, _env).GetKPIPosition(ID, Date));
        }

    }
}