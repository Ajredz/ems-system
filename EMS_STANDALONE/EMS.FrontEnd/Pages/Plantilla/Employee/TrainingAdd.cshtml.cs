using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.IPM.Data.DataDuplication.EmployeeMovement;
using EMS.Workflow.Transfer.Training;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.FrontEnd.Pages.Plantilla.Employee
{
    public class TrainingAddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.Training.EmployeeTrainingForm EmployeeTraining { get; set; }

        public TrainingAddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }
        public virtual async Task OnGetAsync(int EmployeeID)
        {
            ViewData["Function"] = "Add";
            ViewData["HasAdd"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/TRAININGADD")).Count() > 0 ? "true" : "false";

            var Status = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(7)).WorkflowStepList;

            EmployeeTraining = new EMS.Workflow.Transfer.Training.EmployeeTrainingForm
            {
                EmployeeID = EmployeeID,
                Status = Status.Where(x => x.Order.Equals(1)).Select(y => y.StepCode).FirstOrDefault(),
                StatusDescription = Status.Where(x => x.Order.Equals(1)).Select(y => y.StepDescription).FirstOrDefault()
            };

            var Type = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                .GetReferenceValueByRefCode("TRAINING_TYPE");
            ViewData["TypeSelectList"] =
            Type.Select(x => new SelectListItem
            {
                Value = x.Value,
                Text = x.Description
            }).ToList();
        }

        public async Task<JsonResult> OnPostAsync()
        {
            var (IsSuccess, Message) = await new Common_Training(_iconfiguration, _globalCurrentUser, _env)
                .AddEmployeeTraining(EmployeeTraining);

            _resultView.Result = Message;
            _resultView.IsSuccess = IsSuccess;

            if (IsSuccess)
            {
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = "ADD",
                        TableName = "EmployeeTraining",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat("Training Added to ",EmployeeTraining.EmployeeID),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }
            return new JsonResult(_resultView);
        }
    }
}
