using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.FrontEnd.Pages.Administrator.Training
{
    public class AddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.Training.TrainingTempateInput Training { get; set; }

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }
        public virtual async Task OnGetAsync()
        {
            ViewData["Function"] = "Add";
        }
        public async Task<JsonResult> OnPostAsync()
        {
            var (IsSuccess, ErrorMessage) = await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                .Add(Training);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = ErrorMessage;

            if (IsSuccess)
            {
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "training_template",
                        TableID = 0,
                        Remarks = string.Concat(Training.TemplateName, " added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                if (Training.TrainingTempateDetailsInputList != null && Training.TrainingTempateDetailsInputList.Count() > 0)
                {
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "training_template_details",
                            TableID = 0,
                            Remarks = string.Concat(Training.TemplateName, " Details added"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }
            }

            return new JsonResult(_resultView);
        }
    }
}
