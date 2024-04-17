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

namespace EMS.FrontEnd.Pages.IPM.EmployeeScore
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.IPM.Transfer.EmployeeScore.Form EmployeeScore { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(GetByIDInput param)
        {
            List<SelectListItem> roles = new List<SelectListItem>();

            if (_globalCurrentUser != null)
            {
                roles = (await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                    .GetSystemUserRoleDropDownByUserID(_globalCurrentUser.UserID)).ToList();
                param.RoleIDs = string.Join(",", roles.Select(x => x.Value).ToArray());

                EmployeeScore = await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeScore(param);
            }

            var status = (await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetWorkflowStepByRole(new GetWorkflowStepByRoleInput
                {
                    WorkflowCode = "IPM",
                    RoleIDDelimited = string.Join(",", roles.Select(x => x.Value).ToArray())
                })).ToList();

            ViewData["StatusSelectList"] =
            status.OrderByDescending(x => x.Order).Select(x => new SelectListItem
            {
                Value = x.Code,
                Text = x.Description,
                Selected = x.Code.Equals(EmployeeScore.Status)
            }).ToList();

            var description = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetWorkflowStepByWorkflowIDAndCode(new GetWorkflowStepByWorkflowIDAndCodeInput
                    {
                        WorkflowCode = "IPM",
                        Code = EmployeeScore.Status
                    });

            ViewData["CurrentStatusDescription"] = description.Description;

            if (_systemURL != null)
            {
                if (description.Description == "NEW")
                {
                    ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/EMPLOYEESCORE/DELETE")).Count() > 0 ? "true" : "false";
                }

                ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/EMPLOYEESCORE/EDIT")).Count() > 0 ? "true" : "false";
            }
        }
    }
}