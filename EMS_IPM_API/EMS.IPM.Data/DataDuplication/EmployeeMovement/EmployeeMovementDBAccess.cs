using EMS.IPM.Data.DBContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.IPM.Data.DataDuplication.EmployeeMovement
{
    public interface IEmployeeMovementDBAccess
    {
        Task<IEnumerable<EmployeeMovement>> GetBySyncIDs(List<long> IDs);

        Task<bool> Sync(List<EmployeeMovement> toDelete,
            List<EmployeeMovement> toAdd,
            List<EmployeeMovement> toUpdate);

        Task<IEnumerable<EmployeeMovement>> GetAll();
    }

    public class EmployeeMovementDBAccess : IEmployeeMovementDBAccess
    {
        private readonly IPMContext _dbContext;

        public EmployeeMovementDBAccess(IPMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Sync(List<EmployeeMovement> toDelete,
            List<EmployeeMovement> toAdd,
            List<EmployeeMovement> toUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Execute filtered records to their respective actions.
                _dbContext.EmployeeMovement.RemoveRange(toDelete);
                _dbContext.EmployeeMovement.AddRange(toAdd);
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

        public async Task<IEnumerable<EmployeeMovement>> GetBySyncIDs(List<long> IDs)
        {
            return await _dbContext.EmployeeMovement.AsNoTracking().Where(x => IDs.Contains(x.SyncID)).ToListAsync();
        }

        public async Task<IEnumerable<EmployeeMovement>> GetAll()
        {
            return await _dbContext.EmployeeMovement.AsNoTracking().Where(x => x.IsActive).ToListAsync();
        }
    }
}