using EMS.FrontEnd.SharedClasses;
using EMS.Plantilla.Transfer.OrgGroup;
using EMS.Plantilla.Transfer.Position;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Plantilla.Count.Admin
{
    public class IndexModel : Count.IndexModel
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = true) : base(iconfiguration, env, IsAdminAccess)
        {
        }

        
    }
}