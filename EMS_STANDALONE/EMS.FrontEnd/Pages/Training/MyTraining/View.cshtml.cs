using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.FrontEnd.Pages.Training.MyTraining
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.Training.EmployeeTrainingForm EmployeeTraining { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }
        public virtual async Task OnGetAsync(int ID)
        {
            ViewData["Function"] = "View";

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
    }
}
