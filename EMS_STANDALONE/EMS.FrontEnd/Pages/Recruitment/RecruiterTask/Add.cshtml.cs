using System.Threading.Tasks;
using Utilities.API;
using EMS.FrontEnd.SharedClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using EMS.FrontEnd.SharedClasses.Common_Security;

namespace EMS.FrontEnd.Pages.Recruitment.RecruiterTask
{
    public class AddModel : SharedClasses.Utilities
    {

        [BindProperty]
        public EMS.Recruitment.Transfer.RecruiterTask.Form RecruiterTask { get; set; }

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
            if (_globalCurrentUser != null)
            {
                RecruiterTask = new EMS.Recruitment.Transfer.RecruiterTask.Form();
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            RecruiterTask.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_recruitmentBaseURL,
                               _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("RecruiterTask").GetSection("Add").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(RecruiterTask, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "RecruiterTask",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(RecruiterTask.Description, " added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }
    }
}