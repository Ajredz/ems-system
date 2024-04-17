using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using EMS.Security.Transfer.SystemUser;
using EMS_SecurityServiceModel.SystemUser;
using Org.BouncyCastle.Crypto;

namespace EMS.FrontEnd.SharedClasses.Common_Security
{
    public class Common_SystemUser : Utilities
    {
        public Common_SystemUser(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetSystemUserAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_securityBaseURL,
                     _iconfiguration.GetSection("SecurityService_API_URL").GetSection("GetSystemUserIDByAutoComplete").Value, "?",
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

        public async Task<EMS.Security.Transfer.SystemUser.Form> GetSystemUserByID(int ID)
        {
            var URL = string.Concat(_securityBaseURL,
                     _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("GetByID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new EMS.Security.Transfer.SystemUser.Form(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<EMS.Security.Transfer.SystemUser.Form>> GetSystemUserByIDs(List<int> IDs)
        {
            var URL = string.Concat(_securityBaseURL,
                     _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("GetByIDs").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.PostFromAPI(new List<EMS.Security.Transfer.SystemUser.Form>(), IDs, URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(Form, bool, string)> AddSystemUser(GetByNameInput param)
        {
            var URL = string.Concat(_securityBaseURL,
                      _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("Add").Value, "?",
                      "userid=", _globalCurrentUser.UserID, "&",
                      "EmployeeCode=", param.EmployeeCode, "&",
                      "FirstName=", param.FirstName, "&",
                      "MiddleName=", param.MiddleName, "&",
                      "LastName=", param.LastName);

            //var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(param, URL);
            //if (IsSuccess)
            //    return true;
            //else
            //    throw new Exception(Message);
            return await SharedUtilities.PostFromAPI(new Form(), param, URL);
        }

        public async Task<List<SelectListItem>> GetSystemUserDropDown()
        {
            var URL = string.Concat(_securityBaseURL,
                     _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("GetSystemUserDropDown").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetSystemUserRoleDropDownByUserID(int ID)
        {
            var URL = string.Concat(_securityBaseURL,
                     _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("GetSystemUserRoleDropDownByUserID").Value, "?",
                      "userid=", _globalCurrentUser.UserID, "&",
                      "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<bool> DisableEmployeeAccount(UpdateSystemUser param)
        {
            var URL = string.Concat(_securityBaseURL,
                      _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("DisableEmployeeAccount").Value, "?",
                      "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(param, URL);
            if (IsSuccess)
                return true;
            else
                throw new Exception(Message);
        }
        public async Task<List<SystemUser>> GetUserByRoleIDs(List<int> IDs)
        {
            var URL = string.Concat(_securityBaseURL,
                     _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("GetUserByRoleIDs").Value, "?",
                      "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.PostFromAPI(new List<SystemUser>(), IDs, URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
    }
}
