using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.Manpower.Transfer.DataDuplication.Position;
using Microsoft.AspNetCore.Hosting;

namespace EMS.FrontEnd.SharedClasses.Common_Manpower
{
    public class Common_Synced_Position : Utilities
    {
        public Common_Synced_Position(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetPositionAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_manpowerBaseURL,
                     _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Position").GetSection("GetIDByAutoComplete").Value, "?",
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

        public async Task<List<SelectListItem>> GetDropdownByPositionLevel(GetDropDownInput param)
        {
            var URL = string.Concat(_manpowerBaseURL,
               _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Position").GetSection("GetDropdownByPositionLevel").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&SelectedValue=", param.SelectedValue,
                "&PositionLevelID=", param.PositionLevelID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        
        public async Task<List<SelectListItem>> GetDropDownByParentPositionID(GetDropDownByParentPositionIDInput param)
        {
            var URL = string.Concat(_manpowerBaseURL,
               _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Position").GetSection("GetDropDownByParentPositionID").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&SelectedValue=", param.SelectedValue,
                "&ParentPositionID=", param.ParentPositionID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetDropdownByOrgGroup(GetDropdownByOrgGroupInput param)
        {
            var URL = string.Concat(_manpowerBaseURL,
               _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Position").GetSection("GetDropdownByOrgGroup").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&SelectedValue=", param.SelectedValue,
                "&OrgGroupID=", param.OrgGroupID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<Form> GetPositionByID(int ID)
        {
            var URL = string.Concat(_manpowerBaseURL,
               _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Position").GetSection("GetByID").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new Form(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetDropdownByOrgGroupWithCount(GetDropdownByOrgGroupInput param)
        {
            var URL = string.Concat(_manpowerBaseURL,
               _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Position").GetSection("GetDropdownByOrgGroupWithCount").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&SelectedValue=", param.SelectedValue,
                "&OrgGroupID=", param.OrgGroupID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

    }
}
