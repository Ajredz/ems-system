using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;
using System;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.IPM.Transfer.EmployeeScore;
using EMS.FrontEnd.SharedClasses.Common_IPM;
using EMS.FrontEnd.SharedClasses;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMS.FrontEnd.Pages.IPM.EmployeeScoreApproval
{
    public class IndexModel : IPM.EmployeeScoreKeyIn.IndexModel
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool isApproval = true) : base(iconfiguration, env, isApproval)
        { }
    }
}