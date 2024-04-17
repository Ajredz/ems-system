using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Plantilla.Transfer.Region;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;

namespace EMS.FrontEnd.SharedClasses.Common_Plantilla
{
    public class Common_Region : Utilities
    {
        public Common_Region(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }


        public async Task<Form> GetRegion(int ID)
        {

            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Region").GetSection("GetByID").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "id=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new Form(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetRegionAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Region").GetSection("GetIDByAutoComplete").Value, "?",
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

        public async Task<List<SelectListItem>> GetRegionDropDown(int id = 0)
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Region").GetSection("GetDropDown").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&id=", id);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
    }
}
