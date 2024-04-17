using EMS.Recruitment.Data.DBContexts;
using EMS.Recruitment.Transfer.DataDuplication.Position;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Recruitment.Data.DataDuplication.Position
{
    public interface IPositionDBAccess
    {
        Task<IEnumerable<Position>> GetAutoComplete(GetAutoCompleteInput param);

        Task<IEnumerable<Position>> GetBySyncIDs(List<int> IDs);

        Task<bool> Sync(List<Position> toDelete,
            List<Position> toAdd,
            List<Position> toUpdate);

        Task<IEnumerable<Position>> GetAll();
    }

    public class PositionDBAccess : IPositionDBAccess
    {
        private readonly RecruitmentContext _dbContext;

        public PositionDBAccess(RecruitmentContext dbContext)
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

        public async Task<IEnumerable<Position>> GetAll()
        {
            return await _dbContext.Position.AsNoTracking().Where(x => x.IsActive).ToListAsync();
        }
    }
}