using EMS.Plantilla.Data.DBObjects.NPRF;
using EMS.Plantilla.Data.DBObjects.NPRFSignatories;
using Microsoft.EntityFrameworkCore;

namespace EMS.Plantilla.Data.DBContexts
{
    public class PlantillaContext : DbContext
    {
        public PlantillaContext(DbContextOptions<PlantillaContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configure your database connection here

                // Enable sensitive data logging
                optionsBuilder.EnableSensitiveDataLogging();

                // Other configuration options
            }
        }

        #region Tables

        public DbSet<Employee.Employee> Employee { get; set; }
        public DbSet<Employee.EmployeeSkills> EmployeeSkills { get; set; }
        public DbSet<Employee.EmployeeRoving> EmployeeRoving { get; set; }
        public DbSet<Employee.EmployeeFamily> EmployeeFamily { get; set; }
        public DbSet<Employee.EmployeeWorkingHistory> EmployeeWorkingHistory { get; set; }
        public DbSet<Employee.EmploymentStatusHistory> EmploymentStatusHistory { get; set; }
        public DbSet<Employee.EmployeeOnboarding> EmployeeOnboarding { get; set; }
        public DbSet<Employee.EmployeeCompensation> EmployeeCompensation { get; set; }
        public DbSet<Employee.EmployeeEducation> EmployeeEducation { get; set; }
        public DbSet<OrgGroup.OrgGroup> OrgGroup { get; set; }
        public DbSet<OrgGroup.OrgGroupHistory> OrgGroupHistory { get; set; }
        public DbSet<OrgGroup.SyncOrgGroupToH2Pay> SyncOrgGroupToH2Pay { get; set; }
        public DbSet<OrgGroup.OrgGroupPosition> OrgGroupPosition { get; set; }
        public DbSet<OrgGroup.OrgGroupTag> OrgGroupTag { get; set; }
        public DbSet<Region.Region> Region { get; set; }
        public DbSet<NPRFHeader> NPRFHeader { get; set; }
        public DbSet<NPRFPosition> NPRFPosition { get; set; }
        public DbSet<NPRFApprover> NPRFApprover { get; set; }
        public DbSet<NPRFSignatories> NPRFSignatories { get; set; }
        public DbSet<PositionLevel.PositionLevel> PositionLevel { get; set; }
        public DbSet<Position.Position> Position { get; set; }
        public DbSet<Reference.Reference> Reference { get; set; }
        public DbSet<Reference.ReferenceValue> ReferenceValue { get; set; }
        public DbSet<OrgGroup.OrgGroupNPRF> OrgGroupNPRF { get; set; }
        public DbSet<PSGC.PSGCRegion> PSGCRegion { get; set; }
        public DbSet<PSGC.PSGCProvince> PSGCProvince { get; set; }
        public DbSet<PSGC.PSGCCityMunicipality> PSGCCityMunicipality { get; set; }
        public DbSet<PSGC.PSGCBarangay> PSGCBarangay { get; set; }
        public DbSet<EmployeeMovement.EmployeeMovement> EmployeeMovement { get; set; }
        public DbSet<EmployeeMovement.EmployeeMovementStatusHistory> EmployeeMovementStatusHistory { get; set; }
        public DbSet<WageRate.WageRate> WageRate { get; set; }
        public DbSet<EmployeeMovement.EmployeeMovementMapping> EmployeeMovementMapping { get; set; }
        public DbSet<Employee.EmployeeAttachment> EmployeeAttachment { get; set; }
        public DbSet<SystemErrorLog.ErrorLog> ErrorLog { get; set; }


        public DbSet<Employee.UpdateEmployee> UpdateEmployee { get; set; }
        public DbSet<Employee.UpdateEmployeeEducation> UpdateEmployeeEducation { get; set; }
        public DbSet<Employee.UpdateEmployeeFamily> UpdateEmployeeFamily { get; set; }
        public DbSet<Employee.UpdateEmployeeWorkingHistory> UpdateEmployeeWorkingHistory { get; set; }

        #endregion Tables

        #region Table Variables
        public DbSet<OrgGroup.TableVarOrgGroup> TableVarOrgGroup { get; set; }
        public DbSet<OrgGroup.TableVarOrgGroupBranchInfo> TableVarOrgGroupBranchInfo { get; set; }
        public DbSet<OrgGroup.TableVarOrgGroupExportCountByOrgType> TableVarOrgGroupExportCountByOrgType { get; set; }
        public DbSet<OrgGroup.TableVarOrgGroupExportCountByOrgID> TableVarOrgGroupExportCountByOrgID { get; set; }
        public DbSet<Position.TableVarPosition> TableVarPosition { get; set; }
        public DbSet<Region.TableVarRegion> TableVarRegion { get; set; }
        public DbSet<PositionLevel.TableVarPositionLevel> TableVarPositionLevel { get; set; }
        public DbSet<OrgGroup.TableVarOrgGroupPosition> TableVarOrgGroupPosition { get; set; }
        public DbSet<OrgGroup.TableVarOrgChartPosition> TableVarOrgChartPosition { get; set; }
        public DbSet<Employee.TableVarEmployeeGetByID> TableVarEmployeeGetByID { get; set; }
        public DbSet<OrgGroup.TableVarOrgGroupExportList> TableVarOrgGroupExportList { get; set; }
        public DbSet<OrgGroup.TableVarOrgGroupHierarchy> TableVarOrgGroupHierarchy { get; set; }
        public DbSet<OrgGroup.TableVarOrgGroupEmployee> TableVarOrgGroupEmployee { get; set; }
        public DbSet<OrgGroup.TableVarPlantillaCount> TableVarPlantillaCount { get; set; }
        public DbSet<Employee.TableVarEmployee> TableVarEmployee { get; set; }
        public DbSet<Employee.TableVarGetEmail> TableVarGetEmail { get; set; }
        public DbSet<Employee.TableVarEmployeeCorporateEmail> TableVarEmployeeCorporateEmail { get; set; }
        public DbSet<Employee.TableVarEmployeeSkills> TableVarEmployeeSkills { get; set; }
        public DbSet<OrgGroup.TableVarHierarchyLevel> TableVarHierarchyLevel { get; set; }
        public DbSet<OrgGroup.TableVarOrgGroupNPRF> TableVarOrgGroupNPRF { get; set; }
        public DbSet<OrgGroup.TableVarOrgGroupDescendants> TableVarOrgGroupDescendants { get; set; }
        public DbSet<Dashboard.TableVarEmployeeDashboard> TableVarEmployeeDashboard { get; set; }
        public DbSet<Dashboard.TableVarEmployeeProbationaryCountByOrgGroup> TableVarEmployeeProbationaryCountByOrgGroup { get; set; }
        public DbSet<Dashboard.TableVarEmployeeProbationaryCountByPosition> TableVarEmployeeProbationaryCountByPosition { get; set; }
        public DbSet<Dashboard.TableVarProbationaryStatusCountBeyond6Months> TableVarProbationaryStatusCountBeyond6Months { get; set; }
        public DbSet<Dashboard.TableVarProbationaryStatusCountExpiring1Month> TableVarProbationaryStatusCountExpiring1Month { get; set; }
        public DbSet<Dashboard.TableVarBirthdayCount> TableVarBirthdayCount { get; set; }
        public DbSet<Dashboard.TableVarActiveEmployeeCountByOrgGroup> TableVarActiveEmployeeCountByOrgGroup { get; set; }
        public DbSet<Dashboard.TableVarActiveEmployeeCountByPosition> TableVarActiveEmployeeCountByPosition { get; set; }
        public DbSet<Dashboard.TableVarInactiveEmployeeCount> TableVarInactiveEmployeeCount { get; set; }
        public DbSet<Employee.TableVarNewEmployeeCode> TableVarNewEmployeeCode { get; set; }
        public DbSet<OrgGroup.TableVarOrgGroupRollupPositionDropdown> TableVarOrgGroupRollupPositionDropdown { get; set; }
        public DbSet<EmployeeMovement.TableVarEmployeeMovement> TableVarEmployeeMovement { get; set; }
        public DbSet<EmployeeMovement.TableVarEmployeeMovementAutoComplete> TableVarEmployeeMovementAutoComplete { get; set; }
        public DbSet<EmployeeMovement.TableVarEmployeeMovementByEmployeeIDs> TableVarEmployeeMovementByEmployeeIDs { get; set; }
        public DbSet<OrgGroup.TableVarGetRegionByOrgGroupID> TableVarGetRegionByOrgGroupID { get; set; }
        public DbSet<Employee.TableVarEmployeeETF> TableVarEmployeeETF { get; set; }
        public DbSet<EmployeeMovement.TableVarEmployeeMovementGetPrint> TableVarEmployeeMovementGetPrint { get; set; }
        public DbSet<EmployeeMovement.TableVarEmployeeMovementAutoPopulate> TableVarEmployeeMovementAutoPopulate { get; set; }
        public DbSet<Employee.TableVarPrintCOE> TableVarPrintCOE { get; set; }
        public DbSet<Position.TableVarPositionWithLevelAutoComplete> TableVarPositionWithLevelAutoComplete { get; set; }
        public DbSet<OrgGroup.TableVarPositionOrgGroupUpwardAutocomplete> TableVarPositionOrgGroupUpwardAutocomplete { get; set; }
        public DbSet<OrgGroup.TableVarOrgGroupHistoryList> TableVarOrgGroupHistoryList { get; set; }
        public DbSet<OrgGroup.TableVarOrgGroupHistoryByDate> TableVarOrgGroupHistoryByDate { get; set; }
        public DbSet<SystemErrorLog.TableVarErrorLog> TableVarErrorLog { get; set; }

        public DbSet<Dashboard.TableVarPlantillaDashboard> TableVarPlantillaDashboard { get; set; }
        public DbSet<Employee.TablerVarGetEmployeeLastEmploymentStatus> TablerVarGetEmployeeLastEmploymentStatus { get; set; }
        public DbSet<Employee.TableVarEmployeeReport> TableVarEmployeeReport { get; set; }
        public DbSet<Employee.TableVarEmployeeReportResult> TableVarEmployeeReportResult { get; set; }
        public DbSet<Employee.TableVarEmployeeReportGet> TableVarEmployeeReportGet { get; set; }
        public DbSet<Employee.TableVarEmployeeReportGetOrg> TableVarEmployeeReportGetOrg { get; set; }
        public DbSet<Employee.TableVarEmployeeEvaluation> TableVarEmployeeEvaluation { get; set; }
        public DbSet<Employee.TableVarLoginExternal> TableVarLoginExternal { get; set; }




        public DbSet<OrgGroup.TableVarOrgGroupFormat> TableVarOrgGroupFormat { get; set; }
        public DbSet<OrgGroup.TableVarOrgGroupSOMD> TableVarOrgGroupSOMD { get; set; }

        #endregion Table Variables

        #region Views
        public DbSet<Employee.ViewEmployeeRoving> ViewEmployeeRoving { get; set; } 
        #endregion

    }
}