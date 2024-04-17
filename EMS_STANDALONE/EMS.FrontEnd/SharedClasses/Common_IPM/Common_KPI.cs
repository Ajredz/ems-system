using Utilities.API;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.IPM.Transfer.KPI;
using Microsoft.AspNetCore.Hosting;
using EMS_SecurityServiceModel.Reference;

namespace EMS.FrontEnd.SharedClasses.Common_IPM
{
    public class Common_KPI : Utilities
    {
        public Common_KPI(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<Form> GetKPI(int ID)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPI").GetSection("getbyid").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "id=", ID);
            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new Form(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetKPICodeDropDown(int id = 0)
        {
            var URL = string.Concat(_ipmBaseURL,
               _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPI").GetSection("GetCodeDropDown").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&id=", id);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetKRAGroupDropdown(int id = 0)
        {
            var URL = string.Concat(_ipmBaseURL,
               _iconfiguration.GetSection("IPMService_API_URL").GetSection("KRAGroup").GetSection("GetKRAGroupDropdown").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&id=", id);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetKRASubGroupDropdown(int KRAGroupID = 0, int id = 0)
        {
            var URL = string.Concat(_ipmBaseURL,
               _iconfiguration.GetSection("IPMService_API_URL").GetSection("KRASubGroup").GetSection("GetKRASubGroupDropdown").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&kragroupid=", KRAGroupID,
                "&id=", id);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetKPIAutoComplete(string term, int TopResults)
        {
            var URL = string.Concat(_ipmBaseURL,
                     _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPI").GetSection("GetIDByAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "Term=", term, "&",
                     "TopResults=", TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetIDByAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetAllKPIDetailsOutput>> GetAllKPIDetails()
        {
            var URL = string.Concat(_ipmBaseURL,
               _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPI").GetSection("GetAllDetails").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetAllKPIDetailsOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<IPM.Transfer.Shared.GetAutoCompleteOutput>> GetKRAGroupAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_ipmBaseURL,
                     _iconfiguration.GetSection("IPMService_API_URL").GetSection("KRAGroup").GetSection("GetAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "term=", Term, "&",
                     "topresults=", TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<IPM.Transfer.Shared.GetAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<IPM.Transfer.Shared.GetAutoCompleteOutput>> GetKRASubGroupAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_ipmBaseURL,
                     _iconfiguration.GetSection("IPMService_API_URL").GetSection("KRASubGroup").GetSection("GetAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "term=", Term, "&",
                     "topresults=", TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<IPM.Transfer.Shared.GetAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<ReferenceValue>> GetReferenceValueByRefCode(string RefCode)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPI").GetSection("GetByRefCodes").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "RefCodes=", RefCode);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<ReferenceValue>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
    }
}
