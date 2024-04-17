using EMS.IPM.Data.DBContexts;
using EMS.IPM.Data.Shared;
using EMS.IPM.Transfer.DataDuplication.OrgGroup;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.IPM.Data.DataDuplication.OrgGroup
{
    public interface IOrgGroupDBAccess
    {
        Task<IEnumerable<OrgGroup>> GetAutoComplete(GetAutoCompleteInput param);

        Task<IEnumerable<OrgGroup>> GetFilteredIDByAutoComplete(GetAutoCompleteInput param);

        Task<bool> Sync(List<OrgGroup> toDelete,
           List<OrgGroup> toAdd,
           List<OrgGroup> toUpdate);

        Task<IEnumerable<OrgGroup>> GetBySyncIDs(List<int> IDs);

        Task<bool> SyncPosition(List<OrgGroupPosition> toDelete,
           List<OrgGroupPosition> toAdd,
           List<OrgGroupPosition> toUpdate);

        Task<IEnumerable<OrgGroupPosition>> GetPositionBySyncIDs(List<int> IDs);

        Task<IEnumerable<OrgGroupPosition>> GetPositionByOrgGroupID(int OrgGroupID);

        Task<IEnumerable<OrgGroup>> GetAll();

        Task<OrgGroup> GetByID(int ID);

        Task<IEnumerable<TableVariableAutoComplete>> GetRegionAutoComplete(Transfer.Shared.GetAutoCompleteInput param);

        Task<IEnumerable<TableVariableAutoComplete>> GetBranchAutoComplete(Transfer.Shared.GetAutoCompleteInput param);
    }

    public class OrgGroupDBAccess : IOrgGroupDBAccess
    {
        private readonly IPMContext _dbContext;

        public OrgGroupDBAccess(IPMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<OrgGroup>> GetAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.OrgGroup
                .FromSqlRaw("CALL sp_org_group_autocomplete({0},{1})", (param.Term ?? ""), param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }
        
        public async Task<IEnumerable<TableVariableAutoComplete>> GetRegionAutoComplete(Transfer.Shared.GetAutoCompleteInput param)
        {
            return await _dbContext.TableVariableAutoComplete
                .FromSqlRaw("CALL sp_org_group_region_autocomplete({0},{1})", (param.Term ?? ""), param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }
          public async Task<IEnumerable<TableVariableAutoComplete>> GetBranchAutoComplete(Transfer.Shared.GetAutoCompleteInput param)
        {
            return await _dbContext.TableVariableAutoComplete
                .FromSqlRaw("CALL sp_org_group_branch_autocomplete({0},{1})", (param.Term ?? ""), param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<OrgGroup>> GetFilteredIDByAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.OrgGroup
                .FromSqlRaw("CALL sp_filtered_org_group_autocomplete({0},{1},{2})", (param.Term ?? ""), param.TopResults, (param.Filter ?? ""))
                .AsNoTracking()
                .ToListAsync();
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

        public async Task<IEnumerable<OrgGroup>> GetBySyncIDs(List<int> IDs)
        {
            return await _dbContext.OrgGroup.AsNoTracking().Where(x => IDs.Contains(x.SyncID)).ToListAsync();
        }

        public async Task<bool> SyncPosition(List<OrgGroupPosition> toDelete,
           List<OrgGroupPosition> toAdd,
           List<OrgGroupPosition> toUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Execute filtered records to their respective actions.
                _dbContext.OrgGroupPosition.RemoveRange(toDelete);
                _dbContext.OrgGroupPosition.AddRange(toAdd);
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

        public async Task<IEnumerable<OrgGroupPosition>> GetPositionBySyncIDs(List<int> IDs)
        {
            return await _dbContext.OrgGroupPosition.AsNoTracking().Where(x => IDs.Contains(x.SyncID)).ToListAsync();
        }

        public async Task<IEnumerable<OrgGroupPosition>> GetPositionByOrgGroupID(int OrgGroupID)
        {
            return await _dbContext.OrgGroupPosition.AsNoTracking().Where(x => x.OrgGroupID == OrgGroupID).ToListAsync();
        }

        public async Task<IEnumerable<OrgGroup>> GetAll()
        {
            return await _dbContext.OrgGroup.AsNoTracking().Where(x => x.IsActive).ToListAsync();
        }

        public async Task<OrgGroup> GetByID(int ID)
        {
            return await _dbContext.OrgGroup.Where(x => x.SyncID == ID).FirstAsync();
        }
    }
}