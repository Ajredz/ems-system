using EMS.Manpower.Transfer.DataDuplication.OrgGroup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.SharedClasses.Common_Manpower
{
    public class Common_Synced_OrgGroup : Utilities
    {
        public Common_Synced_OrgGroup(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetOrgGroupAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_manpowerBaseURL,
                     _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("OrgGroup").GetSection("GetIDByAutoComplete").Value, "?",
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

        public async Task<List<SelectListItem>> GetOrgGroupDropDown(GetDropDownInput param)
        {
            var URL = string.Concat(_manpowerBaseURL,
               _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("OrgGroup").GetSection("GetDropDown").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&id=", param.ID, "&",
                "AdminAccess.OrgGroupDescendantsDelimited=", param.AdminAccess.OrgGroupDescendantsDelimited, "&",
                "AdminAccess.IsAdminAccess=", param.AdminAccess.IsAdminAccess
                );


            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<Form> GetOrgGroupByID(int ID)
        {
            var URL = string.Concat(_manpowerBaseURL,
               _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("OrgGroup").GetSection("GetByID").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new Form(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetIDByOrgTypeAutoCompleteOutput>> GetOrgGroupByOrgTypeAutoComplete(GetByOrgTypeAutoCompleteInput param)
        {
            var URL = string.Concat(_manpowerBaseURL,
                     _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("OrgGroup").GetSection("GetIDByOrgTypeAutoComplete").Value, "?",
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

        public async Task<List<SelectListItem>> GetDropDownExcludeByOrgType(GetDropDownExcludeByOrgTypeInput param)
        {
            var URL = string.Concat(_manpowerBaseURL,
               _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("OrgGroup").GetSection("GetDropDownExcludeByOrgType").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&id=", param.ID, "&",
                "&OrgTypeDelimited=", param.OrgTypeDelimited, "&",
                "AdminAccess.OrgGroupDescendantsDelimited=", param.AdminAccess.OrgGroupDescendantsDelimited, "&",
                "AdminAccess.IsAdminAccess=", param.AdminAccess.IsAdminAccess
                );


            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
    }
}