using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilities.API;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using System.Text;
using NPOI.HSSF.UserModel;
using EMS.Plantilla.Transfer.OrgGroup;
using Microsoft.AspNetCore.Hosting;


namespace EMS.FrontEnd.Pages.Plantilla.OrgGroup.Admin
{
    public class UploadEditModel : OrgGroup.UploadEditModel
    {

        public UploadEditModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = true) : base(iconfiguration, env, IsAdminAccess)
        {
            _env = env;
        }

    }
}