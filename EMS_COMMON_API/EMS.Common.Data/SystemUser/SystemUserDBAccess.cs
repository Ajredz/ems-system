using EMS.Common.Data.DBContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Common.Data.SystemUser
{
    public interface ISystemUserDBAccess
    {
        Task<IEnumerable<SystemUser>> GetSystemUserByUsername(string username, int param);
        Task<IEnumerable<tv_CurrentUser>> GetSystemUser(string username, string password);
    }

    public class SystemUserDBAccess : ISystemUserDBAccess
    {
        private readonly SystemAccessContext _dbContext;

        public SystemUserDBAccess(SystemAccessContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<SystemUser>> GetSystemUserByUsername(string username, int param)
        {

            List<SystemUser> result = new List<SystemUser>();
            try 
            { 
                result = await _dbContext.SystemUser.AsNoTracking().Where(x => x.Username == username).ToListAsync();

            } 
            catch (Exception ex) 
            { 
            
            }
            
            return result;
        }

        public async Task<IEnumerable<tv_CurrentUser>> GetSystemUser(string username, string password)
        {

            List<tv_CurrentUser> result = new List<tv_CurrentUser>();
            try
            {
                result = await _dbContext.tv_CurrentUser
                    .FromSqlRaw("CALL sp_login_authentication({0},{1})", username, password)
                    .ToListAsync();

            }
            catch (Exception ex)
            {

            }

            return result;
        }


    }
}
