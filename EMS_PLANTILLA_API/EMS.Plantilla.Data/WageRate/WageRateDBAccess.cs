using EMS.Plantilla.Data.DBContexts;
using EMS.Plantilla.Transfer.PSGC;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Plantilla.Data.WageRate
{

    public interface IWageRateDBAccess
    {

    }

    public class WageRateDBAccess : IWageRateDBAccess
    {
        private readonly PlantillaContext _dbContext;

        public WageRateDBAccess(PlantillaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<WageRate>> GetRegionAutoComplete(string RegionCode)
        {
            return await _dbContext.WageRate
                .FromSqlRaw("CALL sp_wage_rate_get_by_region_code({0})", RegionCode)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
