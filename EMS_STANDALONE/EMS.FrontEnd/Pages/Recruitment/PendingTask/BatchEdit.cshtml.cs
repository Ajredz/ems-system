using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Recruitment.PendingTask
{
    public class BatchEditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Recruitment.Transfer.RecruiterTask.BatchForm BatchTask { get; set; }

        public BatchEditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync()
        {

            var result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                                       .GetReferenceValueByRefCode(ReferenceCodes_Recruitment.TASK_STATUS.ToString());

            ViewData["StatusSelectList"] = result.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description
                }).ToList();

        }

        public async Task<JsonResult> OnPostAsync()
        {

            var URL = string.Concat(_recruitmentBaseURL,
                               _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("PendingTask").GetSection("BatchUpdate").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(BatchTask, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}