using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Security.Transfer.AuditLog;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;

namespace EMS.FrontEnd.SharedClasses.Common_Security
{
    public class Common_AuditLog : Utilities
    {
        public Common_AuditLog(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public enum EventType { 
            LOGIN,
            LOGOUT,
            ADD,
            PRINT,
            EXPORT,
            EDIT,
            RERUN,
            DELETE,
            VOID,
            CHANGE_PASSWORD,
            FORCE_CHANGE_PASSWORD,
            UPLOAD,
            REVISE,
            CHANGE_STATUS
        }

        public async Task<(bool, string)> AddAuditLog(Form param)
        {
            var URL = string.Concat(_securityBaseURL,
                     _iconfiguration.GetSection("SecurityService_API_URL").GetSection("AuditLog").GetSection("Add").Value, "?",
                     "userid=", _globalCurrentUser.UserID);
            param.IPAddress = string.Concat(_globalCurrentUser.IPAddress, ", ", _globalCurrentUser.ComputerName);

            return await SharedUtilities.PostFromAPI(param, URL);
        }

        public async Task<List<GetEventTypeByAutoCompleteOutput>> GetEventTypeAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_securityBaseURL,
                     _iconfiguration.GetSection("SecurityService_API_URL").GetSection("AuditLog").GetSection("GetEventTypeByAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "term=", Term, "&",
                     "topresults=", TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetEventTypeByAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetTableNameByAutoCompleteOutput>> GetTableNameAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_securityBaseURL,
                     _iconfiguration.GetSection("SecurityService_API_URL").GetSection("AuditLog").GetSection("GetTableNameByAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "term=", Term, "&",
                     "topresults=", TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetTableNameByAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
    }
}
