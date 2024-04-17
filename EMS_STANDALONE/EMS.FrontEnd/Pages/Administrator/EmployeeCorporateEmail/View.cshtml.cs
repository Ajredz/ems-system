using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;

namespace EMS.FrontEnd.Pages.Administrator.EmployeeCorporateEmail
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.Employee.Form Employee { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/EMPLOYEECORPORATEEMAIL/EDIT")).Count() > 0 ? "true" : "false";
            }

            Employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployee(ID);

            var OrgGroup = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(Employee.OrgGroupID);

            if (Employee.PositionID != 0)
            {
                var positionList = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionDropdown(0);
                var position = positionList.Where(x => x.Value.Equals(Employee.PositionID.ToString())).First();

                ViewData["Movement_Position"] = position.Text;
            }

            ViewData["OrgCode"] = OrgGroup.Code;
            ViewData["OrgDescription"] = OrgGroup.Description;
            ViewData["OrgParent"] = OrgGroup.ParentOrgID;
            ViewData["OrgGroup"] = OrgGroup.Code + " - " + OrgGroup.Description;
            ViewData["FullName"] = (Employee.PersonalInformation.LastName+", "+Employee.PersonalInformation.FirstName+" "+Employee.PersonalInformation.MiddleName+" "+Employee.PersonalInformation.Suffix);
        
        }

        
    }
}