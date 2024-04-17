using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.IPM.Transfer.DataDuplication.Employee;
using Microsoft.AspNetCore.Hosting;

namespace EMS.FrontEnd.SharedClasses.Common_IPM
{
    public class Common_Employee : Utilities
    {
        public Common_Employee(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetEmployeeFilteredAutoComplete(string Term, int TopResults, string Filter, string FilterID)
        {
            var URL = string.Concat(_ipmBaseURL,
                     _iconfiguration.GetSection("IPMService_API_URL").GetSection("Employee").GetSection("GetFilteredIDByAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "term=", Term, "&",
                     "topresults=", TopResults, "&",
                     "filter=", Filter, "&",
                     "filterID=", FilterID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetIDByAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetEmployeeAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_ipmBaseURL,
                     _iconfiguration.GetSection("IPMService_API_URL").GetSection("Employee").GetSection("GetIDByAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "term=", Term, "&",
                     "topresults=", TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetIDByAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
    }
}
