using System;
using System.Collections.Generic;
using System.Text;
using Utilities.API;

namespace EMS.Plantilla.Transfer.Dashboard
{
    public enum ReferenceCodes_ReportType
    {
        PRO_EMP_PER_GROUP,
        PRO_EMP_PER_POSITION,
        PRO_STATUS_BEYOND_6,
        PRO_STATUS_EXPIRING,
        BIRTHDAY_PER_MONTH,
        ACT_EMP_PER_GROUP,
        ACT_EMP_PER_POSITION,
        INACTIVE_EMPLOYEE
    }

    public class GetEmployeeDashboardInput : JQGridFilter
    {
        public string ReportType { get; set; }
        public GetEmployeeProbationaryCountByOrgGroupInput Report1Input { get; set; }
        public GetEmployeeProbationaryCountByPositionInput Report2Input { get; set; }
        public GetProbationaryStatusCountBeyond6MonthsInput Report3Input { get; set; }
        public GetProbationaryStatusCountExpiring1MonthInput Report4Input { get; set; }
        public GetBirthdayCountInput Report5Input { get; set; }
        public GetActiveEmployeeCountByOrgGroupInput Report6Input { get; set; }
        public GetActiveEmployeeCountByPositionInput Report7Input { get; set; }
        public GetInactiveEmployeeCountInput Report8Input { get; set; }
    }

    public class GetEmployeeProbationaryCountByOrgGroupInput 
    {
        public string OrgGroup { get; set; }
        public int CountMin { get; set; }
        public int CountMax { get; set; }
        public bool IsExport { get; set; }
    }

    public class GetEmployeeProbationaryCountByPositionInput
    {
        public string Position { get; set; }
        public int CountMin { get; set; }
        public int CountMax { get; set; }
        public bool IsExport { get; set; }
    }

    public class GetProbationaryStatusCountBeyond6MonthsInput
    {
        public string Status { get; set; }
        public int CountMin { get; set; }
        public int CountMax { get; set; }
        public bool IsExport { get; set; }
    }

    public class GetProbationaryStatusCountExpiring1MonthInput
    {
        public string Status { get; set; }
        public int CountMin { get; set; }
        public int CountMax { get; set; }
        public bool IsExport { get; set; }
    }

    public class GetBirthdayCountInput
    {
        public string Month { get; set; }
        public int CountMin { get; set; }
        public int CountMax { get; set; }
        public bool IsExport { get; set; }
    }

    public class GetActiveEmployeeCountByOrgGroupInput
    {
        public string OrgGroup { get; set; }
        public int CountMin { get; set; }
        public int CountMax { get; set; }
        public bool IsExport { get; set; }
    }

    public class GetActiveEmployeeCountByPositionInput
    {
        public string Position { get; set; }
        public int CountMin { get; set; }
        public int CountMax { get; set; }
        public bool IsExport { get; set; }
    }

    public class GetInactiveEmployeeCountInput
    {
        public string Status { get; set; }
        public int CountMin { get; set; }
        public int CountMax { get; set; }
        public bool IsExport { get; set; }
    }

    public class PlantillaDashboardInput
    {
        public string DashboardData { get; set; }
        public List<int> OrgGroupID { get; set; }
        public List<int> PositionID { get; set; }
        public string EmploymentStatus { get; set; }
    }
}
