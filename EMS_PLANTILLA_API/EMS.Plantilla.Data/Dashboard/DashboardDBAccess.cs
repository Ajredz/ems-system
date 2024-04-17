using EMS.Plantilla.Data.DBContexts;
using EMS.Plantilla.Transfer.Dashboard;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Plantilla.Data.Dashboard
{
    public interface IDashboardDBAccess
    {
        //Task<IEnumerable<TableVarEmployeeDashboard>> GetEmployeeDashboardByReportType(GetEmployeeDashboardInput input, int rowStart);
        Task<IEnumerable<TableVarEmployeeProbationaryCountByOrgGroup>> GetEmployeeProbationaryCountByOrgGroup(GetEmployeeDashboardInput input, int rowStart);
        Task<IEnumerable<TableVarEmployeeProbationaryCountByPosition>> GetEmployeeProbationaryCountByPosition(GetEmployeeDashboardInput input, int rowStart);
        Task<IEnumerable<TableVarProbationaryStatusCountBeyond6Months>> GetProbationaryStatusCountBeyond6Months(GetEmployeeDashboardInput input, int rowStart);
        Task<IEnumerable<TableVarProbationaryStatusCountExpiring1Month>> GetProbationaryStatusCountExpiring1Month(GetEmployeeDashboardInput input, int rowStart);
        Task<IEnumerable<TableVarBirthdayCount>> GetBirthdayCount(GetEmployeeDashboardInput input, int rowStart);
        Task<IEnumerable<TableVarActiveEmployeeCountByOrgGroup>> GetActiveEmployeeCountByOrgGroup(GetEmployeeDashboardInput input, int rowStart);
        Task<IEnumerable<TableVarActiveEmployeeCountByPosition>> GetActiveEmployeeCountByPosition(GetEmployeeDashboardInput input, int rowStart);
        Task<IEnumerable<TableVarInactiveEmployeeCount>> GetInactiveEmployeeCount(GetEmployeeDashboardInput input, int rowStart);
        Task<IEnumerable<TableVarPlantillaDashboard>> GetPlantillaDashboard(PlantillaDashboardInput param);
    }

    public class DashboardDBAccess : IDashboardDBAccess
    {
        private readonly PlantillaContext _dbContext;

        public DashboardDBAccess(PlantillaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarEmployeeProbationaryCountByOrgGroup>> GetEmployeeProbationaryCountByOrgGroup(GetEmployeeDashboardInput input, int rowStart)
        {
            return await _dbContext.TableVarEmployeeProbationaryCountByOrgGroup
               .FromSqlRaw(@"CALL sp_employee_get_probationary_count_by_org_group(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                        )",
                            input.Report1Input.OrgGroup ?? ""
                          , input.Report1Input.CountMin
                          , input.Report1Input.CountMax
                          , input.Report1Input.IsExport
                          , input.sidx ?? ""
                          , input.sord ?? ""
                          , rowStart
                          , input.rows
                      )
             .AsNoTracking()
             .ToListAsync();
        }

        public async Task<IEnumerable<TableVarEmployeeProbationaryCountByPosition>> GetEmployeeProbationaryCountByPosition(GetEmployeeDashboardInput input, int rowStart)
        {
            return await _dbContext.TableVarEmployeeProbationaryCountByPosition
               .FromSqlRaw(@"CALL sp_employee_get_probationary_count_by_position(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                        )",
                            input.Report2Input.Position ?? ""
                          , input.Report2Input.CountMin
                          , input.Report2Input.CountMax
                          , input.Report2Input.IsExport
                          , input.sidx ?? ""
                          , input.sord ?? ""
                          , rowStart
                          , input.rows
                      )
             .AsNoTracking()
             .ToListAsync();
        }

        public async Task<IEnumerable<TableVarProbationaryStatusCountBeyond6Months>> GetProbationaryStatusCountBeyond6Months(GetEmployeeDashboardInput input, int rowStart)
        {
            return await _dbContext.TableVarProbationaryStatusCountBeyond6Months
               .FromSqlRaw(@"CALL sp_employee_get_probationary_status_beyond_6_months(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                        )",
                            input.Report3Input.Status ?? ""
                          , input.Report3Input.CountMin
                          , input.Report3Input.CountMax
                          , input.Report3Input.IsExport
                          , input.sidx ?? ""
                          , input.sord ?? ""
                          , rowStart
                          , input.rows
                      )
             .AsNoTracking()
             .ToListAsync();
        }

        public async Task<IEnumerable<TableVarProbationaryStatusCountExpiring1Month>> GetProbationaryStatusCountExpiring1Month(GetEmployeeDashboardInput input, int rowStart)
        {
            return await _dbContext.TableVarProbationaryStatusCountExpiring1Month
               .FromSqlRaw(@"CALL sp_employee_get_probationary_status_expiring_1_month(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                        )",
                            input.Report4Input.Status ?? ""
                          , input.Report4Input.CountMin
                          , input.Report4Input.CountMax
                          , input.Report4Input.IsExport
                          , input.sidx ?? ""
                          , input.sord ?? ""
                          , rowStart
                          , input.rows
                      )
             .AsNoTracking()
             .ToListAsync();
        }

        public async Task<IEnumerable<TableVarBirthdayCount>> GetBirthdayCount(GetEmployeeDashboardInput input, int rowStart)
        {
            return await _dbContext.TableVarBirthdayCount
               .FromSqlRaw(@"CALL sp_employee_get_birthday_this_next_month(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                        )",
                            input.Report5Input.Month ?? ""
                          , input.Report5Input.CountMin
                          , input.Report5Input.CountMax
                          , input.Report5Input.IsExport
                          , input.sidx ?? ""
                          , input.sord ?? ""
                          , rowStart
                          , input.rows
                      )
             .AsNoTracking()
             .ToListAsync();
        }

        public async Task<IEnumerable<TableVarActiveEmployeeCountByOrgGroup>> GetActiveEmployeeCountByOrgGroup(GetEmployeeDashboardInput input, int rowStart)
        {
            return await _dbContext.TableVarActiveEmployeeCountByOrgGroup
               .FromSqlRaw(@"CALL sp_employee_get_active_count_by_org_group(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                        )",
                            input.Report6Input.OrgGroup ?? ""
                          , input.Report6Input.CountMin
                          , input.Report6Input.CountMax
                          , input.Report6Input.IsExport
                          , input.sidx ?? ""
                          , input.sord ?? ""
                          , rowStart
                          , input.rows
                      )
             .AsNoTracking()
             .ToListAsync();
        }

        public async Task<IEnumerable<TableVarActiveEmployeeCountByPosition>> GetActiveEmployeeCountByPosition(GetEmployeeDashboardInput input, int rowStart)
        {
            return await _dbContext.TableVarActiveEmployeeCountByPosition
               .FromSqlRaw(@"CALL sp_employee_get_active_count_by_position(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                        )",
                            input.Report7Input.Position ?? ""
                          , input.Report7Input.CountMin
                          , input.Report7Input.CountMax
                          , input.Report7Input.IsExport
                          , input.sidx ?? ""
                          , input.sord ?? ""
                          , rowStart
                          , input.rows
                      )
             .AsNoTracking()
             .ToListAsync();
        }

        public async Task<IEnumerable<TableVarInactiveEmployeeCount>> GetInactiveEmployeeCount(GetEmployeeDashboardInput input, int rowStart)
        {
            return await _dbContext.TableVarInactiveEmployeeCount
               .FromSqlRaw(@"CALL sp_employee_get_inactive_count(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                        )",
                            input.Report8Input.Status ?? ""
                          , input.Report8Input.CountMin
                          , input.Report8Input.CountMax
                          , input.Report8Input.IsExport
                          , input.sidx ?? ""
                          , input.sord ?? ""
                          , rowStart
                          , input.rows
                      )
             .AsNoTracking()
             .ToListAsync();
        }
        public async Task<IEnumerable<TableVarPlantillaDashboard>> GetPlantillaDashboard(PlantillaDashboardInput param)
        {
            return await _dbContext.TableVarPlantillaDashboard
               .FromSqlRaw(@"CALL sp_plantilla_dashboard(
                    {0}
                  , {1}
                  , {2}
                  , {3}
                )"
                , param.DashboardData ?? ""
                , param.OrgGroupID == null ? "" : string.Join(",", param.OrgGroupID)
                , param.PositionID == null ? "" : string.Join(",", param.PositionID)
                , param.EmploymentStatus ?? "")
             .AsNoTracking()
             .ToListAsync();
        }
    }
}
