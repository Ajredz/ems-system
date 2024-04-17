using EMS.Manpower.Data.ApproverSetup;
using EMS.Manpower.Data.Dashboard;
using EMS.Manpower.Data.DataDuplication.OrgGroup;
using EMS.Manpower.Data.MRF;
using EMS.Manpower.Data.MRFSignatories;
using Microsoft.EntityFrameworkCore;

namespace EMS.Manpower.Data.DBContexts
{
    public class ManpowerContext : DbContext
    {
        public ManpowerContext(DbContextOptions<ManpowerContext> options) : base(options)
        {
        }

        #region Data Duplication

        public DbSet<DataDuplication.Region.Region> Region { get; set; }
        public DbSet<DataDuplication.SystemRole.SystemRole> SystemRole { get; set; }
        public DbSet<DataDuplication.Position.Position> Position { get; set; }
        public DbSet<DataDuplication.PositionLevel.PositionLevel> PositionLevel { get; set; }
        public DbSet<DataDuplication.OrgGroup.OrgGroup> OrgGroup { get; set; }
        public DbSet<DataDuplication.OrgGroup.OrgGroupPosition> OrgGroupPosition { get; set; }

        #endregion Data Duplication

        #region Tables

        public DbSet<Reference.Reference> Reference { get; set; }
        public DbSet<Reference.ReferenceValue> ReferenceValue { get; set; }
        public DbSet<MRFSignatories.MRFSignatories> MRFSignatories { get; set; }
        public DbSet<MRF.MRF> MRF { get; set; }
        public DbSet<MRF.MRFStatusHistory> MRFStatusHistory { get; set; }
        public DbSet<MRF.MRFKickoutQuestion> MRFKickoutQuestion { get; set; }
        public DbSet<MRF.MRFApplicant> MRFApplicant { get; set; }
        public DbSet<MRF.MRFComments> MRFComments { get; set; }
        public DbSet<MRF.MRFApplicantComments> MRFApplicantComments { get; set; }
        public DbSet<ApproverSetup.MRFDefinedApprover> MRFDefinedApprover { get; set; }

        public DbSet<Workflow.Workflow> Workflow { get; set; }
        public DbSet<Workflow.WorkflowStep> WorkflowStep { get; set; }
        public DbSet<Workflow.WorkflowStepApprover> WorkflowStepApprover { get; set; }
        public DbSet<Workflow.WorkflowTransaction> WorkflowTransaction { get; set; }
        public DbSet<SystemErrorLog.ErrorLog> ErrorLog { get; set; }
        public DbSet<MRF.KickoutQuestion> KickoutQuestion { get; set; }
        public DbSet<MRF.ApplicantKickoutQuestion> ApplicantKickoutQuestion { get; set; }

        #endregion Tables

        #region Table Variables

        public DbSet<TableVarMRFApprovalHistory> TableVarMRFApprovalHistory { get; set; }
        public DbSet<TableVarMRFTransID> TableVarMRFTransID { get; set; }
        public DbSet<TableVarMRFSignatoriesAdd> TableVarMRFSignatoriesAdd { get; set; }
        public DbSet<TableVarMRFSignatories> TableVarMRFSignatories { get; set; }
        public DbSet<TableVarMRF> TableVarMRF { get; set; }
        public DbSet<TableVarMRFOnline> TableVarMRFOnline { get; set; }
        public DbSet<TableVarMRFFormSignatories> TableVarMRFFormSignatories { get; set; }
        public DbSet<TableVarDescendants> TableVarMRFDashboardByAge { get; set; }
        public DbSet<TableVarOrgGroupDescendants> TableVarOrgGroupDescendants { get; set; }
        public DbSet<TableVarMRFExistingApplicant> TableVarMRFExistingApplicant { get; set; }
        public DbSet<TableVarMRFDefinedApprover> TableVarMRFDefinedApprover { get; set; }
        public DbSet<TableVarApproverSetupGet> TableVarApproverSetupGet { get; set; }
        public DbSet<SystemErrorLog.TableVarErrorLog> TableVarErrorLog { get; set; }

        #endregion Table Variables
    }
}