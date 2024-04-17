using Microsoft.EntityFrameworkCore;
using EMS_SecurityServiceModel.Reference;
using EMS_SecurityServiceModel.SystemPage;
using EMS_SecurityServiceModel.SystemRole;
using EMS_SecurityServiceModel.SystemRolePage;
using EMS_SecurityServiceModel.SystemURL;
using EMS_SecurityServiceModel.SystemUser;
using EMS_SecurityServiceModel.SystemUserRole;

namespace EMS_IPMService.DBContexts
{
    public class SystemAccessContext : DbContext
    {
        public SystemAccessContext(DbContextOptions<SystemAccessContext> options) : base(options)
        {
        }

        public DbSet<Reference> Reference { get; set; }
        public DbSet<ReferenceValue> ReferenceValue { get; set; }
        public DbSet<SystemPage> SystemPage { get; set; }
        public DbSet<SystemRole> SystemRole { get; set; }
        public DbSet<SystemRolePage> SystemRolePage { get; set; }
        public DbSet<SystemURL> SystemURL { get; set; }
        public DbSet<SystemUser> SystemUser { get; set; }
        public DbSet<SystemUserRole> SystemUserRole { get; set; }

        public DbSet<tv_CurrentUser> tt_CurrentUser { get; set; }

    }
}
