using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API.ReferenceMaintenance;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using EMS.Workflow.Transfer.Reference;
using EMS.Recruitment.Transfer.Reference;
using EMS.Workflow.Transfer.EmailServerCredential;
using Microsoft.AspNetCore.Mvc;

namespace EMS.FrontEnd.SharedClasses
{
    public class Common_Reference : Utilities
    {
        private readonly string _baseURL;
        public Common_Reference(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, string baseURL, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
            _baseURL = baseURL;
        }

        public async Task<List<Reference>> GetReferenceByCode(string[] codes)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var code in codes)
                builder.Append(string.Concat("&Codes=",code));

            var URL = string.Concat(_baseURL,
                  _iconfiguration.GetSection("Reference").GetSection("GetByCodes").Value, "?",
                  "userid=", _globalCurrentUser.UserID, builder.ToString());

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<Reference>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<Reference>> GetReferenceByIsMaintainable()
        {
            var URL = string.Concat(_baseURL,
                  _iconfiguration.GetSection("Reference").GetSection("GetMaintainable").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<Reference>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }


        public async Task<List<ReferenceValue>> GetReferenceValueByRefCode(string RefCode)
        {
            var URL = string.Concat(_baseURL,
                  _iconfiguration.GetSection("Reference").GetSection("GetByRefCodes").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "RefCodes=", RefCode);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<ReferenceValue>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<ReferenceValue>> GetReferenceValueBySecurityRefCode(string RefCode)
        {
            var URL = string.Concat(_baseURL,
                  _iconfiguration.GetSection("Reference").GetSection("GetBySecurityRefCodes").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "RefCode=", RefCode);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<ReferenceValue>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<ReferenceValue>> GetReferenceValueByRefCodeAndValuePrefix(GetByRefCodeAndValuePrefixInput param)
        {
            var URL = string.Concat(_baseURL,
                  _iconfiguration.GetSection("Reference").GetSection("GetByRefCodeAndValuePrefix").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "RefCode=", param.RefCode, "&",
                  "ValuePrefix=", param.ValuePrefix
                  );

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<ReferenceValue>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<ReferenceValue>> GetReferenceValueByRefCodes(List<string> RefCodes)
        {
            var URL = string.Concat(_baseURL,
                  _iconfiguration.GetSection("Reference").GetSection("GetByRefCodes").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "RefCodes=", string.Join("&RefCodes=", RefCodes));

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<ReferenceValue>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<ReferenceValue> GetReferenceValueByRefCodeAndValue(string RefCode, string Value)
        {
            var URL = string.Concat(_baseURL,
                  _iconfiguration.GetSection("Reference").GetSection("GetByRefCodeValue").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "RefCode=", RefCode, "&" ,
                  "Value=", Value);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new ReferenceValue(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<ReferenceValue>> GetReferenceValueAutoComplete(string RefCode, string term)
        {
            var URL = string.Concat(_baseURL,
                  _iconfiguration.GetSection("Reference").GetSection("GetIDByAutoComplete").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "refcode=", RefCode, "&", 
                  "term=", term);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<ReferenceValue>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetReferenceValueListOutput>> GetReferenceValueList(string RefCode)
        {
            var URL = string.Concat(_baseURL,
                     _iconfiguration.GetSection("Reference").GetSection("GetReferenceValueList").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "RefCode=", RefCode);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetReferenceValueListOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<ReferenceValue> GetReferenceValueByID(int ID)
        {

            var URL = string.Concat(_baseURL,
                  _iconfiguration.GetSection("Reference").GetSection("GetReferenceValueByID").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "id=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new ReferenceValue(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetDropDownByRefCode(string RefCode, string ID = "")
        {
            var URL = string.Concat(_baseURL,
                  _iconfiguration.GetSection("Reference").GetSection("GetDropDown").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "RefCodes=", RefCode, "&",
                  "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<IPM.Transfer.Reference.GetReferenceValueOutput> GetRefValueByRefCode(string RefCode)
        {
            var URL = string.Concat(_baseURL,
                     _iconfiguration.GetSection("Reference").GetSection("GetByRefCode").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "RefCode=", RefCode);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new IPM.Transfer.Reference.GetReferenceValueOutput(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
    }
}
