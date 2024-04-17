using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Utilities.API;

namespace EMS.Common.Data.DBContexts
{
    public class CommonContext : DbContext
    {
        public CommonContext(DbContextOptions<CommonContext> options) : base(options)
        {
        }


        #region Tables
        public DbSet<CompanyDatabase.vwCompanyDatabase> vwCompanyDatabase { get; set; }
        public DbSet<SystemUser.SystemUser> SystemUser { get; set; }

        #endregion Tables

    }

}