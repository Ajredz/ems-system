using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Workflow.Transfer.Training;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

namespace EMS.FrontEnd.Pages.Training.MyTraining
{
    public class IndexModel : SharedClasses.Utilities
    {
        [BindProperty]
        public Utilities.API.ChangeStatus ChangeStatus { get; set; }
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        {
        }

        public void OnGet()
        {
            ViewData["HasView"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/TRAINING/MYTRAINING/VIEW")).Count() > 0 ? "true" : "false";
        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] GetEmployeeTrainingListInput param)
        {
            var StatusColor = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(7)).WorkflowStepList;

            param.UnderAccess = _globalCurrentUser.EmployeeID.ToString();
            var (Result, IsSuccess, ErrorMessage) = await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeTrainingList(param);

            List<EMS.Security.Transfer.SystemUser.Form> systemUsersCreatedBy =
                await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy).Distinct().ToList());
            List<EMS.Security.Transfer.SystemUser.Form> systemUsersModifiedBy =
                await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(Result.Where(x => x.ModifiedBy != null).Select(x => (int)x.ModifiedBy).Distinct().ToList());

            Result = (from left in Result
                      join right in StatusColor on left.Status equals right.StepCode
                      join rightCreatedBy in systemUsersCreatedBy on left.CreatedBy equals rightCreatedBy.ID
                      join rightModifiedBy in systemUsersModifiedBy on left.ModifiedBy equals rightModifiedBy.ID into joinedList
                      from sub in joinedList.DefaultIfEmpty()
                      select new GetEmployeeTrainingListOutput
                      {
                          TotalNoOfRecord = left.TotalNoOfRecord,
                          NoOfPages = left.NoOfPages,
                          Row = left.Row,

                          ID = left.ID,
                          EmployeeID = left.EmployeeID,
                          Status = left.Status,
                          StatusDescription = right.StepDescription,
                          StatusColor = right.StatusColor,
                          StatusUpdateDate = left.StatusUpdateDate,
                          DateSchedule = left.DateSchedule,
                          Type = left.Type,
                          Title = left.Title,
                          Description = left.Description,
                          CreatedBy = left.CreatedBy,
                          CreatedByName = string.Concat("(", rightCreatedBy.Username, ") ", rightCreatedBy.LastName, ", ", rightCreatedBy.FirstName, " ", rightCreatedBy.MiddleName),
                          CreatedDate = left.CreatedDate,
                          ModifiedBy = left.ModifiedBy,
                          ModifiedByName = sub == null ? "" : string.Concat("(", sub.Username, ") ", sub.LastName, ", ", sub.FirstName, " ", sub.MiddleName),
                          ModifiedDate = left.ModifiedDate
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

        public async Task<JsonResult> OnGetStatusFilter()
        {
            var StatusFilter = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(7)).WorkflowStepList;
            _resultView.Result = StatusFilter.Select(x => new Utilities.API.ReferenceMaintenance.MultiSelectedFilter()
            {
                ID = x.StepCode,
                Description = x.StepDescription
            });
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
        public async Task<JsonResult> OnGetSystemUserAutoCompleteAsync(string Term, int TopResults)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployeeWithSystemUserAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetChangeStatus([FromQuery] string CurrentStatus)
        {
            var roles = (await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                               .GetSystemUserRoleDropDownByUserID(_globalCurrentUser.UserID)).ToList();

            var status = (await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetNextWorkflowStep(new GetNextWorkflowStepInput
                {
                    WorkflowCode = "TRAINING",
                    CurrentStepCode = CurrentStatus,
                    RoleIDDelimited = string.Join(",", roles.Select(x => x.Value).ToArray())
                })).Where(x => !x.Code.Equals(CurrentStatus)).ToList();

            _resultView.Result = status.OrderByDescending(x => x.Order).Select(x => new SelectListItem
            {
                Value = x.Code,
                Text = x.Description
            }).ToList();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetTrainingStatusHistory(int ID)
        {
            List<GetEmployeeTrainingStatusHistoryOutput> statusHistory =
                   await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                   .GetEmployeeTrainingStatusHistory(ID);

            List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
               await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
               .GetSystemUserByIDs(statusHistory.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy).Distinct().ToList());

            statusHistory = (from left in statusHistory
                             join right in systemUsers on left.CreatedBy equals right.ID
                             select new GetEmployeeTrainingStatusHistoryOutput
                             {
                                 Status = left.Status,
                                 Remarks = left.Remarks,
                                 CreatedName = string.Concat("(", right.Username, ") ", right.LastName, ", ", right.FirstName, " ", right.MiddleName),
                                 CreatedDate = left.CreatedDate
                             }).ToList();

            var jsonData = new
            {
                rows = statusHistory
            };
            return new JsonResult(jsonData);
        }
        public async Task<JsonResult> OnGetTrainingScore(int ID)
        {
            List<GetEmployeeTrainingScoreOutput> statusHistory =
                   await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                   .GetEmployeeTrainingScore(ID);

            var jsonData = new
            {
                rows = statusHistory
            };
            return new JsonResult(jsonData);
        }

        public async Task<IActionResult> OnGetExportListAsync([FromQuery] GetEmployeeTrainingListInput param)
        {
            param.UnderAccess = _globalCurrentUser.EmployeeID.ToString();
            var (Result, IsSuccess, ErrorMessage) = await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeTrainingList(param);

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

        public async Task<IActionResult> OnGetExportDownloadListAsync([FromQuery] GetEmployeeTrainingListInput param)
        {
            var StatusColor = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(7)).WorkflowStepList;

            param.UnderAccess = _globalCurrentUser.EmployeeID.ToString();
            var (Result, IsSuccess, ErrorMessage) = await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeTrainingList(param);

            List<EMS.Security.Transfer.SystemUser.Form> systemUsersCreatedBy =
                await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy).Distinct().ToList());
            List<EMS.Security.Transfer.SystemUser.Form> systemUsersModifiedBy =
                await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(Result.Where(x => x.ModifiedBy != null).Select(x => (int)x.ModifiedBy).Distinct().ToList());

            Result = (from left in Result
                      join right in StatusColor on left.Status equals right.StepCode
                      join rightCreatedBy in systemUsersCreatedBy on left.CreatedBy equals rightCreatedBy.ID
                      join rightModifiedBy in systemUsersModifiedBy on left.ModifiedBy equals rightModifiedBy.ID into joinedList
                      from sub in joinedList.DefaultIfEmpty()
                      select new GetEmployeeTrainingListOutput
                      {
                          TotalNoOfRecord = left.TotalNoOfRecord,
                          NoOfPages = left.NoOfPages,
                          Row = left.Row,

                          ID = left.ID,
                          EmployeeID = left.EmployeeID,
                          Status = left.Status,
                          StatusDescription = right.StepDescription,
                          StatusColor = right.StatusColor,
                          StatusUpdateDate = left.StatusUpdateDate,
                          DateSchedule = left.DateSchedule,
                          Type = left.Type,
                          Title = left.Title,
                          Description = left.Description,
                          CreatedBy = left.CreatedBy,
                          CreatedByName = string.Concat("(", rightCreatedBy.Username, ") ", rightCreatedBy.LastName, ", ", rightCreatedBy.FirstName, " ", rightCreatedBy.MiddleName),
                          CreatedDate = left.CreatedDate,
                          ModifiedBy = left.ModifiedBy,
                          ModifiedByName = sub == null ? "" : string.Concat("(", sub.Username, ") ", sub.LastName, ", ", sub.FirstName, " ", sub.MiddleName),
                          ModifiedDate = left.ModifiedDate
                      }).ToList();

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "TrainingList.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Training List");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("ID");
                    row.CreateCell(1).SetCellValue("Type");
                    row.CreateCell(2).SetCellValue("Title");
                    row.CreateCell(3).SetCellValue("Status");
                    row.CreateCell(4).SetCellValue("Date Schedule");
                    row.CreateCell(5).SetCellValue("Created By");
                    row.CreateCell(6).SetCellValue("Created Date");
                    row.CreateCell(7).SetCellValue("Modified By");
                    row.CreateCell(8).SetCellValue("Modified Date");

                    excelSheet.SetColumnWidth(0, 3000);
                    excelSheet.SetColumnWidth(1, 4000);
                    excelSheet.SetColumnWidth(2, 6000);
                    excelSheet.SetColumnWidth(3, 4000);
                    excelSheet.SetColumnWidth(4, 5000);
                    excelSheet.SetColumnWidth(5, 12000);
                    excelSheet.SetColumnWidth(6, 5000);
                    excelSheet.SetColumnWidth(7, 12000);
                    excelSheet.SetColumnWidth(8, 5000);

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
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.ID.ToString());
                        row.CreateCell(1).SetCellValue(item.Type);
                        row.CreateCell(2).SetCellValue(item.Title);
                        row.CreateCell(3).SetCellValue(item.StatusDescription);
                        row.CreateCell(4).SetCellValue(item.DateSchedule);
                        row.CreateCell(5).SetCellValue(item.CreatedByName);
                        row.CreateCell(6).SetCellValue(item.CreatedDate);
                        row.CreateCell(7).SetCellValue(item.ModifiedByName);
                        row.CreateCell(8).SetCellValue(item.ModifiedDate);

                        row.Cells[0].CellStyle = alignCenter;

                        irow++;
                    }
                    #endregion

                    workbook.Write(fs);
                }

                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EXPORT.ToString(),
                        TableName = "EmployeeTraining",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(Result.Count(), " Employee Training Exported"),
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