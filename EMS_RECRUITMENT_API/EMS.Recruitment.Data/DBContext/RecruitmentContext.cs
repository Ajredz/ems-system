using EMS.Manpower.Data.Applicant;
using EMS.Recruitment.Data.Applicant;
using EMS.Recruitment.Data.ApplicantDashboard;
using EMS.Recruitment.Data.ApplicantTagging;
using EMS.Recruitment.Data.DataDuplication.SystemUser;
using Microsoft.EntityFrameworkCore;

namespace EMS.Recruitment.Data.DBContexts
{
    public class RecruitmentContext : DbContext
    {
        public RecruitmentContext(DbContextOptions<RecruitmentContext> options) : base(options)
        {
        }

        #region Data Duplication

        public DbSet<DataDuplication.Position.Position> Position { get; set; }
        public DbSet<DataDuplication.PositionLevel.PositionLevel> PositionLevel { get; set; }
        public DbSet<DataDuplication.OrgGroup.OrgGroup> OrgGroup { get; set; }
        public DbSet<DataDuplication.SystemUser.SystemUser> SystemUser { get; set; }

        #endregion Data Duplication

        #region Tables
        public DbSet<Applicant.Applicant> Applicant { get; set; }
        public DbSet<Applicant.ApplicantHistory> ApplicantHistory { get; set; }
        public DbSet<Applicant.ApplicantLegalProfile> ApplicantLegalProfile { get; set; }
        public DbSet<Applicant.ApplicantAttachment> ApplicantAttachment { get; set; }
        public DbSet<Reference.Reference> Reference { get; set; }
        public DbSet<Reference.ReferenceValue> ReferenceValue { get; set; }
        public DbSet<Workflow.Workflow> Workflow { get; set; }
        public DbSet<Workflow.WorkflowStep> WorkflowStep { get; set; }
        public DbSet<Workflow.WorkflowStepApprover> WorkflowStepApprover { get; set; }
        public DbSet<RecruiterTask.RecruiterTask> RecruiterTask { get; set; }
        public DbSet<PSGC.PSGCRegion> PSGCRegion { get; set; }
        public DbSet<PSGC.PSGCProvince> PSGCProvince { get; set; }
        public DbSet<PSGC.PSGCCityMunicipality> PSGCCityMunicipality { get; set; }
        public DbSet<PSGC.PSGCBarangay> PSGCBarangay { get; set; }
        public DbSet<SystemErrorLog.ErrorLog> ErrorLog { get; set; }

        #endregion Tables

        #region Table Variables
        public DbSet<Workflow.TableVarWorkflow> TableVarWorkflow { get; set; }
        public DbSet<TableVarApplicant> TableVarApplicant { get; set; }
        public DbSet<TableVarApplicantLegalProfile> TableVarApplicantLegalProfile { get; set; }
        public DbSet<TableVarApplicationApproval> TableVarApplicationApproval { get; set; }
        public DbSet<TableVarApplicantName> TableVarApplicantName { get; set; }
        public DbSet<RecruiterTask.TableVarRecruiterTask> TableVarRecruiterTask { get; set; }
        public DbSet<RecruiterTask.TableVarPendingTask> TableVarPendingTask { get; set; }
        public DbSet<TableVarSystemUserName> TableVarSystemUserName { get; set; }
        public DbSet<TableVarApplicantHistory> TableVarApplicantHistory { get; set; }
        public DbSet<TableVarMRFExistingApplicant> TableVarMRFExistingApplicant { get; set; }
        public DbSet<TableVarApplicantTagging> TableVarApplicantTagging { get; set; }
        public DbSet<TableVarApplicantCountByOrgGroup> TableVarApplicantCountByOrgGroup { get; set; }
        public DbSet<SystemErrorLog.TableVarErrorLog> TableVarErrorLog { get; set; }
        #endregion Table Variables
    }
}