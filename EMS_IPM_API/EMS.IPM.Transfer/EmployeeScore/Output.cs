using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Transfer.EmployeeScore
{
    public class GetListOutput : JQGridResult
    {
        public int? TransSummaryID { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public int? ID { get; set; }
        
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
        public string DateEffectiveFrom { get; set; }
        public string DateEffectiveTo { get; set; }
        public decimal QualiPlan { get; set; }
        public decimal QualiActual { get; set; }
        public decimal QualiBranchPerformance { get; set; }
        public decimal QualiProRatePerformance { get; set; }
        public decimal QualiFinal { get; set; }
        public string QualiRemarks { get; set; }
        public int IPMMonths { get; set; }
        public bool HasEditAccess { get; set; }
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
        public string Branch { get; set; }
        public string Position { get; set; }
        public int Count { get; set; }
    }

    public class GetEmployeeScoreCountByRegionOutput
    {
        public string Region { get; set; }
        public string Position { get; set; }
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
        public bool IsDone { get; set; }
        public int TransSummaryID { get; set; }
    }

    public class TransOutput
    { 
        public int TransSummaryID { get; set; }
        public List<int> TransIDList { get; set; }
        public string Message { get; set; }
    }

    public class TransOutputForTreading
    {
        public List<int> EmployeeID { get; set; }
        public int TransSummaryID { get; set; }
        public bool OverRide { get; set; }
        public string Message { get; set; }
    }

    public class ReRunParamOutput
    {
        public string Description { get; set; }
        public string Filter { get; set; }
        public string Ids { get; set; }
        public string Employees { get; set; }
        public string Position { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public bool UseCurrent { get; set; }
        public bool RegularOnly { get; set; }
        public bool IncludeLvlBelow { get; set; }
        public bool Override { get; set; }
        public int TransSummaryID { get; set; }
        public bool IncludeSecDesig { get; set; }
        public string RoleID { get; set; }
        public int CreatedBy { get; set; }
    }

    public class GetTransEmployeeScoreSummaryOutput
    {
        public string Description { get; set; }
        public string FilterBy { get; set; }
        public string FilterOrgGroup { get; set; }
        public string FilterIncludeLevelBelow { get; set; }
        public string FilterPosition { get; set; }
        public string FilterEmployee { get; set; }
        public string FilterOverride { get; set; }
        public string FilterUseCurrent { get; set; }
        public int ID { get; set; }
        public string TDateFrom { get; set; }
        public string TDateTo { get; set; }
        public long ProcessedEmployees { get; set; }
        public long TotalNumOfEmployees { get; set; }
        public int EmployeesWithIPM { get; set; }
        public int RatingEEEmployees { get; set; }
        public int RatingMEEmployees { get; set; }
        public int RatingSBEEmployees { get; set; }
        public int RatingBEEmployees { get; set; }
        public string RatingEEMin { get; set; }
        public string RatingEEMax { get; set; }
        public string RatingMEMin { get; set; }
        public string RatingMEMax { get; set; }
        public string RatingSBEMin { get; set; }
        public string RatingSBEMax { get; set; }
        public string RatingBEMin { get; set; }
        public string RatingBEMax { get; set; }
        public bool IsDone { get; set; }
        public bool IsTransActive { get; set; }
        public string CreatedDate { get; set; }
        /*public int TotalEmployeesWithIPM { get; set; }
        public int EmployeesWithMultiple { get; set; }
        public int TotalIPMResult { get; set; }
        public DateTime? RunStart { get; set; }
        public DateTime? RunEnd { get; set; }*/
    }

    public class EmployeeKPIScoreGetListOutput : JQGridResult
    {
        public int ID { get; set; }
        public int KPIID { get; set; }
        public string KRAGroup { get; set; }
        public string KPICode { get; set; }
        public string KPIName { get; set; }
        public string KPIDescription { get; set; }
        public string KPIGuidelines { get; set; }
        public decimal Weight { get; set; }
        public decimal Target { get; set; }
        public decimal Actual { get; set; }
        public decimal Rate { get; set; }
        public decimal Total { get; set; }
        public string Grade { get; set; }
        public string SourceType { get; set; }
        public bool IsEditable { get; set; }
    }

    public class GetFinalScoreListOutput : JQGridResult
    {
        public int? ID { get; set; }
        public int? RunID { get; set; }
        public string RunTitle { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int IPMCount { get; set; }
        public string IPMMonths { get; set; }
        public decimal FinalScore { get; set; }
        public decimal FinalQuali { get; set; }
        public string FinalRemarks { get; set; }
        public string IsOld { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
    }

}