using System;
using System.Collections.Generic;
using Utilities.API;

namespace EMS.IPM.Transfer.EmployeeScore
{
    public class Form
    {
        public int TID { get; set; }

        public int TransSummaryID { get; set; }

        public string Employee { get; set; }

        public string EmployeeCode { get; set; }

        public string Position { get; set; }

        public string OrgGroup { get; set; }

        public string TDateFrom { get; set; }

        public string TDateTo { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }

        public decimal TotalScore { get; set; }

        public int LevelOfApproval { get; set; }

        public bool isApproval { get; set; }

        public bool isAdmin { get; set; }

        public string Requestor { get; set; }

        public List<EmployeeScoreForm> EmployeeScoreList { get; set; }

        public string NextApproverRoleIDs { get; set; }

        public bool HasEditAccess { get; set; }

        public string MaxValue { get; set; }
    }

    public class EmployeeScoreForm
    {
        public int ID { get; set; }

        public string KRAGroup { get; set; }

        public string KPICode { get; set; }

        public string KPIName { get; set; }

        public string KPIDescription { get; set; }
        public string KPIGuidelines { get; set; }

        public decimal Weight { get; set; }
        public decimal Target { get; set; }
        public decimal Actual { get; set; }

        public decimal? Rate { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }
        public string Grade { get; set; }
        public string SourceType { get; set; }

        public bool IsEditable { get; set; }

        public string Requestor { get; set; }
    }

    public class RunScoreForm
    {
        public string Description { get; set; }

        public string Filter { get; set; }

        public string IDs { get; set; }

        public string Employees { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }

        public bool UseCurrent { get; set; }

        public bool RegularOnly { get; set; }

        public bool IncludeAllLevelsBelow { get; set; }

        public bool Override { get; set; }
        public bool IncludeSecDesig { get; set; }
        public string RoleIDs { get; set; }

        // Additional Filter
        public string strEmployeeIDList { get; set; }
        public int TransSummaryID { get; set; }
        public string Pk { get; set; }

    }

    public class SaveRunScoreForm
    {
        public bool Override { get; set; }

        public List<SaveRunScores> Scores { get; set; }
    }

    public class SaveRunScores
    {
        public int Employee { get; set; }

        public int KPI { get; set; }

        public decimal? KPIScore { get; set; }

        public decimal KPIWeight { get; set; }

        public int OrgGroup { get; set; }

        public int Position { get; set; }

        public DateTime TDateFrom { get; set; }

        public DateTime TDateTo { get; set; }

        public DateTime PDateFrom { get; set; }

        public DateTime PDateTo { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public string RoleIDs { get; set; }
        public string TransSummaryIDDelimited { get; set; }
        public string Description { get; set; }
        public string IsActiveDelimited { get; set; }
        public int? ID { get; set; }

        public string NameDelimited { get; set; }

        public string ParentOrgGroup { get; set; }

        public string OrgGroupDelimited { get; set; }

        public string PositionDelimited { get; set; }

        public decimal? ScoreFrom { get; set; }

        public decimal? ScoreTo { get; set; }

        //RunDate
        public string DateFromFrom { get; set; }
        public string DateFromTo { get; set; }
        public string DateToFrom { get; set; }
        public string DateToTo { get; set; }

        //DateEffective
        public string DateEffectiveFromFrom { get; set; }
        public string DateEffectiveFromTo { get; set; }
        public string DateEffectiveToFrom { get; set; }
        public string DateEffectiveToTo { get; set; }

        //KPIPositionDate
        //public string PDateFromFrom { get; set; }
        //public string PDateFromTo { get; set; }
        //public string PDateToFrom { get; set; }
        //public string PDateToTo { get; set; }

        public string StatusDelimited { get; set; }

        public bool ShowForEvaluation { get; set; }
        public bool ShowNoScore { get; set; }

        public bool IsExport { get; set; }

        //public string UserOrgGroupDelimited { get; set; }

        //public string OrgGroupDescendantsDelimited { get; set; }

        public bool isApproval { get; set; }
        public bool isShowAll { get; set; }
    }

    public class GetByIDInput
    {
        public int ID { get; set; }
        public string RoleIDs { get; set; }
    }

    public class GetEmployeeScoreDashboardInput : JQGridFilter
    {
        public string DashboardType { get; set; }
        public GetEmployeeScoreCountByPositionInput Dashboard1Input { get; set; }
        public GetEmployeeScoreCountByAreaInput Dashboard2Input { get; set; }
        public GetEmployeeScoreCountByRegionInput Dashboard3Input { get; set; }
    }

    public class GetEmployeeScoreCountByPositionInput
    {
        public string Position { get; set; }
        public int CountMin { get; set; }
        public int CountMax { get; set; }
    }

    public class GetEmployeeScoreCountByAreaInput
    {
        public string Branch { get; set; }
        public string Position { get; set; }
        public int CountMin { get; set; }
        public int CountMax { get; set; }
    }

    public class GetEmployeeScoreCountByRegionInput
    {
        public string Region { get; set; }
        public string Position { get; set; }
        public int CountMin { get; set; }
        public int CountMax { get; set; }
    }

    public class EmployeeScoreApprovalResponse
    {
        public int TID { get; set; }
        public Enums.EmployeeScore_ApproverStatus Status { get; set; }
        public string Remarks { get; set; }
    }

    public class BulkDeleteForm
    {
        public List<int> IDs { get; set; }
    }

    public class BulkVoidForm
    {
        public List<int> IDs { get; set; }

        public int id { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }

    public class BulkApprovedForm
    {
        public List<int> IDs { get; set; }

        public int id { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
    public class UpdateRunDescriptionInput
    {
        public int RunID { get; set; }
        public string Description { get; set; }
        public bool IsTransActive { get; set; }
    }

    public class RerunForm
    {
        public int RunID { get; set; }
        //public string DateFrom { get; set; }
        //public string DateTo { get; set; }
        //public string RoleIDs { get; set; }

        public string Description { get; set; }

        public string Filter { get; set; }

        public string IDs { get; set; }

        public string Employees { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }

        public bool UseCurrent { get; set; }

        public bool RegularOnly { get; set; }

        public bool IncludeAllLevelsBelow { get; set; }

        public bool Override { get; set; }
        public bool IncludeSecDesig { get; set; }
        public string RoleIDs { get; set; }

        // Additional Filter
        public string strEmployeeIDList { get; set; }
        public int TransSummaryID { get; set; }
        public string Pk { get; set; }
    }

    public class BatchEmployeeScoreForm
    {
        public List<int> IDs { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }

    public class BatchEmployeesScoreForm
    {
        public List<int> IDs { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }

    public class EmployeeKPIScoreGetListInput : JQGridFilter
    {
        public int? ID { get; set; }
        public string KRAGroup { get; set; }
        public string KPICode { get; set; }
        public string KPIName { get; set; }
        public string KPIDescription { get; set; }
        public string KPIGuidelines { get; set; }
        public decimal? WeightMin { get; set; }
        public decimal? WeightMax { get; set; }
        public decimal? TargetMin { get; set; }
        public decimal? TargetMax { get; set; }
        public decimal? ActualMin { get; set; }
        public decimal? ActualMax { get; set; }
        public decimal? RateMin { get; set; }
        public decimal? RateMax { get; set; }
        public decimal? TotalMin { get; set; }
        public decimal? TotalMax { get; set; }
        public string GradeDelimited { get; set; }
        public string SourceTypeDelimited { get; set; }

    }

    public class GetSummaryAutoCompleteInput
    {
        public string Term { get; set; }

        public int TopResults { get; set; }

        public bool IsAdmin { get; set; }
    }

    public class GetFinalScoreListInput : JQGridFilter
    {
        public int ID { get; set; }
        public string RunIDDelimited { get; set; }
        public string EmployeeIDDelimited { get; set; }
        public string IPMCount { get; set; }
        public string IPMMonths { get; set; }
        public decimal? FinalScoreFrom { get; set; }
        public decimal? FinalScoreTo { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDateFrom { get; set; }
        public string CreatedDateTo { get; set; }
        public bool IsExport { get; set; }
    }




}