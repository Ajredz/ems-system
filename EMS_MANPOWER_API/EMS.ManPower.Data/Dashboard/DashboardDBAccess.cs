using EMS.Manpower.Data.DBContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Manpower.Data.Dashboard
{
    public interface IDashboardDBAccess
    {
        Task<IEnumerable<TableVarDescendants>> GetList(int userID, bool isAdmin);
    }

    public class DashboardDBAccess : IDashboardDBAccess
    {
        private readonly ManpowerContext _dbContext;

        public DashboardDBAccess(ManpowerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarDescendants>> GetList(int userID, bool isAdmin)
        {
            return await _dbContext.TableVarMRFDashboardByAge
               .FromSqlRaw(@"CALL sp_mrf_dashboard_by_age({0}, {1})", userID, isAdmin)
               .AsNoTracking()
               .ToListAsync();
        }
    }
}
