using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EMS.FrontEnd.Pages.LogActivity.Preloaded
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.LogActivityPreloadedForm LogActivityPreloaded { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync(int ID)
        {

            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/PRELOADED/DELETE")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/PRELOADED/EDIT")).Count() > 0 ? "true" : "false";
            }

            LogActivityPreloaded = await new Common_LogActivity(_iconfiguration, _globalCurrentUser, _env).GetLogActivityPreloadedByID(ID);
        }
    }
}