using Microsoft.EntityFrameworkCore;

namespace EMS.Security.Data.DBContexts
{
    public class SystemAccessContext : DbContext
    {
        public SystemAccessContext(DbContextOptions<SystemAccessContext> options) : base(options)
        {
        }
        public DbSet<AuditLog.AuditLog> AuditLog { get; set; }
        public DbSet<SystemUser.SystemUser> SystemUser { get; set; }
        public DbSet<SystemRole.SystemRole> SystemRole { get; set; }
        public DbSet<SystemRole.SystemRolePage> SystemRolePage { get; set; }
        public DbSet<SystemRole.SystemUserRole> SystemUserRole { get; set; }
        public DbSet<Workflow.Workflow> Workflow { get; set; }
        public DbSet<Workflow.WorkflowStep> WorkflowStep { get; set; }
        public DbSet<Workflow.WorkflowStepApprover> WorkflowStepApprover { get; set; }
        public DbSet<Workflow.WorkflowTransaction> WorkflowTransaction { get; set; }
        public DbSet<SystemErrorLog.ErrorLog> ErrorLog { get; set; }

        #region Table Variables
        public DbSet<SystemUser.TableVarSystemUser> TableVarSystemUser { get; set; }
        public DbSet<SystemRole.TableVarSystemRole> TableVarSystemRole { get; set; }
        public DbSet<AuditLog.TableVarAuditLogs> TableVarAuditLogs { get; set; }
        public DbSet<AuditLog.TableVarEventType> TableVarEventType { get; set; }
        public DbSet<AuditLog.TableVarTableName> TableVarTableName { get; set; }
        public DbSet<SystemUser.tv_CurrentUser> tv_CurrentUser { get; set; }
        public DbSet<SystemRole.TableVarSystemRoleAccess> TableVarSystemRoleAccess { get; set; }
        public DbSet<SystemErrorLog.TableVarErrorLog> TableVarErrorLog { get; set; }
        #endregion
    }
}