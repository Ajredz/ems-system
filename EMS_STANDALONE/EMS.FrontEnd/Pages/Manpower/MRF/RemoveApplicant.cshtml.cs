using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.Recruitment.Transfer.Applicant;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Manpower.MRF
{
    public class RemoveApplicantModel : SharedClasses.Utilities
    {
        public RemoveApplicantModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
        }

        public async Task<JsonResult> OnPostAsync(EMS.Manpower.Transfer.MRF.RemoveApplicantInput param)
        {
            var URL = string.Concat(_manpowerBaseURL,
                   _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("RemoveApplicant").Value, "?",
                   "userid=", _globalCurrentUser.UserID, "&",
                   "MRFID=", param.MRFID, "&",
                   "MRFApplicantID=", param.MRFApplicantID);

            var (IsSuccess, Message) = await SharedUtilities.DeleteFromAPI(URL);
            if (IsSuccess)
            {
                await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env).UpdateMRFTransactionID(new UpdateMRFTransactionIDForm
                {
                    MRFTransactionID = null,
                    ApplicantIDs = new List<int> { param.MRFApplicantID }
                });

                await new SharedClasses.Common_Recruitment.Common_Applicant(_iconfiguration, _globalCurrentUser, _env)
                        .UpdateCurrentWorkflowStep(new EMS.Recruitment.Transfer.Applicant.UpdateCurrentWorkflowStepInput
                        {
                            ApplicantID = param.MRFApplicantID,
                            CurrentStepCode = "",
                            CurrentStepDescription = "",
                            CurrentStepApproverRoleIDs = "",
                            WorkflowStatus = "",
                            DateScheduled = "",
                            DateCompleted = "",
                            ApproverRemarks = "",
                        });
            }


            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}