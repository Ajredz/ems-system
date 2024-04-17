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

namespace EMS.FrontEnd.Pages.Recruitment.PendingTask
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Recruitment.Transfer.RecruiterTask.Form PendingTask { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID)
        {

            PendingTask = await new Common_RecruiterTask(_iconfiguration, _globalCurrentUser, _env).GetRecruiterTask(ID);

            var result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                                       .GetReferenceValueByRefCode(ReferenceCodes_Recruitment.TASK_STATUS.ToString());

            ViewData["StatusSelectList"] = result.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description,
                    Selected = PendingTask.Status == x.Value
                }).ToList();

        }

        public async Task<JsonResult> OnPostAsync()
        {
            PendingTask.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_recruitmentBaseURL,
                               _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("PendingTask").GetSection("Edit").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(PendingTask, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}