using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Security.Transfer.SystemUser;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Utilities.API;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;

namespace EMS.FrontEnd.Pages.Administrator.EmployeeCorporateEmail
{
    public class IndexModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.Employee.EmployeeAttachmentForm EmployeeAttachmentForm { get; set; }


        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] EMS.Plantilla.Transfer.Employee.GetListCorporateEmailInput param)
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

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        private async Task<(List<EMS.Plantilla.Transfer.Employee.GetListCorporateEmailOutput>, bool, string)> GetDataList([FromQuery] EMS.Plantilla.Transfer.Employee.GetListCorporateEmailInput param, bool IsExport)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("ListCorporateEmail").Value, "?",
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
                  "CorporateEmail=", param.CorporateEmail, "&",
                  "OfficeMobile=", param.OfficeMobile, "&",
                  "IsDisplayDirectory=", param.IsDisplayDirectory, "&",
                  "OldEmployeeID=", param.OldEmployeeID, "&",
                  "IsExport=", IsExport);

            return await SharedUtilities.GetFromAPI(new List<EMS.Plantilla.Transfer.Employee.GetListCorporateEmailOutput>(), URL);
        }

        public async Task<IActionResult> OnGetCheckEmployeeExportListAsync([FromQuery] EMS.Plantilla.Transfer.Employee.GetListCorporateEmailInput param, bool IsExport)
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
        
        public async Task<IActionResult> OnGetDownloadEmployeeExportListAsync([FromQuery] EMS.Plantilla.Transfer.Employee.GetListCorporateEmailInput param)
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
                    row.CreateCell(3).SetCellValue("Corporate Email");
                    row.CreateCell(4).SetCellValue("Office Mobile");
                    row.CreateCell(5).SetCellValue("Organizational Group");
                    row.CreateCell(6).SetCellValue("Position");
                    row.CreateCell(7).SetCellValue("Employment Status");
                    row.CreateCell(8).SetCellValue("Show In Corporate Directory");
                    row.CreateCell(9).SetCellValue("Old Employee ID");

                    excelSheet.SetColumnWidth(0, 3500);
                    excelSheet.SetColumnWidth(1, 5500);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 10000);
                    excelSheet.SetColumnWidth(4, 10000);
                    excelSheet.SetColumnWidth(5, 6500);
                    excelSheet.SetColumnWidth(6, 4500);
                    excelSheet.SetColumnWidth(7, 5500);
                    excelSheet.SetColumnWidth(8, 4500);
                    excelSheet.SetColumnWidth(9, 4500);

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
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(Convert.ToDouble(item.ID));
                        row.CreateCell(1).SetCellValue(item.Code);
                        row.CreateCell(2).SetCellValue(item.Name);
                        row.CreateCell(3).SetCellValue(item.CorporateEmail);
                        row.CreateCell(4).SetCellValue(item.OfficeMobile);
                        row.CreateCell(5).SetCellValue(item.OrgGroup);
                        row.CreateCell(6).SetCellValue(item.Position);
                        row.CreateCell(7).SetCellValue(item.EmploymentStatus);
                        row.CreateCell(8).SetCellValue(item.IsDisplayDirectory);
                        row.CreateCell(9).SetCellValue(item.OldEmployeeID);

                        //row.Cells[0].CellStyle = alignCenter;
                        //row.Cells[5].CellStyle = alignCenter;
                        //row.Cells[6].CellStyle = alignCenter;
                        //row.Cells[7].CellStyle = alignCenter;
                        //row.Cells[8].CellStyle = alignCenter;
                        //row.Cells[8].CellStyle = alignCenter;

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

        public async Task<JsonResult> OnGetOrgGroupHierarchy(int ID)
        {
            _resultView.Result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupHierarchy(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
    }
}