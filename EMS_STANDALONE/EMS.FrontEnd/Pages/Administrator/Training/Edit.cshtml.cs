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
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.Training.TrainingTempateInput Training { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }
        public virtual async Task OnGetAsync(int ID)
        {
            ViewData["Function"] = "Edit";


            Training = await new Common_Training(_iconfiguration, _globalCurrentUser, _env).GetByID(ID);
        }
        public async Task<JsonResult> OnPostAsync()
        {
            var (IsSuccess, ErrorMessage) = await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                .Edit(Training);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = ErrorMessage;

            if (IsSuccess)
            {
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EDIT.ToString(),
                        TableName = "training_template",
                        TableID = 0,
                        Remarks = string.Concat(Training.TemplateName, " updated"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                if (Training.TrainingTempateDetailsInputList != null && Training.TrainingTempateDetailsInputList.Count() > 0)
                {
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "training_template_details",
                            TableID = 0,
                            Remarks = string.Concat(Training.TemplateName, " Details updated"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }
            }

            return new JsonResult(_resultView);
        }
    }
}
