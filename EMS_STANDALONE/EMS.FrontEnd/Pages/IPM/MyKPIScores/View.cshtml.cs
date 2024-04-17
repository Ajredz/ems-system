using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_IPM;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.IPM.Transfer.MyKPIScores;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.FrontEnd.Pages.IPM.MyKPIScores
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.IPM.Transfer.MyKPIScores.Form MyKPIScore { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(GetByIDInput param)
        {
            if (_globalCurrentUser != null)
            {
                var roles = (await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                    .GetSystemUserRoleDropDownByUserID(_globalCurrentUser.UserID)).ToList();
                param.RoleIDs = string.Join(",", roles.Select(x => x.Value).ToArray());

                MyKPIScore = await new Common_MyKPIScores(_iconfiguration, _globalCurrentUser, _env).GetMyKPIScore(param);
            }

            var description = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetWorkflowStepByWorkflowIDAndCode(new GetWorkflowStepByWorkflowIDAndCodeInput
                    {
                        WorkflowCode = "IPM",
                        Code = MyKPIScore.Status
                    });

            ViewData["CurrentStatusDescription"] = description.Description;
        }
    }
}