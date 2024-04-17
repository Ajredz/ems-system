using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.FrontEnd.Pages.Plantilla.Position
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.Position.Form Position { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/POSITION/DELETE")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/POSITION/EDIT")).Count() > 0 ? "true" : "false";
            }

            if (_globalCurrentUser != null)
            {
                Position = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPosition(ID);

               
                ViewData["PositionLevelSelectList"] = 
                    await new Common_PositionLevel(_iconfiguration, _globalCurrentUser, _env).GetPositionLevelDropdown(Position.PositionLevelID);

                //ViewData["ParentPositionSelectList"] = 
                //    await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionDropdown(Position.ParentPositionID ?? 0);

                var jobClass = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.JOB_CLASS.ToString());
                ViewData["JobClassSelectList"] =
                    jobClass.Select(x => new SelectListItem 
                    { Value = x.Value, Text = x.Description, Selected = x.Value == Position.JobClassCode }
                    ).ToList();
            }
        }
    }
}