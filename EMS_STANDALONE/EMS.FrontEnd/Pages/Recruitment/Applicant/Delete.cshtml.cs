using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Recruitment.Applicant
{
    public class DeleteModel : SharedClasses.Utilities
    {
        public DeleteModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task<JsonResult> OnPostAsync(int ID)
        {
            bool isUsed = (await new SharedClasses.Common_Manpower.Common_MRF(_iconfiguration, _globalCurrentUser, _env)
                .ValidateApplicantIsTagged(ID));

            var URL = string.Concat(_recruitmentBaseURL,
                   _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("Delete").Value, "?",
                   "userid=", _globalCurrentUser.UserID, "&",
                   "id=", ID);

            if (isUsed)
            {
                _resultView.IsSuccess = false;
                _resultView.Result = MessageUtilities.ERRMSG_APPLICANT_IS_USED;
            }
            else
            {
                var (IsSuccess, Message) = await SharedUtilities.DeleteFromAPI(URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;
            }

            return new JsonResult(_resultView);
        }
    }
}