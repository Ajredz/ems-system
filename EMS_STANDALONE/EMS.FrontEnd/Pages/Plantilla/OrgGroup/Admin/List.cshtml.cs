using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.Plantilla.Transfer.OrgGroup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
using Utilities.API.ReferenceMaintenance;

namespace EMS.FrontEnd.Pages.Plantilla.OrgGroup.Admin
{
    public class ListModel : OrgGroup.ListModel
    {
        public ListModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = true) : base(iconfiguration, env, IsAdminAccess)
        { }

        public override void OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/ADMIN/ADD")).Count() > 0 ? "true" : "false";
                ViewData["HasUploadInsertFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/ADMIN/UPLOADINSERT")).Count() > 0 ? "true" : "false";
                ViewData["HasUploadEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/ADMIN/UPLOADEDIT")).Count() > 0 ? "true" : "false";
            }
        }
    }
}