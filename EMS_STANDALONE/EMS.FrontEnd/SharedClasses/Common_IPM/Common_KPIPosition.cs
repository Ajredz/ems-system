using Utilities.API;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.IPM.Transfer.KPIPosition;
using Microsoft.AspNetCore.Hosting;

namespace EMS.FrontEnd.SharedClasses.Common_IPM
{
    public class Common_KPIPosition : Utilities
    {
        public Common_KPIPosition(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<Form> GetKPIPosition(int ID, string EffectiveDate)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPIPosition").GetSection("getbyid").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "ID=", ID, "&",
                  "EffectiveDate=", EffectiveDate);
            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new Form(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetKPIDropdown(int id = 0)
        {
            var URL = string.Concat(_ipmBaseURL,
               _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPI").GetSection("GetDropdown").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&id=", id);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetAllKPIOutput>> GetAllKPI()
        {
            var URL = string.Concat(_ipmBaseURL,
               _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPI").GetSection("GetAll").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetAllKPIOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetAllKPIPositionOutput>> GetAllKPIPosition()
        {
            var URL = string.Concat(_ipmBaseURL,
               _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPIPosition").GetSection("GetAll").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetAllKPIPositionOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetAllKPIPositionDetailsOutput>> GetAllKPIPositionDetails()
        {
            var URL = string.Concat(_ipmBaseURL,
               _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPIPosition").GetSection("GetAllDetails").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetAllKPIPositionDetailsOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetCopyPositionOutput>> GetCopyPosition(string term, int TopResults)
        {
            var URL = string.Concat(_ipmBaseURL,
                     _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPIPosition").GetSection("GetCopyPosition").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "Term=", term, "&",
                     "TopResults=", TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetCopyPositionOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
    }
}
