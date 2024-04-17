using EMS.Recruitment.Data.DBContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Recruitment.Data.ApplicantDashboard
{

    public interface IApplicantDashboardDBAccess
    {
        Task<IEnumerable<TableVarApplicantCountByOrgGroup>> GetList();
    }

    public class ApplicantDashboardDBAccess : IApplicantDashboardDBAccess
    {
        private readonly RecruitmentContext _dbContext;

        public ApplicantDashboardDBAccess(RecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarApplicantCountByOrgGroup>> GetList()
        {
            return await _dbContext.TableVarApplicantCountByOrgGroup
               .FromSqlRaw(@"CALL sp_applicant_count_by_org_group()")
               .AsNoTracking()
               .ToListAsync();
        }
    }
}
