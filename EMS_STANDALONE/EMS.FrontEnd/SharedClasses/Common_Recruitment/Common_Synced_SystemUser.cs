using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.Recruitment.Transfer.DataDuplication.SystemUser;
using Microsoft.AspNetCore.Hosting;

namespace EMS.FrontEnd.SharedClasses.Common_Recruitment
{
    public class Common_Synced_SystemUser : Utilities
    {

        public Common_Synced_SystemUser(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetSystemUserAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_recruitmentBaseURL,
                     _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("SystemUser").GetSection("GetSystemUserIDByAutoComplete").Value, "?",
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

        public async Task<GetIDByAutoCompleteOutput> GetSystemUserBySyncID(int ID)
        {

            var URL = string.Concat(_recruitmentBaseURL,
                  _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("SystemUser").GetSection("GetSystemUserBySyncID").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "id=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetIDByAutoCompleteOutput(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
    }
}