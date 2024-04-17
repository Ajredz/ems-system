using EMS.Manpower.Data.DBContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Manpower.Data.DataDuplication.SystemRole
{
    public interface ISystemRoleDBAccess
    {
        Task<IEnumerable<SystemRole>> GetAutoComplete(string Term, int TopResult, short CompanyID);

        Task<IEnumerable<SystemRole>> GetAll();

        Task<IEnumerable<SystemRole>> GetBySyncIDs(List<int> IDs);

        Task Sync(List<SystemRole> toDelete,
          List<SystemRole> toAdd,
          List<SystemRole> toUpdate);
    }

    public class SystemRoleDBAccess : ISystemRoleDBAccess
    {
        private readonly ManpowerContext _dbContext;

        public SystemRoleDBAccess(ManpowerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<SystemRole>> GetAutoComplete(string Term, int TopResult, short CompanyID)
        {
            return await _dbContext.SystemRole
                .FromSqlRaw("CALL sp_system_role_autocomplete({0},{1},{2})", Term ?? "", TopResult, CompanyID)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<SystemRole>> GetAll()
        {
            return await _dbContext.SystemRole.AsNoTracking().Where(x => x.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<SystemRole>> GetBySyncIDs(List<int> IDs)
        {
            return await _dbContext.SystemRole.AsNoTracking().Where(x => IDs.Contains(x.SyncID)).ToListAsync();
        }

        public async Task Sync(List<SystemRole> toDelete,
          List<SystemRole> toAdd,
          List<SystemRole> toUpdate)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            // Execute filtered records to their respective actions.
            _dbContext.SystemRole.RemoveRange(toDelete);
            _dbContext.SystemRole.AddRange(toAdd);
            toUpdate.Select(x =>
            {
                _dbContext.Entry(x).State = EntityState.Modified;
                return x;
            }).ToList();

            await _dbContext.SaveChangesAsync();
            transaction.Commit();
        }
    }
}