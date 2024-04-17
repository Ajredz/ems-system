using EMS.IPM.Data.DBContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.IPM.Data.DataDuplication.PSGCRegion
{
    public interface IPSGCRegionDBAccess
    {
        Task<IEnumerable<PSGCRegion>> GetBySyncIDs(List<int> IDs);

        Task<bool> Sync(List<PSGCRegion> toDelete,
            List<PSGCRegion> toAdd,
            List<PSGCRegion> toUpdate);

        Task<IEnumerable<PSGCRegion>> GetAll();
    }

    public class PSGCRegionDBAccess : IPSGCRegionDBAccess
    {
        private readonly IPMContext _dbContext;

        public PSGCRegionDBAccess(IPMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Sync(List<PSGCRegion> toDelete,
            List<PSGCRegion> toAdd,
            List<PSGCRegion> toUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Execute filtered records to their respective actions.
                _dbContext.PSGCRegion.RemoveRange(toDelete);
                _dbContext.PSGCRegion.AddRange(toAdd);
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

        public async Task<IEnumerable<PSGCRegion>> GetBySyncIDs(List<int> IDs)
        {
            return await _dbContext.PSGCRegion.AsNoTracking().Where(x => IDs.Contains(x.SyncID)).ToListAsync();
        }

        public async Task<IEnumerable<PSGCRegion>> GetAll()
        {
            return await _dbContext.PSGCRegion.AsNoTracking().ToListAsync();
        }
    }
}