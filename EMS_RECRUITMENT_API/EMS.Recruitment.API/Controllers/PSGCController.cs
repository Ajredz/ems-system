﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Recruitment.Core.PSGC;
using EMS.Recruitment.Transfer.PSGC;
using Microsoft.AspNetCore.Mvc;
using Utilities.API;

namespace EMS.Recruitment.API.Controllers
{
    /// <summary>
    /// PSGC Maintenance
    /// </summary>
    [Route("Recruitment/[controller]")]
    [ApiController]
    public class PSGCController : ControllerBase
    {
        private readonly IPSGCService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public PSGCController(IPSGCService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get records to be displayed on dropdown elements
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-region-dropdown")]
        public async Task<IActionResult> GetRegionDropDown([FromQuery] APICredentials credentials, [FromQuery] string Code)
        {
            return await _service.GetRegionDropDown(credentials, Code).ConfigureAwait(true);
        }
        
        [HttpGet]
        [Route("get-all-region")]
        public async Task<IActionResult> GetAllRegion([FromQuery] APICredentials credentials)
        {
            return await _service.GetAllRegion(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-all-province")]
        public async Task<IActionResult> GetAllProvince([FromQuery] APICredentials credentials)
        {
            return await _service.GetAllProvince(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-all-city-municipality")]
        public async Task<IActionResult> GetAllCityMunicipality([FromQuery] APICredentials credentials)
        {
            return await _service.GetAllCityMunicipality(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-all-barangay")]
        public async Task<IActionResult> GetAllBarangay([FromQuery] APICredentials credentials)
        {
            return await _service.GetAllBarangay(credentials).ConfigureAwait(true);
        }

        /// <summary>
        /// Get records to be displayed on dropdown elements
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="Code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-province-dropdown")]
        public async Task<IActionResult> GetCityDropDown([FromQuery] APICredentials credentials, [FromQuery] string Code)
        {
            return await _service.GetProvinceDropDown(credentials, Code).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-city-municipality-dropdown")]
        public async Task<IActionResult> GetCityMunicipalityDropDown([FromQuery] APICredentials credentials)
        {
            return await _service.GetCityMunicipalityDropDown(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-barangay-dropdown")]
        public async Task<IActionResult> GetBarangayDropDown([FromQuery] APICredentials credentials)
        {
            return await _service.GetBarangayDropDown(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-region-value-by-code")]
        public async Task<IActionResult> GetRegionValueByCode([FromQuery] APICredentials credentials, [FromQuery] string RegionCode)
        {
            return await _service.GetRegionValueByCode(credentials, RegionCode).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-province-value-by-code")]
        public async Task<IActionResult> GetProvinceValueByCode([FromQuery] APICredentials credentials, [FromQuery] string ProvinceCode)
        {
            return await _service.GetProvinceValueByCode(credentials, ProvinceCode).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-city-municipality-value-by-code")]
        public async Task<IActionResult> GetCityMunicipalityValueByCode([FromQuery] APICredentials credentials, [FromQuery] string CityMunicipalityCode)
        {
            return await _service.GetCityMunicipalityValueByCode(credentials, CityMunicipalityCode).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-barangay-value-by-code")]
        public async Task<IActionResult> GetBarangayValueByCode([FromQuery] APICredentials credentials, [FromQuery] string BarangayCode)
        {
            return await _service.GetBarangayValueByCode(credentials, BarangayCode).ConfigureAwait(true);
        }

        /// <summary>
        /// Get records to be displayed on dropdown elements
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="RegionCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-province-dropdown-by-region")]
        public async Task<IActionResult> GetProvinceDropDownByRegion([FromQuery] APICredentials credentials, [FromQuery] string RegionCode)
        {
            return await _service.GetProvinceDropDownByRegion(credentials, RegionCode).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-city-municipality-dropdown-by-province")]
        public async Task<IActionResult> GetCityMunicipalityDropDownByProvince([FromQuery] APICredentials credentials, [FromQuery] string ProvinceCode)
        {
            return await _service.GetCityMunicipalityDropDownByProvince(credentials, ProvinceCode).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-barangay-dropdown-by-city-municipality")]
        public async Task<IActionResult> GetBarangayDropDownByCityMunicipality([FromQuery] APICredentials credentials, [FromQuery] string CityMunicipalityCode)
        {
            return await _service.GetBarangayDropDownByCityMunicipality(credentials, CityMunicipalityCode).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-region-autocomplete")]
        public async Task<IActionResult> GetRegionAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetRegionAutoCompleteInput param)
        {
            return await _service.GetRegionAutoComplete(credentials, param).ConfigureAwait(true);
        }

        //[HttpGet]
        //[Route("get-city-dropdown-by-region-code")]
        //public async Task<IActionResult> GetCityDropDownByRegionCode([FromQuery] APICredentials credentials, [FromQuery] string RegionCode, [FromQuery] int ID)
        //{
        //    return await _service.GetCityDropDownByRegionCode(credentials, RegionCode, ID).ConfigureAwait(true);
        //}
    }
}