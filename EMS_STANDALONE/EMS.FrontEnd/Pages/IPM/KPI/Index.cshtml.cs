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
using EMS.IPM.Transfer.KPI;
using EMS.FrontEnd.SharedClasses.Common_IPM;
using EMS.FrontEnd.SharedClasses;
using NPOI.SS.Util;

namespace EMS.FrontEnd.Pages.IPM.KPI
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/KPI/ADD")).Count() > 0 ? "true" : "false";
                ViewData["HasUploadFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/KPI/UPLOAD")).Count() > 0 ? "true" : "false";
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

        public async Task<JsonResult> OnGetKRAGroupDropdown(int KRAGroup)
        {
            _resultView.Result = await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetKRAGroupDropdown(KRAGroup);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetKRASubGroupDropdown(int KRAGroup)
        {
            _resultView.Result = await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetKRASubGroupDropdown(KRAGroup);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        private async Task<(List<GetListOutput>, bool, string)> GetListData(GetListInput param, bool IsExport)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPI").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",
                  "IsExport=", IsExport, "&",

                  "ID=", param.ID, "&",
                  "Code=", param.Code, "&",
                  "KRATypeDelimited=", param.KRATypeDelimited, "&",
                  "KRAGroup=", param.KRAGroup, "&",
                  "KRASubGroup=", param.KRASubGroup, "&",
                  "OldKPICode=", param.OldKPICode, "&",
                  "Name=", param.Name, "&",
                  "KPITypeDelimited=", param.KPITypeDelimited, "&",
                  "SourceTypeDelimited=", param.SourceTypeDelimited);

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

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _ipmBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetDownloadExportList([FromQuery] GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetListData(param, true);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "KPI List.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("KPI List");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("KRA Group");
                    row.CreateCell(1).SetCellValue("KRA Sub Group");
                    row.CreateCell(2).SetCellValue("KPI Code");
                    row.CreateCell(3).SetCellValue("KPI Name");
                    row.CreateCell(4).SetCellValue("Old KPI Code");
                    row.CreateCell(5).SetCellValue("KPI Type");
                    row.CreateCell(6).SetCellValue("Source Type");

                    excelSheet.SetColumnWidth(0, 7000);
                    excelSheet.SetColumnWidth(1, 9000);
                    excelSheet.SetColumnWidth(2, 5000);
                    excelSheet.SetColumnWidth(3, 15000);
                    excelSheet.SetColumnWidth(4, 5000);
                    excelSheet.SetColumnWidth(5, 5000);
                    excelSheet.SetColumnWidth(6, 5000);

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
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.KRAGroup);
                        row.CreateCell(1).SetCellValue(item.KRASubGroup);
                        row.CreateCell(2).SetCellValue(item.Code);
                        row.CreateCell(3).SetCellValue(item.Name);
                        row.CreateCell(4).SetCellValue(item.OldKPICode);
                        row.CreateCell(5).SetCellValue(item.KPIType);
                        row.CreateCell(6).SetCellValue(item.SourceType);
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
                        TableName = "KPI",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "KPI exported",
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

        public async Task<JsonResult> OnGetOrgTypeAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetKRAGroupAutoComplete(string Term, int TopResults)
        {
            var result = await new Common_KPI(_iconfiguration, _globalCurrentUser, _env)
                .GetKRAGroupAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }


        public async Task<JsonResult> OnGetKRASubGroupAutoComplete(string Term, int TopResults)
        {
            var result = await new Common_KPI(_iconfiguration, _globalCurrentUser, _env)
                .GetKRASubGroupAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetDownloadKPITemplate()
        {
            string sWebRootFolder = _env.WebRootPath;
            string sFileName = "KPI.xlsx";
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

                var kraGroupList = (await new Common_KRAGroup(_iconfiguration, _globalCurrentUser, _env).GetAllKRAGroup()).ToList();

                var kpiTypeList = (await new Common_KPI(_iconfiguration, _globalCurrentUser, _env)
                    .GetReferenceValueByRefCode(EMS.IPM.Transfer.Enums.ReferenceCodes.KPI_TYPE.ToString()))
                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();

                var sourceTypeList = (await new Common_KPI(_iconfiguration, _globalCurrentUser, _env)
                    .GetReferenceValueByRefCode(EMS.IPM.Transfer.Enums.ReferenceCodes.KPI_SOURCE_TYPE.ToString()))
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

                void AddKPISheet()
                {
                    ISheet excelSheet = workbook.CreateSheet("KPI List");
                    int rowCtr = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(rowCtr); rowCtr++;

                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 5, 5), kpiTypeList, excelSheet));
                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 6, 6), sourceTypeList, excelSheet));

                    row.CreateCell(0).SetCellValue("** KRA Group");
                    row.CreateCell(1).SetCellValue("* Old KPI Code");
                    row.CreateCell(2).SetCellValue("* KPI Name");
                    row.CreateCell(3).SetCellValue("* Description");
                    row.CreateCell(4).SetCellValue("* Target Guidelines");
                    row.CreateCell(5).SetCellValue("* KPI Type");
                    row.CreateCell(6).SetCellValue("* Source Type");

                    excelSheet.SetColumnWidth(0, 4500);
                    excelSheet.SetColumnWidth(1, 4500);
                    excelSheet.SetColumnWidth(2, 15000);
                    excelSheet.SetColumnWidth(3, 15000);
                    excelSheet.SetColumnWidth(4, 15000);
                    excelSheet.SetColumnWidth(5, 4500);
                    excelSheet.SetColumnWidth(6, 4500);
                    excelSheet.SetColumnWidth(7, 10000); // Legend

                    for (int i = 0; i <= 6; i++)
                        row.Cells[i].CellStyle = colHeaderStyle;

                    #endregion

                    for (int x = rowCtr; x <= 10000; x++)
                    {
                        IRow rowDate = excelSheet.CreateRow(x);
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;

                        for (int i = 0; i <= 6; i++)
                            rowDate.CreateCell(i, CellType.String).SetCellValue("");
                        
                        for (int i = 0; i <= 6; i++)
                            rowDate.Cells[i].CellStyle = textCS;

                        if (x == 1)
                        {
                            rowDate.CreateCell(6 + 1).SetCellValue(legendsMessage);
                            rowDate.Cells[6 + 1].CellStyle = legendStyle;
                        }
                    }

                }

                var kpiList = (await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetAllKPIDetails()).ToList();

                void AddReferenceLookupSheet()
                {
                    ISheet ReferenceLookupSheet = workbook.CreateSheet("Reference Lookup");
                    int rowCtr = 0;
                    IRow pRow = ReferenceLookupSheet.CreateRow(rowCtr); rowCtr++;
                    pRow.CreateCell(0).SetCellValue("KRA Group");
                    ReferenceLookupSheet.SetColumnWidth(0, 5000);
                    pRow.Cells[0].CellStyle = colHeaderStyle;

                    foreach (var item in kraGroupList.ToList())
                    {
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;
                        IRow rowLookup = ReferenceLookupSheet.CreateRow(rowCtr);
                        if (rowLookup != null)
                        {
                            rowLookup.CreateCell(0, CellType.String).SetCellValue(item.Name);
                        }
                        else
                        {
                            IRow newRow = ReferenceLookupSheet.CreateRow(rowCtr);
                            newRow.CreateCell(0, CellType.String).SetCellValue(item.Name);
                        }
                        rowCtr++;
                    }

                    IRow nRow = ReferenceLookupSheet.CreateRow(rowCtr); rowCtr++;
                    nRow.CreateCell(0).SetCellValue("KPI List");
                    ReferenceLookupSheet.SetColumnWidth(0, 5000);
                    nRow.Cells[0].CellStyle = colHeaderStyle;

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
                }

                AddKPISheet();
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
    }
}