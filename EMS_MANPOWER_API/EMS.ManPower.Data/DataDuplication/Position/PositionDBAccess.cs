using EMS.Manpower.Data.DBContexts;
using EMS.Manpower.Transfer.DataDuplication.Position;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Manpower.Data.DataDuplication.Position
{
    public interface IPositionDBAccess
    {
        Task<IEnumerable<Position>> GetAutoComplete(GetAutoCompleteInput param);

        Task<Position> GetByID(int ID);

        Task<IEnumerable<Position>> GetByPositionLevel(int PositionLevelID);

        Task<IEnumerable<Position>> GetAll();

        Task<IEnumerable<Position>> GetBySyncIDs(List<int> IDs);

        Task<bool> Sync(List<Position> toDelete,
            List<Position> toAdd,
            List<Position> toUpdate);

        Task<IEnumerable<Position>> GetByPositionLevelAndSyncIDs(int PositionLevelID, List<int> IDs);

        Task<IEnumerable<Position>> GetByParentPositionID(int ParentPositionID);
    }

    public class PositionDBAccess : IPositionDBAccess
    {
        private readonly ManpowerContext _dbContext;

        public PositionDBAccess(ManpowerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Position>> GetAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.Position
                .FromSqlRaw("CALL sp_position_autocomplete({0},{1})", param.Term ?? "", param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Position> GetByID(int ID)
        {
            return await _dbContext.Position.Where(x => x.SyncID == ID & x.IsActive).FirstAsync();
        }

        public async Task<IEnumerable<Position>> GetByPositionLevel(int PositionLevelID)
        {
            return await _dbContext.Position.AsNoTracking()
                .Where(x => x.PositionLevelID == PositionLevelID & x.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Position>> GetAll()
        {
            return await _dbContext.Position.AsNoTracking().Where(x => x.IsActive).ToListAsync();
        }

        public async Task<bool> Sync(List<Position> toDelete,
            List<Position> toAdd,
            List<Position> toUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Execute filtered records to their respective actions.
                _dbContext.Position.RemoveRange(toDelete);
                _dbContext.Position.AddRange(toAdd);
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

        public async Task<IEnumerable<Position>> GetBySyncIDs(List<int> IDs)
        {
            return await _dbContext.Position.AsNoTracking().Where(x => IDs.Contains(x.SyncID)).ToListAsync();
        }

        public async Task<IEnumerable<Position>> GetByPositionLevelAndSyncIDs(int PositionLevelID, List<int> IDs)
        {
            return await _dbContext.Position.AsNoTracking()
                .Where(x => x.PositionLevelID == PositionLevelID && IDs.Contains(x.SyncID)).ToListAsync();
        }

        public async Task<IEnumerable<Position>> GetByParentPositionID(int ParentPositionID)
        {
            return await _dbContext.Position.AsNoTracking()
                .Where(x => x.ParentPositionID == ParentPositionID & x.IsActive).ToListAsync();
        }

    }
}