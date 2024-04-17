using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Utilities.API;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;

namespace EMS.FrontEnd.Pages.Administrator.EmployeeCorporateEmail
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.Employee.Form Employee { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync(int ID)
        {

            if (_systemURL != null)
            {
                ViewData["HasEditFeatureHomeBranch"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/EMPLOYEECORPORATEEMAIL/EDITHOMEBRANCH")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeatureOfficeMobile"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/EMPLOYEECORPORATEEMAIL/EDITOFFICEMOBILE")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeatureCorporateEmail"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/EMPLOYEECORPORATEEMAIL/EDITCORPORATEEMAIL")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeatureOfficeLandline"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/EMPLOYEECORPORATEEMAIL/EDITOFFICELANDLINE")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeatureDisplayDirectory"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/EMPLOYEECORPORATEEMAIL/EDITDISPLAYDIRECTORY")).Count() > 0 ? "true" : "false";
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
            ViewData["FullName"] = (Employee.PersonalInformation.LastName + ", " + Employee.PersonalInformation.FirstName + " " + Employee.PersonalInformation.MiddleName + " " + Employee.PersonalInformation.Suffix);
        }

        public async Task<JsonResult> OnPostAsync()
        {
            var oldValue = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployee(Employee.ID);

            var URL = string.Concat(_plantillaBaseURL,
                               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("Edit").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            oldValue.HomeBranchID = Employee.HomeBranchID;
            oldValue.CorporateEmail = (string.IsNullOrEmpty(Employee.CorporateEmail) ? oldValue.CorporateEmail : Employee.CorporateEmail);
            oldValue.OfficeMobile = (string.IsNullOrEmpty(Employee.OfficeMobile) ? oldValue.OfficeMobile : Employee.OfficeMobile);
            oldValue.OfficeLandline = (string.IsNullOrEmpty(Employee.OfficeLandline) ? oldValue.OfficeLandline : Employee.OfficeLandline);
            oldValue.IsDisplayDirectory = Employee.IsDisplayDirectory;

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(oldValue, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}