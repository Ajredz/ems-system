using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.External.API.DBContext
{
    public class ExternalContext : DbContext
    {
        public ExternalContext(DbContextOptions<ExternalContext> options) : base(options)
        {
        }
        public DbSet<Shared.AuthenticationKey> AuthenticationKey { get; set; }
    }
}
