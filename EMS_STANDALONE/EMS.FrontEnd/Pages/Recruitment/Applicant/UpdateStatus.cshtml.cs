using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Recruitment.Applicant
{
    public class UpdateStatusModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Recruitment.Transfer.Applicant.Form Applicant { get; set; }

        public UpdateStatusModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID)
        {
            if (_globalCurrentUser != null)
            {
                if (_systemURL != null)
                {
                    //ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/MANPOWER/APPLICANT/DELETE")).Count() > 0 ? "true" : "false";
                }

                Applicant = await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env).GetApplicant(ID);

            }
        }

        public async Task<JsonResult> OnPostAsync(EMS.Recruitment.Transfer.Applicant.ApproverResponse param)
        {
            var URL = string.Concat(_manpowerBaseURL,
                   _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("UpdateStatus").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(param, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}