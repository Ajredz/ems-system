using EMS.Recruitment.Data.DBContexts;
using EMS.Recruitment.Transfer.DataDuplication.SystemUser;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Recruitment.Data.DataDuplication.SystemUser
{
    public interface ISystemUserDBAccess
    {
        Task<IEnumerable<TableVarSystemUserName>> GetAutoComplete(GetAutoCompleteInput param);

        Task<SystemUser> GetBySyncID(int ID);

        Task<bool> Sync(List<SystemUser> toDelete,
          List<SystemUser> toAdd,
          List<SystemUser> toUpdate);

        Task<IEnumerable<SystemUser>> GetBySyncIDs(List<int> IDs);
    }

    public class SystemUserDBAccess : ISystemUserDBAccess
    {
        private readonly RecruitmentContext _dbContext;

        public SystemUserDBAccess(RecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarSystemUserName>> GetAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.TableVarSystemUserName
                .FromSqlRaw("CALL sp_system_user_name_autocomplete({0},{1})", (param.Term ?? ""), param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SystemUser> GetBySyncID(int ID)
        {
            return await _dbContext.SystemUser.Where(x => x.SyncID == ID).FirstOrDefaultAsync();
        }

        public async Task<bool> Sync(List<SystemUser> toDelete,
          List<SystemUser> toAdd,
          List<SystemUser> toUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Execute filtered records to their respective actions.
                _dbContext.SystemUser.RemoveRange(toDelete);
                _dbContext.SystemUser.AddRange(toAdd);
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

        public async Task<IEnumerable<SystemUser>> GetBySyncIDs(List<int> IDs)
        {
            return await _dbContext.SystemUser.AsNoTracking().Where(x => IDs.Contains(x.SyncID)).ToListAsync();
        }
    }
}