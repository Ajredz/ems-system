using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Common.Data.DBContexts
{ 
    public class SystemAccessContext : DbContext
    {
        public SystemAccessContext(DbContextOptions<SystemAccessContext> options) : base(options)
        {
        }
        public DbSet<SystemUser.SystemUser> SystemUser { get; set; }
    }
}
