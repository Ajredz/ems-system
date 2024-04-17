using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.Manpower.Transfer.DataDuplication.PositionLevel;
using Microsoft.AspNetCore.Hosting;

namespace EMS.FrontEnd.SharedClasses.Common_Manpower
{
    public class Common_Synced_PositionLevel : Utilities
    {
        public Common_Synced_PositionLevel(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<List<SelectListItem>> GetPositionLevelDropdown(int id = 0)
        {
            var URL = string.Concat(_manpowerBaseURL,
               _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("PositionLevel").GetSection("GetPositionLevelDropdown").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&id=", id);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetPositionLevelAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_manpowerBaseURL,
                     _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("PositionLevel").GetSection("GetIDByAutoComplete").Value, "?",
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

        public async Task<List<SelectListItem>> GetPositionLevelDropdownByOrgGroupID(GetByOrgGroupIDInput param)
        {
            var URL = string.Concat(_manpowerBaseURL,
               _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("PositionLevel").GetSection("GetDropdownByOrgGroupID").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&OrgGroupID=", param.OrgGroupID,
                "&SelectedValue=", param.SelectedValue);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<Form> GetPositionLevelByID(int ID)
        {
            var URL = string.Concat(_manpowerBaseURL,
               _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("PositionLevel").GetSection("GetByID").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new Form(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
    }
}
