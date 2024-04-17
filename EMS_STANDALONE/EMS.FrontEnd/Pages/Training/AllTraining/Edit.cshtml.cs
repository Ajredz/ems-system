using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.FrontEnd.Pages.Training.AllTraining
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.Training.EmployeeTrainingForm EmployeeTraining { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }
        public virtual async Task OnGetAsync(int ID)
        {
            ViewData["Function"] = "Edit";
            ViewData["HasEdit"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/TRAINING/ALLTRAINING/EDIT")).Count() > 0 ? "true" : "false";

            EmployeeTraining = await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeTrainingByID(ID);

            var Status = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(7)).WorkflowStepList;
            EmployeeTraining.StatusDescription = Status.Where(x => x.StepCode.Equals(EmployeeTraining.Status)).Select(y => y.StepDescription).FirstOrDefault();

            var Type = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                .GetReferenceValueByRefCode("TRAINING_TYPE");
            ViewData["TypeSelectList"] =
                Type.Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description,
                    Selected = x.Value.Equals(EmployeeTraining.Type)
                }).ToList();

            ViewData["Title"] = string.Concat("ID# ", EmployeeTraining.ID.ToString().PadLeft(7, '0'), " | ", EmployeeTraining.Title, " | ", EmployeeTraining.StatusDescription);
        }

        public async Task<JsonResult> OnPostAsync()
        {
            var (IsSuccess, Message) = await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                .EditEmployeeTraining(EmployeeTraining);

            _resultView.Result = Message;
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
            {
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = "EDIT",
                        TableName = "EmployeeTraining",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat("ID: ", EmployeeTraining.ID, " Training Edit successful"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }
            return new JsonResult(_resultView);
        }
    }
}
