using EMS.Manpower.Data.DBContexts;
using EMS.Manpower.Transfer.DataDuplication.OrgGroup;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Manpower.Data.DataDuplication.OrgGroup
{
    public interface IOrgGroupDBAccess
    {
        Task<IEnumerable<OrgGroup>> GetAutoComplete(GetAutoCompleteInput param);

        Task<IEnumerable<OrgGroup>> GetAll();

        Task<bool> Sync(List<OrgGroup> toDelete,
           List<OrgGroup> toAdd,
           List<OrgGroup> toUpdate);

        Task<IEnumerable<OrgGroup>> GetBySyncIDs(List<int> IDs);

        Task<bool> SyncPosition(List<OrgGroupPosition> toDelete,
           List<OrgGroupPosition> toAdd,
           List<OrgGroupPosition> toUpdate);

        Task<IEnumerable<OrgGroupPosition>> GetPositionBySyncIDs(List<int> IDs);

        Task<OrgGroup> GetByID(int ID);

        Task<IEnumerable<OrgGroupPosition>> GetPositionByOrgGroup(int OrgGroupID);

        Task<IEnumerable<int>> GetDescendants(int OrgGroupID);

        Task<IEnumerable<OrgGroup>> GetByOrgGroupAutoComplete(GetByOrgTypeAutoCompleteInput param);

        Task<IEnumerable<OrgGroup>> GetExcludeByOrgType(List<string> OrgTypeList);

        Task<IEnumerable<OrgGroup>> GetExcludeByOrgTypeAndSyncIDs(List<string> OrgTypeList, List<int> IDs);
    }

    public class OrgGroupDBAccess : IOrgGroupDBAccess
    {
        private readonly ManpowerContext _dbContext;

        public OrgGroupDBAccess(ManpowerContext dbContext)
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

        public async Task<IEnumerable<OrgGroup>> GetAll()
        {
            return await _dbContext.OrgGroup.AsNoTracking().Where(x => x.IsActive).ToListAsync();
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

        public async Task<OrgGroup> GetByID(int ID)
        {
            return await _dbContext.OrgGroup.Where(x => x.SyncID == ID).FirstAsync();
        }

        public async Task<IEnumerable<OrgGroupPosition>> GetPositionByOrgGroup(int OrgGroupID)
        {
            return await _dbContext.OrgGroupPosition.AsNoTracking()
                .Where(x => x.OrgGroupID == OrgGroupID & x.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<int>> GetDescendants(int OrgGroupID)
        {
            TableVarOrgGroupDescendants result = (await _dbContext.TableVarOrgGroupDescendants
                .FromSqlRaw("CALL sp_org_group_get_descendants({0})", OrgGroupID)
                .AsNoTracking().ToListAsync()).First();

            List<int> convertedResult = result.Descendants.Split(",").Select(int.Parse).ToList();

            return convertedResult;
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

        public async Task<IEnumerable<OrgGroup>> GetExcludeByOrgType(List<string> OrgTypeList)
        {
            return await _dbContext.OrgGroup.AsNoTracking()
                .Where(x => !OrgTypeList.Contains(x.OrgType)).ToListAsync();
        }

        public async Task<IEnumerable<OrgGroup>> GetExcludeByOrgTypeAndSyncIDs(List<string> OrgTypeList, List<int> IDs)
        {
            return await _dbContext.OrgGroup.AsNoTracking()
                .Where(y => IDs.Contains(y.SyncID))
                .Where(x => !OrgTypeList.Contains(x.OrgType)).ToListAsync();
        }


    }
}