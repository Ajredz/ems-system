using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Utilities.API;

namespace EMS.FrontEnd.Pages.ManPower.Dashboard.User
{
    public class UserModel : SharedClasses.Utilities
    {

        public UserModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
            
        }

        public async Task<JsonResult> OnGetMRFDashboardByAge()
        {

            var URL = string.Concat(_manpowerBaseURL,
                           _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Dashboard").GetSection("List").Value, "?",
                            "UserID=", _globalCurrentUser.UserID, "&", 
                            "isAdmin", false);

            EMS.Manpower.Transfer.Dashboard.MRFDashboardList result = new EMS.Manpower.Transfer.Dashboard.MRFDashboardList();

            var (APIResult, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(result, URL);

            if (IsSuccess)
            {
                if (APIResult != null)
                {
                    _resultView.Result = APIResult;
                    _resultView.IsSuccess = true;
                }
            }
            else
            {
                _resultView.IsSuccess = false;
                _resultView.Result = ErrorMessage;
            }

            return new JsonResult(_resultView);
        }
    }
}