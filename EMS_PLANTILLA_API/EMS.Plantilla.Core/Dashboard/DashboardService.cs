using EMS.Plantilla.Data.Dashboard;
using EMS.Plantilla.Data.DBContexts;
using EMS.Plantilla.Transfer.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Plantilla.Core.Dashboard
{
    public interface IDashboardService
    {
        Task<IActionResult> GetEmployeeDashboardByReportType(APICredentials credentials, GetEmployeeDashboardInput input);
        Task<IActionResult> GetPlantillaDashboard(APICredentials credentials, PlantillaDashboardInput param);
    }

    public class DashboardService : Core.Shared.Utilities, IDashboardService
    {

        private readonly IDashboardDBAccess _dbAccess;

        public DashboardService(PlantillaContext dbContext, IConfiguration iconfiguration,
            IDashboardDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetEmployeeDashboardByReportType(APICredentials credentials, GetEmployeeDashboardInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            GetEmployeeDashboardOutput output = new GetEmployeeDashboardOutput();

            if (input.ReportType == ReferenceCodes_ReportType.PRO_EMP_PER_GROUP.ToString())
            {
                IEnumerable<TableVarEmployeeProbationaryCountByOrgGroup> report = await _dbAccess.GetEmployeeProbationaryCountByOrgGroup(input, rowStart);

                output.ReportType = ReferenceCodes_ReportType.PRO_EMP_PER_GROUP.ToString();
                output.Report1Output = 
                    report.Select(x => new GetEmployeeProbationaryCountByOrgGroupOutput
                    {
                        OrgGroup = x.OrgGroup,
                        Count = x.Count
                    }).ToList();
                output.TotalNoOfRecord = report.Count() == 0 ? 0 : report.First().TotalNum;
                output.NoOfPages = report.Count() == 0 ? 0 : (int)Math.Ceiling((float)report.First().TotalNum / (float)input.rows);
            }
            else if (input.ReportType == ReferenceCodes_ReportType.PRO_EMP_PER_POSITION.ToString())
            {
                IEnumerable<TableVarEmployeeProbationaryCountByPosition> report = await _dbAccess.GetEmployeeProbationaryCountByPosition(input, rowStart);

                output.ReportType = ReferenceCodes_ReportType.PRO_EMP_PER_POSITION.ToString();
                output.Report2Output =
                    report.Select(x => new GetEmployeeProbationaryCountByPositionOutput
                    {
                        Position = x.Position,
                        Count = x.Count
                    }).ToList();
                output.TotalNoOfRecord = report.Count() == 0 ? 0 : report.First().TotalNum;
                output.NoOfPages = report.Count() == 0 ? 0 : (int)Math.Ceiling((float)report.First().TotalNum / (float)input.rows);
            }
            else if (input.ReportType == ReferenceCodes_ReportType.PRO_STATUS_BEYOND_6.ToString())
            {
                IEnumerable<TableVarProbationaryStatusCountBeyond6Months> report = await _dbAccess.GetProbationaryStatusCountBeyond6Months(input, rowStart);

                output.ReportType = ReferenceCodes_ReportType.PRO_STATUS_BEYOND_6.ToString();
                output.Report3Output =
                    report.Select(x => new GetProbationaryStatusCountBeyond6MonthsOutput
                    {
                        Status = x.Status,
                        Count = x.Count
                    }).ToList();
                output.TotalNoOfRecord = report.Count() == 0 ? 0 : report.First().TotalNum;
                output.NoOfPages = report.Count() == 0 ? 0 : (int)Math.Ceiling((float)report.First().TotalNum / (float)input.rows);
            }
            else if (input.ReportType == ReferenceCodes_ReportType.PRO_STATUS_EXPIRING.ToString())
            {
                IEnumerable<TableVarProbationaryStatusCountExpiring1Month> report = await _dbAccess.GetProbationaryStatusCountExpiring1Month(input, rowStart);

                output.ReportType = ReferenceCodes_ReportType.PRO_STATUS_EXPIRING.ToString();
                output.Report4Output =
                    report.Select(x => new GetProbationaryStatusCountExpiring1MonthOutput
                    {
                        Status = x.Status,
                        Count = x.Count
                    }).ToList();
                output.TotalNoOfRecord = report.Count() == 0 ? 0 : report.First().TotalNum;
                output.NoOfPages = report.Count() == 0 ? 0 : (int)Math.Ceiling((float)report.First().TotalNum / (float)input.rows);
            }
            else if (input.ReportType == ReferenceCodes_ReportType.BIRTHDAY_PER_MONTH.ToString())
            {
                IEnumerable<TableVarBirthdayCount> report = await _dbAccess.GetBirthdayCount(input, rowStart);

                output.ReportType = ReferenceCodes_ReportType.BIRTHDAY_PER_MONTH.ToString();
                output.Report5Output =
                    report.Select(x => new GetBirthdayCountOutput
                    {
                        Month = x.Month,
                        Count = x.Count
                    }).ToList();
                output.TotalNoOfRecord = report.Count() == 0 ? 0 : report.First().TotalNum;
                output.NoOfPages = report.Count() == 0 ? 0 : (int)Math.Ceiling((float)report.First().TotalNum / (float)input.rows);
            }
            else if (input.ReportType == ReferenceCodes_ReportType.ACT_EMP_PER_GROUP.ToString())
            {
                IEnumerable<TableVarActiveEmployeeCountByOrgGroup> report = await _dbAccess.GetActiveEmployeeCountByOrgGroup(input, rowStart);

                output.ReportType = ReferenceCodes_ReportType.ACT_EMP_PER_GROUP.ToString();
                output.Report6Output =
                    report.Select(x => new GetActiveEmployeeCountByOrgGroupOutput
                    {
                        OrgGroup = x.OrgGroup,
                        Count = x.Count
                    }).ToList();
                output.TotalNoOfRecord = report.Count() == 0 ? 0 : report.First().TotalNum;
                output.NoOfPages = report.Count() == 0 ? 0 : (int)Math.Ceiling((float)report.First().TotalNum / (float)input.rows);
            }
            else if (input.ReportType == ReferenceCodes_ReportType.ACT_EMP_PER_POSITION.ToString())
            {
                IEnumerable<TableVarActiveEmployeeCountByPosition> report = await _dbAccess.GetActiveEmployeeCountByPosition(input, rowStart);

                output.ReportType = ReferenceCodes_ReportType.ACT_EMP_PER_POSITION.ToString();
                output.Report7Output =
                    report.Select(x => new GetActiveEmployeeCountByPositionOutput
                    {
                        Position = x.Position,
                        Count = x.Count
                    }).ToList();
                output.TotalNoOfRecord = report.Count() == 0 ? 0 : report.First().TotalNum;
                output.NoOfPages = report.Count() == 0 ? 0 : (int)Math.Ceiling((float)report.First().TotalNum / (float)input.rows);
            }
            else if (input.ReportType == ReferenceCodes_ReportType.INACTIVE_EMPLOYEE.ToString())
            {
                IEnumerable<TableVarInactiveEmployeeCount> report = await _dbAccess.GetInactiveEmployeeCount(input, rowStart);

                output.ReportType = ReferenceCodes_ReportType.INACTIVE_EMPLOYEE.ToString();
                output.Report8Output =
                    report.Select(x => new GetInactiveEmployeeCountOutput
                    {
                        Status = x.Status,
                        Count = x.Count
                    }).ToList();
                output.TotalNoOfRecord = report.Count() == 0 ? 0 : report.First().TotalNum;
                output.NoOfPages = report.Count() == 0 ? 0 : (int)Math.Ceiling((float)report.First().TotalNum / (float)input.rows);
            }

            return new OkObjectResult(output);
        }

        public async Task<IActionResult> GetPlantillaDashboard(APICredentials credentials, PlantillaDashboardInput param)
        {
            return new OkObjectResult(await _dbAccess.GetPlantillaDashboard(param));
        }
    }
}
