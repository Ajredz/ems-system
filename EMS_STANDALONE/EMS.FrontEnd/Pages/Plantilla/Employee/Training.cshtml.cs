using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Workflow.Transfer.EmailServerCredential;
using EMS.Workflow.Transfer.Training;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Plantilla.Employee
{
    public class TrainingModel :  SharedClasses.Utilities
    {
        [BindProperty]
        public Utilities.API.ChangeStatus ChangeStatus { get; set; }
        public TrainingModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }
        public virtual async Task OnGetAsync()
        {

            if (_systemURL != null)
            {
                ViewData["HasAdd"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/TRAININGADD")).Count() > 0 ? "true" : "false";
                ViewData["HasChangeStatus"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/TRAININGCHANGESTATUS")).Count() > 0 ? "true" : "false";
            }

        }
        public async Task<IActionResult> OnGetListAsync([FromQuery] GetEmployeeTrainingListInput param)
        {
            var StatusColor = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(7)).WorkflowStepList;

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
                          Status = left.Status,
                          StatusDescription = right.StepDescription,
                          StatusColor = right.StatusColor,
                          StatusUpdateDate = left.StatusUpdateDate,
                          Type = left.Type,
                          Title = left.Title,
                          Description = left.Description,
                          CreatedBy = left.CreatedBy,
                          CreatedByName = string.Concat("(",rightCreatedBy.Username,") ",rightCreatedBy.LastName,", ",rightCreatedBy.FirstName," ",rightCreatedBy.MiddleName),
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
        public async Task<JsonResult> OnGetTrainingTemplateDropdown()
        {
            _resultView.Result = await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                .GetTrainingTemplateDropdown();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetTrainingTemplateByID([FromQuery] int TrainingTemplateID)
        {
            var Result = await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                .GetTrainingTemplateDetails(TrainingTemplateID);
            var jsonData = new
            {
                total = Result.Count,
                pageNumber = 1,
                sort = "Title",
                records = Result.Count,
                rows = Result
            };
            return new JsonResult(jsonData);
        }
        public async Task<JsonResult> OnPostAddEmployeeTrainingTemplate([FromQuery] long EmployeeID, [FromQuery] int TrainingTemplateID)
        {
            var (IsSuccess,Message) = (await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                .AddEmployeeTrainingTemplate(new AddEmployeeTrainingInput
                {
                    EmployeeID = EmployeeID,
                    TrainingEmployeeID = new List<int> { TrainingTemplateID }
                }));

            _resultView.Result = Message;
            _resultView.IsSuccess = IsSuccess;
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
            var JsonConvertresult = (JsonConvert.DeserializeObject<GetClassroomFromELMS>(result).Data).Where(x => string.Concat("(", x.Id.ToString().PadLeft(4, '0'), ") ", x.Classroom).ToUpper().Contains((term ?? "").Trim().ToUpper())).Take(TopResults).ToList();
            List<Dropdown> dropdown = JsonConvertresult.Select(x => new Dropdown()
            {
                Text = string.Concat("(", x.Id.ToString().PadLeft(4, '0'), ") ", x.Classroom),
                Value = x.Id.ToString()
            }).ToList();

            _resultView.IsSuccess = true;
            _resultView.Result = dropdown;
            return new JsonResult(_resultView);
        }
    }
}
