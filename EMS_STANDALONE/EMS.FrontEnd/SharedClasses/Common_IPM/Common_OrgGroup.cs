using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.IPM.Transfer.DataDuplication.OrgGroup;
using Microsoft.AspNetCore.Hosting;

namespace EMS.FrontEnd.SharedClasses.Common_IPM
{
    public class Common_OrgGroup : Utilities
    {
        public Common_OrgGroup(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }


        public async Task<Form> GetOrgGroup(int ID)
        {

            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("OrgGroup").GetSection("getbyid").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "id=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new Form(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);

        }

        public async Task<List<SelectListItem>> GetOrgGroupDropDown(int id = 0)
        {
            var URL = string.Concat(_ipmBaseURL,
               _iconfiguration.GetSection("IPMService_API_URL").GetSection("OrgGroup").GetSection("GetDropDown").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&id=", id);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetOrgGroupCodeDropDown(int id = 0)
        {
            var URL = string.Concat(_ipmBaseURL,
               _iconfiguration.GetSection("IPMService_API_URL").GetSection("OrgGroup").GetSection("GetCodeDropDown").Value, "?",
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
            var URL = string.Concat(_ipmBaseURL,
                     _iconfiguration.GetSection("IPMService_API_URL").GetSection("OrgGroup").GetSection("GetIDByAutoComplete").Value, "?",
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

        public async Task<List<GetIDByAutoCompleteOutput>> GetOrgGroupFilteredAutoComplete(string Term, int TopResults, string Filter)
        {
            var URL = string.Concat(_ipmBaseURL,
                     _iconfiguration.GetSection("IPMService_API_URL").GetSection("OrgGroup").GetSection("GetFilteredIDByAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "term=", Term, "&",
                     "topresults=", TopResults, "&",
                     "filter=", Filter);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetIDByAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<EMS.IPM.Transfer.Shared.GetAutoCompleteOutput>> GetOrgGroupRegionAutoComplete(EMS.IPM.Transfer.Shared.GetAutoCompleteInput param)
        {
            var URL = string.Concat(_ipmBaseURL,
                     _iconfiguration.GetSection("IPMService_API_URL").GetSection("OrgGroup").GetSection("GetRegionAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "Term=", param.Term, "&",
                     "TopResults=", param.TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<EMS.IPM.Transfer.Shared.GetAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        
        public async Task<List<EMS.IPM.Transfer.Shared.GetAutoCompleteOutput>> GetOrgGroupBranchAutoComplete(EMS.IPM.Transfer.Shared.GetAutoCompleteInput param)
        {
            var URL = string.Concat(_ipmBaseURL,
                     _iconfiguration.GetSection("IPMService_API_URL").GetSection("OrgGroup").GetSection("GetBranchAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "Term=", param.Term, "&",
                     "TopResults=", param.TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<EMS.IPM.Transfer.Shared.GetAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
    }
}
