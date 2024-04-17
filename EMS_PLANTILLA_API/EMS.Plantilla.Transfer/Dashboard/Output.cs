using System;
using System.Collections.Generic;
using System.Text;
using Utilities.API;

namespace EMS.Plantilla.Transfer.Dashboard
{
    public class GetEmployeeDashboardOutput : JQGridResult
    {
        public string ReportType { get; set; }
        public List<GetEmployeeProbationaryCountByOrgGroupOutput> Report1Output { get; set; }
        public List<GetEmployeeProbationaryCountByPositionOutput> Report2Output { get; set; }
        public List<GetProbationaryStatusCountBeyond6MonthsOutput> Report3Output { get; set; }
        public List<GetProbationaryStatusCountExpiring1MonthOutput> Report4Output { get; set; }
        public List<GetBirthdayCountOutput> Report5Output { get; set; }
        public List<GetActiveEmployeeCountByOrgGroupOutput> Report6Output { get; set; }
        public List<GetActiveEmployeeCountByPositionOutput> Report7Output { get; set; }
        public List<GetInactiveEmployeeCountOutput> Report8Output { get; set; }
    }

    public class GetEmployeeProbationaryCountByOrgGroupOutput
    {
        public string OrgGroup { get; set; }
        public int Count { get; set; }
    }

    public class GetEmployeeProbationaryCountByPositionOutput
    {
        public string Position { get; set; }
        public int Count { get; set; }
    }

    public class GetProbationaryStatusCountBeyond6MonthsOutput
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class GetProbationaryStatusCountExpiring1MonthOutput
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class GetBirthdayCountOutput
    {
        public string Month { get; set; }
        public int Count { get; set; }
    }

    public class GetActiveEmployeeCountByOrgGroupOutput
    {
        public string OrgGroup { get; set; }
        public int Count { get; set; }
    }

    public class GetActiveEmployeeCountByPositionOutput
    {
        public string Position { get; set; }
        public int Count { get; set; }
    }

    public class GetInactiveEmployeeCountOutput
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class GetPlantillaDashboardOutput
    {
        public string Value { get; set; }
        public int Count { get; set; }
    }
}
