using EMS.Security.Data.DBContexts;
using EMS.Security.Data.SystemRole;
using EMS.Security.Transfer.SystemUser;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Security.Data.SystemUser
{
    public interface ISystemUserDBAccess
    {
        Task<IEnumerable<SystemUser>> GetAutoComplete(GetAutoCompleteInput param);

        Task<SystemUser> GetByID(int ID);

        Task<IEnumerable<SystemUser>> GetLastModified(DateTime? From, DateTime? To);

        Task<IEnumerable<SystemUser>> GetByIDs(List<int> IDs);

        Task<IEnumerable<SystemUser>> AddSystemUser(GetByNameInput param);
        Task<bool> AddSystemUsers(List<SystemUser> param);

        Task<IEnumerable<SystemUser>> GetSystemUserByID(int ID);

        Task<IEnumerable<SystemUser>> GetAllSystemUser();

        Task<IEnumerable<SystemUser>> GetAllUsers();

        Task<IEnumerable<SystemUser>> GetDefaultPassword();

        Task<bool> Sync(List<SystemUser> toDelete,
          List<SystemUser> toAdd,
          List<SystemUser> toUpdate);

        Task<bool> ChangePassword(SystemUser param);

        Task<tv_CurrentUser> GetSystemUserByUsernamePassword(string Username, string Password);

        Task<IEnumerable<SystemUser>> EncryptPassword(string Password);

        Task<IEnumerable<TableVarSystemUser>> GetList(GetListInput input, int rowStart);

        Task<IEnumerable<SystemUserRole>> GetRoles(int ID);

        Task<bool> UpdateSystemUserRole(SystemUser systemUser
            , List<SystemUserRole> toAdd
            , List<SystemUserRole> toDelete
            );

        Task<bool> AddSystemUserRole(SystemUser systemUser, List<SystemUserRole> toAdd);
        Task<bool> AddSystemUserRoles(List<SystemUserRole> param);

        Task<SystemUser> GetByUsername(string Username);

        Task<IEnumerable<SystemUserRole>> GetUserRoleByUserID(int UserID);

        Task<bool> Put(List<SystemUser> systemUsers);

        Task<bool> Put(SystemUser systemUser);

        Task<IEnumerable<SystemUser>> GetBySyncIDs(List<string> IDs);
        Task<IEnumerable<SystemUserRole>> GetUserByRoleIDs(List<int> ID);
        Task<IEnumerable<SystemUser>> GetSystemUserByUsername(List<string> Username);
    }

    public class SystemUserDBAccess : ISystemUserDBAccess
    {
        private readonly SystemAccessContext _dbContext;

        public SystemUserDBAccess(SystemAccessContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SystemUser> GetByID(int ID)
        {
            return await _dbContext.SystemUser.AsNoTracking().Where(x => x.ID == ID).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SystemUser>> GetAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.SystemUser
                .FromSqlRaw("CALL sp_system_user_autocomplete({0},{1},{2})"
                , param.Term ?? ""
                , param.TopResults
                , param.CompanyID)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<SystemUser>> GetLastModified(DateTime? From, DateTime? To)
        {
            return await _dbContext.SystemUser.AsNoTracking().Where(x =>
                    (x.ModifiedDate ?? x.CreatedDate) >= (From ?? (x.ModifiedDate ?? x.CreatedDate))
                    &
                    (x.ModifiedDate ?? x.CreatedDate) <= (To ?? (x.ModifiedDate ?? x.CreatedDate))
            ).ToListAsync();
        }

        public async Task<IEnumerable<SystemUser>> GetByIDs(List<int> IDs)
        {
            return await _dbContext.SystemUser.AsNoTracking().Where(x => IDs.Contains(x.ID)).ToListAsync();
        }

        public async Task<IEnumerable<SystemUser>> AddSystemUser(GetByNameInput param)
        {
            List<Data.SystemUser.SystemUser> systemUser = new List<Data.SystemUser.SystemUser>();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                systemUser = await _dbContext.SystemUser
                .FromSqlRaw("CALL sp_system_user_add({0},{1},{2},{3},{4})"
                    , param.EmployeeCode ?? ""
                    , param.FirstName ?? ""
                    , param.MiddleName ?? ""
                    , param.LastName ?? ""
                    , param.CreatedBy
                )
                .AsNoTracking().ToListAsync();

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return systemUser;
        }
        public async Task<bool> AddSystemUsers(List<SystemUser> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.SystemUser.AddRangeAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<IEnumerable<SystemUser>> GetSystemUserByID(int ID)
        {
            return await _dbContext.SystemUser.AsNoTracking()
                .Where(x => x.ID == ID & x.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<SystemUser>> GetAllSystemUser()
        {
            return await _dbContext.SystemUser.AsNoTracking().Where(x => x.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<SystemUser>> GetAllUsers()
        {
            return await _dbContext.SystemUser.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<SystemUser>> GetDefaultPassword()
        {
            return await _dbContext.SystemUser
                .FromSqlRaw("CALL sp_get_default_password()")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> Sync(List<SystemUser> toDelete,
          List<SystemUser> toAdd,
          List<SystemUser> toUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Execute filtered records to their respective actions.
                _dbContext.SystemUser.RemoveRange(toDelete);

                await _dbContext.SystemUser.AddRangeAsync(toAdd);
                await _dbContext.SaveChangesAsync();

                if (toAdd.Count > 0)
                {
                    await _dbContext.SystemUser
                                   .FromSqlRaw("CALL sp_default_users({0},{1})"
                                       , string.Join(",", toAdd.Select(x => x.ID)) ?? ""
                                       , toAdd.First().CreatedBy
                                   )
                                   .AsNoTracking().ToListAsync(); 
                }

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


        public async Task<bool> ChangePassword(SystemUser param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<tv_CurrentUser> GetSystemUserByUsernamePassword(string Username, string Password)
        {
            return (await _dbContext.tv_CurrentUser
                .FromSqlRaw("CALL sp_login_authentication({0},{1})", Username, Password)
                .ToListAsync()).FirstOrDefault();
        }

        public async Task<IEnumerable<SystemUser>> EncryptPassword(string Password)
        {
            return await _dbContext.SystemUser
                .FromSqlRaw("CALL sp_encrypt_password({0})", Password)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVarSystemUser>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarSystemUser
                .FromSqlRaw(@"CALL sp_system_user_get_list(
                                 {0}
                               , {1}
                               , {2}
                               , {3}
                               , {4}
                               , {5}
                               , {6}
                               , {7}
                               , {8}
                               , {9}
                               , {10}
                               , {11}
                            )" , input.Username ?? ""
                               , input.Name ?? ""
                               , input.Status ?? ""
                               , input.DateModifiedFrom ?? ""
                               , input.DateModifiedTo ?? ""
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

        public async Task<IEnumerable<SystemUserRole>> GetRoles(int ID)
        {
            return await _dbContext.SystemUserRole.AsNoTracking().Where(x => x.UserID == ID).ToListAsync();
        }

        public async Task<bool> UpdateSystemUserRole(SystemUser systemUser
            , List<SystemUserRole> toAdd
            , List<SystemUserRole> toDelete
            )
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Execute filtered records to their respective actions.
                if (toDelete != null)
                {
                    _dbContext.SystemUserRole.RemoveRange(toDelete);
                }

                if (toAdd != null)
                {
                    await _dbContext.SystemUserRole.AddRangeAsync(toAdd);
                }

                _dbContext.Entry(systemUser).State = EntityState.Modified;

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<bool> AddSystemUserRole(SystemUser systemUser, List<SystemUserRole> toAdd)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.SystemUser.AddAsync(systemUser);
                await _dbContext.SaveChangesAsync();

                if (toAdd != null)
                {
                    await _dbContext.SystemUserRole.AddRangeAsync(toAdd.Select(x => { x.UserID = systemUser.ID; return x; }));
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }
        public async Task<bool> AddSystemUserRoles(List<SystemUserRole> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.SystemUserRole.AddRangeAsync(param);
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }
            return true;
        }

        public async Task<SystemUser> GetByUsername(string Username)
        {
            return await _dbContext.SystemUser.AsNoTracking().Where(x => x.Username.Equals(Username)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SystemUserRole>> GetUserRoleByUserID(int UserID)
        {
            return await _dbContext.SystemUserRole.AsNoTracking()
                .Where(x => x.UserID == UserID).ToListAsync();
        }

        public async Task<bool> Put(List<SystemUser> systemUsers)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                systemUsers.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<bool> Put(SystemUser systemUser)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                _dbContext.Entry(systemUser).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<IEnumerable<SystemUser>> GetBySyncIDs(List<string> IDs)
        {
            return await _dbContext.SystemUser.AsNoTracking().Where(x => IDs.Contains(x.IntegrationKey)).ToListAsync();
        }
        public async Task<IEnumerable<SystemUserRole>> GetUserByRoleIDs(List<int> ID)
        {
            return await _dbContext.SystemUserRole.AsNoTracking().Where(x => ID.Contains(x.RoleID)).ToListAsync();
        }
        public async Task<IEnumerable<SystemUser>> GetSystemUserByUsername(List<string> Username)
        {
            return await _dbContext.SystemUser.AsNoTracking().Where(x => Username.Contains(x.Username)).ToListAsync();
        }
    }
}