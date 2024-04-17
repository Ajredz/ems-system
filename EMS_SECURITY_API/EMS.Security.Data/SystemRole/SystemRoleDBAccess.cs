using EMS.Security.Data.DBContexts;
using EMS.Security.Transfer.SystemRole;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Security.Data.SystemRole
{
    public interface ISystemRoleDBAccess
    {
        Task<IEnumerable<SystemRole>> GetAutoComplete(string Term, int TopResult, short CompanyID);

        Task<IEnumerable<SystemRole>> GetAll();

        Task<IEnumerable<SystemUserRole>> GetByUserID(int UserID);

        Task<IEnumerable<SystemRole>> GetLastModified(DateTime? From, DateTime? To);

        Task<IEnumerable<TableVarSystemRole>> GetList(GetListInput input, int rowStart);

        Task<SystemRole> GetByID(int ID);

        Task<IEnumerable<SystemRole>> GetByRoleName(string RoleName);

        Task<IEnumerable<SystemUserRole>> GetByRoleID(int RoleID);

        Task<bool> Post(SystemRole systemRole, List<SystemRolePage> systemRoleList);

        Task<bool> Put(SystemRole param);

        Task<bool> Put(SystemRole systemRole, List<SystemRolePage> systemRolePageToAdd, List<SystemRolePage> systemRolePageToDelete);

        Task<IEnumerable<TableVarSystemRoleAccess>> GetAccess(int ID);

        Task<IEnumerable<SystemRolePage>> GetPage(int ID);
    }

    public class SystemRoleDBAccess : ISystemRoleDBAccess
    {
        private readonly SystemAccessContext _dbContext;

        public SystemRoleDBAccess(SystemAccessContext dbContext)
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
            return await _dbContext.SystemRole.AsNoTracking()
                .Where(x => x.IsActive).ToListAsync();
        }
        
        public async Task<IEnumerable<SystemUserRole>> GetByUserID(int UserID)
        {
            return await _dbContext.SystemUserRole.AsNoTracking()
                .Where(x => x.UserID == UserID).ToListAsync();
        }

        public async Task<IEnumerable<SystemRole>> GetLastModified(DateTime? From, DateTime? To)
        {
            return await _dbContext.SystemRole.AsNoTracking().Where(x =>
                    (x.ModifiedDate ?? x.CreatedDate) >= (From ?? (x.ModifiedDate ?? x.CreatedDate))
                    &
                    (x.ModifiedDate ?? x.CreatedDate) <= (To ?? (x.ModifiedDate ?? x.CreatedDate))
            ).ToListAsync();
        }

        public async Task<IEnumerable<TableVarSystemRole>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarSystemRole
                .FromSqlRaw(@"CALL sp_system_role_get_list(
                                 {0}
                               , {1}
                               , {2}
                               , {3}
                               , {4}
                               , {5}
                               , {6}
                               , {7}
                               , {8}
                            )", input.ID ?? 0
                               , input.RoleName ?? ""
                               , input.DateCreatedFrom ?? ""
                               , input.DateCreatedTo ?? ""
                               , input.IsExport
                               , input.sidx ?? ""
                               , input.sord ?? ""
                               , rowStart
                               , input.rows)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SystemRole> GetByID(int ID)
        {
            return await _dbContext.SystemRole.FindAsync(ID);
        }

        public async Task<IEnumerable<SystemRole>> GetByRoleName(string RoleName)
        {
            return await _dbContext.SystemRole.AsNoTracking()
                .Where(x => x.RoleName.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase) & x.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<SystemUserRole>> GetByRoleID(int RoleID)
        {
            return await _dbContext.SystemUserRole.AsNoTracking()
                .Where(x => x.RoleID == RoleID).ToListAsync();
        }

        public async Task<bool> Post(SystemRole systemRole, List<SystemRolePage> systemRoleList)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.SystemRole.AddAsync(systemRole);
                await _dbContext.SaveChangesAsync();

                if (systemRoleList != null)
                {
                    await _dbContext.SystemRolePage.AddRangeAsync(systemRoleList.Select(x => { x.RoleID = systemRole.ID; return x; }));
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Put(SystemRole param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Put(SystemRole systemRole, List<SystemRolePage> systemRolePageToAdd, List<SystemRolePage> systemRolePageToDelete)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(systemRole).State = EntityState.Modified;

                _dbContext.SystemRolePage.AddRange(systemRolePageToAdd);
                _dbContext.SystemRolePage.RemoveRange(systemRolePageToDelete);

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<IEnumerable<TableVarSystemRoleAccess>> GetAccess(int ID)
        {
            return await _dbContext.TableVarSystemRoleAccess
                .FromSqlRaw("CALL sp_system_role_access_get({0})", ID)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<SystemRolePage>> GetPage(int ID)
        {
            return await _dbContext.SystemRolePage.AsNoTracking()
                .Where(x => x.RoleID == ID).ToListAsync();
        }
    }
}