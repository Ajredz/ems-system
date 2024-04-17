using EMS.Manpower.Data.DBContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Manpower.Data.DataDuplication.Region
{
    public interface IRegionDBAccess
    {
        Task<IEnumerable<Region>> GetAll();

        Task<bool> Sync(List<Region> toDelete,
           List<Region> toAdd,
           List<Region> toUpdate);

        Task<IEnumerable<Region>> GetBySyncIDs(List<int> IDs);

    }

    public class RegionDBAccess : IRegionDBAccess
    {
        private readonly ManpowerContext _dbContext;

        public RegionDBAccess(ManpowerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Region>> GetAll()
        {
            return await _dbContext.Region.AsNoTracking().ToListAsync();
        }

        public async Task<bool> Sync(List<Region> toDelete,
           List<Region> toAdd,
           List<Region> toUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Execute filtered records to their respective actions.
                _dbContext.Region.RemoveRange(toDelete);
                _dbContext.Region.AddRange(toAdd);
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

        public async Task<IEnumerable<Region>> GetBySyncIDs(List<int> IDs)
        {
            return await _dbContext.Region.AsNoTracking().Where(x => IDs.Contains(x.SyncID)).ToListAsync();
        }
    }
}