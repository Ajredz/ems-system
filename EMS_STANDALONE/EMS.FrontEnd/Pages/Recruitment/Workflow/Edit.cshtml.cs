using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Recruitment.Workflow
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.Workflow.Form Workflow { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/RECRUITMENT/WORKFLOW/DELETE")).Count() > 0 ? "true" : "false";
            }

            Workflow = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(ID);

        }

        public async Task<JsonResult> OnPostAsync()
        {
            Workflow.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_recruitmentBaseURL,
                               _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Workflow").GetSection("Edit").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(Workflow, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetResultTypeDropDown()
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env).GetReferenceValueByRefCode(ReferenceCodes_Recruitment.RESULT_TYPE.ToString());
            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetApproverRoleDropDown()
        {
            _resultView.Result = await new Common_SystemRole(_iconfiguration, _globalCurrentUser, _env).GetSystemRoleDropDown();
            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }
    }
}