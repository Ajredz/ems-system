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
using EMS.IPM.Transfer.MyKPIScores;
using EMS.FrontEnd.SharedClasses.Common_IPM;
using EMS.FrontEnd.SharedClasses;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using NPOI.XSSF.UserModel.Extensions;

namespace EMS.FrontEnd.Pages.IPM.MyKPIScores
{
    public class IndexModel : SharedClasses.Utilities
    {

        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        { }

        public async Task<IActionResult> OnGetListAsync([FromQuery] GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetExportData(param, false);

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
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetOrgTypeFilteredAutoCompleteAsync(string term, int TopResults, string filter)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupFilteredAutoComplete(term, TopResults, filter);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        private async Task<(List<GetListOutput>, bool, string)> GetExportData(GetListInput param, bool IsExport)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("MyKPIScores").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",
                  "IsExport=", IsExport, "&",

                  "ID=", param.ID, "&",
                  "TransSummaryIDDelimited=", param.TransSummaryIDDelimited, "&",
                  "OrgGroupDelimited=", param.OrgGroupDelimited, "&",
                  "PositionDelimited=", param.PositionDelimited, "&",
                  "ScoreFrom=", param.ScoreFrom, "&",
                  "ScoreTo=", param.ScoreTo, "&",
                  "DateFromFrom=", param.DateFromFrom, "&",
                  "DateFromTo=", param.DateFromTo, "&",
                  "DateToFrom=", param.DateToFrom, "&",
                  "DateToTo=", param.DateToTo);

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

        public async Task<IActionResult> OnGetCheckExportEmployee([FromQuery] GetByIDInput param)
        {

            Form MyKPIScore = await new Common_MyKPIScores(_iconfiguration, _globalCurrentUser, _env).GetMyKPIScore(param);

            if (MyKPIScore != null)
            {
                if (MyKPIScore.MyKPIScoreList != null)
                {
                    if (MyKPIScore.MyKPIScoreList.Count > 0)
                    {
                        _resultView.IsSuccess = true;
                    }
                }
            }
            else
            {
                _resultView.IsSuccess = false;
                _resultView.Result = MessageUtilities.ERRMSG_NO_RECORDS;
            }

            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetDownloadExportEmployee([FromQuery] GetByIDInput param)
        {
            Form MyKPIScore = await new Common_MyKPIScores(_iconfiguration, _globalCurrentUser, _env).GetMyKPIScore(param);

            decimal TotalWeight = 0;
            decimal? WeightedScore;
            decimal? TotalScore = 0;

            string sWebRootFolder = _env.WebRootPath;
            string sFileName = MyKPIScore.Employee + ".xlsx";
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(MyKPIScore.Employee);

                int irow;

                //HEADER
                #region Top Header
                for (irow = 0; irow < 5; irow++)
                {
                    IRow topHeader = excelSheet.CreateRow(irow);

                    XSSFCellStyle colTopHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle colTopDetailsStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle alignLeft = (XSSFCellStyle)workbook.CreateCellStyle();

                    var colTopHeaderFont = workbook.CreateFont();
                    colTopHeaderFont.IsBold = true;
                    colTopHeaderFont.FontHeightInPoints = 9;
                    colTopHeaderStyle.SetFont(colTopHeaderFont);
                    colTopHeaderStyle.Alignment = HorizontalAlignment.Left;
                    alignLeft.Alignment = HorizontalAlignment.Left;

                    var colTopDetailsFont = workbook.CreateFont();
                    colTopDetailsFont.FontHeightInPoints = 9;
                    colTopDetailsStyle.SetFont(colTopDetailsFont);

                    //cell borders
                    colTopHeaderStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    colTopHeaderStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    colTopHeaderStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    colTopHeaderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    colTopHeaderStyle.SetBorderColor(BorderSide.RIGHT, new XSSFColor(Color.Black));
                    colTopHeaderStyle.SetBorderColor(BorderSide.TOP, new XSSFColor(Color.Black));
                    colTopHeaderStyle.SetBorderColor(BorderSide.RIGHT, new XSSFColor(Color.Black));
                    colTopHeaderStyle.SetBorderColor(BorderSide.BOTTOM, new XSSFColor(Color.Black));

                    colTopDetailsStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    colTopDetailsStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    colTopDetailsStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    colTopDetailsStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    colTopDetailsStyle.SetBorderColor(BorderSide.RIGHT, new XSSFColor(Color.Black));
                    colTopDetailsStyle.SetBorderColor(BorderSide.TOP, new XSSFColor(Color.Black));
                    colTopDetailsStyle.SetBorderColor(BorderSide.RIGHT, new XSSFColor(Color.Black));
                    colTopDetailsStyle.SetBorderColor(BorderSide.BOTTOM, new XSSFColor(Color.Black));


                    var header = new NPOI.SS.Util.CellRangeAddress(irow, irow, 0, 1);
                    var headerDetails = new NPOI.SS.Util.CellRangeAddress(irow, irow, 2, 10);

                    excelSheet.AddMergedRegion(header);
                    excelSheet.AddMergedRegion(headerDetails);

                    if (irow == 0)
                    {
                        topHeader.CreateCell(0).SetCellValue("Employee");
                        topHeader.CreateCell(1).SetCellValue("");
                        topHeader.CreateCell(2).SetCellValue(MyKPIScore.Employee);
                        topHeader.CreateCell(3).SetCellValue("");
                        topHeader.CreateCell(4).SetCellValue("");
                        topHeader.CreateCell(5).SetCellValue("");
                        topHeader.CreateCell(6).SetCellValue("");
                        topHeader.CreateCell(7).SetCellValue("");
                        topHeader.CreateCell(8).SetCellValue("");
                        topHeader.CreateCell(9).SetCellValue("");
                        topHeader.CreateCell(10).SetCellValue("");
                        topHeader.CreateCell(11).SetCellValue("");

                        topHeader.Cells[0].CellStyle = colTopHeaderStyle;
                        topHeader.Cells[1].CellStyle = colTopHeaderStyle;
                        topHeader.Cells[2].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[3].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[4].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[5].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[6].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[7].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[8].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[9].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[10].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[11].CellStyle = colTopDetailsStyle;
                    }

                    else if (irow == 1)
                    {
                        topHeader.CreateCell(0).SetCellValue("Status");
                        topHeader.CreateCell(1).SetCellValue("");
                        topHeader.CreateCell(2).SetCellValue(MyKPIScore.Status);
                        topHeader.CreateCell(3).SetCellValue("");
                        topHeader.CreateCell(4).SetCellValue("");
                        topHeader.CreateCell(5).SetCellValue("");
                        topHeader.CreateCell(6).SetCellValue("");
                        topHeader.CreateCell(7).SetCellValue("");
                        topHeader.CreateCell(8).SetCellValue("");
                        topHeader.CreateCell(9).SetCellValue("");
                        topHeader.CreateCell(10).SetCellValue("");
                        topHeader.CreateCell(11).SetCellValue("");

                        topHeader.Cells[0].CellStyle = colTopHeaderStyle;
                        topHeader.Cells[1].CellStyle = colTopHeaderStyle;
                        topHeader.Cells[2].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[3].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[4].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[5].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[6].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[7].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[8].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[9].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[10].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[11].CellStyle = colTopDetailsStyle;
                    }

                    else if (irow == 2)
                    {
                        topHeader.CreateCell(0).SetCellValue("Effective Date");
                        topHeader.CreateCell(1).SetCellValue("");
                        topHeader.CreateCell(2).SetCellValue(MyKPIScore.TDateFrom + " - " + MyKPIScore.TDateTo);
                        topHeader.CreateCell(3).SetCellValue("");
                        topHeader.CreateCell(4).SetCellValue("");
                        topHeader.CreateCell(5).SetCellValue("");
                        topHeader.CreateCell(6).SetCellValue("");
                        topHeader.CreateCell(7).SetCellValue("");
                        topHeader.CreateCell(8).SetCellValue("");
                        topHeader.CreateCell(9).SetCellValue("");
                        topHeader.CreateCell(10).SetCellValue("");
                        topHeader.CreateCell(11).SetCellValue("");

                        topHeader.Cells[0].CellStyle = colTopHeaderStyle;
                        topHeader.Cells[1].CellStyle = colTopHeaderStyle;
                        topHeader.Cells[2].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[3].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[4].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[5].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[6].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[7].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[8].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[9].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[10].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[11].CellStyle = colTopDetailsStyle;
                    }

                    else if (irow == 3)
                    {
                        topHeader.CreateCell(0).SetCellValue("Org. Group");
                        topHeader.CreateCell(1).SetCellValue("");
                        topHeader.CreateCell(2).SetCellValue(MyKPIScore.OrgGroup);
                        topHeader.CreateCell(3).SetCellValue("");
                        topHeader.CreateCell(4).SetCellValue("");
                        topHeader.CreateCell(5).SetCellValue("");
                        topHeader.CreateCell(6).SetCellValue("");
                        topHeader.CreateCell(7).SetCellValue("");
                        topHeader.CreateCell(8).SetCellValue("");
                        topHeader.CreateCell(9).SetCellValue("");
                        topHeader.CreateCell(10).SetCellValue("");
                        topHeader.CreateCell(11).SetCellValue("");

                        topHeader.Cells[0].CellStyle = colTopHeaderStyle;
                        topHeader.Cells[1].CellStyle = colTopHeaderStyle;
                        topHeader.Cells[2].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[3].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[4].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[5].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[6].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[7].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[8].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[9].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[10].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[11].CellStyle = colTopDetailsStyle;
                    }

                    else if (irow == 4)
                    {
                        topHeader.CreateCell(0).SetCellValue("Position");
                        topHeader.CreateCell(1).SetCellValue("");
                        topHeader.CreateCell(2).SetCellValue(MyKPIScore.Position);
                        topHeader.CreateCell(3).SetCellValue("");
                        topHeader.CreateCell(4).SetCellValue("");
                        topHeader.CreateCell(5).SetCellValue("");
                        topHeader.CreateCell(6).SetCellValue("");
                        topHeader.CreateCell(7).SetCellValue("");
                        topHeader.CreateCell(8).SetCellValue("");
                        topHeader.CreateCell(9).SetCellValue("");
                        topHeader.CreateCell(10).SetCellValue("");
                        topHeader.CreateCell(11).SetCellValue("");

                        topHeader.Cells[0].CellStyle = colTopHeaderStyle;
                        topHeader.Cells[1].CellStyle = colTopHeaderStyle;
                        topHeader.Cells[2].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[3].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[4].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[5].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[6].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[7].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[8].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[9].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[10].CellStyle = colTopDetailsStyle;
                        topHeader.Cells[11].CellStyle = colTopDetailsStyle;
                    }
                }
                #endregion

                //RECORD LIST
                #region Column Headers
                IRow row = excelSheet.CreateRow(irow); irow++;
                excelSheet.ProtectSheet(_iconfiguration.GetSection("ExcelPassword").Value);
                row.CreateCell(0).SetCellValue("KRA Group");
                row.CreateCell(1).SetCellValue("KPI Code");
                row.CreateCell(2).SetCellValue("KPI Name");
                row.CreateCell(3).SetCellValue("KPI Description");
                row.CreateCell(4).SetCellValue("KPI Guidelines");
                row.CreateCell(5).SetCellValue("Weight");
                row.CreateCell(6).SetCellValue("Target");
                row.CreateCell(7).SetCellValue("Actual");
                row.CreateCell(8).SetCellValue("Rate");
                row.CreateCell(9).SetCellValue("Total");
                row.CreateCell(10).SetCellValue("Grade");
                row.CreateCell(11).SetCellValue("Source Type");

                excelSheet.SetColumnWidth(0, 3300);
                excelSheet.SetColumnWidth(1, 2000);
                excelSheet.SetColumnWidth(2, 7500);
                excelSheet.SetColumnWidth(3, 10000);
                excelSheet.SetColumnWidth(4, 10000);
                excelSheet.SetColumnWidth(5, 2200);
                excelSheet.SetColumnWidth(6, 2200);
                excelSheet.SetColumnWidth(7, 2200);
                excelSheet.SetColumnWidth(8, 2200);
                excelSheet.SetColumnWidth(9, 2200);
                excelSheet.SetColumnWidth(10, 2200);
                excelSheet.SetColumnWidth(11, 2200);

                XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();

                var colHeaderFont = workbook.CreateFont();
                colHeaderFont.IsBold = true;
                colHeaderFont.FontHeightInPoints = 9;
                colHeaderStyle.SetFont(colHeaderFont);
                colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                colHeaderStyle.Alignment = HorizontalAlignment.Center;
                alignCenter.Alignment = HorizontalAlignment.Center;
                //cell borders
                colHeaderStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                colHeaderStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                colHeaderStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                colHeaderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                colHeaderStyle.SetBorderColor(BorderSide.RIGHT, new XSSFColor(Color.Black));
                colHeaderStyle.SetBorderColor(BorderSide.TOP, new XSSFColor(Color.Black));
                colHeaderStyle.SetBorderColor(BorderSide.RIGHT, new XSSFColor(Color.Black));
                colHeaderStyle.SetBorderColor(BorderSide.BOTTOM, new XSSFColor(Color.Black));

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
                #endregion

                #region Column Details
                XSSFCellStyle weightScoreStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                XSSFCellStyle textStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                XSSFCellStyle textStyleCenter = (XSSFCellStyle)workbook.CreateCellStyle();

                var fontSize = workbook.CreateFont();
                fontSize.FontHeightInPoints = 9;

                textStyle.WrapText = true;
                textStyle.SetFont(fontSize);
                //cell borders
                textStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                textStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                textStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                textStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                textStyle.SetBorderColor(BorderSide.RIGHT, new XSSFColor(Color.Black));
                textStyle.SetBorderColor(BorderSide.TOP, new XSSFColor(Color.Black));
                textStyle.SetBorderColor(BorderSide.RIGHT, new XSSFColor(Color.Black));
                textStyle.SetBorderColor(BorderSide.BOTTOM, new XSSFColor(Color.Black));

                textStyleCenter.WrapText = true;
                textStyleCenter.SetFont(fontSize);
                textStyleCenter.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                textStyleCenter.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                textStyleCenter.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                textStyleCenter.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                textStyleCenter.SetBorderColor(BorderSide.RIGHT, new XSSFColor(Color.Black));
                textStyleCenter.SetBorderColor(BorderSide.TOP, new XSSFColor(Color.Black));
                textStyleCenter.SetBorderColor(BorderSide.RIGHT, new XSSFColor(Color.Black));
                textStyleCenter.SetBorderColor(BorderSide.BOTTOM, new XSSFColor(Color.Black));
                textStyleCenter.Alignment = HorizontalAlignment.Center;

                weightScoreStyle.Alignment = HorizontalAlignment.Right;
                weightScoreStyle.SetFont(fontSize);
                //cell borders
                weightScoreStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                weightScoreStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                weightScoreStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                weightScoreStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                weightScoreStyle.SetBorderColor(BorderSide.RIGHT, new XSSFColor(Color.Black));
                weightScoreStyle.SetBorderColor(BorderSide.TOP, new XSSFColor(Color.Black));
                weightScoreStyle.SetBorderColor(BorderSide.RIGHT, new XSSFColor(Color.Black));
                weightScoreStyle.SetBorderColor(BorderSide.BOTTOM, new XSSFColor(Color.Black));

                foreach (var item in MyKPIScore.MyKPIScoreList)
                {
                    WeightedScore = item.Rate * item.Weight;

                    row = excelSheet.CreateRow(Convert.ToInt32(irow));
                    row.CreateCell(0).SetCellValue(item.KRAGroup);
                    row.CreateCell(1).SetCellValue(item.KPICode);
                    row.CreateCell(2).SetCellValue(item.KPIName);
                    row.CreateCell(3).SetCellValue(item.KPIDescription);
                    row.CreateCell(4).SetCellValue(item.KPIGuidelines);
                    row.CreateCell(5).SetCellValue(item.Weight.ToString());
                    row.CreateCell(6).SetCellValue(item.Target.ToString());
                    row.CreateCell(7).SetCellValue(item.Actual.ToString());
                    row.CreateCell(8).SetCellValue(item.Rate.ToString());
                    row.CreateCell(9).SetCellValue(WeightedScore.ToString());
                    row.CreateCell(10).SetCellValue(item.Grade);
                    row.CreateCell(11).SetCellValue(item.SourceType);
                    irow++;

                    row.Cells[0].CellStyle = textStyle;
                    row.Cells[1].CellStyle = textStyle;
                    row.Cells[2].CellStyle = textStyle;
                    row.Cells[3].CellStyle = textStyle;
                    row.Cells[4].CellStyle = textStyle;
                    row.Cells[5].CellStyle = weightScoreStyle;
                    row.Cells[6].CellStyle = weightScoreStyle;
                    row.Cells[7].CellStyle = weightScoreStyle;
                    row.Cells[8].CellStyle = weightScoreStyle;
                    row.Cells[9].CellStyle = weightScoreStyle;
                    row.Cells[10].CellStyle = textStyleCenter;
                    row.Cells[11].CellStyle = textStyleCenter;

                    TotalWeight += item.Weight;
                    TotalScore += WeightedScore;
                }
                #endregion

                //TOTAL
                #region Total
                IRow rowTotal = excelSheet.CreateRow(Convert.ToInt32(irow));

                var totalHeader = new NPOI.SS.Util.CellRangeAddress(irow, irow, 7, 8);

                excelSheet.AddMergedRegion(totalHeader);

                rowTotal.CreateCell(0).SetCellValue("");
                rowTotal.CreateCell(1).SetCellValue("");
                rowTotal.CreateCell(2).SetCellValue("");
                rowTotal.CreateCell(3).SetCellValue("");
                rowTotal.CreateCell(4).SetCellValue("Total Weight");
                rowTotal.CreateCell(5).SetCellValue(TotalWeight.ToString());
                rowTotal.CreateCell(6).SetCellValue("");
                rowTotal.CreateCell(7).SetCellValue("Overall Performance");
                rowTotal.CreateCell(8).SetCellValue("");
                rowTotal.CreateCell(9).SetCellValue(TotalScore.ToString());

                XSSFCellStyle totalLabelStyle = (XSSFCellStyle)workbook.CreateCellStyle();

                var totalLabelFont = workbook.CreateFont();
                totalLabelFont.IsBold = true;
                totalLabelFont.FontHeightInPoints = 9;
                totalLabelStyle.SetFont(totalLabelFont);
                totalLabelStyle.Alignment = HorizontalAlignment.Right;

                rowTotal.Cells[4].CellStyle = totalLabelStyle;
                rowTotal.Cells[5].CellStyle = totalLabelStyle;
                rowTotal.Cells[7].CellStyle = totalLabelStyle;
                rowTotal.Cells[9].CellStyle = totalLabelStyle;
                #endregion

                XSSFCellStyle footerStyle = (XSSFCellStyle)workbook.CreateCellStyle();

                var footerFont = workbook.CreateFont();
                footerFont.FontHeightInPoints = 9;
                footerStyle.SetFont(footerFont);

                irow += 3;
                IRow footerRater = excelSheet.CreateRow(Convert.ToInt32(irow));

                var footerRaterMerged = new NPOI.SS.Util.CellRangeAddress(irow, irow, 0, 1);

                excelSheet.AddMergedRegion(footerRaterMerged);

                footerRater.CreateCell(0).SetCellValue("Discussed by:");
                footerRater.CreateCell(1).SetCellValue("");
                footerRater.CreateCell(2).SetCellValue(MyKPIScore.Requestor);

                footerRater.Cells[0].CellStyle = footerStyle;
                footerRater.Cells[1].CellStyle = footerStyle;
                footerRater.Cells[2].CellStyle = footerStyle;

                irow += 4;
                IRow footerEmployee = excelSheet.CreateRow(Convert.ToInt32(irow));

                var footerEmployeeMerged = new NPOI.SS.Util.CellRangeAddress(irow, irow, 0, 1);

                excelSheet.AddMergedRegion(footerEmployeeMerged);

                footerEmployee.CreateCell(0).SetCellValue("Conforme (employee):");
                footerEmployee.CreateCell(1).SetCellValue("");
                footerEmployee.CreateCell(2).SetCellValue(MyKPIScore.Employee);

                footerEmployee.Cells[0].CellStyle = footerStyle;
                footerEmployee.Cells[1].CellStyle = footerStyle;
                footerEmployee.Cells[2].CellStyle = footerStyle;

                workbook.Write(fs);
            }

            /*Add AuditLog*/
            await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                .AddAuditLog(new Security.Transfer.AuditLog.Form
                {
                    EventType = Common_AuditLog.EventType.EXPORT.ToString(),
                    TableName = "Employee Score",
                    TableID = 0, // New Record, no ID yet
                    Remarks = "Employee Score record exported",
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

        //public async Task<JsonResult> OnGetRunIDAutoComplete(string term, int TopResults)
        //{
        //    var result = await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env)
        //        .GetSummaryAutoComplete(new GetSummaryAutoCompleteInput { Term = term, TopResults = TopResults });
        //    _resultView.IsSuccess = true;
        //    _resultView.Result = result;
        //    return new JsonResult(_resultView);
        //}

        public async Task<IActionResult> OnGetEmployeeKPIScoreGetListAsync([FromQuery] EMS.IPM.Transfer.EmployeeScore.EmployeeKPIScoreGetListInput param)
        {

            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("EmployeeeKPIScoreGetList").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ID=", param.ID, "&",
                  "KRAGroup=", param.KRAGroup, "&",
                  "KPICode=", param.KPICode, "&",
                  "KPIName=", param.KPIName, "&",
                  "KPIDescription=", param.KPIDescription, "&",
                  "KPIGuidelines=", param.KPIGuidelines, "&",
                  "WeightMin=", param.WeightMin, "&",
                  "WeightMax=", param.WeightMax, "&",
                  "TargetMin=", param.TargetMin, "&",
                  "TargetMax=", param.TargetMax, "&",
                  "ActualMin=", param.ActualMin, "&",
                  "ActualMax=", param.ActualMax, "&",
                  "RateMin=", param.RateMin, "&",
                  "RateMax=", param.RateMax, "&",
                  "TotalMin=", param.TotalMin, "&",
                  "TotalMax=", param.TotalMax, "&",
                  "GradeDelimited=", param.GradeDelimited, "&",
                  "SourceTypeDelimited=", param.SourceTypeDelimited);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<EMS.IPM.Transfer.EmployeeScore.EmployeeKPIScoreGetListOutput>(), URL);

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

        public async Task<JsonResult> OnGetRatingGrades()
        {
            _resultView.Result = await new Common_RatingTable(_iconfiguration, _globalCurrentUser, _env).GetAll();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }


    }
}