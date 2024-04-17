using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.IPM.Data.DataDuplication.Employee;
using EMS.IPM.Data.DataDuplication.Position;
using EMS.Workflow.Transfer.Accountability;
using EMS.Workflow.Transfer.EmailServerCredential;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using NPOI.HSSF.Record;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;
using EMS.Plantilla.Transfer.Employee;
using Utilities.API.ReferenceMaintenance;
using System.Linq;
using Microsoft.AspNetCore.Html;
using EMS.IPM.Data.DataDuplication.OrgGroup;
using EMS.FrontEnd.SharedClasses.Common_Manpower;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;
using System.IO;
using MySqlX.XDevAPI.Common;

namespace EMS.FrontEnd.Pages.Cronjob
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnGetEmployeeBirthday()
        {
            var EmailSubject = (await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode("EMAIL_FORMAT")).Where(x => x.Value.Equals("BIRTHDAY_SUBJECT")).Select(y => y.Description).FirstOrDefault();
            var EmailBody = (await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode("EMAIL_FORMAT")).Where(x => x.Value.Equals("BIRTHDAY_BODY")).Select(y => y.Description).FirstOrDefault();

            var (Result,IsSuccess,Message) = await new Common_Employee(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .GetEmployeeByBirthday();
            Result = Result.Where(x=>!string.IsNullOrEmpty(x.CorporateEmail)).ToList();

            await new Common_EmailServerCredential(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .AddCronLogs(new List<CronLogsInput> {
                        new CronLogsInput(){
                            CronName = "EmployeeBirthday",
                            CronLink = "http://192.168.150.6:86/cronjob/?handler=EmployeeBirthday",
                            Status = (IsSuccess ? "SUCCESS" : "FAILED"),
                            Remarks = string.Concat("Message: ",Message," | Count: ",Result.Count()," | Affected: ",string.Join(",",Result.Select(x => x.Code).ToList()))
                        }
                });

            if (IsSuccess)
            {
                var Position = (await new Common_Position(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                        .GetAll());
                var OrgGroup = (await new Common_OrgGroup(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                    .GetOrgGroupByIDs(Result.Select(x => x.OrgGroupID).ToList())).Item1;

                var emails = (from left in Result
                              join rightPosition in Position on left.PositionID equals rightPosition.ID
                              join rightOrgGroup in OrgGroup on left.OrgGroupID equals rightOrgGroup.ID
                              select new EmailLogsInput()
                              {
                                  PositionTitle = String.Concat(rightPosition.Code, " - ", rightPosition.Title),
                                  Status = left.EmploymentStatus,
                                  SystemCode = "EMS CRONJOB",
                                  SenderName = "MOTORTRADE",
                                  FromEmailAddress = "noreply@motortrade.com.ph",
                                  Name = String.Concat("(", left.Code, ") ", left.LastName, ", ", left.FirstName, " ", string.IsNullOrEmpty(left.MiddleName) ? "" : left.MiddleName.Substring(0, 1)),
                                  ToEmailAddress = left.CorporateEmail,
                                  Subject = EmailSubject
                                         .Replace("@FullName", String.Concat(left.LastName, ", ", left.FirstName, " ", string.IsNullOrEmpty(left.MiddleName) ? "" : left.MiddleName.Substring(0, 1)))
                                         .Replace("@FirstName", left.FirstName)
                                         .Replace("@LastName", left.LastName)
                                         .Replace("@Position", String.Concat(rightPosition.Code, " - ", rightPosition.Title))
                                         .Replace("@OrgGroup", String.Concat(rightOrgGroup.Code, " - ", rightOrgGroup.Description)),
                                  Body = EmailBody
                                         .Replace("@FullName", String.Concat(left.LastName, ", ", left.FirstName, " ", string.IsNullOrEmpty(left.MiddleName) ? "" : left.MiddleName.Substring(0, 1)))
                                         .Replace("@FirstName", left.FirstName)
                                         .Replace("@LastName", left.LastName)
                                         .Replace("@Position", String.Concat(rightPosition.Code, " - ", rightPosition.Title))
                                         .Replace("@OrgGroup", String.Concat(rightOrgGroup.Code, " - ", rightOrgGroup.Description)),
                              }).ToList();

                var (IsSuccessEmail, MessageEmail) = await new Common_EmailServerCredential(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                    .AddMultipleEmailLogs(emails);
            }

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Result.Select(x=>x.Code).ToList();

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetEmployeeEvaluation()
        {
            var EmailBody = (await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode("EMAIL_FORMAT")).Where(x => x.Value.Equals("PROBATIONARY")).Select(y => y.Description).FirstOrDefault();
            var (Result, IsSuccess, Message) = await new Common_Employee(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .GetEmployeeEvaluation();

            await new Common_EmailServerCredential(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .AddCronLogs(new List<CronLogsInput> {
                        new CronLogsInput(){
                            CronName = "EmployeeEvaluation",
                            CronLink = "http://192.168.150.6:86/cronjob/?handler=EmployeeEvaluation",
                            Status = (IsSuccess ? "SUCCESS" : "FAILED"),
                            Remarks = string.Concat("Message: ",Message," | Count: ",Result.Count()," | Affected: ",string.Join(",",Result.Select(x => x.Code).ToList()))
                        }
                });

            if (IsSuccess)
            {
                var emails = (from left in Result
                              select new EmailLogsInput()
                              {
                                  PositionTitle = left.Position,
                                  Status = "PROBATIONARY",
                                  SystemCode = "EMS CRONJOB",
                                  SenderName = "MOTORTRADE",
                                  FromEmailAddress = "noreply@motortrade.com.ph",
                                  CCEmailAddress = left.CCEmail,
                                  Name = String.Concat("(",left.Code,") ",left.LastName, ", ", left.FirstName, " ", left.MiddleName.Substring(0, 1)),
                                  ToEmailAddress = left.CorporateEmail,
                                  Subject = String.Concat("Notice of Pending Regularization - ", left.LastName, ", ", left.FirstName, " ", string.IsNullOrEmpty(left.MiddleName) ? "" : left.MiddleName.Substring(0, 1)),
                                  Body = EmailBody
                                         .Replace("&lt;Sending&gt;", DateTime.Now.ToString("MMMM dd, yyyy"))
                                         .Replace("&lt;Supervisor&gt;", left.SupervisorName)
                                         .Replace("&lt;Employee&gt;", left.FullName)
                                         .Replace("&lt;OrgGroup&gt;", left.OrgGroup)
                                         .Replace("&lt;Position&gt;", left.Position)
                                         .Replace("&lt;DateHired&gt;", left.DateHired)
                                         .Replace("&lt;Regularization&gt;", left.Regularization)
                                         .Replace("&lt;FiveMonths&gt;", left.Fivemonths)
                                         .Replace("&lt;DismissedDate&gt;", left.DismissedDate),
                              }).ToList();

                var (IsSuccessEmail, MessageEmail) = await new Common_EmailServerCredential(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                    .AddMultipleEmailLogs(emails);
            }

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Result.Select(x => x.Code).ToList();

            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetMRFAutoCancelled()
        {
            var (Result, IsSuccess, Message) = await new Common_MRF(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .GetMRFAutoCancelled();

            var msg = "";

            if (IsSuccess)
            {
                var Employee = ((await new Common_Employee(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                        .GetEmployeeByUserIDs(Result.Select(x => x.CreatedBy).ToList())).Item1).Where(x => !string.IsNullOrEmpty(x.CorporateEmail)).ToList();
                var Position = (await new Common_Position(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                        .GetAll());
                var OrgGroup = (await new Common_OrgGroup(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                    .GetOrgGroupByIDs(Employee.Select(x => x.OrgGroupID).ToList())).Item1;
                var OrgGroupMrf = (await new Common_OrgGroup(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                    .GetOrgGroupByIDs(Result.Select(x => x.OrgGroupID).ToList())).Item1;

                //DateTime.Now.ToString("MMMM dd, yyyy")

                var emails = (from left in Result
                              join rightEmployee in Employee on left.CreatedBy equals rightEmployee.SystemUserID
                              join rightPosition in Position on rightEmployee.PositionID equals rightPosition.ID
                              join rightPositionMrf in Position on left.PositionID equals rightPositionMrf.ID
                              join rightOrgGroup in OrgGroup on rightEmployee.OrgGroupID equals rightOrgGroup.ID
                              join rightOrgGroupMrf in OrgGroupMrf on left.OrgGroupID equals rightOrgGroupMrf.ID
                              select new EmailLogsInput()
                              {
                                  PositionTitle = string.Concat(rightPosition.Code, " - ", rightPosition.Title),
                                  Status = left.Status.ToUpper(),
                                  SystemCode = "EMS CRONJOB",
                                  SenderName = "EMS RECRUITMENT",
                                  FromEmailAddress = "noreply@motortrade.com.ph",
                                  Name = string.Concat("(", rightEmployee.Code, ") ", rightEmployee.PersonalInformation.LastName, ", ", rightEmployee.PersonalInformation.FirstName, " ", string.IsNullOrEmpty(rightEmployee.PersonalInformation.MiddleName) ? "" : rightEmployee.PersonalInformation.MiddleName.Substring(0, 1)),
                                  ToEmailAddress = rightEmployee.CorporateEmail,
                                  Subject = string.Concat("CHANGE STATUS: MRF ID# ", left.MRFTransactionID, " | ", rightOrgGroupMrf.Code, " - ", rightOrgGroupMrf.Description, " | ", rightPositionMrf.Code, " - ", rightPositionMrf.Title, " | ", left.Status),
                                  Body = MessageUtilities.EMAIL_BODY_MRF_AUTO_CANCELLED
                                        .Replace("&lt;FormName&gt;", "EMS RECRUITMENT")
                                        .Replace("&lt;RecordID&gt;", left.MRFTransactionID)
                                        .Replace("&lt;Requestor&gt;", string.Concat("(", rightEmployee.Code, ") ", rightEmployee.PersonalInformation.LastName, ", ", rightEmployee.PersonalInformation.FirstName, " ", string.IsNullOrEmpty(rightEmployee.PersonalInformation.MiddleName) ? "" : rightEmployee.PersonalInformation.MiddleName.Substring(0, 1)))
                                        .Replace("&lt;Org&gt;", string.Concat(rightOrgGroupMrf.Code, " - ", rightOrgGroupMrf.Description))
                                        .Replace("&lt;Position&gt;", string.Concat(rightPositionMrf.Code, " - ", rightPositionMrf.Title))
                                        .Replace("&lt;CurrentStatus&gt;", left.Status.ToUpper()),
                              }).ToList();

                var (IsSuccessEmail, MessageEmail) = await new Common_EmailServerCredential(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                    .AddMultipleEmailLogs(emails);

                msg = MessageEmail;
            }

            await new Common_EmailServerCredential(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .AddCronLogs(new List<CronLogsInput> {
                        new CronLogsInput(){
                            CronName = "MRFAutoCancelled",
                            CronLink = "http://192.168.150.6:86/cronjob/?handler=MRFAutoCancelled",
                            Status = (IsSuccess ? "SUCCESS" : "FAILED"),
                            Remarks = string.Concat("Message: ",msg," | Count: ",Result.Count()," | Affected: ",string.Join(",",Result.Select(x => x.MRFTransactionID).ToList()))
                        }
                });

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Result.Select(x => x.MRFTransactionID).ToList();

            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetMRFAutoCancelledReminder()
        {
            var (Result, IsSuccess, Message) = await new Common_MRF(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .GetMRFAutoCancelledReminder();
            var msg = "";
            if (IsSuccess)
            {
                var Employee = ((await new Common_Employee(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                        .GetEmployeeByUserIDs(Result.Select(x => x.CreatedBy).ToList())).Item1).Where(x => !string.IsNullOrEmpty(x.CorporateEmail)).ToList();
                var Position = (await new Common_Position(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                        .GetAll());
                var OrgGroup = (await new Common_OrgGroup(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                    .GetOrgGroupByIDs(Employee.Select(x => x.OrgGroupID).ToList())).Item1;
                var OrgGroupMrf = (await new Common_OrgGroup(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                    .GetOrgGroupByIDs(Result.Select(x => x.OrgGroupID).ToList())).Item1;

                //DateTime.Now.ToString("MMMM dd, yyyy")

                var emails = (from left in Result
                              join rightEmployee in Employee on left.CreatedBy equals rightEmployee.SystemUserID
                              join rightPosition in Position on rightEmployee.PositionID equals rightPosition.ID
                              join rightPositionMrf in Position on left.PositionID equals rightPositionMrf.ID
                              join rightOrgGroup in OrgGroup on rightEmployee.OrgGroupID equals rightOrgGroup.ID
                              join rightOrgGroupMrf in OrgGroupMrf on left.OrgGroupID equals rightOrgGroupMrf.ID
                              select new EmailLogsInput()
                              {
                                  PositionTitle = string.Concat(rightPosition.Code, " - ", rightPosition.Title),
                                  Status = left.Status.ToUpper(),
                                  SystemCode = "EMS CRONJOB",
                                  SenderName = "EMS RECRUITMENT",
                                  FromEmailAddress = "noreply@motortrade.com.ph",
                                  Name = string.Concat("(", rightEmployee.Code, ") ", rightEmployee.PersonalInformation.LastName, ", ", rightEmployee.PersonalInformation.FirstName, " ", string.IsNullOrEmpty(rightEmployee.PersonalInformation.MiddleName) ? "" : rightEmployee.PersonalInformation.MiddleName.Substring(0, 1)),
                                  ToEmailAddress = rightEmployee.CorporateEmail,
                                  Subject = string.Concat("REMINDER: MRF ID# ", left.MRFTransactionID, " | ", rightOrgGroupMrf.Code, " - ", rightOrgGroupMrf.Description, " | ", rightPositionMrf.Code, " - ", rightPositionMrf.Title),
                                  Body = MessageUtilities.EMAIL_BODY_MRF_AUTO_CANCELLED_REMINDER
                                        .Replace("&lt;FormName&gt;", "EMS RECRUITMENT")
                                        .Replace("&lt;RecordID&gt;", left.MRFTransactionID)
                                        .Replace("&lt;Requestor&gt;", string.Concat("(", rightEmployee.Code, ") ", rightEmployee.PersonalInformation.LastName, ", ", rightEmployee.PersonalInformation.FirstName, " ", string.IsNullOrEmpty(rightEmployee.PersonalInformation.MiddleName) ? "" : rightEmployee.PersonalInformation.MiddleName.Substring(0, 1)))
                                        .Replace("&lt;Org&gt;", string.Concat(rightOrgGroupMrf.Code, " - ", rightOrgGroupMrf.Description))
                                        .Replace("&lt;Position&gt;", string.Concat(rightPositionMrf.Code, " - ", rightPositionMrf.Title))
                                        .Replace("&lt;CurrentStatus&gt;", left.Status.ToUpper()),
                              }).ToList();

                var (IsSuccessEmail, MessageEmail) = await new Common_EmailServerCredential(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                    .AddMultipleEmailLogs(emails);
                msg = MessageEmail;
            }

            await new Common_EmailServerCredential(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .AddCronLogs(new List<CronLogsInput> {
                        new CronLogsInput(){
                            CronName = "MRFAutoCancelledReminder",
                            CronLink = "http://192.168.150.6:86/cronjob/?handler=MRFAutoCancelledReminder",
                            Status = (IsSuccess ? "SUCCESS" : "FAILED"),
                            Remarks = string.Concat("Message: ",msg," | Count: ",Result.Count()," | Affected: ",string.Join(",",Result.Select(x => x.MRFTransactionID).ToList()))
                        }
                });

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Result.Select(x => x.MRFTransactionID).ToList();

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetSetupMRFApprover()
        {
            var (Result1, IsSuccess1, Message1) = await new Common_MRF(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .GetSetupMRFApproverInsert();
            var (Result2, IsSuccess2, Message2) = await new Common_MRF(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .GetSetupMRFApproverUpdate();

            await new Common_EmailServerCredential(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .AddCronLogs(new List<CronLogsInput> {
                        new CronLogsInput(){
                            CronName = "SetupMRFApprover(Insert)",
                            CronLink = "http://192.168.150.6:86/cronjob/?handler=SetupMRFApprover",
                            Status = (IsSuccess1 ? "SUCCESS" : "FAILED"),
                            Remarks = string.Concat("Message: ",Message1," | Count: ",Result1.Count()," | Affected: ",string.Join(",",Result1.Select(x => x.ID).ToList()))
                        }
                });
            await new Common_EmailServerCredential(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .AddCronLogs(new List<CronLogsInput> {
                        new CronLogsInput(){
                            CronName = "SetupMRFApprover(Update)",
                            CronLink = "http://192.168.150.6:86/cronjob/?handler=SetupMRFApprover",
                            Status = (IsSuccess2 ? "SUCCESS" : "FAILED"),
                            Remarks = string.Concat("Message: ",Message2," | Count: ",Result2.Count()," | Affected: ",string.Join(",",Result2.Select(x => x.ID).ToList()))
                        }
                });

            List<int> Result = new List<int>();
            Result.AddRange(Result1.Select(x => x.ID).ToList());
            Result.AddRange(Result2.Select(x => x.ID).ToList());

            _resultView.IsSuccess = (IsSuccess1 ? true : IsSuccess2 ? true : false);
            _resultView.Result = Result;

            return new JsonResult(_resultView);
        }


        public async Task<IActionResult> OnGetEmployeeReport()
        {
            var DateNow = DateTime.Now.ToString("yyyy-MM-dd");
            var CronLink = "http://192.168.150.6:86/cronjob/?handler=EmployeeReport";

            List<string> AttachmentName = new List<string>();
            List<string> AttachmentLink = new List<string>();

            var UrlPath = (await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode("URL_LINK")).Where(x => x.Value.Equals("EMPLOYEE_REPORT")).Select(y => y.Description).FirstOrDefault();

            var (Result1, IsSuccess1, Message1) = await new Common_Employee(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .AddEmployeeReport();

            var (Result2, IsSuccess2, Message2) = await new Common_Employee(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .GetEmployeeReportByTDate(DateNow);
            var (Result3, IsSuccess3, Message3) = await new Common_Employee(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .GetEmployeeReportOrgByTDate(DateNow);
            var (Result4, IsSuccess4, Message4) = await new Common_Employee(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .GetEmployeeReportRegionByTDate(DateNow);

            await new Common_EmailServerCredential(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .AddCronLogs(new List<CronLogsInput> {
                        new CronLogsInput(){
                            CronName = "EmployeeReport(Insert)",
                            CronLink = CronLink,
                            Status = (IsSuccess1 ? "SUCCESS" : "FAILED"),
                            Remarks = string.Concat("Message: ",Message1," | Count: Employee: ",Result1.Select(x=>x.Employee).FirstOrDefault(),", Org: ",Result1.Select(x=>x.Org).FirstOrDefault()," | Affected: Employee: ",Result1.Select(x=>x.Employee).FirstOrDefault(),", Org: ",Result1.Select(x=>x.Org).FirstOrDefault())
                        }
                });

            await new Common_EmailServerCredential(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .AddCronLogs(new List<CronLogsInput> {
                        new CronLogsInput(){
                            CronName = "EmployeeReport(Export)",
                            CronLink = CronLink,
                            Status = (IsSuccess2 ? "SUCCESS" : "FAILED"),
                            Remarks = string.Concat("Message: ",Message2," | Count: ",Result2.Count()," | Affected: ",string.Join(",",Result2.Select(x=>x.ID).ToList()))
                        }
                });
            await new Common_EmailServerCredential(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .AddCronLogs(new List<CronLogsInput> {
                        new CronLogsInput(){
                            CronName = "EmployeeReportOrg(Export)",
                            CronLink = CronLink,
                            Status = (IsSuccess3 ? "SUCCESS" : "FAILED"),
                            Remarks = string.Concat("Message: ",Message3," | Count: ",Result3.Count()," | Affected: ",string.Join(",",Result3.Select(x=>x.OrgGroupID).ToList()))
                        }
                });
            await new Common_EmailServerCredential(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .AddCronLogs(new List<CronLogsInput> {
                        new CronLogsInput(){
                            CronName = "EmployeeReportRegion(Export)",
                            CronLink = CronLink,
                            Status = (IsSuccess4 ? "SUCCESS" : "FAILED"),
                            Remarks = string.Concat("Message: ",Message4," | Count: ",Result4.Count()," | Affected: ",string.Join(",",Result4.Select(x=>x.Region).ToList()))
                        }
                });

            if (IsSuccess2)
            {
                string sWebRootFolder = string.Concat(_env.WebRootPath, "/EmployeeReport");
                string sFileName = string.Concat("EmployeeReportAsOf", DateNow, ".xlsx");
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Employee Report");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("New Employee ID");
                    row.CreateCell(1).SetCellValue("Old Employee ID");
                    row.CreateCell(2).SetCellValue("Full Name");
                    row.CreateCell(3).SetCellValue("Position");
                    row.CreateCell(4).SetCellValue("Org Group");
                    row.CreateCell(5).SetCellValue("Org Type");
                    row.CreateCell(6).SetCellValue("Home Branch");
                    row.CreateCell(7).SetCellValue("Region");
                    row.CreateCell(8).SetCellValue("Status");
                    row.CreateCell(9).SetCellValue("Status Updated Date");
                    row.CreateCell(10).SetCellValue("Date Hired");
                    row.CreateCell(11).SetCellValue("Company Tag");

                    excelSheet.SetColumnWidth(0, 3000);
                    excelSheet.SetColumnWidth(1, 3000);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 6000);
                    excelSheet.SetColumnWidth(4, 6000);
                    excelSheet.SetColumnWidth(5, 3000);
                    excelSheet.SetColumnWidth(6, 6000);
                    excelSheet.SetColumnWidth(7, 6000);
                    excelSheet.SetColumnWidth(8, 3000);
                    excelSheet.SetColumnWidth(9, 3000);
                    excelSheet.SetColumnWidth(10, 3000);
                    excelSheet.SetColumnWidth(11, 3000);

                    XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();

                    var colHeaderFont = workbook.CreateFont();
                    colHeaderFont.IsBold = true;
                    colHeaderStyle.SetFont(colHeaderFont);
                    colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                    colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                    colHeaderStyle.Alignment = HorizontalAlignment.Center;
                    alignCenter.Alignment = HorizontalAlignment.Center;

                    for (int i = 0; i <= 11; i++)
                        row.Cells[i].CellStyle = colHeaderStyle;

                    #endregion

                    #region Column Details
                    foreach (var item in Result2)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.Code);
                        row.CreateCell(1).SetCellValue(item.OldEmployeeID);
                        row.CreateCell(2).SetCellValue(item.FullName);
                        row.CreateCell(3).SetCellValue(item.Position);
                        row.CreateCell(4).SetCellValue(item.OrgGroup);
                        row.CreateCell(5).SetCellValue(item.OrgType);
                        row.CreateCell(6).SetCellValue(item.HomeBranch);
                        row.CreateCell(7).SetCellValue(item.Region);
                        row.CreateCell(8).SetCellValue(item.Status);
                        row.CreateCell(9).SetCellValue(item.StatusUpdatedDate);
                        row.CreateCell(10).SetCellValue(item.DateHired);
                        row.CreateCell(11).SetCellValue(item.CompanyTag);

                        row.Cells[0].CellStyle = alignCenter;
                        row.Cells[1].CellStyle = alignCenter;

                        irow++;
                    }
                    #endregion

                    workbook.Write(fs);
                }

                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }

                AttachmentName.Add(sFileName);
                AttachmentLink.Add(string.Concat(UrlPath, "/", sFileName));
            }
            if (IsSuccess3)
            {
                string sWebRootFolder = string.Concat(_env.WebRootPath, "/EmployeeReport");
                string sFileName = string.Concat("EmployeeReportOrgGroupAsOf", DateNow, ".xlsx");
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Employee Count Report Org Group");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Org Group");
                    row.CreateCell(1).SetCellValue("Position");
                    row.CreateCell(2).SetCellValue("Org Type");
                    row.CreateCell(3).SetCellValue("Region");
                    row.CreateCell(4).SetCellValue("Planned Count");
                    row.CreateCell(5).SetCellValue("Draft");
                    row.CreateCell(6).SetCellValue("Probationary");
                    row.CreateCell(7).SetCellValue("Regular");
                    row.CreateCell(8).SetCellValue("Probationary Promoted");
                    row.CreateCell(9).SetCellValue("Outgoing");
                    row.CreateCell(10).SetCellValue("Awol");
                    row.CreateCell(11).SetCellValue("Backout");
                    row.CreateCell(12).SetCellValue("Resigned");
                    row.CreateCell(13).SetCellValue("Deceased");
                    row.CreateCell(14).SetCellValue("Terminated");
                    row.CreateCell(15).SetCellValue("Total Active (D+Prob+Reg+Prom+Out)");
                    row.CreateCell(16).SetCellValue("Variance ((D+Prob+Reg+Prom+Out)-Plan)");

                    excelSheet.SetColumnWidth(0, 6000);
                    excelSheet.SetColumnWidth(1, 6000);
                    excelSheet.SetColumnWidth(2, 3000);
                    excelSheet.SetColumnWidth(3, 6000);
                    excelSheet.SetColumnWidth(4, 3000);
                    excelSheet.SetColumnWidth(5, 3000);
                    excelSheet.SetColumnWidth(6, 3000);
                    excelSheet.SetColumnWidth(7, 3000);
                    excelSheet.SetColumnWidth(8, 3000);
                    excelSheet.SetColumnWidth(9, 3000);
                    excelSheet.SetColumnWidth(10, 3000);
                    excelSheet.SetColumnWidth(11, 3000);
                    excelSheet.SetColumnWidth(12, 3000);
                    excelSheet.SetColumnWidth(13, 3000);
                    excelSheet.SetColumnWidth(14, 3000);
                    excelSheet.SetColumnWidth(15, 3000);
                    excelSheet.SetColumnWidth(16, 3000);

                    XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();

                    var colHeaderFont = workbook.CreateFont();
                    colHeaderFont.IsBold = true;
                    colHeaderStyle.SetFont(colHeaderFont);
                    colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                    colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                    colHeaderStyle.Alignment = HorizontalAlignment.Center;
                    alignCenter.Alignment = HorizontalAlignment.Center;

                    for (int i = 0; i <= 16; i++)
                        row.Cells[i].CellStyle = colHeaderStyle;

                    #endregion

                    #region Column Details
                    foreach (var item in Result3)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.OrgGroup);
                        row.CreateCell(1).SetCellValue(item.Position);
                        row.CreateCell(2).SetCellValue(item.OrgType);
                        row.CreateCell(3).SetCellValue(item.Region);
                        row.CreateCell(4).SetCellValue(item.PlannedCount);
                        row.CreateCell(5).SetCellValue(item.Draft);
                        row.CreateCell(6).SetCellValue(item.Probationary);
                        row.CreateCell(7).SetCellValue(item.Regular);
                        row.CreateCell(8).SetCellValue(item.Promoted);
                        row.CreateCell(9).SetCellValue(item.Outgoing);
                        row.CreateCell(10).SetCellValue(item.Awol);
                        row.CreateCell(11).SetCellValue(item.Backout);
                        row.CreateCell(12).SetCellValue(item.Resigned);
                        row.CreateCell(13).SetCellValue(item.Deceased);
                        row.CreateCell(14).SetCellValue(item.terminated);
                        row.CreateCell(15).SetCellValue(item.TotalActive);
                        row.CreateCell(16).SetCellValue(item.Variance);

                        for (int i = 4; i <= 16; i++)
                            row.Cells[i].CellStyle = alignCenter;

                        irow++;
                    }
                    #endregion

                    workbook.Write(fs);
                }

                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }

                AttachmentName.Add(sFileName);
                AttachmentLink.Add(string.Concat(UrlPath, "/", sFileName));
            }
            if (IsSuccess4)
            {
                string sWebRootFolder = string.Concat(_env.WebRootPath, "/EmployeeReport");
                string sFileName = string.Concat("EmployeeReportRegionAsOf", DateNow, ".xlsx");
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Employee Count Report Per Region");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Region");
                    row.CreateCell(1).SetCellValue("Planned Count");
                    row.CreateCell(2).SetCellValue("Draft");
                    row.CreateCell(3).SetCellValue("Probationary");
                    row.CreateCell(4).SetCellValue("Regular");
                    row.CreateCell(5).SetCellValue("Probationary Promoted");
                    row.CreateCell(6).SetCellValue("Outgoing");
                    row.CreateCell(7).SetCellValue("Awol");
                    row.CreateCell(8).SetCellValue("Backout");
                    row.CreateCell(9).SetCellValue("Resigned");
                    row.CreateCell(10).SetCellValue("Deceased");
                    row.CreateCell(11).SetCellValue("Terminated");
                    row.CreateCell(12).SetCellValue("Total Active (D+Prob+Reg+Prom+Out)");
                    row.CreateCell(13).SetCellValue("Variance ((D+Prob+Reg+Prom+Out)-Plan)");

                    excelSheet.SetColumnWidth(0, 6000);
                    excelSheet.SetColumnWidth(1, 3000);
                    excelSheet.SetColumnWidth(2, 3000);
                    excelSheet.SetColumnWidth(3, 3000);
                    excelSheet.SetColumnWidth(4, 3000);
                    excelSheet.SetColumnWidth(5, 3000);
                    excelSheet.SetColumnWidth(6, 3000);
                    excelSheet.SetColumnWidth(7, 3000);
                    excelSheet.SetColumnWidth(8, 3000);
                    excelSheet.SetColumnWidth(9, 3000);
                    excelSheet.SetColumnWidth(10, 3000);
                    excelSheet.SetColumnWidth(11, 3000);
                    excelSheet.SetColumnWidth(12, 3000);
                    excelSheet.SetColumnWidth(13, 3000);

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
                    foreach (var item in Result4)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.Region);
                        row.CreateCell(1).SetCellValue(item.PlannedCount);
                        row.CreateCell(2).SetCellValue(item.Draft);
                        row.CreateCell(3).SetCellValue(item.Probationary);
                        row.CreateCell(4).SetCellValue(item.Regular);
                        row.CreateCell(5).SetCellValue(item.Promoted);
                        row.CreateCell(6).SetCellValue(item.Outgoing);
                        row.CreateCell(7).SetCellValue(item.Awol);
                        row.CreateCell(8).SetCellValue(item.Backout);
                        row.CreateCell(9).SetCellValue(item.Resigned);
                        row.CreateCell(10).SetCellValue(item.Deceased);
                        row.CreateCell(11).SetCellValue(item.terminated);
                        row.CreateCell(12).SetCellValue(item.TotalActive);
                        row.CreateCell(13).SetCellValue(item.Variance);

                        for (int i = 1; i <= 13; i++)
                            row.Cells[i].CellStyle = alignCenter;

                        irow++;
                    }
                    #endregion

                    workbook.Write(fs);
                }

                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }

                AttachmentName.Add(sFileName);
                AttachmentLink.Add(string.Concat(UrlPath, "/", sFileName));
            }

            var EmailTo = (await new EMS.FrontEnd.SharedClasses.Common_Reference(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode("EMPLOYEE_REPORT")).Where(x => x.Value.Equals("EMAIL")).Select(y => y.Description).FirstOrDefault();

            List<EmailLogsInput> email = new List<EmailLogsInput>() {
            new EmailLogsInput() {
                PositionTitle = "",
                Status = "",
                SystemCode = "EMS CRONJOB",
                SenderName = "EMS REPORT",
                FromEmailAddress = "noreply@motortrade.com.ph",
                Name = "",
                ToEmailAddress = EmailTo,
                Subject = string.Concat("EMS Plantilla Count as of ",DateNow),
                Body = MessageUtilities.EMAIL_BODY_EMPLOYEE_REPORT
                    .Replace("&lt;ActiveEmployeeCount&gt;", Result1.Select(x=>x.ActiveEmployeeCount.ToString()).FirstOrDefault())
                    .Replace("&lt;PlannedCount&gt;", Result1.Select(x=>x.PlannedCount.ToString()).FirstOrDefault())
                    .Replace("&lt;CountPercent&gt;", string.Concat(Result1.Select(x=>x.CountPercent).FirstOrDefault(),"%")),
                AttachmentName = string.Join(",",AttachmentName),
                AttachmentLink = string.Join(",",AttachmentLink)
                }
            }.ToList();

            var (IsSuccessEmail, MessageEmail) = await new Common_EmailServerCredential(_iconfiguration, new GlobalCurrentUser { UserID = 1 }, _env)
                .AddMultipleEmailLogs(email);

            _resultView.IsSuccess = (IsSuccess1);
            _resultView.Result = Result1;

            return new JsonResult(_resultView);
        }
    }
}
