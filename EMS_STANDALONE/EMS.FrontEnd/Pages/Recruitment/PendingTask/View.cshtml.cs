using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.FrontEnd.Pages.Recruitment.PendingTask
{
    public class ViewModel : SharedClasses.Utilities
    {

        [BindProperty]
        public EMS.Recruitment.Transfer.RecruiterTask.Form PendingTask { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/RECRUITMENT/PENDINGTASK/EDIT")).Count() > 0 ? "true" : "false";
            }

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
    }
}