using EMS.Plantilla.Transfer.PSGC;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.SharedClasses.Common_Plantilla
{
    public class Common_Region_City : Utilities
    {
        public Common_Region_City(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<List<SelectListItem>> GetRegionDropdown(string code = "")
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGC").GetSection("GetRegionDropdown").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&code=", code);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        public async Task<List<SelectListItem>> GetProvinceDropdown()
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGC").GetSection("GetProvinceDropdown").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        public async Task<List<SelectListItem>> GetCityMunicipalityDropdown()
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGC").GetSection("GetCityMunicipalityDropdown").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        public async Task<List<SelectListItem>> GetBarangayDropdown()
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGC").GetSection("GetBarangayDropdown").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetAllPSGCValueOutput>> GetAllRegion()
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGC").GetSection("GetAllRegion").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetAllPSGCValueOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetAllPSGCValueOutput>> GetAllProvince()
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGC").GetSection("GetAllProvince").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetAllPSGCValueOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetAllPSGCValueOutput>> GetAllCityMunicipality()
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGC").GetSection("GetAllCityMunicipality").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetAllPSGCValueOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetAllPSGCValueOutput>> GetAllBarangay()
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGC").GetSection("GetAllBarangay").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetAllPSGCValueOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<GetPSGCValueOutput> GetRegionValueByCode(string RegionCode)
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGC").GetSection("GetRegionValueByCode").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&RegionCode=", RegionCode);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetPSGCValueOutput(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<GetPSGCValueOutput> GetProvinceValueByCode(string ProvinceCode)
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGC").GetSection("GetProvinceValueByCode").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&ProvinceCode=", ProvinceCode);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetPSGCValueOutput(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<GetPSGCValueOutput> GetCityMunicipalityValueByCode(string CityMunicipalityCode)
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGC").GetSection("GetCityMunicipalityValueByCode").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&CityMunicipalityCode=", CityMunicipalityCode);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetPSGCValueOutput(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<GetPSGCValueOutput> GetBarangayValueByCode(string BarangayCode)
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGC").GetSection("GetBarangayValueByCode").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&BarangayCode=", BarangayCode);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetPSGCValueOutput(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetProvinceDropdownByRegion(string RegionCode)
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGC").GetSection("GetProvinceDropdownByRegion").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&RegionCode=", RegionCode);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        public async Task<List<SelectListItem>> GetCityMunicipalityDropdownByProvince(string ProvinceCode)
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGC").GetSection("GetCityMunicipalityDropdownByProvince").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&ProvinceCode=", ProvinceCode);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetBarangayDropdownByCityMunicipality(string CityMunicipalityCode)  
        {
            var URL = string.Concat(_plantillaBaseURL,
               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGC").GetSection("GetBarangayDropdownByCityMunicipality").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&CityMunicipalityCode=", CityMunicipalityCode);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        //public async Task<List<GetRegionAutoCompleteOutput>> GetRegionAutoComplete(GetRegionAutoCompleteInput param)
        //{
        //    var URL = string.Concat(_plantillaBaseURL,
        //             _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("PSGC").GetSection("GetRegionAutoComplete").Value, "?",
        //             "userid=", _globalCurrentUser.UserID, "&",
        //             "Term=", param.Term, "&",
        //             "TopResults=", param.TopResults);

        //    var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetRegionAutoCompleteOutput>(), URL);
        //    if (IsSuccess)
        //        return Result;
        //    else
        //        throw new Exception(ErrorMessage);
        //}
    }
}
