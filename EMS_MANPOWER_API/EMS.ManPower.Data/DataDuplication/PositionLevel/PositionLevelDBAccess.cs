using EMS.Manpower.Data.DBContexts;
using EMS.Manpower.Transfer.DataDuplication.PositionLevel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Manpower.Data.DataDuplication.PositionLevel
{
    public interface IPositionLevelDBAccess
    {
        Task<IEnumerable<PositionLevel>> GetAutoComplete(GetAutoCompleteInput param);

        Task<IEnumerable<PositionLevel>> GetAll();

        Task<bool> Sync(List<PositionLevel> toDelete,
           List<PositionLevel> toAdd,
           List<PositionLevel> toUpdate);

        Task<IEnumerable<PositionLevel>> GetBySyncIDs(List<int> IDs);

        Task<IEnumerable<PositionLevel>> GetByOrgGroupID(int OrgGroupID);

        Task<PositionLevel> GetByID(int ID);
    }

    public class PositionLevelDBAccess : IPositionLevelDBAccess
    {
        private readonly ManpowerContext _dbContext;

        public PositionLevelDBAccess(ManpowerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PositionLevel>> GetAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.PositionLevel
                .FromSqlRaw("CALL sp_position_level_autocomplete({0},{1})", param.Term ?? "", param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<PositionLevel>> GetAll()
        {
            return await _dbContext.PositionLevel.AsNoTracking().Where(x => x.IsActive).ToListAsync();
        }

        public async Task<bool> Sync(List<PositionLevel> toDelete,
           List<PositionLevel> toAdd,
           List<PositionLevel> toUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Execute filtered records to their respective actions.
                _dbContext.PositionLevel.RemoveRange(toDelete);
                _dbContext.PositionLevel.AddRange(toAdd);
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

        public async Task<IEnumerable<PositionLevel>> GetBySyncIDs(List<int> IDs)
        {
            return await _dbContext.PositionLevel.AsNoTracking().Where(x => IDs.Contains(x.SyncID)).ToListAsync();
        }

        public async Task<IEnumerable<PositionLevel>> GetByOrgGroupID(int OrgGroupID)
        {
            return await _dbContext.PositionLevel
                .FromSqlRaw("CALL sp_position_level_get_by_org_group({0})", OrgGroupID)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PositionLevel> GetByID(int ID)
        {
            return await _dbContext.PositionLevel.FindAsync(ID);
        }
    }
}