using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Workflow.Transfer.Accountability;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Utilities.API;

namespace EMS.FrontEnd.Pages.LogActivity.Accountability
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/ACCOUNTABILITY/ADD")).Count() > 0 ? "true" : "false";
            }
        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] GetAccountabilityListInput param)
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

        public async Task<(List<GetAccountabilityListOutput>, bool, string)> GetExportData([FromQuery] GetAccountabilityListInput param, bool IsExport)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "PreloadName=", param.PreloadName, "&",
                  "DateCreatedFrom=", param.DateCreatedFrom, "&",
                  "DateCreatedTo=", param.DateCreatedTo, "&",
                  "IsExport=", IsExport);

            return await SharedUtilities.GetFromAPI(new List<GetAccountabilityListOutput>(), URL);
        }

        public async Task<JsonResult> OnGetAccountabilityDetails(int ID)
        {
            var result = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetAccountabilityDetails(ID);
            // Get OrgGroup description by OrgGroup IDs
            if (result != null)
            {
                List<EMS.Plantilla.Transfer.OrgGroup.Form> orgGroup =
                       (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                       .GetOrgGroupByIDs(result.Where(x => x.OrgGroupID != 0).Select(x => x.OrgGroupID)
                        .Distinct().ToList())).Item1;

                List<EMS.Plantilla.Transfer.Position.Form> position =
                       (await new Common_Position(_iconfiguration, _globalCurrentUser, _env)
                       .GetAll()).ToList();

                List<EMS.Plantilla.Transfer.Employee.Form> employee =
                       (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                       .GetEmployeeByIDs(result.Where(x => x.EmployeeID != 0).Select(x => x.EmployeeID)
                        .Distinct().ToList())).Item1;

                if (orgGroup.Count > 0)
                {

                    result = result
                           .GroupJoin(orgGroup,
                           x => new { x.OrgGroupID },
                           y => new { OrgGroupID = y.ID },
                           (x, y) => new { activities = x, employees = y })
                           .SelectMany(x => x.employees.DefaultIfEmpty(),
                           (x, y) => new { activities = x, orgGroup = y })
                           .Select(x => new GetDetailsByIDOutput
                           {
                               Title = x.activities.activities.Title,
                               Type = x.activities.activities.Type,
                               Description = x.activities.activities.Description,
                               OrgGroupDescription = x.orgGroup == null ? "" :
                                string.Concat(x.orgGroup.Code, " - ", x.orgGroup.Description),
                               OrgGroupID = x.activities.activities.OrgGroupID,
                               PositionID = x.activities.activities.PositionID,
                               EmployeeID = x.activities.activities.EmployeeID,
                           }).ToList();

                    result = result
                           .GroupJoin(position,
                           x => new { x.PositionID },
                           y => new { PositionID = y.ID },
                           (x, y) => new { activities = x, employees = y })
                           .SelectMany(x => x.employees.DefaultIfEmpty(),
                           (x, y) => new { activities = x, position = y })
                           .Select(x => new GetDetailsByIDOutput
                           {
                               Title = x.activities.activities.Title,
                               Type = x.activities.activities.Type,
                               Description = x.activities.activities.Description,
                               OrgGroupDescription = x.activities.activities.OrgGroupDescription,
                               OrgGroupID = x.activities.activities.OrgGroupID,
                               PositionID = x.activities.activities.PositionID,
                               PositionDescription = x.position == null ? "" :
                                string.Concat(x.position.Code, " - ", x.position.Title),
                               EmployeeID = x.activities.activities.EmployeeID,
                           }).ToList();

                    result = result
                           .GroupJoin(employee,
                           x => new { x.EmployeeID },
                           y => new { EmployeeID = y.ID },
                           (x, y) => new { activities = x, employees = y })
                           .SelectMany(x => x.employees.DefaultIfEmpty(),
                           (x, y) => new { activities = x, employee = y })
                           .Select(x => new GetDetailsByIDOutput
                           {
                               Title = x.activities.activities.Title,
                               Type = x.activities.activities.Type,
                               Description = x.activities.activities.Description,
                               OrgGroupDescription = x.activities.activities.OrgGroupDescription,
                               OrgGroupID = x.activities.activities.OrgGroupID,
                               PositionID = x.activities.activities.PositionID,
                               PositionDescription = x.activities.activities.PositionDescription,
                               EmployeeID = x.activities.activities.EmployeeID,
                               EmployeeName = x.employee == null ? "" :
                                string.Concat(x.employee.PersonalInformation.LastName, ", ", 
                                x.employee.PersonalInformation.FirstName," ",x.employee.PersonalInformation.MiddleName, " (",
                                x.employee.Code,")"),
                           }).ToList();
                }
            }

            _resultView.Result = result;
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

        public async Task<JsonResult> OnGetOrgTypeAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupAutoComplete(term, TopResults);
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

        public async Task<JsonResult> OnGetEmployeeAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployeeAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetCheckExportList([FromQuery] GetAccountabilityListInput param)
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

        public async Task<IActionResult> OnGetDownloadExportList([FromQuery] GetAccountabilityListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetExportData(param, true);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "Accountability.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Accountability");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("ID");
                    row.CreateCell(1).SetCellValue("Preload Name");
                    row.CreateCell(2).SetCellValue("Created Date");

                    excelSheet.SetColumnWidth(0, 3000);
                    excelSheet.SetColumnWidth(1, 8000);
                    excelSheet.SetColumnWidth(2, 5000);

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
                        TableName = "Accountability",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Accountability Exported",
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