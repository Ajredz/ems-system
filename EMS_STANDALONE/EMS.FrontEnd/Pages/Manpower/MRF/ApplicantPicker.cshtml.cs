using EMS.FrontEnd.SharedClasses.Common_Manpower;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Manpower.Transfer.MRF;
using EMS.Recruitment.Transfer.Applicant;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Manpower.MRF
{
    public class ApplicantPickerModel : SharedClasses.Utilities
    {
        [BindProperty]
        public MRFPickApplicantForm Form { get; set; }

        [BindProperty]
        public EMS.Workflow.Transfer.Workflow.AddWorkflowTransaction Workflow { get; set; }

        public ApplicantPickerModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
        }

        public void OnGet()
        {
        }

        //public async Task<IActionResult> OnGetListAsync([FromQuery] EMS.Manpower.Transfer.MRF.GetMRFExistingApplicantListInput param)
        //{
        //    var URL = string.Concat(_recruitmentBaseURL,
        //          _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("GetMRFExistingApplicantList").Value, "?",
        //          "userid=", _globalCurrentUser.UserID, "&",
        //          "sidx=", param.sidx, "&",
        //          "sord=", param.sord, "&",
        //          "pageNumber=", param.pageNumber, "&",
        //          "rows=", param.rows, "&",

        //          "IDDelimited=", param.IDDelimited, "&",
        //          "ID=", param.ID, "&",
        //          "ApplicantName=", param.ApplicantName, "&",
        //          "CurrentStepDelimited=", param.CurrentStepDelimited, "&",
        //          "StatusDelimited=", param.StatusDelimited);

        //    var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetMRFExistingApplicantListOutput>(), URL);

        //    if (IsSuccess)
        //    {
        //        var jsonData = new
        //        {
        //            total = Result.Count > 0 ? Result.FirstOrDefault().NoOfPages : 0,
        //            param.pageNumber,
        //            sort = param.sidx,
        //            records = Result.Count > 0 ? Result.Last().TotalNoOfRecord : 0,
        //            rows = Result
        //        };
        //        return new JsonResult(jsonData);
        //    }
        //    else
        //    {
        //        return new BadRequestObjectResult(ErrorMessage);
        //    }
        //}

        public async Task<JsonResult> OnPostAsync()
        {
            var URL = string.Concat(_manpowerBaseURL,
                   _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("AddApplicant").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

            // Get Applicant Names by ApplicantIDs
            List<EMS.Recruitment.Transfer.Applicant.Form> applicants =
               await new SharedClasses.Common_Recruitment.Common_Applicant(_iconfiguration, _globalCurrentUser, _env)
               .GetApplicants(Form.Applicants.Where(x => x.ApplicantID != 0).Select(x => x.ApplicantID)
               .Distinct().ToList());

            //EMS.Workflow.Transfer.Workflow.Form Workflow =
            //        await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(Form.WorkflowID);
            
            Form.Applicants = Form.Applicants
                       .GroupJoin(applicants,
                       x => new { x.ApplicantID },
                       y => new { ApplicantID = y.ID },
                       (x, y) => new { form = x, applicants = y })
                       .SelectMany(x => x.applicants.DefaultIfEmpty(),
                       (x, y) => new { form = x, applicants = y })
                       .Select(x => new MRFApplicantDetails
                       {
                           ApplicantID = x.form.form.ApplicantID,
                           FirstName = x.applicants == null ? "" : x.applicants.PersonalInformation.FirstName,
                           MiddleName = x.applicants == null ? "" : x.applicants.PersonalInformation.MiddleName,
                           LastName = x.applicants == null ? "" : x.applicants.PersonalInformation.LastName,
                           Suffix = x.applicants == null ? "" : x.applicants.PersonalInformation.Suffix,
                           //CurrentStepCode = Workflow.WorkflowStepList.First().StepCode,
                           //CurrentStepDescription = Workflow.WorkflowStepList.First().StepDescription,
                           //CurrentStepApproverRoleIDs = 
                           //string.Join(",", Workflow.WorkflowStepList.First().WorkflowStepApproverList.Select(x => x.RoleID).ToList()),
                           //ResultType = Workflow.WorkflowStepList.First().ResultType

                       }).ToList();


            var (MRFApplicantInfo, IsSuccess, Message) = await SharedUtilities.PostFromAPI(new List<ApplicantIDsAndForHiring>(), Form, URL);

            if (IsSuccess)
            {
                await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env).UpdateMRFTransactionID(new UpdateMRFTransactionIDForm
                {
                    MRFTransactionID = Form.MRFTransactionID,
                    ApplicantIDs = Form.Applicants.Select(x => x.ApplicantID).ToList()
                });

                Workflow.WorkflowCode = "RECRUITMENT";
                Workflow.CurrentStepCode = "0-NEW";
                Workflow.DateCompleted = DateTime.Now.ToString("MM/dd/yyyy");
                Workflow.DateScheduled = DateTime.Now.ToString("MM/dd/yyyy");
                Workflow.Remarks = "NEW";
                Workflow.Result = "YES";
                Workflow.BatchUpdateApplicantIDs = MRFApplicantInfo.Select(x => x.ApplicantID).ToList();
                Workflow.BatchUpdateRecordIDs = MRFApplicantInfo.Select(x => Convert.ToInt32(x.MRFApplicantID)).ToList();

                var (CurrentWorkflowStep, IsWorkflowSuccess, WorkflowMsg) = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).AddTransaction(Workflow);

                if (IsWorkflowSuccess)
                {
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
                                });
                    }
                }
            }

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = MessageUtilities.PRE_SCSSMSG_REC_SAVE;

            return new JsonResult(_resultView);
        }
    }
}