using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.LogActivity.AllTasks
{
    public class BatchUpdateModel : LogActivity.AssignedActivities.BatchUpdateModel
    {
        public BatchUpdateModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }
    }
}