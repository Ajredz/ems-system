using EMS.Recruitment.Data.DBContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Recruitment.Data.DataDuplication.PositionLevel
{
    public interface IPositionLevelDBAccess
    {
        Task<bool> Sync(List<PositionLevel> toDelete,
           List<PositionLevel> toAdd,
           List<PositionLevel> toUpdate);

        Task<IEnumerable<PositionLevel>> GetBySyncIDs(List<int> IDs);
    }

    public class PositionLevelDBAccess : IPositionLevelDBAccess
    {
        private readonly RecruitmentContext _dbContext;

        public PositionLevelDBAccess(RecruitmentContext dbContext)
        {
            _dbContext = dbContext;
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
    }
}