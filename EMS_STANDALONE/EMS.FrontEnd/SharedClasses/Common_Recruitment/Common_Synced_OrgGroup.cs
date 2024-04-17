using EMS.Recruitment.Transfer.DataDuplication.OrgGroup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.SharedClasses.Common_Recruitment
{
    public class Common_Synced_OrgGroup : Utilities
    {
        public Common_Synced_OrgGroup(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<List<SelectListItem>> GetOrgGroupDropDown(int id = 0)
        {
            var URL = string.Concat(_recruitmentBaseURL,
               _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("OrgGroup").GetSection("GetDropDown").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&id=", id);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetOrgGroupAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_recruitmentBaseURL,
                     _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("OrgGroup").GetSection("GetIDByAutoComplete").Value, "?",
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

        public async Task<List<GetIDByOrgTypeAutoCompleteOutput>> GetOrgGroupByOrgTypeAutoComplete(GetByOrgTypeAutoCompleteInput param)
        {
            var URL = string.Concat(_recruitmentBaseURL,
                     _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("OrgGroup").GetSection("GetIDByOrgTypeAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "Term=", param.Term, "&",
                     "OrgType=", param.OrgType, "&",
                     "TopResults=", param.TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetIDByOrgTypeAutoCompleteOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
    }
}