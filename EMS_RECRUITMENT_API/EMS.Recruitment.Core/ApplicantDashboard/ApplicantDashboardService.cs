using EMS.Recruitment.Data.ApplicantDashboard;
using EMS.Recruitment.Data.DBContexts;
using EMS.Recruitment.Transfer.ApplicantDashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Recruitment.Core.ApplicantDashboard
{
    public interface IApplicantDashboardService
    {
        Task<IActionResult> GetList(APICredentials credentials);
    }

    public class ApplicantDashboardService : Core.Shared.Utilities, IApplicantDashboardService
    {

        private readonly IApplicantDashboardDBAccess _dbAccess;

        public ApplicantDashboardService(RecruitmentContext dbContext, IConfiguration iconfiguration,
            IApplicantDashboardDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials)
        {
            IEnumerable<TableVarApplicantCountByOrgGroup> result = await _dbAccess.GetList();

            return new OkObjectResult(result.Select(x => new GetApplicantOrgGroupOutput
            {
                OrgGroup = x.OrgGroup,
                PositionTitle = x.PositionTitle,
                ApplicantCount = x.ApplicantCount
            }).ToList());
        }
    }
}
