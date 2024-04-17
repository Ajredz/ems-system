using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.FrontEnd.Pages.Recruitment.RecruiterTask
{
    public class ViewModel : SharedClasses.Utilities
    {

        [BindProperty]
        public EMS.Recruitment.Transfer.RecruiterTask.Form RecruiterTask { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/RECRUITMENT/RECRUITERTASK/DELETE")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/RECRUITMENT/RECRUITERTASK/EDIT")).Count() > 0 ? "true" : "false";
            }

            RecruiterTask = await new Common_RecruiterTask(_iconfiguration, _globalCurrentUser, _env).GetRecruiterTask(ID);
        }

        //public async Task<JsonResult> OnGetRecruiterByID(int ID)
        //{
        //    _resultView.Result = await new Common_Synced_SystemUser(_iconfiguration, _globalCurrentUser).GetSystemUserBySyncID(ID);
        //    _resultView.IsSuccess = true;
        //    return new JsonResult(_resultView);
        //}

        //public async Task<JsonResult> OnGetApplicantByID(int ID)
        //{
        //    _resultView.Result = await new Common_Applicant(_iconfiguration, _globalCurrentUser).GetApplicantNameByID(ID);
        //    _resultView.IsSuccess = true;
        //    return new JsonResult(_resultView);
        //}
    }
}