using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Plantilla.Transfer.PositionLevel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;

namespace EMS.FrontEnd.SharedClasses.Common_Recruitment
{
    public class Common_Synced_Position : Utilities
    {
        public Common_Synced_Position(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<List<SelectListItem>> GetPositionDropdown(int ID = 0)
        {
            var URL = string.Concat(_recruitmentBaseURL,
               _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Position").GetSection("GetDropdown").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&id=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetPositionAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_recruitmentBaseURL,
                     _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Position").GetSection("GetIDByAutoComplete").Value, "?",
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

        public async Task<List<SelectListItem>> GetDropdownByPositionLevel(int PositionLevelID, int id = 0)
        {
            var URL = string.Concat(_recruitmentBaseURL,
               _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Position").GetSection("GetDropdownByPositionLevel").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&id=", id,
                "&positionlevelid=", PositionLevelID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
    }
}
