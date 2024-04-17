using EMS.Security.Data.DBContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Security.Data._IntegrationModels
{
    public interface Itbl_usersDBAccess
    {
        Task<IEnumerable<tbl_users>> Get();
    }

    public class tbl_usersDBAccess : Itbl_usersDBAccess
    {
        private readonly PortalGlobalContext _dbContext;

        public tbl_usersDBAccess(PortalGlobalContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<tbl_users>> Get()
        {
            return await _dbContext.tbl_users
                .FromSqlRaw("CALL sp_ems_integrate_users()")
                .AsNoTracking()
                .ToListAsync();
        }

       

    }
}