using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Manpower;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Manpower.Transfer.MRF;
using EMS.Recruitment.Transfer.Applicant;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using NPOI.HSSF.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Manpower.MRF
{
    public class AddApplicantModel : SharedClasses.Utilities
    {
        [BindProperty]
        public MRFApplicantForm Form { get; set; }

        [BindProperty]
        public EMS.Workflow.Transfer.Workflow.AddWorkflowTransaction Workflow { get; set; }

        [BindProperty]
        public EMS.Manpower.Transfer.MRF.MRFApplicantCommentsForm CommentsForm { get; set; }

        public AddApplicantModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
        }

        public async Task OnGet(int ID)
        {
            if (_globalCurrentUser != null)
            {
                if (_systemURL != null)
                {
                    ViewData["HasAddApplicantFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/RECRUITMENT/APPLICANT/ADD")).Count() > 0 ? "true" : "false";
                    ViewData["HasRemoveApplicantFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/RECRUITMENT/APPLICANT/ADD")).Count() > 0 ? "true" : "false";
                    ViewData["HasUpdateForHiringFeature"] = 
                        _systemURL.Where(x => x.URL.ToUpper().Equals("/MANPOWER/MRF/ADMIN/UPDATEFORHIRINGAPPLICANT")).Count() > 0 ? "true" : "false";
                    ViewData["HasUpdateStatusFeature"] =
                        _systemURL.Where(x => x.URL.ToUpper().Equals("/MANPOWER/MRF/ADMIN/UPDATESTATUS")).Count() > 0 ? "true" : _systemURL.Where(x => x.URL.ToUpper().Equals("/MANPOWER/MRF/UPDATESTATUS")).Count() > 0 ? "true" : "false";
                }

                Form = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env).GetMRFApplicant(ID);
                Form.DatePrinted = DateTime.Now.ToString("dddd, dd MMMM, yyyy hh:mm tt");

                EMS.Plantilla.Transfer.Employee.GetByIDOutput employee =
                    (new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeByUserID(Form.SubmittedByID)).Result;

                Form.SubmittedBy = string.Concat(employee.LastName,
                        string.IsNullOrEmpty(employee.FirstName) ? "" : string.Concat(", ", employee.FirstName),
                        string.IsNullOrEmpty(employee.MiddleName) ? "" : string.Concat(" ", employee.MiddleName));


                EMS.Recruitment.Transfer.Applicant.Form applicant =
                    Form.ApplicantIDs.Where(x => x.ForHiring).Count() > 0 ?
                    (await new SharedClasses.Common_Recruitment.Common_Applicant(_iconfiguration, _globalCurrentUser, _env).GetApplicant(
                    Form.ApplicantIDs.Where(x => x.ForHiring).FirstOrDefault().ApplicantID
                    )) : new EMS.Recruitment.Transfer.Applicant.Form();

                ViewData["MRFTransactionID"] = Form.MRFTransactionID;

                ViewData["ForHiringApplicantName"] = 
                    string.Concat(applicant.PersonalInformation?.LastName, ", ", 
                    applicant.PersonalInformation?.FirstName, applicant.PersonalInformation?.MiddleName ?? "");

                ViewData["MRFIDForHiring"] = Form.ApplicantIDs.Where(x => x.ForHiring).Count() > 0 ? 
                    Form.ApplicantIDs.Where(x => x.ForHiring).FirstOrDefault().ApplicantID : 0;
                ViewData["MRFStatus"] = Form.StatusCode;


                ViewData["LastStepCode"] = (await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetLastStepByWorkflowCode("RECRUITMENT")).Code;
            }
        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] EMS.Manpower.Transfer.MRF.GetMRFExistingApplicantListInput param)
        {
            var URL = string.Concat(_manpowerBaseURL,
                  _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetMRFExistingApplicantList").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "MRFID=", param.MRFID, "&",
                  "IDDelimited=", param.IDDelimited, "&",
                  "ID=", param.ID, "&",
                  "ForHiringID=", param.ForHiringID, "&",
                  "ApplicantName=", param.ApplicantName, "&",
                  "CurrentStepDelimited=", param.CurrentStepDelimited, "&",
                  "StatusDelimited=", param.StatusDelimited,"&",
                  "DateScheduledFrom=", param.DateScheduledFrom, "&",
                  "DateScheduledTo=", param.DateScheduledTo, "&",
                  "DateCompletedFrom=", param.DateCompletedFrom, "&",
                  "DateCompletedTo=", param.DateCompletedTo, "&",
                  "ApproverRemarks=", param.ApproverRemarks);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetMRFExistingApplicantListOutput>(), URL);

            var LastStatusUpdate = (await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetLastStatusUpdateByRecordIDs(Result.Select(x=>(int)x.MRFApplicantID).ToList())).Item1;

            List<EMS.Security.Transfer.SystemUser.Form> systemUsersUpdateBy =
                await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(LastStatusUpdate.Select(x => x.ApprovedBy).Distinct().ToList());

            Result = (from left in Result
                      join right in 
                      (from left in LastStatusUpdate
                       join right in systemUsersUpdateBy on left.ApprovedBy equals right.ID into joinedList
                       from sub in joinedList.DefaultIfEmpty()
                       select new GetMRFExistingApplicantListOutput
                       {
                           MRFApplicantID = long.Parse(left.RecordID),
                           LastUpdateDate = left.EndDateTime,
                           UpdatedBy = left.ApprovedBy,
                           UpdateByName = sub == null ? "" : string.Concat("(", sub.Username, ") ", sub.LastName, ", ", sub.FirstName, " ", sub.MiddleName),
                       }) on left.MRFApplicantID equals right.MRFApplicantID into joinedList
                      from sub in joinedList.DefaultIfEmpty()
                      select new GetMRFExistingApplicantListOutput
                      {
                          TotalNoOfRecord = left.TotalNoOfRecord,
                          NoOfPages = left.NoOfPages,
                          Row = left.Row,

                          ID = left.ID,
                          MRFApplicantID = left.MRFApplicantID,
                          ApplicantName = left.ApplicantName,
                          ApplicationSource = left.ApplicationSource,
                          CurrentStep = left.CurrentStep,
                          Status = left.Status,
                          WorkflowID = left.WorkflowID,
                          CurrentStepCode = left.CurrentStepCode,
                          CurrentResult = left.CurrentResult,
                          ResultType = left.ResultType,
                          DateScheduled = left.DateScheduled,
                          DateCompleted = left.DateCompleted,
                          ApproverRemarks = left.ApproverRemarks,
                          Points = left.Points,
                          TotalPoints = left.TotalPoints,
                          Flag = left.Flag,
                          LastUpdateDate = sub == null ? "" : sub.LastUpdateDate,
                          UpdatedBy = sub == null ? 0 : sub.UpdatedBy,
                          UpdateByName = sub == null ? "" : sub.UpdateByName,
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

        public async Task<JsonResult> OnPostAsync()
        {
            var URL = string.Concat(_manpowerBaseURL,
                   _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("AddApplicant").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(Form, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "MRFApplicant",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(Form.MRFTransactionID, " Applicant added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }
        
        public async Task<JsonResult> OnPostSaveWorkflow()
        {
            //if (!Form.StartDatetime.HasValue)
            //{
            //    Form.StartDatetime = (await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env)
            //    .GetApplicant(Form.RecordID)).CreatedDate;
            //}

            var (CurrentWorkflowStep, IsSuccess, Message) = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .AddTransaction(Workflow);

            if (IsSuccess)
            {
                List<int> roles =
                (await new Common_SystemRole(_iconfiguration, _globalCurrentUser, _env).GetSystemRoleByUserID())
                .Select(x => x.RoleID).ToList();

                foreach (var item in Workflow.BatchUpdateRecordIDs)
                {
                    await new Common_MRF(_iconfiguration, _globalCurrentUser, _env)
                    .UpdateCurrentWorkflowStep(new EMS.Manpower.Transfer.MRF.UpdateCurrentWorkflowStepInput
                    {
                        ID = item,
                        CurrentStepCode = CurrentWorkflowStep.StepCode,
                        CurrentStepDescription = CurrentWorkflowStep.StepDescription,
                        CurrentStepApproverRoleIDs = CurrentWorkflowStep.ApproverRoleIDs,
                        WorkflowStatus = CurrentWorkflowStep.WorkflowStatus,
                        DateScheduled = Workflow.DateScheduled,
                        DateCompleted = Workflow.DateCompleted,
                        ApproverRemarks = Workflow.Remarks,
                    });
                }

                foreach (var item in Workflow.BatchUpdateApplicantIDs)
                {
                    await new SharedClasses.Common_Recruitment.Common_Applicant(_iconfiguration, _globalCurrentUser, _env)
                            .UpdateCurrentWorkflowStep(new EMS.Recruitment.Transfer.Applicant.UpdateCurrentWorkflowStepInput
                            {
                                ApplicantID = item,
                                CurrentStepCode = CurrentWorkflowStep.StepCode,
                                CurrentStepDescription = CurrentWorkflowStep.StepDescription,
                                CurrentStepApproverRoleIDs = CurrentWorkflowStep.ApproverRoleIDs,
                                WorkflowStatus = CurrentWorkflowStep.WorkflowStatus,
                                DateScheduled = Workflow.DateScheduled,
                                DateCompleted = Workflow.DateCompleted,
                                ApproverRemarks = Workflow.Remarks,
                                Result = Workflow.Result,
                            });
                }
            }

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = IsSuccess ? MessageUtilities.SCSSMSG_REC_UPDATE : Message;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            var result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            _resultView.Result = result.Select(x => new SelectListItem() { 
                    Value = x.Value,
                    Text = x.Description
            });
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetWorkflowStep(string WorkflowCode, string StepCode)
        {
            var result = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetWorkflowStepByWorkflowIDAndCode(new GetWorkflowStepByWorkflowIDAndCodeInput {
                    WorkflowCode = WorkflowCode,
                    Code = StepCode
                });
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetWorkflowTransaction(int WorkflowID, int MRFApplicantID)
        {
            AddWorkflowTransaction result = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetTransactionByRecordID(new GetTransactionByRecordIDInput { WorkflowID = WorkflowID, RecordID = MRFApplicantID });

            var jsonData = new
            {
                total = result.History.Count,
                pageNumber = 1,
                sort = "Timestamp",
                records = result.History.Count,
                rows = result.History
            };
            return new JsonResult(jsonData);
        }

        public async Task<JsonResult> OnGetMRFApplicantComments(int MRFID)
        {
            _resultView.Result = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env).GetMRFApplicantComments(MRFID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostSaveComments()
        {
            CommentsForm.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_manpowerBaseURL,
                _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("AddApplicantComments").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(CommentsForm, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "MRFApplicantComments",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(Form.MRFTransactionID, " Applicant Comment added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetApplicantByMRFIDAndID(int MRFID, int ApplicantID)
        {
            _resultView.Result = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env).GetApplicantByMRFIDAndID(
                new GetApplicantByMRFIDAndIDInput
                {
                     MRFID = MRFID,
                     ApplicantID = ApplicantID
                });
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }


    }
}