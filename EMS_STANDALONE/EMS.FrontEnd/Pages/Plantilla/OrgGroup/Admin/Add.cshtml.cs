using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using EMS.FrontEnd.SharedClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;

namespace EMS.FrontEnd.Pages.Plantilla.OrgGroup.Admin
{
    public class AddModel : OrgGroup.AddModel
    {
        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = true) : base(iconfiguration, env, IsAdminAccess)
        { }

    }
}