using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Workflow.Transfer.LogActivity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Utilities.API;

namespace EMS.FrontEnd.Pages.LogActivity.Preloaded
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/PRELOADED/ADD")).Count() > 0 ? "true" : "false";
            }
        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] GetPreLoadedListInput param)
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

        public async Task<(List<GetPreLoadedListOutput>, bool, string)> GetExportData([FromQuery] GetPreLoadedListInput param, bool IsExport)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetLogActivityPreloadedList").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "PreloadName=", param.PreloadName, "&",
                  "DateCreatedFrom=", param.DateCreatedFrom, "&",
                  "DateCreatedTo=", param.DateCreatedTo, "&",
                  "IsExport=", IsExport);

            return await SharedUtilities.GetFromAPI(new List<GetPreLoadedListOutput>(), URL);

        }

        public async Task<JsonResult> OnGetPreloadedItems(int ID)
        {

            List<GetLogActivityByPreloadedIDOutput> res = await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env).GetPreloadedItemsByID(ID);

            // Get User Names by User IDs
            List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
               await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
               .GetSystemUserByIDs(res.Where(x => x.AssignedUserID != 0).Select(x => x.AssignedUserID)
               .Distinct().ToList());

            if (systemUsers.Count > 0)
            {

                res = res
                       .GroupJoin(systemUsers,
                       x => new { x.AssignedUserID },
                       y => new { AssignedUserID = y.ID },
                       (x, y) => new { preloaded = x, systemuser = y })
                       .SelectMany(x => x.systemuser.DefaultIfEmpty(),
                       (x, y) => new { preloaded = x, systemuser = y })
                       .Select(x => new GetLogActivityByPreloadedIDOutput
                       {

                           Module = x.preloaded.preloaded.Module,
                           Type = x.preloaded.preloaded.Type,
                           SubType = x.preloaded.preloaded.SubType,
                           Title = x.preloaded.preloaded.Title,
                           Description = x.preloaded.preloaded.Description,
                           IsPassFail = x.preloaded.preloaded.IsPassFail,
                           IsAssignment = x.preloaded.preloaded.IsAssignment,
                           IsVisible = x.preloaded.preloaded.IsVisible,
                           AssignedUserID = x.preloaded.preloaded.AssignedUserID,
                           AssignedUserName = x.systemuser == null ? "" : string.Concat(x.systemuser.LastName,
                               string.IsNullOrEmpty(x.systemuser.FirstName) ? "" : string.Concat(", ", x.systemuser.FirstName),
                               string.IsNullOrEmpty(x.systemuser.MiddleName) ? "" : string.Concat(" ", x.systemuser.MiddleName))
                       }).ToList();
            }

            _resultView.Result = res;
            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetSubType(string Type)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                    .GetReferenceValueByRefCode(Type);
            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetAssignedUser(string Term, int TopResults)
        {
            var result = await new Common_Synced_SystemUser(_iconfiguration, _globalCurrentUser, _env).GetSystemUserAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetCheckExportList([FromQuery] GetPreLoadedListInput param)
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

        public async Task<IActionResult> OnGetDownloadExportList([FromQuery] GetPreLoadedListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetExportData(param, true);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "Preloaded Task.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Preloaded Task");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("ID");
                    row.CreateCell(1).SetCellValue("Preloaded Task Name");
                    row.CreateCell(2).SetCellValue("Created Date");

                    excelSheet.SetColumnWidth(0, 3000);
                    excelSheet.SetColumnWidth(1, 10000);
                    excelSheet.SetColumnWidth(2, 6000);

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
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(Convert.ToInt32(item.ID));
                        row.CreateCell(1).SetCellValue(item.PreloadName);
                        row.CreateCell(2).SetCellValue(item.DateCreated);

                        row.Cells[0].CellStyle = alignCenter;
                        row.Cells[2].CellStyle = alignCenter;

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
                        TableName = "LogActivityPreloaded",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Preloaded Task Exported",
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