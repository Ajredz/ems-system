using EMS.Common.Data.SystemUser;
using Microsoft.EntityFrameworkCore;

namespace EMS.Common.API.DBContexts
{ 
    public class SystemAccessContext : DbContext
    {
        public SystemAccessContext(DbContextOptions<SystemAccessContext> options) : base(options)
        {
        }

        public DbSet<SystemUser> SystemUser { get; set; }
    }
}
