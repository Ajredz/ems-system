using EMS.Recruitment.Data.DBContexts;
using EMS.Recruitment.Transfer.ApplicantTagging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Recruitment.Data.ApplicantTagging
{
    public interface IApplicantTaggingDBAccess
    {
        Task<IEnumerable<TableVarApplicantTagging>> GetList(GetListInput input, int rowStart);
    }

    public class ApplicantTaggingDBAccess : IApplicantTaggingDBAccess
    {
        private readonly RecruitmentContext _dbContext;

        public ApplicantTaggingDBAccess(RecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarApplicantTagging>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarApplicantTagging
               .FromSqlRaw(@"CALL sp_applicant_tagging_get_list(
                            {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                            , {8}
                        )", input.ID ?? 0
                            , input.ApplicantName ?? ""
                            , input.ApplicationSourceDelimited ?? ""
                            , input.PositionRemarks ?? ""
                            , input.ReferredBy ?? ""
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , rowStart
                            , input.rows)
               .AsNoTracking()
               .ToListAsync();
        }
    }
}
