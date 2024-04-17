using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Security.Transfer.SystemRole;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;

namespace EMS.FrontEnd.SharedClasses.Common_Manpower
{
    public class Common_Synced_SystemRole : Utilities
    {
        public Common_Synced_SystemRole(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetSystemRoleAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_manpowerBaseURL,
                     _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("SystemRole").GetSection("GetIDByAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "term=", Term, "&",
                     "topresults=", TopResults, "&",
                     "companyid=", _globalCurrentUser.CompanyID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetIDByAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetSystemRoleDropDown()
        {
            var URL = string.Concat(_manpowerBaseURL,
               _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("SystemRole").GetSection("GetDropDown").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

    }
}
