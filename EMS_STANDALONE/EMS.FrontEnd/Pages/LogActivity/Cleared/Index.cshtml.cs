using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Workflow.Transfer.Accountability;
using EMS.Workflow.Transfer.EmailServerCredential;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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

namespace EMS.FrontEnd.Pages.LogActivity.Cleared
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        {
        }

        public void OnGet()
        {
            ViewData["HasEditComputation"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARED/PAYROLL")).Count() > 0 ? "true" : "false";
        }

        public async Task<IActionResult> OnGetList(ClearedEmployeeListInput param)
        {
            List<string> Status = new List<string>();
            if (_systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARED/HR")).Count() > 0 && (param.Status == "" || param.Status == null))
            {
                Status.Add("CLEARED");
                Status.Add("RETURNED_HR");
            }
            if (_systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARED/PAYROLL")).Count() > 0 && (param.Status == "" || param.Status == null))
            {
                Status.Add("COMPUTATION");
                Status.Add("ENDORSED_PAYROLL");
                Status.Add("PENDING_EMPLOYEE");
            }
            if (_systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARED/ACCOUNTING")).Count() > 0 && (param.Status == "" || param.Status == null))
            {
                Status.Add("ENDORSED_ACCOUNTING");
                Status.Add("ENDORSED_HR");
            }
            if (_systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARED/TREASURY")).Count() > 0 && (param.Status == "" || param.Status == null))
            {
                Status.Add("CHECK_PREP");
                Status.Add("ENDORSED_TREASURY");
                Status.Add("FOR_REALEASE");
                Status.Add("RELEASED");
            }
            if(param.Status == "" || param.Status == null)
            {
                param.Status = string.Join(",", Status);
            }
            

            var (Result, IsSuccess, Message) = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeClearedList(param));

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


        public async Task<IActionResult> OnGetClearedEmployeeByID(int ID)
        {
            var (Result, IsSuccess, Message) = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetClearedEmployeeByID(ID));

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Result.FirstOrDefault();
            return new JsonResult(_resultView);
        }
        public async Task<IActionResult> OnGetClearedEmployeeComments(int ClearedEmployeeID)
        {
            var (Result, IsSuccess, Message) = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetClearedEmployeeComments(ClearedEmployeeID));

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Result;
            return new JsonResult(_resultView);
        }
        public async Task<IActionResult> OnPostClearedEmployeeComments([FromForm] PostClearedEmployeeCommentsInput param)
        {
            var (IsSuccess, Message) = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .AddClearedEmployeeComments(param));

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;
            return new JsonResult(_resultView);
        }
        public async Task<IActionResult> OnGetClearedEmployeeStatusHistory(int ClearedEmployeeID)
        {
            var (Result, IsSuccess, Message) = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetClearedEmployeeStatusHistory(ClearedEmployeeID));

            var jsonData = new
            {
                rows = Result
            };
            return new JsonResult(jsonData);
        }
        public async Task<IActionResult> OnGetEmployeeAccountability(int EmployeeID)
        {
            var (Result, IsSuccess, Message) = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeAccountability(EmployeeID));

            var jsonData = new
            {
                rows = Result
            };
            return new JsonResult(jsonData);
        }
        public async Task<JsonResult> OnGetEmployeeMovementByEmployeeIDs(int EmployeeID)
        {
            var (Result, IsSuccess, Message) = await new Common_EmployeeMovement(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeMovementByEmployeeIDs(new List<int>() { EmployeeID });
            var jsonData = new
            {
                rows = Result
            };
            return new JsonResult(jsonData);
        }
        public async Task<IActionResult> OnPostClearedEmployeeComputation([FromForm] PostClearedEmployeeComputationInput param)
        {
            var (IsSuccess, Message) = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .AddClearedEmployeeComputation(param));

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = "EDIT COMPUTATION",
                        TableName = "ClearedEmployee",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat("ID: ", param.ClearedEmployeeID, " | ", param.Computation),
                        IsSuccess = IsSuccess,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                var (Result1, IsSuccess1, Message1) = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                    .GetClearedEmployeeByID(param.ClearedEmployeeID));

                var Employee = (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployee((Result1.FirstOrDefault()).EmployeeID));

                var Number = Employee.PersonalInformation.CellphoneNumber;

                if (!string.IsNullOrEmpty(Number) && Number.Length == 11)
                {
                    var TextMessage = (await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                        .GetReferenceValueByRefCode("TEXT_MESSAGE")).Where(x => x.Value.Equals("LAST_PAY_COMPUTATION")).Select(y => y.Description).FirstOrDefault();

                    SmsInput SmsDetails = new SmsInput();
                    SmsDetails.phone_number = Number;
                    SmsDetails.content = TextMessage;
                    SmsDetails.module = "ADD_COMPUTATION";

                    await new Common_EmailServerCredential(_iconfiguration, _globalCurrentUser, _env)
                        .SendTextMessage(SmsDetails);
                }

            }

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetClearedEmployeeChangeStatus([FromQuery] string CurrentStatus)
        {
            var roles = (await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                               .GetSystemUserRoleDropDownByUserID(_globalCurrentUser.UserID)).ToList();

            var status = (await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetNextWorkflowStep(new GetNextWorkflowStepInput
                {
                    WorkflowCode = "CLEARED_EMPLOYEE",
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
        public async Task<JsonResult> OnPostClearedEmployeeChangeStatus(string ID, string NewStatus, string Remarks)
        {
            PostClearedEmployeeStatusInput changeStatus = new PostClearedEmployeeStatusInput();
            changeStatus.ID = (ID.Split(",")).Select(x => int.Parse(x)).ToList();
            changeStatus.Status = NewStatus;
            changeStatus.Remarks = Remarks;

            var (IsSuccess, Message) = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env).AddClearedEmployeeChangeStatus(changeStatus);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = "CHANGE STATUS",
                        TableName = "ClearedEmployeeChangeStatus",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat("Change Status ID: ", ID),
                        IsSuccess = IsSuccess,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                var AllStatus = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetWorkflow(8)).WorkflowStepList;
                var CurrentStatus = AllStatus.Where(x => x.StepCode.Equals(changeStatus.Status)).FirstOrDefault();
                if (CurrentStatus.SendEmailToApprover || CurrentStatus.SendEmailToRequester)
                {
                    if (CurrentStatus.SendEmailToRequester)
                    {
                    }
                    if (CurrentStatus.SendEmailToApprover)
                    {
                    }
                }
            }

            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetEmployeeAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployeeAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetOrgGroupAutoCompleteAsync(string term, int TopResults)
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
        public async Task<JsonResult> OnGetSystemUserAutoCompleteAsync(string Term, int TopResults)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployeeWithSystemUserAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetStatusFilter()
        {
            var StatusFilter = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(8)).WorkflowStepList;
            _resultView.Result = StatusFilter.Select(x => new Utilities.API.ReferenceMaintenance.MultiSelectedFilter()
            {
                ID = x.StepCode,
                Description = x.StepDescription
            });
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetExportListAsync([FromQuery] ClearedEmployeeListInput param)
        {
            List<string> Status = new List<string>();
            if (_systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARED/HR")).Count() > 0 && (param.Status == "" || param.Status == null))
            {
                Status.Add("CLEARED");
                Status.Add("ENDORSED_PAYROLL");
            }
            if (_systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARED/PAYROLL")).Count() > 0 && (param.Status == "" || param.Status == null))
            {
                Status.Add("ENDORSED_PAYROLL");
                Status.Add("COMPUTATION");
                Status.Add("ENDORSED_ACCOUNTING");
            }
            if (_systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARED/ACCOUNTING")).Count() > 0 && (param.Status == "" || param.Status == null))
            {
                Status.Add("ENDORSED_ACCOUNTING");
                Status.Add("ENDORSED_TREASURY");
            }
            if (_systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/CLEARED/TREASURY")).Count() > 0 && (param.Status == "" || param.Status == null))
            {
                Status.Add("ENDORSED_TREASURY");
                Status.Add("RELEASED");
            }
            if (param.Status == "" || param.Status == null)
            {
                param.Status = string.Join(",", Status);
            }
            var (Result, IsSuccess, Message) = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeClearedList(param));

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "ClearedEmployeeList.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Cleared Employee List");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("ID");
                    row.CreateCell(1).SetCellValue("Employee Name");
                    row.CreateCell(2).SetCellValue("Org Group");
                    row.CreateCell(3).SetCellValue("Position");
                    row.CreateCell(4).SetCellValue("Accountability");
                    row.CreateCell(5).SetCellValue("Status");
                    row.CreateCell(6).SetCellValue("Status Updated By");
                    row.CreateCell(7).SetCellValue("Status Updated Date");
                    row.CreateCell(8).SetCellValue("Status Remarks");
                    row.CreateCell(9).SetCellValue("Computation");
                    row.CreateCell(10).SetCellValue("Agreement");
                    row.CreateCell(11).SetCellValue("Agreement Date");
                    row.CreateCell(12).SetCellValue("Last Comment");
                    row.CreateCell(13).SetCellValue("Last Comment Date");

                    excelSheet.SetColumnWidth(0, 3000);
                    excelSheet.SetColumnWidth(1, 10000);
                    excelSheet.SetColumnWidth(2, 7000);
                    excelSheet.SetColumnWidth(3, 7000);
                    excelSheet.SetColumnWidth(4, 5000);
                    excelSheet.SetColumnWidth(5, 7000);
                    excelSheet.SetColumnWidth(6, 10000);
                    excelSheet.SetColumnWidth(7, 5000);
                    excelSheet.SetColumnWidth(8, 5000);
                    excelSheet.SetColumnWidth(9, 5000);
                    excelSheet.SetColumnWidth(10, 5000);
                    excelSheet.SetColumnWidth(11, 5000);
                    excelSheet.SetColumnWidth(12, 5000);
                    excelSheet.SetColumnWidth(13, 5000);

                    XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();

                    var colHeaderFont = workbook.CreateFont();
                    colHeaderFont.IsBold = true;
                    colHeaderStyle.SetFont(colHeaderFont);
                    colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                    colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                    colHeaderStyle.Alignment = HorizontalAlignment.Center;
                    alignCenter.Alignment = HorizontalAlignment.Center;

                    for (int i = 0; i <= 13; i++)
                        row.Cells[i].CellStyle = colHeaderStyle;

                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));

                        row.CreateCell(0).SetCellValue(item.ID.ToString());
                        row.CreateCell(1).SetCellValue(item.FullName);
                        row.CreateCell(2).SetCellValue(item.OrgGroup);
                        row.CreateCell(3).SetCellValue(item.Position);
                        row.CreateCell(4).SetCellValue(item.Accountability);
                        row.CreateCell(5).SetCellValue(item.StatusDescription);
                        row.CreateCell(6).SetCellValue(item.StatusUpdatedBy);
                        row.CreateCell(7).SetCellValue(item.StatusUpdatedDate);
                        row.CreateCell(8).SetCellValue(item.StatusRemarks);
                        row.CreateCell(9).SetCellValue(item.Computation);
                        row.CreateCell(10).SetCellValue(item.Agreed);
                        row.CreateCell(11).SetCellValue(item.AgreedDate);
                        row.CreateCell(12).SetCellValue(item.LastComment);
                        row.CreateCell(13).SetCellValue(item.LastCommentDate);

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
                        Remarks = string.Concat(Result.Count(), " Cleared Employee Exported"),
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
                return new BadRequestObjectResult(Message);
            }
        }
    }
}
