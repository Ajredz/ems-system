using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Administrator.SystemErrorLog
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Security.Transfer.SystemErrorLog.Form ErrorLog { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync(int ID, string ReportType)
        {
            var Database =
                (ReportType ?? "").Equals("FRONT_END") ?
                    string.Concat(_securityBaseURL,
                  _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemErrorLog").GetSection("GetByID").Value, "?") :
                (ReportType ?? "").Equals("PLANTILLA") ?
                    string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("SystemErrorLog").GetSection("GetByID").Value, "?") :
                (ReportType ?? "").Equals("RECRUITMENT") ?
                    string.Concat(_recruitmentBaseURL,
                  _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("SystemErrorLog").GetSection("GetByID").Value, "?") :
                (ReportType ?? "").Equals("MANPOWER") ?
                    string.Concat(_manpowerBaseURL,
                  _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("SystemErrorLog").GetSection("GetByID").Value, "?") :
                (ReportType ?? "").Equals("IPM") ?
                    string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("SystemErrorLog").GetSection("GetByID").Value, "?") :
                  string.Concat(_securityBaseURL,
                  _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemErrorLog").GetSection("GetByID").Value, "?");


            var URL = string.Concat(Database,
                    "userid=", _globalCurrentUser.UserID, "&",
                    "ID=", ID);

            var (Result, IsSuccess, _) = await SharedUtilities
                .GetFromAPI(new EMS.Security.Transfer.SystemErrorLog.Form(), URL);
            _resultView.IsSuccess = IsSuccess;

            // Get Employee description by System User IDs
            List<EMS.Plantilla.Transfer.Employee.Form> systemUsers =
                (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeByUserIDs(new List<int> { Result.UserID })).Item1;

            if (systemUsers.Count > 0)
            {
                Result.User = systemUsers.Count == 0 ? "" : systemUsers.First().PersonalInformation == null ? "" :
                string.Concat((systemUsers.First().PersonalInformation.LastName ?? "").Trim(),
                    string.IsNullOrEmpty((systemUsers.First().PersonalInformation.FirstName ?? "").Trim()) ? ""
                        : string.Concat(", ", systemUsers.First().PersonalInformation.FirstName),
                    string.IsNullOrEmpty((systemUsers.First().PersonalInformation.MiddleName ?? "").Trim()) ? ""
                        : string.Concat(" ", systemUsers.First().PersonalInformation.MiddleName), " (", systemUsers.First().Code, ") ");
  
            }
            ErrorLog = Result;
        }

        
    }
}