using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Plantilla.Transfer.OrgGroup;
using EMS.Workflow.Transfer.Accountability;
using EMS.Workflow.Transfer.EmailServerCredential;
using EMS.Workflow.Transfer.Training;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NPOI.HSSF.Record;
using NPOI.HSSF.UserModel;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Training.AllTraining
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
            ViewData["HasView"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/TRAINING/ALLTRAINING/VIEW")).Count() > 0 ? "true" : "false";
            ViewData["HasUpload"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/TRAINING/ALLTRAINING/UPLOAD")).Count() > 0 ? "true" : "false";
            ViewData["HasChangeStatus"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/TRAINING/ALLTRAINING/CHANGESTATUS")).Count() > 0 ? "true" : "false";
        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] GetEmployeeTrainingListInput param)
        {
            var StatusColor = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(7)).WorkflowStepList;

            param.IsAdminAccess = _systemURL.Where(x => x.URL.ToUpper().Equals("/TRAINING/ALLTRAINING/ADMIN")).Count() > 0 ? true : false;
            if (!param.IsAdminAccess)
            {
                param.UnderAccess = _globalCurrentUser.EmployeeUnderAccess;
            }

            var (Result, IsSuccess, ErrorMessage) = await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeTrainingList(param);

            List<EMS.Plantilla.Transfer.Employee.Form> employeeName =
                (await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeByIDs(Result.Where(x => x.EmployeeID != 0).Select(x => x.EmployeeID).Distinct().ToList())).Item1;
            List<EMS.Security.Transfer.SystemUser.Form> systemUsersCreatedBy =
                await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy).Distinct().ToList());
            List<EMS.Security.Transfer.SystemUser.Form> systemUsersModifiedBy =
                await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(Result.Where(x => x.ModifiedBy != null).Select(x => (int)x.ModifiedBy).Distinct().ToList());

            Result = (from left in Result
                      join right in StatusColor on left.Status equals right.StepCode
                      join rightEmployeeName in employeeName on left.EmployeeID equals rightEmployeeName.ID
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
                          EmployeeName = string.Concat("(", rightEmployeeName.Code, ") ", rightEmployeeName.PersonalInformation.LastName, ", ", rightEmployeeName.PersonalInformation.FirstName, " ", rightEmployeeName.PersonalInformation.MiddleName),
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
        public async Task<JsonResult> OnGetEmployeeAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployeeAutoComplete(term, TopResults);
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
        public async Task<JsonResult> OnPostChangeStatus([FromQuery] string ID)
        {
            ChangeStatus.ID = (ID.Split(",")).Select(x => long.Parse(x)).ToList();

            var (IsSuccess, Message) = await new Common_Training(_iconfiguration, _globalCurrentUser, _env).PostChangeStatus(ChangeStatus);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = "CHANGE STATUS",
                        TableName = "EmployeeTrainingStatusHistory",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat("Change Status ID: ", ID),
                        IsSuccess = IsSuccess,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                var AllStatus = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetWorkflow(7)).WorkflowStepList;
                var CurrentStatus = AllStatus.Where(x => x.StepCode.Equals(ChangeStatus.Status)).FirstOrDefault();
                if (CurrentStatus.SendEmailToApprover || CurrentStatus.SendEmailToRequester)
                {
                    var NextStatus = AllStatus.Where(x => x.Order.Equals(CurrentStatus.Order + 1)).ToList();

                    var EmployeeTrainings = (await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                        .GetEmployeeByEmployeeTrainingIDs(ChangeStatus.ID)).Item1;
                    var Employee = ((await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                        .GetEmployeeByIDs(EmployeeTrainings.Select(x => x.EmployeeID).Distinct().ToList())).Item1).Where(x => !string.IsNullOrEmpty(x.CorporateEmail)).ToList();
                    var Position = (await new Common_Position(_iconfiguration, _globalCurrentUser, _env)
                        .GetAll());
                    var OrgGroup = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                        .GetOrgGroupByIDs(Employee.Select(x => x.OrgGroupID).ToList())).Item1;

                    if (CurrentStatus.SendEmailToRequester)
                    {
                        var emails = (from left in Employee
                                      join right in EmployeeTrainings on left.ID equals right.EmployeeID
                                      join rightPosition in Position on left.PositionID equals rightPosition.ID
                                      join rightOrgGroup in OrgGroup on left.OrgGroupID equals rightOrgGroup.ID
                                      select new EmailLogsInput()
                                      {
                                          PositionTitle = String.Concat(rightPosition.Code, " - ", rightPosition.Title),
                                          Status = CurrentStatus.StepDescription,
                                          SystemCode = "EMS Training",
                                          SenderName = "EMS Training",
                                          FromEmailAddress = "noreply@motortrade.com.ph",
                                          Name = String.Concat(left.PersonalInformation.LastName, ", ", left.PersonalInformation.FirstName, " ", left.PersonalInformation.MiddleName.Substring(0, 1)),
                                          ToEmailAddress = left.CorporateEmail,
                                          Subject = String.Concat("CHANGE STATUS: TRAINING | ", right.Title, " | ", CurrentStatus.StepDescription),
                                          Body = MessageUtilities.EMAIL_BODY_TRAINING
                                                    .Replace("&lt;FormName&gt;", "TRAINING")
                                                    .Replace("&lt;RecordID&gt;", right.ID.ToString().PadLeft(7, '0'))
                                                    .Replace("&lt;EmployeeName&gt;", String.Concat(left.PersonalInformation.LastName, ", ", left.PersonalInformation.FirstName, " ", left.PersonalInformation.MiddleName.Substring(0, 1)))
                                                    .Replace("&lt;Remarks&gt;", ChangeStatus.Remarks)
                                                    .Replace("&lt;CurrentStatus&gt;", CurrentStatus.StepDescription)
                                                    .Replace("&lt;UpdatedBy&gt;", String.Concat("(", _globalCurrentUser.Username, ") ", _globalCurrentUser.LastName ?? "", ", ", _globalCurrentUser.FirstName ?? "", " ", string.IsNullOrEmpty(_globalCurrentUser.MiddleName ?? "").ToString().Substring(0, 1)))
                                                    .Replace("&lt;UpdatedDate&gt;", DateTime.Now.ToString("yyyy-MM-dd HH:mm")),
                                      }).ToList();

                        var (IsSuccessEmail, MessageEmail) = await new Common_EmailServerCredential(_iconfiguration, _globalCurrentUser, _env)
                            .AddMultipleEmailLogs(emails);
                    }
                    if (CurrentStatus.SendEmailToApprover)
                    {

                    }
                }
            }

            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetDownloadTrainingTemplate()
        {
            string sWebRootFolder = _env.WebRootPath;
            string sFileName = "Training.xlsx";
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();

                // Header style
                XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();
                var format = workbook.CreateDataFormat().GetFormat("text");
                var colHeaderFont = workbook.CreateFont();
                colHeaderFont.IsBold = true;
                colHeaderStyle.SetFont(colHeaderFont);
                colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                colHeaderStyle.Alignment = HorizontalAlignment.Center;
                colHeaderStyle.DataFormat = format;
                alignCenter.Alignment = HorizontalAlignment.Center;

                // Dropdown values
                var typeList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                    .GetReferenceValueByRefCode("TRAINING_TYPE"))
                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();
                var Status = ((await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetWorkflow(7)).WorkflowStepList).Select(x=>x.StepCode).ToArray();
        
                IDataValidation getDropdownValidation(CellRangeAddressList addressList, string[] stringArray, ISheet excelSheet)
                {
                    IDataValidationHelper validationHelper = new XSSFDataValidationHelper((XSSFSheet)excelSheet);
                    IDataValidationConstraint constraint = validationHelper.CreateExplicitListConstraint(stringArray);
                    IDataValidation dataValidation = validationHelper.CreateValidation(constraint, addressList);
                    dataValidation.SuppressDropDownArrow = true;
                    dataValidation.ShowErrorBox = true;
                    dataValidation.CreateErrorBox("", "Please select from Dropdown values.");
                    return dataValidation;
                }


                void AddETFSheet()
                {
                    ISheet excelSheet = workbook.CreateSheet("Training");
                    int rowCtr = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(rowCtr); rowCtr++;

                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 1, 1), typeList, excelSheet));
                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 3, 3), Status, excelSheet));

                    row.CreateCell(0).SetCellValue("* Employee Code");
                    row.CreateCell(1).SetCellValue("* Type");
                    row.CreateCell(2).SetCellValue("* Title");
                    row.CreateCell(3).SetCellValue("* Status");
                    row.CreateCell(4).SetCellValue("* Date Schedule (MM/DD/YYYY)");
                    row.CreateCell(5).SetCellValue("Description");
                    row.CreateCell(6).SetCellValue("Classroom ID");

                    excelSheet.SetColumnWidth(0, 6500);
                    excelSheet.SetColumnWidth(1, 4500);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 6500);
                    excelSheet.SetColumnWidth(4, 7500);
                    excelSheet.SetColumnWidth(5, 6500);
                    excelSheet.SetColumnWidth(6, 4500);

                    for (int i = 0; i <= 6; i++)
                        row.Cells[i].CellStyle = colHeaderStyle;

                    #endregion

                    for (int x = rowCtr; x <= 10000; x++)
                    {
                        IRow rowDate = excelSheet.CreateRow(x);
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;

                        for (int i = 0; i <= 6; i++)
                            rowDate.CreateCell(i, CellType.String).SetCellValue("");

                        for (int i = 0; i <= 6; i++)
                            rowDate.Cells[i].CellStyle = textCS;
                    }

                }

                AddETFSheet();

                workbook.Write(fs);
            }

            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        public async Task<IActionResult> OnPostUploadInsert()
        {
            IFormFile file = Request.Form.Files[0];
            string filePath = Path.Combine(_env.WebRootPath, "\\Training\\Uploads");
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(filePath, file.FileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;

                    if (sFileExtension == ".xls")//This will read the Excel 97-2000 formats    
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                    else //This will read 2007 Excel format    
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }

                    IRow headerRow = sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;

                    List<TrainingUploadFile> uploadList = new List<TrainingUploadFile>();
                    int blankRows = 0;

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        if (row != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()))
                            {
                                TrainingUploadFile obj = new TrainingUploadFile
                                {
                                    RowNum = (i + 1).ToString(),
                                    EmployeeCode = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Type = row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Title = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Status = row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    DateSchedule = row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Description = row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    ClassroomIDString = row.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()
                                };
                                uploadList.Add(obj);
                            }
                        }
                        else
                        {
                            blankRows++;
                            if (blankRows > 4)
                                break;
                        }

                    }

                    var EmployeeCodesDelimited = string.Join(",", uploadList.Select(x => x.EmployeeCode).Distinct());
                    var EmployeeDetails = await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                        .GetByCodes(EmployeeCodesDelimited);
                    //var Status = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(7)).WorkflowStepList;
                    
                    var ClassroomResult = (await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                        .GetClassroomFromELMS("", 0)).Item2;
                    var JsonClassroomResult = (JsonConvert.DeserializeObject<GetClassroomFromELMS>(ClassroomResult).Data).ToList();

                    var Upload = (from left in uploadList
                                  join right in EmployeeDetails on left.EmployeeCode equals right.Code
                                  join rightClassroom in JsonClassroomResult on (left.ClassroomIDString == "" ? 0 : Convert.ToInt32(left.ClassroomIDString)) equals rightClassroom.Id into classList
                                  from sub in classList.DefaultIfEmpty()
                                  select new TrainingUploadFile
                                  {
                                      RowNum = left.RowNum,
                                      EmployeeID = right.ID,
                                      EmployeeCode = left.EmployeeCode,
                                      Status = left.Status,
                                      DateSchedule = left.DateSchedule,
                                      Type = left.Type,
                                      Title = left.Title,
                                      Description = left.Description,
                                      ClassroomIDString = left.ClassroomIDString,
                                      ClassroomName = sub == null ? "" : string.Concat("(", sub.Id.ToString().PadLeft(4, '0'), ") ", sub.Classroom)
                                  }).ToList();

                    var (IsSuccess, Message) = await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                        .UploadInsert(Upload);

                    if (IsSuccess)
                    {
                        await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                            .AddAuditLog(new Security.Transfer.AuditLog.Form
                            {
                                EventType = "UPLOAD",
                                TableName = "EmployeeTraining",
                                TableID = 0, // New Record, no ID yet
                                Remarks = string.Concat("Training Added to EmployeeCode: ", EmployeeCodesDelimited),
                                IsSuccess = true,
                                CreatedBy = _globalCurrentUser.UserID
                            });
                    }

                    _resultView.IsSuccess = IsSuccess;
                    _resultView.Result = Message;
                }
            }

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

        public async Task<JsonResult> OnGetClassroomNameAutoComplete(string term, int TopResults)
        {
            var result = (await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                .GetClassroomFromELMS(term, TopResults)).Item2;
            var JsonConvertresult = (JsonConvert.DeserializeObject<GetClassroomFromELMS>(result).Data).Where(x => string.Concat("(",x.Id.ToString().PadLeft(4, '0'), ") ", x.Classroom).ToUpper().Contains((term ?? "").Trim().ToUpper())).Take(TopResults).ToList();
            List<Dropdown> dropdown = JsonConvertresult.Select(x=> new Dropdown() { 
                                    Text = string.Concat("(",x.Id.ToString().PadLeft(4,'0'),") ",x.Classroom),
                                    Value = x.Id.ToString()
                                    }).ToList();

            _resultView.IsSuccess = true;
            _resultView.Result = dropdown;
            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetExportListAsync([FromQuery] GetEmployeeTrainingListInput param)
        {
            param.IsAdminAccess = _systemURL.Where(x => x.URL.ToUpper().Equals("/TRAINING/ALLTRAINING/ADMIN")).Count() > 0 ? true : false;
            if (!param.IsAdminAccess)
            {
                param.UnderAccess = _globalCurrentUser.EmployeeUnderAccess;
            }

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

            param.IsAdminAccess = _systemURL.Where(x => x.URL.ToUpper().Equals("/TRAINING/ALLTRAINING/ADMIN")).Count() > 0 ? true : false;
            if (!param.IsAdminAccess)
            {
                param.UnderAccess = _globalCurrentUser.EmployeeUnderAccess;
            }

            var (Result, IsSuccess, ErrorMessage) = await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeTrainingList(param);

            List<EMS.Plantilla.Transfer.Employee.Form> employeeName =
                (await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeByIDs(Result.Where(x => x.EmployeeID != 0).Select(x => x.EmployeeID).Distinct().ToList())).Item1;
            List<EMS.Security.Transfer.SystemUser.Form> systemUsersCreatedBy =
                await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy).Distinct().ToList());
            List<EMS.Security.Transfer.SystemUser.Form> systemUsersModifiedBy =
                await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(Result.Where(x => x.ModifiedBy != null).Select(x => (int)x.ModifiedBy).Distinct().ToList());
            List<OrgGroupSOMDOutput> orgGroupSOMDOutputs =
                (await new SharedClasses.Common_Plantilla.Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupSOMD(employeeName.Select(x => x.OrgGroupID).Distinct().ToList())).Item1;

            var OrgGroup = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupByIDs(employeeName.Select(x => x.OrgGroupID).Distinct().ToList())).Item1;
            var employeePos = (await new Common_Position(_iconfiguration, _globalCurrentUser, _env)
                        .GetAll());

            Result = (from left in Result
                      join right in StatusColor on left.Status equals right.StepCode
                      join rightEmployeeName in employeeName on left.EmployeeID equals rightEmployeeName.ID
                      join rightPosition in employeePos on rightEmployeeName.PositionID equals rightPosition.ID
                      join rightOrg in OrgGroup on rightEmployeeName.OrgGroupID equals rightOrg.ID
                      join rightOrgSOMD in orgGroupSOMDOutputs on rightEmployeeName.OrgGroupID equals rightOrgSOMD.ID
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
                          EmployeeName = string.Concat("(", rightEmployeeName.Code, ") ", rightEmployeeName.PersonalInformation.LastName, ", ", rightEmployeeName.PersonalInformation.FirstName, " ", rightEmployeeName.PersonalInformation.MiddleName),
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
                          ModifiedDate = left.ModifiedDate,
                          Position = String.Concat(rightPosition.Code," - ",rightPosition.Title),
                          OrgGroup = String.Concat(rightOrg.Code, " - ", rightOrg.Description),
                          Clus = rightOrgSOMD.Clus,
                          Area = rightOrgSOMD.Area,
                          Reg = rightOrgSOMD.Reg,
                          Zone = rightOrgSOMD.Zone,
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
                    row.CreateCell(1).SetCellValue("Employee");
                    row.CreateCell(2).SetCellValue("Type");
                    row.CreateCell(3).SetCellValue("Title");
                    row.CreateCell(4).SetCellValue("Status");
                    row.CreateCell(5).SetCellValue("Date Schedule");
                    row.CreateCell(6).SetCellValue("Position");
                    row.CreateCell(7).SetCellValue("Org Group");
                    row.CreateCell(8).SetCellValue("Cluster");
                    row.CreateCell(9).SetCellValue("Area");
                    row.CreateCell(10).SetCellValue("Region");
                    row.CreateCell(11).SetCellValue("Zone");
                    row.CreateCell(12).SetCellValue("Created By");
                    row.CreateCell(13).SetCellValue("Created Date");
                    row.CreateCell(14).SetCellValue("Modified By");
                    row.CreateCell(15).SetCellValue("Modified Date");

                    excelSheet.SetColumnWidth(0, 3000);
                    excelSheet.SetColumnWidth(1, 12000);
                    excelSheet.SetColumnWidth(2, 4000);
                    excelSheet.SetColumnWidth(3, 6000);
                    excelSheet.SetColumnWidth(4, 4000);
                    excelSheet.SetColumnWidth(5, 5000);
                    excelSheet.SetColumnWidth(6, 6000);
                    excelSheet.SetColumnWidth(7, 6000);
                    excelSheet.SetColumnWidth(8, 6000);
                    excelSheet.SetColumnWidth(9, 6000);
                    excelSheet.SetColumnWidth(10, 6000);
                    excelSheet.SetColumnWidth(11, 6000);
                    excelSheet.SetColumnWidth(12, 12000);
                    excelSheet.SetColumnWidth(13, 5000);
                    excelSheet.SetColumnWidth(14, 12000);
                    excelSheet.SetColumnWidth(15, 5000);

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
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.ID.ToString());
                        row.CreateCell(1).SetCellValue(item.EmployeeName);
                        row.CreateCell(2).SetCellValue(item.Type);
                        row.CreateCell(3).SetCellValue(item.Title);
                        row.CreateCell(4).SetCellValue(item.StatusDescription);
                        row.CreateCell(5).SetCellValue(item.DateSchedule);
                        row.CreateCell(6).SetCellValue(item.Position);
                        row.CreateCell(7).SetCellValue(item.OrgGroup);
                        row.CreateCell(8).SetCellValue(item.Clus);
                        row.CreateCell(9).SetCellValue(item.Area);
                        row.CreateCell(10).SetCellValue(item.Reg);
                        row.CreateCell(11).SetCellValue(item.Zone);
                        row.CreateCell(12).SetCellValue(item.CreatedByName);
                        row.CreateCell(13).SetCellValue(item.CreatedDate);
                        row.CreateCell(14).SetCellValue(item.ModifiedByName);
                        row.CreateCell(15).SetCellValue(item.ModifiedDate);

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
