using EMS.IPM.Data.DBContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.IPM.Data.DataDuplication.PSGCCity
{
    public interface IPSGCCityDBAccess
    {
        Task<IEnumerable<PSGCCity>> GetBySyncIDs(List<int> IDs);

        Task<bool> Sync(List<PSGCCity> toDelete,
            List<PSGCCity> toAdd,
            List<PSGCCity> toUpdate);

        Task<IEnumerable<PSGCCity>> GetAll();
    }

    public class PSGCCityDBAccess : IPSGCCityDBAccess
    {
        private readonly IPMContext _dbContext;

        public PSGCCityDBAccess(IPMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Sync(List<PSGCCity> toDelete,
            List<PSGCCity> toAdd,
            List<PSGCCity> toUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Execute filtered records to their respective actions.
                _dbContext.PSGCCity.RemoveRange(toDelete);
                _dbContext.PSGCCity.AddRange(toAdd);
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

        public async Task<IEnumerable<PSGCCity>> GetBySyncIDs(List<int> IDs)
        {
            return await _dbContext.PSGCCity.AsNoTracking().Where(x => IDs.Contains(x.SyncID)).ToListAsync();
        }

        public async Task<IEnumerable<PSGCCity>> GetAll()
        {
            return await _dbContext.PSGCCity.AsNoTracking().ToListAsync();
        }
    }
}