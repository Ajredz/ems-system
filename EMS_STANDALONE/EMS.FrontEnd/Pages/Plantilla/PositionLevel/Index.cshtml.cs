using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.Plantilla.Transfer.PositionLevel;
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

namespace EMS.FrontEnd.Pages.Plantilla.PositionLevel
{
    public class IndexModel : SharedClasses.Utilities
    {

        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/POSITIONLEVEL/ADD")).Count() > 0 ? "true" : "false";
            }
        }

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

        public async Task<(List<GetListOutput>, bool, string)> GetExportData([FromQuery] GetListInput param, bool IsExport)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PositionLevel").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ID=", param.ID, "&",
                  "Description=", param.Description, "&",
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
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "PositionLevel.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Position Level");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("ID");
                    row.CreateCell(1).SetCellValue("Description");

                    excelSheet.SetColumnWidth(0, 3000);
                    excelSheet.SetColumnWidth(1, 7000);

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
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(Convert.ToInt32(item.ID));
                        row.CreateCell(1).SetCellValue(item.Description);

                        row.Cells[0].CellStyle = alignCenter;

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
                        TableName = "PositionLevel",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Position Level Exported",
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