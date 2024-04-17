using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Recruitment.RecruiterTask
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Recruitment.Transfer.RecruiterTask.Form RecruiterTask { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/RECRUITMENT/RECRUITERTASK/DELETE")).Count() > 0 ? "true" : "false";
            }

            RecruiterTask = await new Common_RecruiterTask(_iconfiguration, _globalCurrentUser, _env).GetRecruiterTask(ID);

        }

        public async Task<JsonResult> OnPostAsync()
        {
            RecruiterTask.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_recruitmentBaseURL,
                               _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("RecruiterTask").GetSection("Edit").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var oldValue = await new Common_RecruiterTask(_iconfiguration, _globalCurrentUser, _env).GetRecruiterTask(RecruiterTask.ID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(RecruiterTask, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                StringBuilder Remarks = new StringBuilder();

                if (RecruiterTask.ApplicantID != oldValue.ApplicantID)
                {
                    Remarks.Append(string.Concat("ApplicantID changed from ", oldValue.ApplicantID, " to ", RecruiterTask.ApplicantID, ". "));
                }

                if (RecruiterTask.Description != oldValue.Description)
                {
                    Remarks.Append(string.Concat("Description changed from ", oldValue.Description, " to ", RecruiterTask.Description, ". "));
                }

                if (RecruiterTask.Status != oldValue.Status)
                {
                    Remarks.Append(string.Concat("Status changed from ", oldValue.Status, " to ", RecruiterTask.Status, ". "));
                }

                if (RecruiterTask.Remarks != oldValue.Remarks)
                {
                    Remarks.Append(string.Concat("Remarks changed from ", oldValue.Remarks, " to ", RecruiterTask.Remarks, ". "));
                }

                if (Remarks.Length > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "RecruiterTask",
                            TableID = RecruiterTask.ID,
                            Remarks = Remarks.ToString(),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        }); ;
                }
            }

            return new JsonResult(_resultView);
        }
    }
}