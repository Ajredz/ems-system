using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Security.Transfer.SystemRole;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;

namespace EMS.FrontEnd.SharedClasses.Common_Security
{
    public class Common_SystemRole : Utilities
    {
        public Common_SystemRole(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetSystemRoleAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_securityBaseURL,
                     _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemRole").GetSection("GetIDByAutoComplete").Value, "?",
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
            var URL = string.Concat(_securityBaseURL,
               _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemRole").GetSection("GetDropDown").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetByUserIDOutput>> GetSystemRoleByUserID(int ID = 0)
        {
            var URL = string.Concat(_securityBaseURL,
               _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemRole").GetSection("GetByUserID").Value, "?",
                "userid=", (ID == 0 ? _globalCurrentUser.UserID : ID));

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetByUserIDOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<Form> GetSystemRole(int ID)
        {
            var URL = string.Concat(_securityBaseURL,
                  _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemRole").GetSection("GetByID").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new Form(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);

        }

        public async Task<List<SelectListItem>> GetSystemUserRoleDropDownByRoleID(int ID)
        {
            var URL = string.Concat(_securityBaseURL,
                     _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemRole").GetSection("GetSystemUserRoleDropDownByRoleID").Value, "?",
                      "userid=", _globalCurrentUser.UserID, "&",
                      "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SystemRoleAccess>> GetRolePage(int ID = 0)
        {
            var URL = string.Concat(_securityBaseURL,
               _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemRole").GetSection("GetRolePage").Value, "?",
                "userid=", _globalCurrentUser.UserID, "&",
                "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SystemRoleAccess>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

    }
}
