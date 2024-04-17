﻿using EMS.Recruitment.Data.DBContexts;
using EMS.Recruitment.Data.PSGC;
using EMS.Recruitment.Transfer.PSGC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Recruitment.Core.PSGC
{
    public interface IPSGCService
    {
        Task<IActionResult> GetRegionDropDown(APICredentials credentials, string SelectedCode);
        Task<IActionResult> GetProvinceDropDown(APICredentials credentials, string SelectedCode);
        Task<IActionResult> GetCityMunicipalityDropDown(APICredentials credentials);
        Task<IActionResult> GetBarangayDropDown(APICredentials credentials);

        Task<IActionResult> GetRegionValueByCode(APICredentials credentials, string RegionCode);
        Task<IActionResult> GetProvinceValueByCode(APICredentials credentials, string ProvinceCode);
        Task<IActionResult> GetCityMunicipalityValueByCode(APICredentials credentials, string CityMunicipalityCode);
        Task<IActionResult> GetBarangayValueByCode(APICredentials credentials, string BarangayCode);

        Task<IActionResult> GetProvinceDropDownByRegion(APICredentials credentials, string RegionCode);
        Task<IActionResult> GetCityMunicipalityDropDownByProvince(APICredentials credentials, string ProvinceCode);
        Task<IActionResult> GetBarangayDropDownByCityMunicipality(APICredentials credentials, string CityMunicipalityCode);

        Task<IActionResult> GetAllRegion(APICredentials credentials);

        Task<IActionResult> GetAllProvince(APICredentials credentials);

        Task<IActionResult> GetAllCityMunicipality(APICredentials credentials);

        Task<IActionResult> GetAllBarangay(APICredentials credentials);

        //Task<IActionResult> GetCityDropDownByRegion(APICredentials credentials, int RegionID, int SelectedID);

        Task<IActionResult> GetRegionAutoComplete(APICredentials credentials, GetRegionAutoCompleteInput param);

        //Task<IActionResult> GetCityDropDownByRegionCode(APICredentials credentials, string RegionCode, int SelectedID);
    }

    public class PSGCService : Core.Shared.Utilities, IPSGCService
    {
        private readonly IPSGCDBAccess _dbAccess;

        public PSGCService(RecruitmentContext dbContext, IConfiguration iconfiguration, IPSGCDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetRegionDropDown(APICredentials credentials, string SelectedCode)
        {
            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetAllRegion()).OrderBy(x => x.Description).ToList(), "Code", "Description", "Code", SelectedCode));
        }

        public async Task<IActionResult> GetProvinceDropDown(APICredentials credentials, string SelectedCode)
        {
            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetAllProvince()).OrderBy(x => x.Description).ToList(), "Code", "Description", null, SelectedCode));
        }

        public async Task<IActionResult> GetCityMunicipalityDropDown(APICredentials credentials)
        {
            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetAllCityMunicipality()).OrderBy(x => x.Description).ToList(), "ID", "Description", ""));
        }

        public async Task<IActionResult> GetBarangayDropDown(APICredentials credentials)
        {
            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetAllBarangay()).OrderBy(x => x.Description).ToList(), "ID", "Description", ""));
        }

        public async Task<IActionResult> GetRegionValueByCode(APICredentials credentials, string RegionCode)
        {
            Data.PSGC.PSGCRegion result = await _dbAccess.GetRegionValueByCode(RegionCode);
            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new GetPSGCValueOutput
                {
                    Code = result.Code,
                    Description = result.Description
                });
        }

        public async Task<IActionResult> GetProvinceValueByCode(APICredentials credentials, string ProvinceCode)
        {
            Data.PSGC.PSGCProvince result = await _dbAccess.GetProvinceValueByCode(ProvinceCode);
            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new GetPSGCValueOutput
                {
                    Code = result.Code,
                    Description = result.Description
                });
        }

        public async Task<IActionResult> GetCityMunicipalityValueByCode(APICredentials credentials, string CityMunicipalityCode)
        {
            Data.PSGC.PSGCCityMunicipality result = await _dbAccess.GetCityMunicipalityValueByCode(CityMunicipalityCode);
            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new GetPSGCValueOutput
                {
                    Code = result.Code,
                    Description = result.Description
                });
        }

        public async Task<IActionResult> GetBarangayValueByCode(APICredentials credentials, string BarangayCode)
        {
            Data.PSGC.PSGCBarangay result = await _dbAccess.GetBarangayValueByCode(BarangayCode);
            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new GetPSGCValueOutput
                {
                    Code = result.Code,
                    Description = result.Description
                });
        }

        public async Task<IActionResult> GetProvinceDropDownByRegion(APICredentials credentials, string RegionCode)
        {
            var result = (await _dbAccess.GetRegionByCode(RegionCode)).ToList();
            string prefix = "";
            if (result != null)
                if (result.Count() > 0)
                    prefix = result.First().Prefix;

            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetProvinceByRegion(prefix)).OrderBy(x => x.Description).ToList(), "Code", "Description", ""));
        }

        public async Task<IActionResult> GetCityMunicipalityDropDownByProvince(APICredentials credentials, string ProvinceCode)
        {
            var result = (await _dbAccess.GetProvinceByCode(ProvinceCode)).ToList();
            string prefix = "";
            if (result != null)
                if (result.Count() > 0)
                    prefix = result.First().Prefix;

            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetCityMunicipalityByProvince(prefix)).OrderBy(x => x.Description).ToList(), "Code", "Description", ""));
        }

        public async Task<IActionResult> GetBarangayDropDownByCityMunicipality(APICredentials credentials, string CityMunicipalityCode)
        {
            var result = (await _dbAccess.GetCityMunicipalityByCode(CityMunicipalityCode)).ToList();
            string prefix = "";
            if (result != null)
                if (result.Count() > 0)
                    prefix = result.First().Prefix;

            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetBarangayByCityMunicipality(prefix)).OrderBy(x => x.Description).ToList(), "Code", "Description", ""));
        }

        public async Task<IActionResult> GetAllRegion(APICredentials credentials)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAllRegion())
                .OrderBy(x => x.Description)
                .Select(x => new GetAllPSGCValueOutput
                {
                    Code = x.Code,
                    Prefix = x.Prefix,
                    Description = x.Description
                }).ToList()
            );
        }

        public async Task<IActionResult> GetAllProvince(APICredentials credentials)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAllProvince())
                .OrderBy(x => x.Description)
                .Select(x => new GetAllPSGCValueOutput
                {
                    Code = x.Code,
                    ParentPrefix = x.RegionPrefix,
                    Prefix = x.Prefix,
                    Description = x.Description
                }).ToList()
            );
        }

        public async Task<IActionResult> GetAllCityMunicipality(APICredentials credentials)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAllCityMunicipality())
                .OrderBy(x => x.Description)
                .Select(x => new GetAllPSGCValueOutput
                {
                    Code = x.Code,
                    ParentPrefix = x.ProvincePrefix,
                    Prefix = x.Prefix,
                    Description = x.Description
                }).ToList()
            );
        }

        public async Task<IActionResult> GetAllBarangay(APICredentials credentials)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAllBarangay())
                .OrderBy(x => x.Description)
                .Select(x => new GetAllPSGCValueOutput
                {
                    Code = x.Code,
                    ParentPrefix = x.CityMunicipalityPrefix,
                    Description = x.Description
                }).ToList()
            );
        }

        //public async Task<IActionResult> GetCityDropDownByRegion(APICredentials credentials, int RegionID, int SelectedID)
        //{
        //    return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetCityByRegion(RegionID)).OrderBy(x => x.Description).ToList(), "ID", "Description", "Code", SelectedID));
        //}

        public async Task<IActionResult> GetRegionAutoComplete(APICredentials credentials, GetRegionAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetRegionAutoComplete(param))
                .Select(x => new GetRegionAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = string.Concat(x.Code, " - ", x.Description)
                })
            );
        }

        //public async Task<IActionResult> GetCityDropDownByRegionCode(APICredentials credentials, string RegionCode, int SelectedID)
        //{
        //    var Region = await _dbAccess.GetRegionByCode(RegionCode);
        //    return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetCityByRegion(Region.ID)).OrderBy(x => x.Description).ToList(), "Code", "Description", null, SelectedID));
        //}

    }
}
