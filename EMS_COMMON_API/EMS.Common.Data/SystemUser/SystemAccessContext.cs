using EMS.Common.Data.SystemUser;
using Microsoft.EntityFrameworkCore;

namespace EMS.Common.Data.DBContexts
{ 
    public class SystemAccessContext : DbContext
    {
        public SystemAccessContext(DbContextOptions<SystemAccessContext> options) : base(options)
        {
        }

        public DbSet<SystemUser.SystemUser> SystemUser { get; set; }
        public DbSet<SystemUser.tv_CurrentUser> tv_CurrentUser { get; set; }
        public DbSet<CompanyDatabase.vwCompanyDatabase> vwCompanyDatabase { get; set; }
    }
}
