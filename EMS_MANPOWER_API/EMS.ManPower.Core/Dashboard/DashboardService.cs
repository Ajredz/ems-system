using EMS.Manpower.Data.Dashboard;
using EMS.Manpower.Data.DBContexts;
using EMS.Manpower.Transfer.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Manpower.Core.Dashboard
{
    public interface IDashboardService
    {
        Task<IActionResult> GetList(APICredentials credentials, bool isAdmin);
    }

    public class DashboardService : Core.Shared.Utilities, IDashboardService
    {

        private readonly IDashboardDBAccess _dbAccess;

        public DashboardService(ManpowerContext dbContext, IConfiguration iconfiguration,
            IDashboardDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, bool isAdmin)
        {
            IEnumerable<TableVarDescendants> result = await _dbAccess.GetList(credentials.UserID, isAdmin);

            return new OkObjectResult(new MRFDashboardList
            {
                OpenDescription = result.Select(x => x.OpenDesc).FirstOrDefault(),
                OpenValue = result.Select(x => x.OpenValue).FirstOrDefault(),
                OpenCountList = result.Select(x => x.OpenCount).ToList(),
                ClosedDescription = result.Select(x => x.ClosedDesc).FirstOrDefault(),
                ClosedValue = result.Select(x => x.ClosedValue).FirstOrDefault(),
                ClosedCountList = result.Select(x => x.ClosedCount).ToList(),
                AgeList = result.Select(x => x.Age).ToList()
            });
        }
    }
}
