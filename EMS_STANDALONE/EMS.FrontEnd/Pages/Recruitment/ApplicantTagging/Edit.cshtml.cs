using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Recruitment.ApplicantTagging
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Recruitment.Transfer.ApplicantTagging.Form ApplicantTagging { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID)
        {

            //ApplicantTagging = await new Common_Applicant(_iconfiguration, _globalCurrentUser).GetApplicant(ID);

            var result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                                       .GetReferenceValueByRefCode(ReferenceCodes_Recruitment.TASK_STATUS.ToString());

        }

        public async Task<JsonResult> OnPostAsync()
        {
            var URL = string.Concat(_recruitmentBaseURL,
                               _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("ApplicantTagging").GetSection("Edit").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(ApplicantTagging, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}