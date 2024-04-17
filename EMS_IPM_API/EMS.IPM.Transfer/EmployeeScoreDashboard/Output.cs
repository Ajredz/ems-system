using System;
using System.Collections.Generic;
using Utilities.API;

namespace EMS.IPM.Transfer.EmployeeScoreDashboard
{
    public class GetListSummaryForEvaluationOutput : JQGridResult
    {
        public int? ID { get; set; }
        public string Region { get; set; }
        public int WithCompleteScore { get; set; }
        public int WithMissingScore { get; set; }
        public int NoScore { get; set; }
        public int OnGoingEvaluation { get; set; }
    }
    
    public class GetListSummaryForApprovalOutput : JQGridResult
    {
        public int? ID { get; set; }
        public string Region { get; set; }
        public int NoKeyIn { get; set; }
        public int ForApproval { get; set; }
        public int Finalized { get; set; }
    }

    public class GetListSummaryForApprovalBRNOutput : JQGridResult
    {
        public int? ID { get; set; }
        public string Region { get; set; }
        public string Branch { get; set; }
        public int NoKeyIn { get; set; }
        public int ForApproval { get; set; }
        public int Finalized { get; set; }
    }
    
    public class GetListSummaryForApprovalCLUOutput : JQGridResult
    {
        public int? ID { get; set; }
        public string Region { get; set; }
        public string Cluster { get; set; }
        public int NoKeyIn { get; set; }
        public int ForApproval { get; set; }
        public int Finalized { get; set; }
    }

    public class GetListRegionalWithPositionOutput : JQGridResult
    {
        public int? ID { get; set; }
        public string Region { get; set; }
        public string Position { get; set; }
        public string KRAGroup { get; set; }
        public string KPI { get; set; }
        public int EE { get; set; }
        public int ME { get; set; }
        public int SBE { get; set; }
        public int BE { get; set; }
    }

    public class GetListBranchesWithPositionOutput : JQGridResult
    {
        public int? ID { get; set; }
        public string Branch { get; set; }
        public string Position { get; set; }
        public string KRAGroup { get; set; }
        public string KPI { get; set; }
        public int EE { get; set; }
        public int ME { get; set; }
        public int SBE { get; set; }
        public int BE { get; set; }
    }

    public class GetListPositionOnlyOutput : JQGridResult
    {
        public int? ID { get; set; }
        public string Position { get; set; }
        public string KRAGroup { get; set; }
        public string KPI { get; set; }
        public int EE { get; set; }
        public int ME { get; set; }
        public int SBE { get; set; }
        public int BE { get; set; }
    }

}