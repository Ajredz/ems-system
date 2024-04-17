using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_IPM;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.IPM.Transfer.EmployeeScore;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.FrontEnd.Pages.IPM.EmployeeScoreApproval
{
    public class ViewModel : IPM.EmployeeScoreKeyIn.ViewModel
    {
        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool isApproval = true) : base(iconfiguration, env, isApproval)
        { }
    }
}