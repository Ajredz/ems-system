using System.Collections.Generic;
using Utilities.API;

namespace EMS.Plantilla.Transfer.OrgGroup
{
    public class GetListOutput : JQGridResult
    {
        public int? ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string OrgTypeDescription { get; set; }
        public string ParentOrgDescription { get; set; }
        public string IsBranchActive { get; set; }
        public int ServiceBayCount { get; set; }
    }
    public class GetBranchListListOutput : JQGridResult
    {
        public int? ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string Address { get; set; }
        public string OrgTypeDescription { get; set; }
        public string ParentOrgDescription { get; set; }
        public string IsBranchActive { get; set; }
        public int ServiceBayCount { get; set; }
    }

    public class GetDropDownOutput
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class GetChartOutput
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string OrgType { get; set; }
        public int? ParentOrgID { get; set; }
    }

    public class GetChartPositionOutput
    {
        public int ID { get; set; }
        public string PositionCode { get; set; }
        public string PositionTitle { get; set; }
        public int PlannedCount { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public bool IsHead { get; set; }
    }

    public class OrgGroupTagDynamicFieldOutput
    {
        public string RefCode { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }

        public List<GetDropDownOutput> DropDownOptions { get; set; }
    }


    public class GetExportCountByOrgTypeDataOutput
    {
        public string OrgGroupTypeDescription { get; set; }
        public List<PositionManpowerCount> PositionManpowerCount { get; set; }
    }

    public class GetExportCountByOrgIDDataOutput
    {
        public string SelectedOrgType { get; set; }
        public string SelectedOrgGroupCode { get; set; }
        public string SelectedOrgGroupDescription { get; set; }
        public List<PositionManpowerCount> PositionManpowerCount { get; set; }
    }

    public class PositionManpowerCount
    {
        public string OrgGroupCode { get; set; }
        public string OrgGroupDescription { get; set; }
        public string PositionCode { get; set; }
        public string PositionTitle { get; set; }
        public int PlannedCount { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }

    }

    public class GetByOrgGroupIDAndPositionIDOutput
    {
        public int OrgGroupID { get; set; }
        public int PositionID { get; set; }
        public int PlannedCount { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public bool IsHead { get; set; }
    }

    public class GetExportListOutput
    {
        public string ParentOrgGroup { get; set; }
        public string OrgGroupCode { get; set; }
        public string OrgGroupDesc { get; set; }
        public string OrgType { get; set; }
        public string Address { get; set; }
        public bool IsBranchActive { get; set; }
        public int ServiceBayCount { get; set; }
        public string CompanyTag { get; set; }
        public string PositionCode { get; set; }
        public string ReportingPositionCode { get; set; }
        public int? PlannedCount { get; set; }
        public int? ActiveCount { get; set; }
        public int? InactiveCount { get; set; }
        public string IsHead { get; set; }
    }

    public class GetPlantillaCountOutput : JQGridResult
    {
        public int ScopeOrgGroupID { get; set; }
        public string ScopeOrgGroup { get; set; }
        public int OrgGroupID { get; set; }
        public string OrgGroup { get; set; }
        public int PositionID { get; set; }
        public string Position { get; set; }
        public int PlannedCount { get; set; }
        public int ActiveCount { get; set; }
        public int ActiveProbCount { get; set; }
        public int OutgoingCount { get; set; }
        public int TotalActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int VarianceCount { get; set; }
        public int TotalPlanned { get; set; }
        public int TotalActiveReg { get; set; }
        public int TotalActiveProb { get; set; }
        public int TotalActive { get; set; }
        public int TotalInactive { get; set; }
        public int TotalOutgoing { get; set; }
        public int TotalVariance { get; set; }
    }

    public class GetIDByOrgTypeAutoCompleteOutput
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }

    public class GetOrgGroupHierarchyOutput
    {
        public int ID { get; set; }
        public int ParentOrgID { get; set; }
        public int HierarchyLevel { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string OrgType { get; set; }
    }

    public class GetOrgGroupEmployeeOutput : JQGridResult
    {
        public string Position { get; set; }
        public string EmployeeName { get; set; }
    }

    public class GetIDByAutoCompleteOutput
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }

    public class GetOrgGroupNPRFOutput : JQGridResult
    {
        public string NPRFNumber { get; set; }
        public string ApprovedDate { get; set; }
    }

    public class GetOrgGroupRollupPositionDropdownOutput
    {
        public int ID { get; set; }
        public int OrgGroupID { get; set; }
        public string OrgGroup { get; set; }
        public string Position { get; set; }
        public int PlannedCount { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int VarianceCount { get; set; }
    }

    public class GetRegionByOrgGroupIDOutput
    {
        public string Region { get; set; }
        public string RegionCode { get; set; }
        public decimal MonthlyRate { get; set; }
    }

    public class GetPositionOrgGroupUpwardAutoCompleteOutput
    {
        public int ID { get; set; }
        public string Position { get; set; }
        public string OrgGroup { get; set; }
    }

    public class GetOrgGroupHistoryListOutput : JQGridResult
    {
        public int? ID { get; set; }

        public string TDate { get; set; }

        public bool IsLatest { get; set; }
        public string IsLatestDescription { get; set; }

    }

    public class GetOrgGroupHistoryByDateOutput : JQGridResult
    {
        public int? ID { get; set; }
        public string TDate { get; set; }
        public bool IsLatest { get; set; }
        public string OrgType { get; set; }
        public string IsLatestDescription { get; set; }
        public string Description { get; set; }
        public int ParentOrgId { get; set; }
        public string ParentDescription { get; set; }
        public string ParentDescriptionDisplay { get; set; }
        public string CodeDescription { get; set; }
        public string ParentCodeDescription { get; set; }
        public string Code { get; set; }
        public string ParentCodeDescriptionValue { get; set; }
    }

    public class CorporateEmailOutput
    {
        public string Email { get; set; }
    }

    public class OrgGroupFormatOutput
    { 
        public int ID { get; set; }
        public string Result { get; set; }
    }
    public class OrgGroupSOMDOutput
    {
        public int ID { get; set; }
        public string Clus { get; set; }
        public string Area { get; set; }
        public string Reg { get; set; }
        public string Zone { get; set; }
    }
}