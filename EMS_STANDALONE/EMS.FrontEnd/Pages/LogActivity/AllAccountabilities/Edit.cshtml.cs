using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using EMS.Workflow.Transfer;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.Workflow.Transfer.Accountability;

namespace EMS.FrontEnd.Pages.LogActivity.AllAccountabilities
{
    public class EditModel : LogActivity.MyAccountabilities.EditModel
    {
       
        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = true) 
            : base(iconfiguration, env, IsAdminAccess)
        { }


    }
}