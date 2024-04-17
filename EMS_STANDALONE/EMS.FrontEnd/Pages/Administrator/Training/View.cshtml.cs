using EMS.FrontEnd.SharedClasses.Common_Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.FrontEnd.Pages.Administrator.Training
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.Training.TrainingTempateInput Training { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }
        public virtual async Task OnGetAsync(int ID)
        {
            ViewData["Function"] = "View";

            if (_systemURL != null)
            {
                ViewData["HasDelete"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/TRAINING/DELETE")).Count() > 0 ? "true" : "false";
                ViewData["HasEdit"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/TRAINING/EDIT")).Count() > 0 ? "true" : "false";
            }

            Training = await new Common_Training(_iconfiguration, _globalCurrentUser, _env).GetByID(ID);
        }
    }
}
