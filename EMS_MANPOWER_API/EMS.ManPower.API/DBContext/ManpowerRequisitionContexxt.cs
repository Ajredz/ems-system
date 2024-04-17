using Microsoft.EntityFrameworkCore;
using EMS_ManPowerServiceModel.MRF;
using EMS_ManPowerServiceModel.MRFSignatories;

namespace EMS_ManPowerService.DBContexts
{
    public class ManpowerRequisitionContext : DbContext
    {
        public ManpowerRequisitionContext(DbContextOptions<ManpowerRequisitionContext> options) : base(options)
        {
        }
        public DbSet<MRFSignatories> MRFSignatories { get; set; }
        public DbSet<MRFHeader> MRFHeader { get; set; }
        public DbSet<MRFApprover> MRFApprover { get; set; }
        public DbSet<MRFApplicant> MRFApplicant { get; set; }

    }
}
