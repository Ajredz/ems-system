using System;
using System.Collections.Generic;
using Utilities.API;

namespace EMS.IPM.Transfer.MyKPIScores
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

        public string Requestor { get; set; }

        public List<MyKPIScoreForm> MyKPIScoreList { get; set; }
    }

    public class MyKPIScoreForm
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

        public bool IsEditable { get; set; }

        public string Requestor { get; set; }
        public string Grade { get; set; }
        public string SourceType { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public string TransSummaryIDDelimited { get; set; }

        public int? ID { get; set; }
        public string Description { get; set; }

        public string OrgGroupDelimited { get; set; }

        public string PositionDelimited { get; set; }

        public decimal? ScoreFrom { get; set; }

        public decimal? ScoreTo{ get; set; }

        public string DateFromFrom { get; set; }

        public string DateFromTo { get; set; }

        public string DateToFrom { get; set; }

        public string DateToTo { get; set; }

        public bool IsExport { get; set; }
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
        public string Area { get; set; }
        public int CountMin { get; set; }
        public int CountMax { get; set; }
    }

    public class GetEmployeeScoreCountByRegionInput
    {
        public string Region { get; set; }
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

    public class BatchEmployeeScoreForm
    {
        public List<int> IDs { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}