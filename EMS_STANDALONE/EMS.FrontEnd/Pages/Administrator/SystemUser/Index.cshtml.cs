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
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;
using Microsoft.AspNetCore.Http;

namespace EMS.FrontEnd.Pages.Administrator.SystemUser
{
    public class IndexModel : SharedClasses.Utilities
    {
        [BindProperty]
        public BatchResetPasswordForm Form { get; set; }
        [BindProperty]
        public ChangeStatusForm ChangeStatusForm { get; set; }
        [BindProperty]
        public ResetPassword resetPassword { get; set; }


        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/SYSTEMUSER/ADD")).Count() > 0 ? "true" : "false";
                ViewData["HasBatchResetPasswordFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/SYSTEMUSER?HANDLER=BATCHRESETPASSWORD")).Count() > 0 ? "true" : "false";
                ViewData["HasChangeStatusFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/SYSTEMUSER?HANDLER=CHANGESTATUS")).Count() > 0 ? "true" : "false";
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
            var URL = string.Concat(_securityBaseURL,
                  _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "Username=", param.Username, "&",
                  "Name=", param.Name, "&",
                  "Status=", param.Status, "&",
                  "DateModifiedFrom=", param.DateModifiedFrom, "&",
                  "DateModifiedTo=", param.DateModifiedTo, "&",
                  "DateCreatedFrom=", param.DateCreatedFrom, "&",
                  "DateCreatedTo=", param.DateCreatedTo, "&",
                  "IsExport=", IsExport);

            return await SharedUtilities.GetFromAPI(new List<GetListOutput>(), URL);
        }

        public async Task<JsonResult> OnGetSystemRoleDropDown()
        {
            var result = await new Common_SystemRole(_iconfiguration, _globalCurrentUser, _env).GetSystemRoleDropDown();
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetSystemUserRoleDropDownByUserID(int ID)
        {
            var result = await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env).GetSystemUserRoleDropDownByUserID(ID);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostBatchResetPassword()
        {
            Form.ModifiedBy = _globalCurrentUser.UserID;
            
            var URL = string.Concat(_securityBaseURL,
                    _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("BatchResetPassword").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(Form, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
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
                    row.CreateCell(1).SetCellValue("User Name");
                    row.CreateCell(2).SetCellValue("Name");
                    row.CreateCell(3).SetCellValue("Status");
                    row.CreateCell(4).SetCellValue("Modified Date");
                    row.CreateCell(5).SetCellValue("Created Date");

                    excelSheet.SetColumnWidth(0, 3000);
                    excelSheet.SetColumnWidth(1, 8000);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 3000);
                    excelSheet.SetColumnWidth(4, 5000);
                    excelSheet.SetColumnWidth(5, 5000);

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
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(Convert.ToInt32(item.ID));
                        row.CreateCell(1).SetCellValue(item.Username);
                        row.CreateCell(2).SetCellValue(item.Name);
                        row.CreateCell(3).SetCellValue(item.Status);
                        row.CreateCell(4).SetCellValue(item.DateModified);
                        row.CreateCell(5).SetCellValue(item.DateCreated);

                        row.Cells[0].CellStyle = alignCenter;
                        row.Cells[3].CellStyle = alignCenter;
                        row.Cells[4].CellStyle = alignCenter;
                        row.Cells[5].CellStyle = alignCenter;

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
                        TableName = "SystemUser",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "System User Exported",
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

        public async Task<JsonResult> OnPostChangeStatus()
        {
            ChangeStatusForm.ModifiedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_securityBaseURL,
                    _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("ChangeStatus").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(ChangeStatusForm, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostResetPassword()
        {
            var URL = string.Concat(_securityBaseURL,
                    _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("ResetPassword").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(resetPassword, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}