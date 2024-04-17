using System;
using System.Collections.Generic;
using Utilities.API;

namespace EMS.IPM.Transfer.EmployeeScoreDashboard
{
    public class GetListSummaryForEvaluationInput : JQGridFilter
    {
        public string RunIDDelimited { get; set; }
        public string RegionDelimited { get; set; }
        public int? WithCompleteScoreMin { get; set; }
        public int? WithCompleteScoreMax { get; set; }
        public int? WithMissingScoreMin { get; set; }
        public int? WithMissingScoreMax { get; set; }
        public int? NoScoreMin { get; set; }
        public int? NoScoreMax { get; set; }
        public int? OnGoingEvalMin { get; set; }
        public int? OnGoingEvalMax { get; set; }

        public bool IsExport { get; set; }
    }
    
    public class GetListSummaryForApprovalInput : JQGridFilter
    {
        public string RunIDDelimited { get; set; }
        public string RegionDelimited { get; set; }
        public int? NoKeyInMin { get; set; }
        public int? NoKeyInMax { get; set; }
        public int? ForApprovalMin { get; set; }
        public int? ForApprovalMax { get; set; }
        public int? FinalizedMin { get; set; }
        public int? FinalizedMax { get; set; }
        public bool IsExport { get; set; }
    }

    public class GetListSummaryForApprovalBRNInput : JQGridFilter
    {
        public string RunIDDelimited { get; set; }
        public string RegionDelimited { get; set; }
        public string BranchDelimited { get; set; }
        public int? NoKeyInMin { get; set; }
        public int? NoKeyInMax { get; set; }
        public int? ForApprovalMin { get; set; }
        public int? ForApprovalMax { get; set; }
        public int? FinalizedMin { get; set; }
        public int? FinalizedMax { get; set; }
        public bool IsExport { get; set; }
    } 
    
    public class GetListSummaryForApprovalCLUInput : JQGridFilter
    {
        public string RunIDDelimited { get; set; }
        public string RegionDelimited { get; set; }
        public string ClusterDelimited { get; set; }
        public int? NoKeyInMin { get; set; }
        public int? NoKeyInMax { get; set; }
        public int? ForApprovalMin { get; set; }
        public int? ForApprovalMax { get; set; }
        public int? FinalizedMin { get; set; }
        public int? FinalizedMax { get; set; }
        public bool IsExport { get; set; }
    }

     public class GetListRegionalWithPositionInput : JQGridFilter
    {
        public string RunIDDelimited { get; set; }
        public string RegionDelimited { get; set; }
        public string PositionDelimited { get; set; }
        public string KRAGroupDelimited { get; set; }
        public string KPIDelimited { get; set; }
        public int? EEMin { get; set; }
        public int? EEMax { get; set; }
        public int? MEMin { get; set; }
        public int? MEMax { get; set; }
        public int? SBEMin { get; set; }
        public int? SBEMax { get; set; }
        public int? BEMin { get; set; }
        public int? BEMax { get; set; }
        public bool IsExport { get; set; }
    }

     public class GetListBranchesWithPositionInput : JQGridFilter
    {
        public string RunIDDelimited { get; set; }
        public string BranchDelimited { get; set; }
        public string PositionDelimited { get; set; }
        public string KRAGroupDelimited { get; set; }
        public string KPIDelimited { get; set; }
        public int? EEMin { get; set; }
        public int? EEMax { get; set; }
        public int? MEMin { get; set; }
        public int? MEMax { get; set; }
        public int? SBEMin { get; set; }
        public int? SBEMax { get; set; }
        public int? BEMin { get; set; }
        public int? BEMax { get; set; }
        public bool IsExport { get; set; }
    }
    
     public class GetListPositionOnlyInput : JQGridFilter
    {
        public string RunIDDelimited { get; set; }
        public string PositionDelimited { get; set; }
        public string KRAGroupDelimited { get; set; }
        public string KPIDelimited { get; set; }
        public int? EEMin { get; set; }
        public int? EEMax { get; set; }
        public int? MEMin { get; set; }
        public int? MEMax { get; set; }
        public int? SBEMin { get; set; }
        public int? SBEMax { get; set; }
        public int? BEMin { get; set; }
        public int? BEMax { get; set; }
        public bool IsExport { get; set; }
    }

}