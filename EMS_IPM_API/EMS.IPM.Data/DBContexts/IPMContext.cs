using EMS.IPM.Data.Shared;
using Microsoft.EntityFrameworkCore;

namespace EMS.IPM.Data.DBContexts
{
    public class IPMContext : DbContext
    {
        public IPMContext(DbContextOptions<IPMContext> options) : base(options)
        {
        }

        #region Data Duplication

        public DbSet<DataDuplication.Position.Position> Position { get; set; }
        public DbSet<DataDuplication.Employee.Employee> Employee { get; set; }
        public DbSet<DataDuplication.Employee.EmployeeRoving> EmployeeRoving { get; set; }
        public DbSet<DataDuplication.EmployeeMovement.EmployeeMovement> EmployeeMovement { get; set; }
        public DbSet<DataDuplication.OrgGroup.OrgGroup> OrgGroup { get; set; }
        public DbSet<DataDuplication.OrgGroup.OrgGroupPosition> OrgGroupPosition { get; set; }
        //public DbSet<DataDuplication.PSGCCity.PSGCCity> PSGCCity { get; set; }
        //public DbSet<DataDuplication.PSGCRegion.PSGCRegion> PSGCRegion { get; set; }
        public DbSet<DataDuplication.OrgGroup.TableVarOrgGroupDescendants> TableVarOrgGroupDescendants { get; set; }
        #endregion Data Duplication

        #region Tables
        public DbSet<KRAGroup.KRAGroup> KRAGroup { get; set; }
        public DbSet<KRASubGroup.KRASubGroup> KRASubGroup { get; set; }
        public DbSet<KPI.KPI> KPI { get; set; }
        public DbSet<KPIPosition.KPIPosition> KPIPosition { get; set; }
        public DbSet<KPIScore.KPIScore> KPIScore { get; set; }
        public DbSet<KPIScore.KPIScorePerEmployee> KPIScorePerEmployee { get; set; }
        public DbSet<EmployeeScore.EmployeeScore> EmployeeScore { get; set; }
        public DbSet<EmployeeScore.TransEmployeeFinalScore> TransEmployeeFinalScore { get; set; }
        public DbSet<EmployeeScore.TransEmployeeScore> TransEmployeeScore { get; set; }
        public DbSet<EmployeeScore.TransEmployeeScoreStaging> TransEmployeeScoreStaging { get; set; }
        public DbSet<EmployeeScore.TransEmployeeScoreStagingTest> TransEmployeeScoreStagingTest { get; set; }
        public DbSet<EmployeeScore.TransEmployeeScoreDetails> TransEmployeeScoreDetails { get; set; }
        public DbSet<EmployeeScore.TransEmployeeScoreSummary> TransEmployeeScoreSummary { get; set; }
        public DbSet<EmployeeScore.StagingTransEmployeeScore> StagingTransEmployeeScore { get; set; }
        public DbSet<Reference.Reference> Reference { get; set; }
        public DbSet<Reference.ReferenceValue> ReferenceValue { get; set; }
        public DbSet<SystemErrorLog.ErrorLog> ErrorLog { get; set; }

        #endregion Tables

        #region Table Variables

        public DbSet<KPI.TableVarKPI> TableVarKPI { get; set; }
        public DbSet<KPI.TableVarKPIDetails> TableVarKPIDetails { get; set; }
        public DbSet<KPI.TableVarNewKPICode> TableVarNewKPICode { get; set; }
        public DbSet<KPIPosition.TableVarKPIPosition> TableVarKPIPosition { get; set; }
        public DbSet<KPIPosition.TableVarKPIPositionDetails> TableVarKPIPositionDetails { get; set; }
        public DbSet<KPIPosition.TableVarKPIPositionExport> TableVarKPIPositionExport { get; set; }
        public DbSet<KPIScore.TableVarKPIScore> TableVarKPIScore { get; set; }
        public DbSet<KPIScore.TableVarKPIScoreGetByID> TableVarKPIScoreGetByID { get; set; }
        public DbSet<EmployeeScore.TableVarEmployeeFinalScore> TableVarEmployeeFinalScore { get; set; }
        public DbSet<EmployeeScore.TableVarEmployeeScore> TableVarEmployeeScore { get; set; }
        public DbSet<EmployeeScore.TableVarEmployeeScoreOnly> TableVarEmployeeScoreOnly { get; set; }
        public DbSet<EmployeeScore.TableVarEmployeeScoreGetByID> TableVarEmployeeScoreGetByID { get; set; }
        public DbSet<EmployeeScore.TableVarRunEmployeeScore> TableVarRunEmployeeScore { get; set; }
        public DbSet<EmployeeScore.TableVarAverageScore> TableVarAverageScore { get; set; }
        public DbSet<EmployeeScore.TableVarEmployeeScoreCountByPosition> TableVarEmployeeScoreCountByPosition { get; set; }
        public DbSet<EmployeeScore.TableVarEmployeeScoreCountByBranch> TableVarEmployeeScoreCountByBranch { get; set; }
        public DbSet<EmployeeScore.TableVarEmployeeScoreCountByRegion> TableVarEmployeeScoreCountByRegion { get; set; }
        public DbSet<EmployeeScore.TableVarEmployeeScoreApproval> TableVarEmployeeScoreApproval { get; set; }
        public DbSet<TableVariableAutoComplete> TableVariableAutoComplete { get; set; }
        public DbSet<EmployeeScore.TableVarTransEmployeeScoreSummary> TableVarTransEmployeeScoreSummary { get; set; }
        public DbSet<EmployeeScore.TableVarTransEmployeeScoreSummaryID> TableVarTransEmployeeScoreSummaryID { get; set; }
        public DbSet<MyKPIScores.TableVarMyKPIScores> TableVarMyKPIScores { get; set; }
        public DbSet<MyKPIScores.TableVarMyKPIScoresGetByID> TableVarMyKPIScoresGetByID { get; set; }
        public DbSet<EmployeeScoreDashboard.TableVarDashboardSummaryForEvaluation> TableVarDashboardSummaryForEvaluation { get; set; }
        public DbSet<EmployeeScoreDashboard.TableVarDashboardSummaryForApproval> TableVarDashboardSummaryForApproval { get; set; }
        public DbSet<EmployeeScoreDashboard.TableVarDashboardSummaryForApprovalBRN> TableVarDashboardSummaryForApprovalBRN { get; set; }
        public DbSet<EmployeeScoreDashboard.TableVarDashboardSummaryForApprovalCLU> TableVarDashboardSummaryForApprovalCLU { get; set; }
        public DbSet<EmployeeScoreDashboard.TableVarDashboardRegionalWithPosition> TableVarDashboardRegionalWithPosition { get; set; }
        public DbSet<EmployeeScoreDashboard.TableVarDashboardBranchesWithPosition> TableVarDashboardBranchesWithPosition { get; set; }
        public DbSet<EmployeeScoreDashboard.TableVarDashboardPositionOnly> TableVarDashboardPositionOnly { get; set; }
        public DbSet<EmployeeScore.TableVarEmployeeKPIScore> TableVarEmployeeKPIScore { get; set; }
        public DbSet<SystemErrorLog.TableVarErrorLog> TableVarErrorLog { get; set; }
        public DbSet<RatingTable.RatingTable> RatingTable { get; set; }
        public DbSet<EmployeeScore.TableVarResult> TableVarResult { get; set; }

        #endregion Table Variables

        #region Views
        #endregion

    }
}