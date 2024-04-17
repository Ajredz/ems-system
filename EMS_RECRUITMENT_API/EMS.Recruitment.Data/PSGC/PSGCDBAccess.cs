using EMS.Recruitment.Data.DBContexts;
using EMS.Recruitment.Transfer.PSGC;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Recruitment.Data.PSGC
{

    public interface IPSGCDBAccess
    {
        Task<IEnumerable<PSGCRegion>> GetAllRegion();

        Task<IEnumerable<PSGCProvince>> GetAllProvince();

        Task<IEnumerable<PSGCCityMunicipality>> GetAllCityMunicipality();

        Task<IEnumerable<PSGCBarangay>> GetAllBarangay();

        Task<IEnumerable<PSGCProvince>> GetProvinceByRegion(string RegionPrefix);

        Task<IEnumerable<PSGCCityMunicipality>> GetCityMunicipalityByProvince(string ProvincePrefix);

        Task<IEnumerable<PSGCBarangay>> GetBarangayByCityMunicipality(string CityMunicipalityPrefix);

        Task<IEnumerable<PSGCRegion>> GetRegionAutoComplete(GetRegionAutoCompleteInput param);

        Task<PSGCRegion> GetRegionValueByCode(string Code);
        Task<PSGCProvince> GetProvinceValueByCode(string Code);
        Task<PSGCCityMunicipality> GetCityMunicipalityValueByCode(string Code);
        Task<PSGCBarangay> GetBarangayValueByCode(string Code);

        //Task<PSGCRegion> GetRegionByCode(string Code);

        Task<IEnumerable<PSGCRegion>> GetRegionByCode(string Code);
        Task<IEnumerable<PSGCProvince>> GetProvinceByCode(string Code);
        Task<IEnumerable<PSGCCityMunicipality>> GetCityMunicipalityByCode(string Code);
        Task<IEnumerable<PSGCBarangay>> GetBarangayByCode(string Code);
    }

    public class PSGCDBAccess : IPSGCDBAccess
    {
        private readonly RecruitmentContext _dbContext;

        public PSGCDBAccess(RecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PSGCRegion>> GetAllRegion()
        {
            return await _dbContext.PSGCRegion.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<PSGCProvince>> GetAllProvince()
        {
            return await _dbContext.PSGCProvince.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<PSGCCityMunicipality>> GetAllCityMunicipality()
        {
            return await _dbContext.PSGCCityMunicipality.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<PSGCBarangay>> GetAllBarangay()
        {
            return await _dbContext.PSGCBarangay.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<PSGCProvince>> GetProvinceByRegion(string RegionPrefix)
        {
            return await _dbContext.PSGCProvince.AsNoTracking()
                .Where(x => x.RegionPrefix.Equals(RegionPrefix))
                .ToListAsync();
        }

        public async Task<IEnumerable<PSGCCityMunicipality>> GetCityMunicipalityByProvince(string ProvincePrefix)
        {
            return await _dbContext.PSGCCityMunicipality.AsNoTracking()
                .Where(x => x.ProvincePrefix.Equals(ProvincePrefix))
                .ToListAsync();
        }

        public async Task<IEnumerable<PSGCBarangay>> GetBarangayByCityMunicipality(string CityMunicipalityPrefix)
        {
            return await _dbContext.PSGCBarangay.AsNoTracking()
                .Where(x => x.CityMunicipalityPrefix.Equals(CityMunicipalityPrefix))
                .ToListAsync();
        }

        public async Task<IEnumerable<PSGCRegion>> GetRegionAutoComplete(GetRegionAutoCompleteInput param)
        {
            return await _dbContext.PSGCRegion
                .FromSqlRaw("CALL sp_psgc_region_autocomplete({0},{1})"
                , (param.Term ?? "")
                , param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }

        //For View
        public async Task<PSGCRegion> GetRegionValueByCode(string Code)
        {
            return await _dbContext.PSGCRegion.Where(x => x.Code.Equals(Code)).FirstOrDefaultAsync();
        }

        public async Task<PSGCProvince> GetProvinceValueByCode(string Code)
        {
            return await _dbContext.PSGCProvince.Where(x => x.Code.Equals(Code)).FirstOrDefaultAsync();
        }

        public async Task<PSGCCityMunicipality> GetCityMunicipalityValueByCode(string Code)
        {
            return await _dbContext.PSGCCityMunicipality.Where(x => x.Code.Equals(Code)).FirstOrDefaultAsync();
        }

        public async Task<PSGCBarangay> GetBarangayValueByCode(string Code)
        {
            return await _dbContext.PSGCBarangay.Where(x => x.Code.Equals(Code)).FirstOrDefaultAsync();
        }

        //For Edit
        public async Task<IEnumerable<PSGCRegion>> GetRegionByCode(string Code)
        {
            return await _dbContext.PSGCRegion.AsNoTracking()
                .Where(x => x.Code.Equals(Code))
                .ToListAsync();
        }
        public async Task<IEnumerable<PSGCProvince>> GetProvinceByCode(string Code)
        {
            return await _dbContext.PSGCProvince.AsNoTracking()
                .Where(x => x.Code.Equals(Code))
                .ToListAsync();
        }
        public async Task<IEnumerable<PSGCCityMunicipality>> GetCityMunicipalityByCode(string Code)
        {
            return await _dbContext.PSGCCityMunicipality.AsNoTracking()
                .Where(x => x.Code.Equals(Code))
                .ToListAsync();
        }
        public async Task<IEnumerable<PSGCBarangay>> GetBarangayByCode(string Code)
        {
            return await _dbContext.PSGCBarangay.AsNoTracking()
                .Where(x => x.Code.Equals(Code))
                .ToListAsync();
        }

        //public async Task<PSGCRegion> GetRegionByCode(string Code)
        //{
        //    return await _dbContext.PSGCRegion.AsNoTracking()
        //        .Where(x => x.Code.Equals(Code)).FirstOrDefaultAsync();
        //}
    }
}
