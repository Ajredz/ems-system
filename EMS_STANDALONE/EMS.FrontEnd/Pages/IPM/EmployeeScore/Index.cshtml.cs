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
using System.Text;
using NPOI.XSSF.UserModel.Extensions;
using MySqlX.XDevAPI.Common;


namespace EMS.FrontEnd.Pages.IPM.EmployeeScore
{
    public class IndexModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.IPM.Transfer.EmployeeScore.BulkDeleteForm BulkDelete { get; set; }

        [BindProperty]
        public EMS.IPM.Transfer.EmployeeScore.BulkVoidForm BulkVoid { get; set; }

        [BindProperty]
        public EMS.IPM.Transfer.EmployeeScore.BulkApprovedForm BulkApproved { get; set; }

        [BindProperty]
        public EMS.IPM.Transfer.EmployeeScore.RerunForm Rerun { get; set; }

        [BindProperty]
        public EMS.IPM.Transfer.EmployeeScore.UpdateRunDescriptionInput Description { get; set; }

        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/EMPLOYEESCORE/ADD")).Count() > 0 ? "true" : "false";
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/EMPLOYEESCORE/DELETE")).Count() > 0 ? "true" : "false";


                var maxValue = await new Common_Reference(_iconfiguration, _globalCurrentUser, _ipmBaseURL, _env)
                .GetRefValueByRefCode("KPI_SCORE_MAX_VALUE");
                ViewData["KPIMaxValue"] = maxValue != null ? maxValue.Value : "";
            }

            ViewData["HasEditDescriptionFeature"] = "true";
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


        public async Task<IActionResult> OnPostRunScoresAsync([FromQuery] RunScoreForm param)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("RunScores").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, Message) = await SharedUtilities.PostFromAPI(new RunScoresOutput(), param, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnPostBulkDeleteAsync()
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("BulkDelete").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(BulkDelete, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                StringBuilder stringIDs = new StringBuilder();

                foreach (var obj in BulkDelete.IDs)
                {
                    stringIDs.Append(string.Concat(obj + ", "));
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.DELETE.ToString(),
                        TableName = "Employee Score",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat("Employee Score Transaction/s No. " + stringIDs + "deleted."),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }

        //added
        public async Task<IActionResult> OnPostBulkVoidAsync()
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("BulkVoid").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(BulkVoid, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                var workflowURL = string.Concat(_workflowBaseURL,
                    _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("EmployeeScore").GetSection("AddByBatch").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

                var (IsSuccess1, Message1) =
                    await SharedUtilities.PostFromAPI(BulkVoid.IDs.Select(x =>
                    new EMS.Workflow.Transfer.EmployeeScore.Form
                    {
                        TID = x,
                        Status = "VOID",
                        Remarks = "VOID"
                    }
                    ), workflowURL);

                /*Add AuditLog*/
                StringBuilder stringIDs = new StringBuilder();

                foreach (var obj in BulkVoid.IDs)
                {
                    stringIDs.Append(string.Concat(obj + ", "));
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.VOID.ToString(),
                        TableName = "Employee Score",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat("Status of Employee Score/s No. " + stringIDs + "has been set to VOID."),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnPostBulkApprovedAsync()
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("BulkApproved").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(BulkApproved, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                var workflowURL = string.Concat(_workflowBaseURL,
                    _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("EmployeeScore").GetSection("AddByBatch").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

                var (IsSuccess1, Message1) =
                    await SharedUtilities.PostFromAPI(BulkApproved.IDs.Select(x =>
                    new EMS.Workflow.Transfer.EmployeeScore.Form
                    {
                        TID = x,
                        Status = "APPROVED",
                        Remarks = "APPROVED"
                    }
                    ), workflowURL);

                /*Add AuditLog*/
                StringBuilder stringIDs = new StringBuilder();

                foreach (var obj in BulkApproved.IDs)
                {
                    stringIDs.Append(string.Concat(obj + ", "));
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.VOID.ToString(),
                        TableName = "Employee Score",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat("Status of Employee Score/s No. " + stringIDs + "has been set to Approved."),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnPostUpdatedRunDescription()
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("UpdateRunDescription").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(Description, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                string AuditLogMessage = "No Changes";
                var old = await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env)
                .GetTransSummary(Description.RunID);

                if (old != null)
                {
                    if (!old.Description.Equals(Description.Description))
                    {
                        AuditLogMessage = string.Concat("Description changed from ", old.Description, " to ", Description.Description, ". ");
                    }

                    if (!old.IsTransActive.Equals(Description.IsTransActive))
                    {
                        AuditLogMessage = string.Concat("IsTransActive changed from ", old.IsTransActive, " to ", Description.IsTransActive, ". ");
                    }
                }
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EDIT.ToString(),
                        TableName = "Employee Score",
                        TableID = Description.RunID,
                        Remarks = AuditLogMessage,
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetEmployeeAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployeeAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetEmployeeSystemUserAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployeeWithSystemUserAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
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

        public async Task<JsonResult> OnGetEmployeeFilteredAutoCompleteAsync(string term, int TopResults, string filter, string filterID)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployeeFilteredAutoComplete(term, TopResults, filter, filterID);
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

        public async Task<JsonResult> OnGetGetScores([FromQuery] RunScoreForm param)
        {
            var scores = await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env).GetScores(param);
            var employees = scores.Select(x => new { x.Employee, x.OrgGroup, x.Position, x.TDateFrom, x.TDateTo, x.PDateFrom, x.PDateTo }).Distinct().ToList();

            _resultView.Result = new { scores, employees };
            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }

        private async Task<(List<GetListOutput>, bool, string)> GetExportData(GetListInput param, bool IsExport)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",
                  "IsExport=", IsExport, "&",

                  "TransSummaryIDDelimited=", param.TransSummaryIDDelimited, "&",
                  "Description=", param.Description, "&",
                  "IsActiveDelimited=", param.IsActiveDelimited, "&",
                  "NameDelimited=", param.NameDelimited, "&",
                  "ParentOrgGroup=", param.ParentOrgGroup, "&",
                  "OrgGroupDelimited=", param.OrgGroupDelimited, "&",
                  "PositionDelimited=", param.PositionDelimited, "&",
                  "ScoreFrom=", param.ScoreFrom, "&",
                  "ScoreTo=", param.ScoreTo, "&",
                  "StatusDelimited=", param.StatusDelimited, "&",
                  //RunDate
                  "DateFromFrom=", param.DateFromFrom, "&",
                  "DateFromTo=", param.DateFromTo, "&",
                  "DateToFrom=", param.DateToFrom, "&",
                  "DateToTo=", param.DateToTo, "&",
                  //DateEffective
                  "DateEffectiveFromFrom=", param.DateEffectiveFromFrom, "&",
                  "DateEffectiveFromTo=", param.DateEffectiveFromTo, "&",
                  "DateEffectiveToFrom=", param.DateEffectiveToFrom, "&",
                  "DateEffectiveToTo=", param.DateEffectiveToTo, "&",
                  //KPIPositionDate
                  //"KPIPositionDateFromFrom=", param.

                  "StatusDelimited=", param.StatusDelimited, "&",
                  "ShowForEvaluation=", param.ShowForEvaluation, "&",
                  "ShowNoScore=", param.ShowNoScore, "&",
                  "isShowAll=", param.isShowAll);

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
                string sFileName = "Employee Score List.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Employee Score List");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Run ID");
                    row.CreateCell(1).SetCellValue("Report Name");
                    row.CreateCell(2).SetCellValue("Employee");
                    row.CreateCell(3).SetCellValue("Org. Group");
                    row.CreateCell(4).SetCellValue("Position");
                    row.CreateCell(5).SetCellValue("Score");
                    row.CreateCell(6).SetCellValue("Status");
                    row.CreateCell(7).SetCellValue("Parent Org Group");
                    row.CreateCell(8).SetCellValue("Run Date From");
                    row.CreateCell(9).SetCellValue("Run Date To");
                    row.CreateCell(10).SetCellValue("IPM Date From");
                    row.CreateCell(11).SetCellValue("IPM Date To");
                    row.CreateCell(12).SetCellValue("IPM Month/s");
                    row.CreateCell(13).SetCellValue("Movement Date From");
                    row.CreateCell(14).SetCellValue("Movement Date To");
                    row.CreateCell(15).SetCellValue("Qualifier/s Branch Performance");
                    row.CreateCell(16).SetCellValue("Qualifier/s Pro-Rate Performance");
                    row.CreateCell(17).SetCellValue("Final Qualifier/s Performance");
                    row.CreateCell(18).SetCellValue("Qualifier/s Remarks");

                    excelSheet.SetColumnWidth(0, 5000);
                    excelSheet.SetColumnWidth(1, 10000);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 10000);
                    excelSheet.SetColumnWidth(4, 11000);
                    excelSheet.SetColumnWidth(5, 7000);
                    excelSheet.SetColumnWidth(6, 5000);
                    excelSheet.SetColumnWidth(7, 10000);
                    excelSheet.SetColumnWidth(8, 7000);
                    excelSheet.SetColumnWidth(9, 7000);
                    excelSheet.SetColumnWidth(10, 7000);
                    excelSheet.SetColumnWidth(11, 7000);
                    excelSheet.SetColumnWidth(12, 7000);
                    excelSheet.SetColumnWidth(13, 7000);
                    excelSheet.SetColumnWidth(14, 7000);
                    excelSheet.SetColumnWidth(15, 7000);
                    excelSheet.SetColumnWidth(16, 7000);
                    excelSheet.SetColumnWidth(17, 7000);
                    excelSheet.SetColumnWidth(18, 7000);

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
                    row.Cells[10].CellStyle = colHeaderStyle;
                    row.Cells[11].CellStyle = colHeaderStyle;
                    row.Cells[12].CellStyle = colHeaderStyle;
                    row.Cells[13].CellStyle = colHeaderStyle;
                    row.Cells[14].CellStyle = colHeaderStyle;
                    row.Cells[15].CellStyle = colHeaderStyle;
                    row.Cells[16].CellStyle = colHeaderStyle;
                    row.Cells[17].CellStyle = colHeaderStyle;
                    row.Cells[18].CellStyle = colHeaderStyle;
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.TransSummaryID.ToString());
                        row.CreateCell(1).SetCellValue(item.Description);
                        row.CreateCell(2).SetCellValue(item.Employee);
                        row.CreateCell(3).SetCellValue(item.OrgGroup);
                        row.CreateCell(4).SetCellValue(item.Position);
                        row.CreateCell(5).SetCellValue(string.Concat(item.Score.ToString(), "%"));
                        row.CreateCell(6).SetCellValue(item.Status);
                        row.CreateCell(7).SetCellValue(item.ParentOrgGroup);
                        row.CreateCell(8).SetCellValue(item.TDateFrom);
                        row.CreateCell(9).SetCellValue(item.TDateTo);
                        row.CreateCell(10).SetCellValue(item.PDateFrom);
                        row.CreateCell(11).SetCellValue(item.PDateTo);
                        row.CreateCell(12).SetCellValue(item.IPMMonths);
                        row.CreateCell(13).SetCellValue(item.DateEffectiveFrom);
                        row.CreateCell(14).SetCellValue(item.DateEffectiveTo);
                        row.CreateCell(15).SetCellValue(string.Concat(item.QualiBranchPerformance.ToString(),"%"));
                        row.CreateCell(16).SetCellValue(string.Concat(item.QualiProRatePerformance.ToString(), "%"));
                        row.CreateCell(17).SetCellValue(string.Concat(item.QualiFinal.ToString(), "%"));
                        row.CreateCell(18).SetCellValue(item.QualiRemarks);
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
                        TableName = "Employee Score",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Employee Score list exported",
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

        public async Task<IActionResult> OnGetDownloadExportEmployee([FromQuery] GetByIDInput param)
        {
            Form EmployeeScore = await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env).GetEmployeeScore(param);

            decimal TotalWeight = 0;
            decimal? WeightedScore;
            decimal? TotalScore = 0;

            string sWebRootFolder = _env.WebRootPath;
            EmployeeScore.Employee = (EmployeeScore.Employee.Replace("N/A", "")).TrimEnd();
            string sFileName = EmployeeScore.Employee + ".xlsx";
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(EmployeeScore.Employee);

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
                        topHeader.CreateCell(2).SetCellValue(EmployeeScore.Employee);
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
                        topHeader.CreateCell(2).SetCellValue(EmployeeScore.Status);
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
                        topHeader.CreateCell(2).SetCellValue(EmployeeScore.TDateFrom + " - " + EmployeeScore.TDateTo);
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
                        topHeader.CreateCell(2).SetCellValue(EmployeeScore.OrgGroup);
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
                        topHeader.CreateCell(2).SetCellValue(EmployeeScore.Position);
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
                row.CreateCell(11).SetCellValue("SourceType");

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

                foreach (var item in EmployeeScore.EmployeeScoreList)
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
                footerRater.CreateCell(2).SetCellValue(EmployeeScore.Requestor);

                footerRater.Cells[0].CellStyle = footerStyle;
                footerRater.Cells[1].CellStyle = footerStyle;
                footerRater.Cells[2].CellStyle = footerStyle;

                irow += 4;
                IRow footerEmployee = excelSheet.CreateRow(Convert.ToInt32(irow));

                var footerEmployeeMerged = new NPOI.SS.Util.CellRangeAddress(irow, irow, 0, 1);

                excelSheet.AddMergedRegion(footerEmployeeMerged);

                footerEmployee.CreateCell(0).SetCellValue("Conforme (employee):");
                footerEmployee.CreateCell(1).SetCellValue("");
                footerEmployee.CreateCell(2).SetCellValue(EmployeeScore.Employee);

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

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _ipmBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetEmployeeScoreStatusHistory(int ID)
        {
            List<GetEmployeeScoreStatusHistoryOutput> statusHistory =
                   await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env)
                   .GetEmployeeScoreStatusHistory(ID);

            // Get Employee Names by User IDs
            List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
               await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
               .GetSystemUserByIDs(statusHistory.Where(x => x.UserID != 0).Select(x => x.UserID).Distinct().ToList());

            // Set employee names
            if (systemUsers.Count > 0)
            {
                statusHistory = statusHistory
                        .GroupJoin(systemUsers,
                        x => new { x.UserID },
                        y => new { UserID = y.ID },
                        (x, y) => new { employeeScore = x, employees = y })
                        .SelectMany(x => x.employees.DefaultIfEmpty(),
                        (x, y) => new { employeeScore = x, employees = y })
                        .Select(x => new GetEmployeeScoreStatusHistoryOutput
                        {
                            Status = x.employeeScore.employeeScore.Status,
                            Timestamp = x.employeeScore.employeeScore.Timestamp,
                            Remarks = x.employeeScore.employeeScore.Remarks,
                            User = x.employees == null ? "" : string.Concat((x.employees.LastName ?? "").Trim(),
                                string.IsNullOrEmpty((x.employees.FirstName ?? "").Trim()) ? "" : string.Concat(", ", x.employees.FirstName),
                                string.IsNullOrEmpty((x.employees.MiddleName ?? "").Trim()) ? "" : string.Concat(" ", x.employees.MiddleName))

                        }).ToList();
            }

            var jsonData = new
            {
                rows = statusHistory
            };
            return new JsonResult(jsonData);
        }

        public async Task<JsonResult> OnGetViewSummary(int ID)
        {
            _resultView.Result = await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env)
                .GetTransSummary(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetEmployeeKPIScoreGetListAsync([FromQuery] EmployeeKPIScoreGetListInput param)
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

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<EmployeeKPIScoreGetListOutput>(), URL);

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

        public async Task<JsonResult> OnGetRunIDAutoComplete(string term, int TopResults)
        {
            var result = await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env)
                .GetSummaryAutoComplete(new GetSummaryAutoCompleteInput { Term = term, TopResults = TopResults, IsAdmin = true });
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetCheckExportEmployee([FromQuery] GetByIDInput param)
        {
            Form EmployeeScore = await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env).GetEmployeeScore(param);

            if (EmployeeScore != null)
            {
                if (EmployeeScore.EmployeeScoreList != null)
                {
                    if (EmployeeScore.EmployeeScoreList.Count > 0)
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

        public async Task<JsonResult> OnPostReRun()
        {

            int countOfIDs = 0;

            int maxDegreeOfParallelism = Convert.ToInt32(string.Concat(_iconfiguration.GetSection("MultiThread").GetSection("MaxDegreeOfParallelism").Value));
            int byBatch = Convert.ToInt32(string.Concat(_iconfiguration.GetSection("MultiThread").GetSection("ByBatch").Value));

            List<Task> tasks = new List<Task>();

            // Get parameter for rerun
            var URL = string.Concat(_ipmBaseURL,
                 _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("RerunTransScores").Value, "?",
                 "userid=", _globalCurrentUser.UserID);

            var roles = (new SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
            .GetRolesByWorkflowStepCode(new EMS.Workflow.Transfer.Workflow.GetRolesByWorkflowStepCodeInput
            {
                WorkflowCode = "IPM",
                StepCode = "NEW" // CurrentStep
            })).Result.ToList();
            Rerun.RoleIDs = string.Join(",", roles.Select(x => x.RoleID).ToArray());

            var old = (new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env)
              .GetTransSummary(Rerun.RunID)).Result;

            var (Result, IsSuccess, Message) = await SharedUtilities.PostFromAPI(new ReRunParamOutput(), Rerun, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Result;


            if (IsSuccess)
            {

                StringBuilder Remarks = new StringBuilder();

                Remarks.Append(string.Concat("Employee Score Transactions Initial Re-Run. Filtered by Run ID : ", Rerun.RunID, " from ", Rerun.DateFrom, " to ", Rerun.DateTo, ". "));

                if (Rerun.UseCurrent == true)
                {
                    Remarks.Append(string.Concat("Used recent KPI Position. "));
                }

                if (Rerun.IncludeAllLevelsBelow == true)
                {
                    Remarks.Append(string.Concat("Included all levels below. "));
                }

                if (Rerun.Override == true)
                {
                    Remarks.Append(string.Concat("Overrode existing scores with the same period. "));
                }

                /*Add AuditLog*/
                new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                   .AddAuditLog(new Security.Transfer.AuditLog.Form
                   {
                       EventType = Common_AuditLog.EventType.ADD.ToString(),
                       TableName = "Employee Score",
                       TableID = Result.TransSummaryID, // 0,  // Result.TransSummaryID, // New Record, no ID yet
                       Remarks = Remarks.ToString(), //dates.ToString() + 
                       IsSuccess = true,
                       CreatedBy = _globalCurrentUser.UserID
                   }).Wait();
            }

            //// pass parameter for rerun to staging
            Rerun.Description = Result.Description;
            Rerun.Employees = Result.Employees;
            Rerun.Filter = Result.Filter;
            Rerun.IDs = Result.Ids;
            Rerun.IncludeAllLevelsBelow = Result.IncludeLvlBelow;
            Rerun.IncludeSecDesig = Result.IncludeSecDesig;
            Rerun.Override = Result.Override;
            Rerun.RegularOnly = Result.RegularOnly;
            Rerun.TransSummaryID = Result.TransSummaryID;
            Rerun.UseCurrent = Result.UseCurrent;

            try
            {
                var TaskResult = Task.Run(() => ExecuteTask());

                void ExecuteTask()
                {

                    var URL2 = string.Concat(_ipmBaseURL,
                      _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("RunTransScoresInitialize").Value, "?",
                      "userid=", _globalCurrentUser.UserID);

                    var (Result2, IsSuccess2, Message2) = SharedUtilities.PostFromAPI(new TransOutputForTreading(), Rerun, URL2).Result;



                    countOfIDs = Result2.EmployeeID.Count();

                    var URL3 = string.Concat(_ipmBaseURL, _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("RunTransScoresProcessByBatch").Value, "?", "userid=", _globalCurrentUser.UserID);

                    List<int> employees = Result2.EmployeeID;
                    int loops = Convert.ToInt32(Math.Ceiling(employees.Count / Convert.ToDouble(byBatch)));

                    ParallelOptions po = new ParallelOptions();
                    po.MaxDegreeOfParallelism = maxDegreeOfParallelism;
                    Parallel.For(0, loops, po, (count, state) =>
                    {
                        RunScoreForm ReRunScoreByBatchForm = new RunScoreForm();


                        List<int> employeesByBatch = employees.GetRange(byBatch <= employees.Count ? count * byBatch : 0
                            , byBatch <= employees.Count ? employees.Count - ((count * byBatch)) > byBatch ? byBatch : employees.Count - ((count * byBatch))
                                                                : employees.Count);

                        string PK = count + "-";
                        ReRunScoreByBatchForm.DateFrom = Rerun.DateFrom;
                        ReRunScoreByBatchForm.DateTo = Rerun.DateTo;
                        ReRunScoreByBatchForm.TransSummaryID = Rerun.TransSummaryID;
                        ReRunScoreByBatchForm.Pk = PK;
                        ReRunScoreByBatchForm.strEmployeeIDList = string.Join(",", employeesByBatch.ToArray());
                        tasks.Add(SharedUtilities.PostFromAPI(new TransOutput(), ReRunScoreByBatchForm, URL3));
                    });

                    if (tasks.Any())
                    {
                        Task.WhenAll(tasks).Wait();
                        tasks.Clear();
                        var URLIsDone = string.Concat(_ipmBaseURL, _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("RunTransScoresFinalize").Value, "?", "userid=", _globalCurrentUser.UserID);
                        SharedUtilities.PostFromAPI(new TransOutputForTreading(), Rerun, URLIsDone).Wait();

                    }

                    if (IsSuccess)
                    {
                        StringBuilder Remarks = new StringBuilder();

                        Remarks.Append(string.Concat("Employee Score Transactions Re-Run Completed. ", "Total of Employees ( " + employees.Count() + " ) ", "Filtered by Run ID: ", Rerun.RunID, " and ", Rerun.Filter, " from ", Rerun.DateFrom, " to ", Rerun.DateTo, ". "));


                        if (Rerun.UseCurrent == true)
                        {
                            Remarks.Append(string.Concat("Used recent KPI Position. "));
                        }

                        if (Rerun.IncludeAllLevelsBelow == true)
                        {
                            Remarks.Append(string.Concat("Included all levels below. "));
                        }

                        if (Rerun.Override == true)
                        {
                            Remarks.Append(string.Concat("Overrode existing scores with the same period. "));
                        }

                        /*Add AuditLog*/
                        new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                           .AddAuditLog(new Security.Transfer.AuditLog.Form
                           {
                               EventType = Common_AuditLog.EventType.ADD.ToString(),
                               TableName = "Employee Score",
                               TableID = Result.TransSummaryID, // 0, // New Record, no ID yet
                               Remarks = Remarks.ToString(), // dates.ToString() + 
                               IsSuccess = true,
                               CreatedBy = _globalCurrentUser.UserID
                           }).Wait();

                    }


                }

            }
            catch (Exception ex)
            {

            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetRatingGrades()
        {
            _resultView.Result = await new Common_RatingTable(_iconfiguration, _globalCurrentUser, _env).GetAll();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }


        public async Task<JsonResult> OnGetIPMRaterByTransID(int ID)
        {
            _resultView.Result = await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env).GetIPMRaterByTransID(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetFinalScoreListAsync([FromQuery] GetFinalScoreListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env)
                .GetFinalScoreList(param);

            List<EMS.Security.Transfer.SystemUser.Form> systemUsersCreatedBy =
                await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy).Distinct().ToList());

            Result = (from left in Result
                      join rightCreatedBy in systemUsersCreatedBy on left.CreatedBy equals rightCreatedBy.ID
                      select new GetFinalScoreListOutput
                      {
                          TotalNoOfRecord = left.TotalNoOfRecord,
                          NoOfPages = left.NoOfPages,
                          Row = left.Row,

                          ID = left.ID,
                          RunID = left.RunID,
                          RunTitle = left.RunTitle,
                          EmployeeName = left.EmployeeName,
                          IPMCount = left.IPMCount,
                          IPMMonths = left.IPMMonths,
                          FinalScore = left.FinalScore,
                          CreatedName = string.Concat("(", rightCreatedBy.Username, ") ", rightCreatedBy.LastName, ", ", rightCreatedBy.FirstName, " ", rightCreatedBy.MiddleName),
                          CreatedDate = left.CreatedDate
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

        public async Task<JsonResult> OnGetRunIDDropDown()
        {
            var result = await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env)
                .GetRunIDDropDown();
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostRunFinalScore(int RunID) 
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("GetIPMFinalScore").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(RunID, URL);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;
            return new JsonResult(_resultView);
        }
        public async Task<IActionResult> OnGetFinalScoreCheckExportList([FromQuery] GetFinalScoreListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env)
                 .GetFinalScoreList(param);

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

        public async Task<IActionResult> OnGetDownloadFinalScoreExportList([FromQuery] GetFinalScoreListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env)
                .GetFinalScoreList(param);

            List<EMS.Security.Transfer.SystemUser.Form> systemUsersCreatedBy =
                await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy).Distinct().ToList());

            Result = (from left in Result
                      join rightCreatedBy in systemUsersCreatedBy on left.CreatedBy equals rightCreatedBy.ID
                      select new GetFinalScoreListOutput
                      {
                          TotalNoOfRecord = left.TotalNoOfRecord,
                          NoOfPages = left.NoOfPages,
                          Row = left.Row,

                          ID = left.ID,
                          RunID = left.RunID,
                          RunTitle = left.RunTitle,
                          EmployeeName = left.EmployeeName,
                          IPMCount = left.IPMCount,
                          IPMMonths = left.IPMMonths,
                          FinalScore = left.FinalScore,
                          FinalQuali = left.FinalQuali,
                          FinalRemarks = left.FinalRemarks,
                          CreatedName = string.Concat("(", rightCreatedBy.Username, ") ", rightCreatedBy.LastName, ", ", rightCreatedBy.FirstName, " ", rightCreatedBy.MiddleName),
                          CreatedDate = left.CreatedDate
                      }).ToList();

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "Employee Final Score List.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Employee Final Score List");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("ID");
                    row.CreateCell(1).SetCellValue("Run Title");
                    row.CreateCell(2).SetCellValue("Employee Name");
                    row.CreateCell(3).SetCellValue("IPM Count");
                    row.CreateCell(4).SetCellValue("IPM Months");
                    row.CreateCell(5).SetCellValue("Final Score");
                    row.CreateCell(6).SetCellValue("Final Qualifiers");
                    row.CreateCell(7).SetCellValue("Final Remarks");
                    row.CreateCell(8).SetCellValue("Created By");
                    row.CreateCell(9).SetCellValue("Created Date");

                    excelSheet.SetColumnWidth(0, 5000);
                    excelSheet.SetColumnWidth(1, 10000);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 5000);
                    excelSheet.SetColumnWidth(4, 5000);
                    excelSheet.SetColumnWidth(5, 5000);
                    excelSheet.SetColumnWidth(6, 5000);
                    excelSheet.SetColumnWidth(7, 5000);
                    excelSheet.SetColumnWidth(8, 10000);
                    excelSheet.SetColumnWidth(9, 5000);

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
                        row.CreateCell(0).SetCellValue(item.ID.ToString());
                        row.CreateCell(1).SetCellValue(item.RunTitle);
                        row.CreateCell(2).SetCellValue(item.EmployeeName);
                        row.CreateCell(3).SetCellValue(item.IPMCount);
                        row.CreateCell(4).SetCellValue(item.IPMMonths);
                        row.CreateCell(5).SetCellValue(string.Concat(item.FinalScore.ToString(), "%"));
                        row.CreateCell(6).SetCellValue(string.Concat(item.FinalQuali.ToString(), "%"));
                        row.CreateCell(7).SetCellValue(item.FinalRemarks);
                        row.CreateCell(8).SetCellValue(item.CreatedName);
                        row.CreateCell(9).SetCellValue(item.CreatedDate);
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
                        TableName = "Employee Final Score",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Employee Final Score list exported",
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