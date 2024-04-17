using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Workflow.Transfer.Accountability;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EMS.FrontEnd.Pages.LogActivity.Accountability
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.Accountability.AccountabilityForm Accountability { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync(int ID)
        {

            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/ACCOUNTABILITY/DELETE")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/LOGACTIVITY/ACCOUNTABILITY/EDIT")).Count() > 0 ? "true" : "false";
            }

            Accountability = await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env).GetByID(ID);

        }
    }
}