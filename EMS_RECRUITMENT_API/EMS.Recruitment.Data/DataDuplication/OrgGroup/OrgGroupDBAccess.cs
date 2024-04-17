using EMS.Recruitment.Data.DBContexts;
using EMS.Recruitment.Transfer.DataDuplication.OrgGroup;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Recruitment.Data.DataDuplication.OrgGroup
{
    public interface IOrgGroupDBAccess
    {
        Task<IEnumerable<OrgGroup>> GetAutoComplete(GetAutoCompleteInput param);

        Task<bool> Sync(List<OrgGroup> toDelete,
           List<OrgGroup> toAdd,
           List<OrgGroup> toUpdate);

        Task<IEnumerable<OrgGroup>> GetBySyncIDs(List<int> IDs);

        Task<IEnumerable<OrgGroup>> GetAll();

        Task<IEnumerable<OrgGroup>> GetByOrgGroupAutoComplete(GetByOrgTypeAutoCompleteInput param);
    }

    public class OrgGroupDBAccess : IOrgGroupDBAccess
    {
        private readonly RecruitmentContext _dbContext;

        public OrgGroupDBAccess(RecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Sync(List<OrgGroup> toDelete,
           List<OrgGroup> toAdd,
           List<OrgGroup> toUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Execute filtered records to their respective actions.
                _dbContext.OrgGroup.RemoveRange(toDelete);
                _dbContext.OrgGroup.AddRange(toAdd);
                toUpdate.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<IEnumerable<OrgGroup>> GetAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.OrgGroup
                .FromSqlRaw("CALL sp_org_group_autocomplete({0},{1})", (param.Term ?? ""), param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<OrgGroup>> GetBySyncIDs(List<int> IDs)
        {
            return await _dbContext.OrgGroup.AsNoTracking().Where(x => IDs.Contains(x.SyncID)).ToListAsync();
        }

        public async Task<IEnumerable<OrgGroup>> GetAll()
        {
            return await _dbContext.OrgGroup.AsNoTracking().Where(x => x.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<OrgGroup>> GetByOrgGroupAutoComplete(GetByOrgTypeAutoCompleteInput param)
        {
            return await _dbContext.OrgGroup
                .FromSqlRaw("CALL sp_org_group_by_org_type_autocomplete({0},{1},{2})"
                , (param.Term ?? "")
                , param.TopResults
                , param.OrgType)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}