using EMS.Plantilla.Transfer.Position;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using Microsoft.AspNetCore.Hosting;
using EMS.Workflow.Transfer.LogActivity;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.Workflow.Transfer.Accountability;

namespace EMS.FrontEnd.Pages.LogActivity.Clearance
{
    public class IndexModel : LogActivity.MyAccountabilities.IndexModel 
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false, bool IsClearance = true) 
            : base(iconfiguration, env, IsAdminAccess, IsClearance)
        { }

    }
}