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
using EMS.IPM.Transfer.EmployeeScore;
using EMS.FrontEnd.SharedClasses.Common_IPM;
using EMS.FrontEnd.SharedClasses;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.IPM.Transfer.EmployeeScoreDashboard;

namespace EMS.FrontEnd.Pages.IPM.EmployeeScoreDashboard
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet()
        {
            if (_globalCurrentUser != null)
            {
                var dashboardType = await new Common_Reference(_iconfiguration, _globalCurrentUser, _ipmBaseURL, _env).GetReferenceValueByRefCode(
                             EMS.IPM.Transfer.Enums.ReferenceCodes.DASHBOARD.ToString());

                ViewData["DashboardSelectList"] = dashboardType.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description
                }).ToList(); 
            }
        }

        public async Task<JsonResult> OnGetOrgGroupAutoComplete(string term, int TopResults, string Filter)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupFilteredAutoComplete(term, TopResults, Filter);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
         public async Task<JsonResult> OnGetBranchAutoComplete(string term, int TopResults)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupBranchAutoComplete(new EMS.IPM.Transfer.Shared.GetAutoCompleteInput { Term = term, TopResults = TopResults });
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
        
        public async Task<JsonResult> OnGetPositionAutoComplete(string term, int TopResults)
        {
            var result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env)
                .GetPositionAutoComplete( term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }  
        
        public async Task<JsonResult> OnGetKRAAutoComplete(string term, int TopResults)
        {
            var result = await new Common_KPI(_iconfiguration, _globalCurrentUser, _env)
                .GetKRAGroupAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        } 
        
        public async Task<JsonResult> OnGetKPIAutoComplete(string term, int TopResults)
        {
            var result = await new Common_KPI(_iconfiguration, _globalCurrentUser, _env)
                .GetKPIAutoComplete( term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        } 
        
        public async Task<JsonResult> OnGetRunIDAutoComplete(string term, int TopResults)
        {
            var result = await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env)
                .GetSummaryAutoComplete(new GetSummaryAutoCompleteInput { Term = term, TopResults = TopResults, IsAdmin = true});
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        #region Summary For Evaluation
        
        public async Task<IActionResult> OnGetListSummaryForEvaluation([FromQuery] GetListSummaryForEvaluationInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataListSummaryForEvaluation(param);

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

        private async Task<(List<GetListSummaryForEvaluationOutput>, bool, string)> GetDataListSummaryForEvaluation(GetListSummaryForEvaluationInput param)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScoreDashboard").GetSection("GetListSummaryForEvaluation").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",
                  "IsExport=", param.IsExport, "&",

                  "RunIDDelimited=", param.RunIDDelimited, "&",
                  "RegionDelimited=", param.RegionDelimited, "&",
                  "WithCompleteScoreMin=", param.WithCompleteScoreMin, "&",
                  "WithCompleteScoreMax=", param.WithCompleteScoreMax, "&",
                  "WithMissingScoreMin=", param.WithMissingScoreMin, "&",
                  "WithMissingScoreMax=", param.WithMissingScoreMax, "&",
                  "NoScoreMin=", param.NoScoreMin, "&",
                  "NoScoreMax=", param.NoScoreMax, "&",
                  "OnGoingEvalMin=", param.OnGoingEvalMin, "&",
                  "OnGoingEvalMax=", param.OnGoingEvalMax
                  );

            return await SharedUtilities.GetFromAPI(new List<GetListSummaryForEvaluationOutput>(), URL);
        }

        public async Task<IActionResult> OnGetCheckExportSummaryForEvaluationList([FromQuery] GetListSummaryForEvaluationInput param)
        {
            param.IsExport = true;
            var (Result, IsSuccess, ErrorMessage) = await GetDataListSummaryForEvaluation(param);

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

        public async Task<IActionResult> OnGetDownloadExportSummaryForEvaluationList([FromQuery] GetListSummaryForEvaluationInput param)
        {
            param.IsExport = true;
            var (Result, IsSuccess, ErrorMessage) = await GetDataListSummaryForEvaluation(param);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "Summary For Evaluation List.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Summary For Evaluation List");
                    int irow = 0;
                    long totalWithCompleteScore = 0;
                    long totalWithMissingScore = 0;
                    long totalNoScore = 0;
                    long totalOnGoingEval = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Region");
                    row.CreateCell(1).SetCellValue("With Complete Score");
                    row.CreateCell(2).SetCellValue("With Missing Score");
                    row.CreateCell(3).SetCellValue("No Score");
                    row.CreateCell(4).SetCellValue("On Going Evaluation");

                    excelSheet.SetColumnWidth(0, 6000);
                    excelSheet.SetColumnWidth(1, 6000);
                    excelSheet.SetColumnWidth(2, 6000);
                    excelSheet.SetColumnWidth(3, 6000);
                    excelSheet.SetColumnWidth(4, 6000);

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
                        row.CreateCell(0).SetCellValue(item.Region);
                        row.CreateCell(1).SetCellValue(item.WithCompleteScore);
                        row.CreateCell(2).SetCellValue(item.WithMissingScore);
                        row.CreateCell(3).SetCellValue(item.NoScore);
                        row.CreateCell(4).SetCellValue(item.OnGoingEvaluation);
                        irow++;

                        totalWithCompleteScore += item.WithCompleteScore;
                        totalWithMissingScore += item.WithMissingScore;
                        totalNoScore += item.NoScore;
                        totalOnGoingEval += item.OnGoingEvaluation;
                    }
                    #endregion

                    XSSFCellStyle footerStyle = (XSSFCellStyle)workbook.CreateCellStyle();

                    var footerFont = workbook.CreateFont();
                    footerFont.FontHeightInPoints = 16;
                    footerStyle.SetFont(footerFont);
                    footerStyle.Alignment = HorizontalAlignment.Right;

                    IRow rowTotal = excelSheet.CreateRow(Convert.ToInt32(irow));
                    rowTotal.CreateCell(0).SetCellValue("TOTAL:");
                    rowTotal.CreateCell(1).SetCellValue(totalWithCompleteScore);
                    rowTotal.CreateCell(2).SetCellValue(totalWithMissingScore);
                    rowTotal.CreateCell(3).SetCellValue(totalNoScore);
                    rowTotal.CreateCell(4).SetCellValue(totalOnGoingEval);
                    
                    rowTotal.Cells[0].CellStyle = footerStyle;
                    rowTotal.Cells[1].CellStyle = footerStyle;
                    rowTotal.Cells[2].CellStyle = footerStyle;
                    rowTotal.Cells[3].CellStyle = footerStyle;
                    rowTotal.Cells[4].CellStyle = footerStyle;

                    workbook.Write(fs);
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EXPORT.ToString(),
                        TableName = "Dashboard",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Summary For Evaluation exported",
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

        #endregion

        #region Summary For Approval (Region)

        public async Task<IActionResult> OnGetListSummaryForApproval([FromQuery] GetListSummaryForApprovalInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataListSummaryForApproval(param);

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

        private async Task<(List<GetListSummaryForApprovalOutput>, bool, string)> GetDataListSummaryForApproval(GetListSummaryForApprovalInput param)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScoreDashboard").GetSection("GetListSummaryForApproval").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",
                  "IsExport=", param.IsExport, "&",

                  "RunIDDelimited=", param.RunIDDelimited, "&",
                  "RegionDelimited=", param.RegionDelimited, "&",
                  "NoKeyInMin=", param.NoKeyInMin, "&",
                  "NoKeyInMax=", param.NoKeyInMax, "&",
                  "ForApprovalMin=", param.ForApprovalMin, "&",
                  "ForApprovalMax=", param.ForApprovalMax, "&",
                  "FinalizedMin=", param.FinalizedMin, "&",
                  "FinalizedMax=", param.FinalizedMax);

            return await SharedUtilities.GetFromAPI(new List<GetListSummaryForApprovalOutput>(), URL);
        }

        public async Task<IActionResult> OnGetCheckExportSummaryForApprovalList([FromQuery] GetListSummaryForApprovalInput param)
        {
            param.IsExport = true;
            var (Result, IsSuccess, ErrorMessage) = await GetDataListSummaryForApproval(param);

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

        public async Task<IActionResult> OnGetDownloadExportSummaryForApprovalList([FromQuery] GetListSummaryForApprovalInput param)
        {
            param.IsExport = true;
            var (Result, IsSuccess, ErrorMessage) = await GetDataListSummaryForApproval(param);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "Summary For Approval List.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Summary For Approval List");
                    int irow = 0;
                    long totalNoKeyIn = 0;
                    long totalForApproval = 0;
                    long totalFinalized = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Region");
                    row.CreateCell(1).SetCellValue("No Key In");
                    row.CreateCell(2).SetCellValue("For Approval");
                    row.CreateCell(3).SetCellValue("Finalized");

                    excelSheet.SetColumnWidth(0, 6000);
                    excelSheet.SetColumnWidth(1, 6000);
                    excelSheet.SetColumnWidth(2, 6000);
                    excelSheet.SetColumnWidth(3, 6000);

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

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.Region);
                        row.CreateCell(1).SetCellValue(item.NoKeyIn);
                        row.CreateCell(2).SetCellValue(item.ForApproval);
                        row.CreateCell(3).SetCellValue(item.Finalized);
                        irow++;

                        totalNoKeyIn += item.NoKeyIn;
                        totalForApproval += item.ForApproval;
                        totalFinalized += item.Finalized;
                    }
                    #endregion



                    XSSFCellStyle footerStyle = (XSSFCellStyle)workbook.CreateCellStyle();

                    var footerFont = workbook.CreateFont();
                    footerFont.FontHeightInPoints = 16;
                    footerStyle.SetFont(footerFont);
                    footerStyle.Alignment = HorizontalAlignment.Right;

                    IRow rowTotal = excelSheet.CreateRow(Convert.ToInt32(irow));
                    rowTotal.CreateCell(0).SetCellValue("TOTAL:");
                    rowTotal.CreateCell(1).SetCellValue(totalNoKeyIn);
                    rowTotal.CreateCell(2).SetCellValue(totalForApproval);
                    rowTotal.CreateCell(3).SetCellValue(totalFinalized);

                    rowTotal.Cells[0].CellStyle = footerStyle;
                    rowTotal.Cells[1].CellStyle = footerStyle;
                    rowTotal.Cells[2].CellStyle = footerStyle;
                    rowTotal.Cells[3].CellStyle = footerStyle;

                    workbook.Write(fs);
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EXPORT.ToString(),
                        TableName = "Dashboard",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Summary For Approval exported",
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

        #endregion

        #region Summary For Approval (Branch)

        public async Task<IActionResult> OnGetListSummaryForApprovalBRN([FromQuery] GetListSummaryForApprovalBRNInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataListSummaryForApprovalBRN(param);

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

        private async Task<(List<GetListSummaryForApprovalBRNOutput>, bool, string)> GetDataListSummaryForApprovalBRN(GetListSummaryForApprovalBRNInput param)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScoreDashboard").GetSection("GetListSummaryForApprovalBRN").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",
                  "IsExport=", param.IsExport, "&",

                  "RunIDDelimited=", param.RunIDDelimited, "&",
                  "RegionDelimited=", param.RegionDelimited, "&",
                  "BranchDelimited=", param.BranchDelimited, "&",
                  "NoKeyInMin=", param.NoKeyInMin, "&",
                  "NoKeyInMax=", param.NoKeyInMax, "&",
                  "ForApprovalMin=", param.ForApprovalMin, "&",
                  "ForApprovalMax=", param.ForApprovalMax, "&",
                  "FinalizedMin=", param.FinalizedMin, "&",
                  "FinalizedMax=", param.FinalizedMax);

            return await SharedUtilities.GetFromAPI(new List<GetListSummaryForApprovalBRNOutput>(), URL);
        }

        public async Task<IActionResult> OnGetCheckExportSummaryForApprovalBRNList([FromQuery] GetListSummaryForApprovalBRNInput param)
        {
            param.IsExport = true;
            var (Result, IsSuccess, ErrorMessage) = await GetDataListSummaryForApprovalBRN(param);

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

        public async Task<IActionResult> OnGetDownloadExportSummaryForApprovalBRNList([FromQuery] GetListSummaryForApprovalBRNInput param)
        {
            param.IsExport = true;
            var (Result, IsSuccess, ErrorMessage) = await GetDataListSummaryForApprovalBRN(param);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "Summary For Approval (BRN).xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Summary For Approval (BRN)");
                    int irow = 0;
                    long totalNoKeyIn = 0;
                    long totalForApproval = 0;
                    long totalFinalized = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Region");
                    row.CreateCell(1).SetCellValue("Branches");
                    row.CreateCell(2).SetCellValue("No Key In");
                    row.CreateCell(3).SetCellValue("For Approval");
                    row.CreateCell(4).SetCellValue("Finalized");

                    excelSheet.SetColumnWidth(0, 6000);
                    excelSheet.SetColumnWidth(1, 6000);
                    excelSheet.SetColumnWidth(2, 6000);
                    excelSheet.SetColumnWidth(3, 6000);
                    excelSheet.SetColumnWidth(4, 6000);

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
                        row.CreateCell(0).SetCellValue(item.Region);
                        row.CreateCell(1).SetCellValue(item.Branch);
                        row.CreateCell(2).SetCellValue(item.NoKeyIn);
                        row.CreateCell(3).SetCellValue(item.ForApproval);
                        row.CreateCell(4).SetCellValue(item.Finalized);
                        irow++;

                        totalNoKeyIn += item.NoKeyIn;
                        totalForApproval += item.ForApproval;
                        totalFinalized += item.Finalized;
                    }
                    #endregion



                    XSSFCellStyle footerStyle = (XSSFCellStyle)workbook.CreateCellStyle();

                    var footerFont = workbook.CreateFont();
                    footerFont.FontHeightInPoints = 16;
                    footerStyle.SetFont(footerFont);
                    footerStyle.Alignment = HorizontalAlignment.Right;

                    IRow rowTotal = excelSheet.CreateRow(Convert.ToInt32(irow));
                    rowTotal.CreateCell(0).SetCellValue("");
                    rowTotal.CreateCell(1).SetCellValue("TOTAL:");
                    rowTotal.CreateCell(2).SetCellValue(totalNoKeyIn);
                    rowTotal.CreateCell(3).SetCellValue(totalForApproval);
                    rowTotal.CreateCell(4).SetCellValue(totalFinalized);

                    rowTotal.Cells[0].CellStyle = footerStyle;
                    rowTotal.Cells[1].CellStyle = footerStyle;
                    rowTotal.Cells[2].CellStyle = footerStyle;
                    rowTotal.Cells[3].CellStyle = footerStyle;
                    rowTotal.Cells[4].CellStyle = footerStyle;

                    workbook.Write(fs);
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EXPORT.ToString(),
                        TableName = "Dashboard",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Summary For Approval (BRN) exported",
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

        #endregion 

        #region Summary For Approval (Cluster)

        public async Task<IActionResult> OnGetListSummaryForApprovalCLU([FromQuery] GetListSummaryForApprovalCLUInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataListSummaryForApprovalCLU(param);

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

        private async Task<(List<GetListSummaryForApprovalCLUOutput>, bool, string)> GetDataListSummaryForApprovalCLU(GetListSummaryForApprovalCLUInput param)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScoreDashboard").GetSection("GetListSummaryForApprovalCLU").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",
                  "IsExport=", param.IsExport, "&",

                  "RunIDDelimited=", param.RunIDDelimited, "&",
                  "RegionDelimited=", param.RegionDelimited, "&",
                  "ClusterDelimited=", param.ClusterDelimited, "&",
                  "NoKeyInMin=", param.NoKeyInMin, "&",
                  "NoKeyInMax=", param.NoKeyInMax, "&",
                  "ForApprovalMin=", param.ForApprovalMin, "&",
                  "ForApprovalMax=", param.ForApprovalMax, "&",
                  "FinalizedMin=", param.FinalizedMin, "&",
                  "FinalizedMax=", param.FinalizedMax);

            return await SharedUtilities.GetFromAPI(new List<GetListSummaryForApprovalCLUOutput>(), URL);
        }

        public async Task<IActionResult> OnGetCheckExportSummaryForApprovalCLUList([FromQuery] GetListSummaryForApprovalCLUInput param)
        {
            param.IsExport = true;
            var (Result, IsSuccess, ErrorMessage) = await GetDataListSummaryForApprovalCLU(param);

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

        public async Task<IActionResult> OnGetDownloadExportSummaryForApprovalCLUList([FromQuery] GetListSummaryForApprovalCLUInput param)
        {
            param.IsExport = true;
            var (Result, IsSuccess, ErrorMessage) = await GetDataListSummaryForApprovalCLU(param);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "Summary For Approval (CLU).xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Summary For Approval (CLU)");
                    int irow = 0;
                    long totalNoKeyIn = 0;
                    long totalForApproval = 0;
                    long totalFinalized = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Region");
                    row.CreateCell(1).SetCellValue("Cluster");
                    row.CreateCell(2).SetCellValue("No Key In");
                    row.CreateCell(3).SetCellValue("For Approval");
                    row.CreateCell(4).SetCellValue("Finalized");

                    excelSheet.SetColumnWidth(0, 6000);
                    excelSheet.SetColumnWidth(1, 6000);
                    excelSheet.SetColumnWidth(2, 6000);
                    excelSheet.SetColumnWidth(3, 6000);
                    excelSheet.SetColumnWidth(4, 6000);

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
                        row.CreateCell(0).SetCellValue(item.Region);
                        row.CreateCell(1).SetCellValue(item.Cluster);
                        row.CreateCell(2).SetCellValue(item.NoKeyIn);
                        row.CreateCell(3).SetCellValue(item.ForApproval);
                        row.CreateCell(4).SetCellValue(item.Finalized);
                        irow++;

                        totalNoKeyIn += item.NoKeyIn;
                        totalForApproval += item.ForApproval;
                        totalFinalized += item.Finalized;
                    }
                    #endregion



                    XSSFCellStyle footerStyle = (XSSFCellStyle)workbook.CreateCellStyle();

                    var footerFont = workbook.CreateFont();
                    footerFont.FontHeightInPoints = 16;
                    footerStyle.SetFont(footerFont);
                    footerStyle.Alignment = HorizontalAlignment.Right;

                    IRow rowTotal = excelSheet.CreateRow(Convert.ToInt32(irow));
                    rowTotal.CreateCell(0).SetCellValue("TOTAL:");
                    rowTotal.CreateCell(1).SetCellValue("TOTAL:");
                    rowTotal.CreateCell(2).SetCellValue(totalNoKeyIn);
                    rowTotal.CreateCell(3).SetCellValue(totalForApproval);
                    rowTotal.CreateCell(4).SetCellValue(totalFinalized);

                    rowTotal.Cells[0].CellStyle = footerStyle;
                    rowTotal.Cells[1].CellStyle = footerStyle;
                    rowTotal.Cells[2].CellStyle = footerStyle;
                    rowTotal.Cells[3].CellStyle = footerStyle;
                    rowTotal.Cells[4].CellStyle = footerStyle;

                    workbook.Write(fs);
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EXPORT.ToString(),
                        TableName = "Dashboard",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Summary For Approval (CLU) exported",
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

        #endregion 


        #region Summary Of Results Regional With Position

        public async Task<IActionResult> OnGetListRegionalWithPosition([FromQuery] GetListRegionalWithPositionInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataListRegionalWithPosition(param);

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

        private async Task<(List<GetListRegionalWithPositionOutput>, bool, string)> GetDataListRegionalWithPosition(GetListRegionalWithPositionInput param)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScoreDashboard").GetSection("GetListRegionalWithPosition").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",
                  "IsExport=", param.IsExport, "&",

                  "RunIDDelimited=", param.RunIDDelimited, "&",
                  "RegionDelimited=", param.RegionDelimited, "&",
                  "PositionDelimited=", param.PositionDelimited, "&",
                  "KRAGroupDelimited=", param.KRAGroupDelimited, "&",
                  "KPIDelimited=", param.KPIDelimited, "&",
                  "EEMin=", param.EEMin, "&",
                  "EEMax=", param.EEMax, "&",
                  "MEMin=", param.MEMin, "&",
                  "MEMax=", param.MEMax, "&",
                  "SBEMin=", param.SBEMin, "&",
                  "SBEMax=", param.SBEMax, "&",
                  "BEMin=", param.BEMin, "&",
                  "BEMax=", param.BEMax);

            return await SharedUtilities.GetFromAPI(new List<GetListRegionalWithPositionOutput>(), URL);
        }

        public async Task<IActionResult> OnGetCheckExportRegionalWithPositionList([FromQuery] GetListRegionalWithPositionInput param)
        {
            param.IsExport = true;
            var (Result, IsSuccess, ErrorMessage) = await GetDataListRegionalWithPosition(param);

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

        public async Task<IActionResult> OnGetDownloadExportRegionalWithPositionList([FromQuery] GetListRegionalWithPositionInput param)
        {
            param.IsExport = true;
            var (Result, IsSuccess, ErrorMessage) = await GetDataListRegionalWithPosition(param);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "Summary Of Results Regional With Position List.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Regional With Position List");
                    int irow = 0;
                    long totalEE = 0;
                    long totalME = 0;
                    long totalSBE = 0;
                    long totalBE = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Region");
                    row.CreateCell(1).SetCellValue("Postition");
                    row.CreateCell(2).SetCellValue("KRA");
                    row.CreateCell(3).SetCellValue("KPI Name");
                    row.CreateCell(4).SetCellValue("EE");
                    row.CreateCell(5).SetCellValue("ME");
                    row.CreateCell(6).SetCellValue("SBE");
                    row.CreateCell(7).SetCellValue("BE");

                    excelSheet.SetColumnWidth(0, 6000);
                    excelSheet.SetColumnWidth(1, 10000);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 10000);
                    excelSheet.SetColumnWidth(4, 3000);
                    excelSheet.SetColumnWidth(5, 3000);
                    excelSheet.SetColumnWidth(6, 3000);
                    excelSheet.SetColumnWidth(7, 3000);

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
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.Region);
                        row.CreateCell(1).SetCellValue(item.Position);
                        row.CreateCell(2).SetCellValue(item.KRAGroup);
                        row.CreateCell(3).SetCellValue(item.KPI);
                        row.CreateCell(4).SetCellValue(item.EE);
                        row.CreateCell(5).SetCellValue(item.ME);
                        row.CreateCell(6).SetCellValue(item.SBE);
                        row.CreateCell(7).SetCellValue(item.BE);
                        irow++;

                        totalEE += item.EE;
                        totalME += item.ME;
                        totalSBE += item.SBE;
                        totalBE += item.BE;
                    }
                    #endregion



                    XSSFCellStyle footerStyle = (XSSFCellStyle)workbook.CreateCellStyle();

                    var footerFont = workbook.CreateFont();
                    footerFont.FontHeightInPoints = 16;
                    footerStyle.SetFont(footerFont);
                    footerStyle.Alignment = HorizontalAlignment.Right;

                    IRow rowTotal = excelSheet.CreateRow(Convert.ToInt32(irow));
                    rowTotal.CreateCell(0).SetCellValue("");
                    rowTotal.CreateCell(1).SetCellValue("");
                    rowTotal.CreateCell(2).SetCellValue("");
                    rowTotal.CreateCell(3).SetCellValue("TOTAL:");
                    rowTotal.CreateCell(4).SetCellValue(totalEE);
                    rowTotal.CreateCell(5).SetCellValue(totalME);
                    rowTotal.CreateCell(6).SetCellValue(totalSBE);
                    rowTotal.CreateCell(7).SetCellValue(totalBE);

                    rowTotal.Cells[3].CellStyle = footerStyle;
                    rowTotal.Cells[4].CellStyle = footerStyle;
                    rowTotal.Cells[5].CellStyle = footerStyle;
                    rowTotal.Cells[6].CellStyle = footerStyle;
                    rowTotal.Cells[7].CellStyle = footerStyle;

                    workbook.Write(fs);
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EXPORT.ToString(),
                        TableName = "Dashboard",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Summary Of Results Regional With Position exported",
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

        #endregion

        #region Summary Of Results Branches With Position

        public async Task<IActionResult> OnGetListBranchesWithPosition([FromQuery] GetListBranchesWithPositionInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataListBranchesWithPosition(param);

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

        private async Task<(List<GetListBranchesWithPositionOutput>, bool, string)> GetDataListBranchesWithPosition(GetListBranchesWithPositionInput param)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScoreDashboard").GetSection("GetListBranchesWithPosition").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",
                  "IsExport=", param.IsExport, "&",

                  "RunIDDelimited=", param.RunIDDelimited, "&",
                  "BranchDelimited=", param.BranchDelimited, "&",
                  "PositionDelimited=", param.PositionDelimited, "&",
                  "KRAGroupDelimited=", param.KRAGroupDelimited, "&",
                  "KPIDelimited=", param.KPIDelimited, "&",
                  "EEMin=", param.EEMin, "&",
                  "EEMax=", param.EEMax, "&",
                  "MEMin=", param.MEMin, "&",
                  "MEMax=", param.MEMax, "&",
                  "SBEMin=", param.SBEMin, "&",
                  "SBEMax=", param.SBEMax, "&",
                  "BEMin=", param.BEMin, "&",
                  "BEMax=", param.BEMax);

            return await SharedUtilities.GetFromAPI(new List<GetListBranchesWithPositionOutput>(), URL);
        }

        public async Task<IActionResult> OnGetCheckExportBranchesWithPositionList([FromQuery] GetListBranchesWithPositionInput param)
        {
            param.IsExport = true;
            var (Result, IsSuccess, ErrorMessage) = await GetDataListBranchesWithPosition(param);

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

        public async Task<IActionResult> OnGetDownloadExportBranchesWithPositionList([FromQuery] GetListBranchesWithPositionInput param)
        {
            param.IsExport = true;
            var (Result, IsSuccess, ErrorMessage) = await GetDataListBranchesWithPosition(param);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "Summary Of Results Branches With Position List.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Branches With Position List");
                    int irow = 0;
                    long totalEE = 0;
                    long totalME = 0;
                    long totalSBE = 0;
                    long totalBE = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Branch");
                    row.CreateCell(1).SetCellValue("Postition");
                    row.CreateCell(2).SetCellValue("KRA");
                    row.CreateCell(3).SetCellValue("KPI Name");
                    row.CreateCell(4).SetCellValue("EE");
                    row.CreateCell(5).SetCellValue("ME");
                    row.CreateCell(6).SetCellValue("SBE");
                    row.CreateCell(7).SetCellValue("BE");

                    excelSheet.SetColumnWidth(0, 10000);
                    excelSheet.SetColumnWidth(1, 10000);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 10000);
                    excelSheet.SetColumnWidth(4, 3000);
                    excelSheet.SetColumnWidth(5, 3000);
                    excelSheet.SetColumnWidth(6, 3000);
                    excelSheet.SetColumnWidth(7, 3000);

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
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.Branch);
                        row.CreateCell(1).SetCellValue(item.Position);
                        row.CreateCell(2).SetCellValue(item.KRAGroup);
                        row.CreateCell(3).SetCellValue(item.KPI);
                        row.CreateCell(4).SetCellValue(item.EE);
                        row.CreateCell(5).SetCellValue(item.ME);
                        row.CreateCell(6).SetCellValue(item.SBE);
                        row.CreateCell(7).SetCellValue(item.BE);
                        irow++;

                        totalEE += item.EE;
                        totalME += item.ME;
                        totalSBE += item.SBE;
                        totalBE += item.BE;
                    }
                    #endregion



                    XSSFCellStyle footerStyle = (XSSFCellStyle)workbook.CreateCellStyle();

                    var footerFont = workbook.CreateFont();
                    footerFont.FontHeightInPoints = 16;
                    footerStyle.SetFont(footerFont);
                    footerStyle.Alignment = HorizontalAlignment.Right;

                    IRow rowTotal = excelSheet.CreateRow(Convert.ToInt32(irow));
                    rowTotal.CreateCell(0).SetCellValue("");
                    rowTotal.CreateCell(1).SetCellValue("");
                    rowTotal.CreateCell(2).SetCellValue("");
                    rowTotal.CreateCell(3).SetCellValue("TOTAL:");
                    rowTotal.CreateCell(4).SetCellValue(totalEE);
                    rowTotal.CreateCell(5).SetCellValue(totalME);
                    rowTotal.CreateCell(6).SetCellValue(totalSBE);
                    rowTotal.CreateCell(7).SetCellValue(totalBE);

                    rowTotal.Cells[3].CellStyle = footerStyle;
                    rowTotal.Cells[4].CellStyle = footerStyle;
                    rowTotal.Cells[5].CellStyle = footerStyle;
                    rowTotal.Cells[6].CellStyle = footerStyle;
                    rowTotal.Cells[7].CellStyle = footerStyle;

                    workbook.Write(fs);
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EXPORT.ToString(),
                        TableName = "Dashboard",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Summary Of Results Branches With Position exported",
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

        #endregion

        #region Summary Of Results Position Only

        public async Task<IActionResult> OnGetListPositionOnly([FromQuery] GetListPositionOnlyInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataListPositionOnly(param);

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

        private async Task<(List<GetListPositionOnlyOutput>, bool, string)> GetDataListPositionOnly(GetListPositionOnlyInput param)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScoreDashboard").GetSection("GetListPositionOnly").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",
                  "IsExport=", param.IsExport, "&",

                  "RunIDDelimited=", param.RunIDDelimited, "&",
                  "PositionDelimited=", param.PositionDelimited, "&",
                  "KRAGroupDelimited=", param.KRAGroupDelimited, "&",
                  "KPIDelimited=", param.KPIDelimited, "&",
                  "EEMin=", param.EEMin, "&",
                  "EEMax=", param.EEMax, "&",
                  "MEMin=", param.MEMin, "&",
                  "MEMax=", param.MEMax, "&",
                  "SBEMin=", param.SBEMin, "&",
                  "SBEMax=", param.SBEMax, "&",
                  "BEMin=", param.BEMin, "&",
                  "BEMax=", param.BEMax);

            return await SharedUtilities.GetFromAPI(new List<GetListPositionOnlyOutput>(), URL);
        }

        public async Task<IActionResult> OnGetCheckExportPositionOnlyList([FromQuery] GetListPositionOnlyInput param)
        {
            param.IsExport = true;
            var (Result, IsSuccess, ErrorMessage) = await GetDataListPositionOnly(param);

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

        public async Task<IActionResult> OnGetDownloadExportPositionOnlyList([FromQuery] GetListPositionOnlyInput param)
        {
            param.IsExport = true;
            var (Result, IsSuccess, ErrorMessage) = await GetDataListPositionOnly(param);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "Summary Of Results Position Only List.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Position Only List");
                    int irow = 0;
                    long totalEE = 0;
                    long totalME = 0;
                    long totalSBE = 0;
                    long totalBE = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Postition");
                    row.CreateCell(1).SetCellValue("KRA");
                    row.CreateCell(2).SetCellValue("KPI Name");
                    row.CreateCell(3).SetCellValue("EE");
                    row.CreateCell(4).SetCellValue("ME");
                    row.CreateCell(5).SetCellValue("SBE");
                    row.CreateCell(6).SetCellValue("BE");

                    excelSheet.SetColumnWidth(0, 10000);
                    excelSheet.SetColumnWidth(1, 10000);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 3000);
                    excelSheet.SetColumnWidth(4, 3000);
                    excelSheet.SetColumnWidth(5, 3000);
                    excelSheet.SetColumnWidth(6, 3000);

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
                        row.CreateCell(0).SetCellValue(item.Position);
                        row.CreateCell(1).SetCellValue(item.KRAGroup);
                        row.CreateCell(2).SetCellValue(item.KPI);
                        row.CreateCell(3).SetCellValue(item.EE);
                        row.CreateCell(4).SetCellValue(item.ME);
                        row.CreateCell(5).SetCellValue(item.SBE);
                        row.CreateCell(6).SetCellValue(item.BE);
                        irow++;

                        totalEE += item.EE;
                        totalME += item.ME;
                        totalSBE += item.SBE;
                        totalBE += item.BE;
                    }
                    #endregion



                    XSSFCellStyle footerStyle = (XSSFCellStyle)workbook.CreateCellStyle();

                    var footerFont = workbook.CreateFont();
                    footerFont.FontHeightInPoints = 16;
                    footerStyle.SetFont(footerFont);
                    footerStyle.Alignment = HorizontalAlignment.Right;

                    IRow rowTotal = excelSheet.CreateRow(Convert.ToInt32(irow));
                    rowTotal.CreateCell(0).SetCellValue("");
                    rowTotal.CreateCell(1).SetCellValue("");
                    rowTotal.CreateCell(2).SetCellValue("TOTAL:");
                    rowTotal.CreateCell(3).SetCellValue(totalEE);
                    rowTotal.CreateCell(4).SetCellValue(totalME);
                    rowTotal.CreateCell(5).SetCellValue(totalSBE);
                    rowTotal.CreateCell(6).SetCellValue(totalBE);

                    rowTotal.Cells[2].CellStyle = footerStyle;
                    rowTotal.Cells[3].CellStyle = footerStyle;
                    rowTotal.Cells[4].CellStyle = footerStyle;
                    rowTotal.Cells[5].CellStyle = footerStyle;
                    rowTotal.Cells[6].CellStyle = footerStyle;

                    workbook.Write(fs);
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EXPORT.ToString(),
                        TableName = "Dashboard",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Summary Of Results Position Only exported",
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

        #endregion


    }
}