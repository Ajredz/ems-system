using System;
using System.Collections.Generic;
using Utilities.API;

namespace EMS.IPM.Transfer.MyKPIScores
{
    public class GetListOutput : JQGridResult
    {
        public int? TransSummaryID { get; set; }
        public int? ID { get; set; }
        public string Description { get; set; }
        
        public string Employee { get; set; }

        public string ParentOrgGroup { get; set; }

        public string OrgGroup { get; set; }

        public string Position { get; set; }

        public decimal Score { get; set; }

        public string TDateFrom { get; set; }

        public string TDateTo { get; set; }

        public string Status { get; set; }

        public string PDateFrom { get; set; }

        public string PDateTo { get; set; }
    }

    public class GetAllKPIOutput
    {
        public int ID { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }

    public class GetAllEmployeeScoreOutput
    {
        public int ID { get; set; }

        public DateTime TDate { get; set; }

        public int Position { get; set; }

        public int KPI { get; set; }

        public decimal Weight { get; set; }

    }

    public class RunScoresOutput
    {
        public string Message { get; set; }
    }

    public class ScoresOutput
    {
        public DateTime TDateFrom { get; set; }

        public DateTime TDateTo { get; set; }

        public int Employee { get; set; }

        public int OrgGroup { get; set; }

        public int Position { get; set; }

        public int KPI { get; set; }

        public decimal KPIWeight { get; set; }

        public decimal? KPIScore { get; set; }

        public DateTime PDateFrom { get; set; }

        public DateTime PDateTo { get; set; }
    }

    public class GetEmployeeScoreDashboardOutput : JQGridResult
    {
        public string DashboardType { get; set; }
        public List<GetEmployeeScoreCountByPositionOutput> Dashboard1Output { get; set; }
        public List<GetEmployeeScoreCountByAreaOutput> Dashboard2Output { get; set; }
        public List<GetEmployeeScoreCountByRegionOutput> Dashboard3Output { get; set; }
    }

    public class GetEmployeeScoreCountByPositionOutput
    {
        public string Position { get; set; }
        public int Count { get; set; }
    }

    public class GetEmployeeScoreCountByAreaOutput
    {
        public string Area { get; set; }
        public int Count { get; set; }
    }

    public class GetEmployeeScoreCountByRegionOutput
    {
        public string Region { get; set; }
        public int Count { get; set; }
    }

    public class GetEmployeeScoreStatusHistoryOutput
    {
        public string Status { get; set; }
        public string Timestamp { get; set; }
        public int UserID { get; set; }
        public string User { get; set; }
        public string Remarks { get; set; }
	}

    public class GetTransProgressOutput
    {
        public long ProcessedEmployees { get; set; }
        public long EmployeesWithIPM { get; set; }
        public int CreatedBy { get; set; }
    }

    public class TransOutput
    { 
        public int TransSummaryID { get; set; }
        public List<int> TransIDList { get; set; }
        public string Message { get; set; }
    } 
    
    public class GetTransEmployeeScoreSummaryOutput
    {
        public string FilterBy { get; set; }
        public string FilterOrgGroup { get; set; }
        public bool FilterIncludeLevelBelow { get; set; }
        public string FilterPosition { get; set; }
        public string FilterEmployee { get; set; }
        public bool FilterOverride { get; set; }
        public bool FilterUseCurrent { get; set; }
        public int ID { get; set; }
        public string TDateFrom { get; set; }
        public string TDateTo { get; set; }
        public long ProcessedEmployees { get; set; }
        public long TotalNumOfEmployees { get; set; }
        public decimal PassingScore { get; set; }
        public int EmployeesWithIPM { get; set; }
        public int PassedEmployees { get; set; }
        public int FailedEmployees { get; set; }
        public bool IsDone { get; set; }
    }
}