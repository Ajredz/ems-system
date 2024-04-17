using Microsoft.EntityFrameworkCore;

namespace EMS.Security.Data.DBContexts
{
    public class PortalGlobalContext : DbContext
    {
        public PortalGlobalContext(DbContextOptions<PortalGlobalContext> options) : base(options)
        {
        }
        public DbSet<_IntegrationModels.tbl_users> tbl_users { get; set; }
    }
}