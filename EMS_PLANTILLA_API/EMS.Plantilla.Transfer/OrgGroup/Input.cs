using EMS.Plantilla.Transfer.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Utilities.API;

namespace EMS.Plantilla.Transfer.OrgGroup
{
    public class Form
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? CSODAM { get; set; }
        public int? HRBP { get; set; }
        public int? RRT { get; set; }
        public string Category { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string OrgType { get; set; }
        public string Psgc { get; set; }
        public string Address { get; set; }
        public string BranchSize { get; set; }
        public string ParkingSize { get; set; }
        public string Sign { get; set; }
        public string Page { get; set; }
        public bool IsBranchActive { get; set; }
        public int ServiceBayCount { get; set; }
        public int? ParentOrgID { get; set; }
        public string ParentOrgDescription { get; set; }
        public int CreatedBy { get; set; }
        public List<OrgGroupTagForm> OrgGroupTagList { get; set; }
        public List<int> ChildrenOrgIDList { get; set; }
        public List<OrgGroupPositionForm> OrgGroupPositionList { get; set; }
        public List<OrgGroupNPRFForm> OrgGroupNPRFList { get; set; }
    }

    public class OrgGroupTagForm
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
    }

    public class PlantillaCountUpdateForm
    {
        public int OrgGroupID { get; set; }
        public int PositionID { get; set; }
        public string PositionDescription { get; set; }
        public int PlannedCount { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int ModifiedBy { get; set; }
        public int ReportingPositionID { get; set; }
        public string ReportingPositionDescription { get; set; }
        public bool IsHead { get; set; }
    }

    public class OrgGroupPositionForm
    {
        public int PositionLevelID { get; set; }
        public int PositionID { get; set; }
        public int PlannedCount { get; set; }
        public int ActiveCount { get; set; }
        public int ActiveProbCount { get; set; }
        public int OutgoingCount { get; set; }
        public int TotalActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public bool IsHead { get; set; }
        public int ReportingPositionID { get; set; }
        public string PositionDescription { get; set; }
        public string ReportingPositionDescription { get; set; }
    }

    public class OrgGroupNPRFForm
    {
        public int OrgGroupID { get; set; }
        public string NPRFNumber { get; set; }
        public DateTime ApprovedDate { get; set; }

        [IgnoreDataMember]
        public IFormFile File { get; set; }
        public string SourceFile { get; set; }
        public string ServerFile { get; set; }
        public int CreatedBy { get; set; }
        public string UploadedBy { get; set; }
        public string Timestamp { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public int? ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string OrgTypeDelimited { get; set; }
        public string ParentOrgDescription { get; set; }
        public string IsBranchActive { get; set; }
        public int? ServiceBayCountMin { get; set; }
        public int? ServiceBayCountMax { get; set; }
        public AdminAccess AdminAccess { get; set; }
    }
    public class GetBranchInfoListInput : JQGridFilter
    {
        public int? ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string Address { get; set; }
        public string OrgTypeDelimited { get; set; }
        public string ParentOrgDescription { get; set; }
        public string IsBranchActive { get; set; }
        public int? ServiceBayCountMin { get; set; }
        public int? ServiceBayCountMax { get; set; }
        public AdminAccess AdminAccess { get; set; }
    }

    public class GetEmployeeListInput : JQGridFilter
    {
        public int? ID { get; set; }
        public string PositionDelimited { get; set; }
        public string EmployeeName { get; set; }
    }

    public class GetPlantillaCountInput : JQGridFilter
    {
        public string OrgType { get; set; }
        public int? OrgGroupID { get; set; }
        public string OrgGroupDelimited { get; set; }
        public string ScopeOrgType { get; set; }
        public string ScopeOrgGroupDelimited { get; set; }
        public string PositionDelimited { get; set; }
        public int PlannedMin { get; set; }
        public int PlannedMax { get; set; }
        public int ActiveMin { get; set; }
        public int ActiveMax { get; set; }
        public int InactiveMin { get; set; }
        public int InactiveMax { get; set; }
        public int VarianceMin { get; set; }
        public int VarianceMax { get; set; }
        public bool IsExport { get; set; }
        public AdminAccess AdminAccess { get; set; }
    }

    public class GetByIDInput
    {
        public int ID { get; set; }
    }

    public class GetChartInput
    {
        public int OrgGroupID { get; set; }

        public int Depth { get; set; }

        public bool ShowClosedBranches { get; set; }

        public AdminAccess AdminAccess { get; set; }
    }

    public class GetDropDownInput
    {
        public int ID { get; set; }

    }

    public class GetByOrgGroupIDAndPositionIDInput
    {
        public int OrgGroupID { get; set; }
        public int PositionID { get; set; }
    }

    public class GetByOrgTypeAutoCompleteInput
    {
        public string Term { get; set; }
        public string OrgType { get; set; }
        public int TopResults { get; set; }
    }
    
    public class GetPositionOrgGroupUpwardAutoCompleteInput
    {
        public string Term { get; set; }
        public int OrgGroupID { get; set; }
        public int TopResults { get; set; }
    }

    public class GetAutoCompleteInput
    {
        public string Term { get; set; }
        public int TopResults { get; set; }
    }

    public class GetOrgGroupNPRFListInput : JQGridFilter
    {
        public int ID { get; set; }
        public string NPRFNumber { get; set; }
        public string DateApprovedFrom { get; set; }
        public string DateApprovedTo { get; set; }
    }

    public class ValidateMRFExistingActualInput
    {
        public int OrgGroupID { get; set; }
        public int PositionID { get; set; }
        public int PlannedCount { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
    }

    public class OrgGroupHistoryListInput : JQGridFilter
    { 
        public string TDateFrom { get; set; }
        public string TDateTo { get; set; }
        public string IsLatest { get; set; }
    }
    public class OrgGroupHistoryByDateInput : JQGridFilter
    {
        public string TDate { get; set; }
        public string IsLatest { get; set; }
    }
    public class AddOrgGroupHistoryInput
    {
        public int ID { get; set; }
        public string TDate { get; set; }
        public bool IsLatest { get; set; }
        public string IsLatestDescription { get; set; }
        public string Description { get; set; }
        public int ParentOrgId { get; set; }
        public string ParentDescription { get; set; }
        public string CodeDescription { get; set; }
        public string ParentCodeDescription { get; set; }
        public string Code { get; set; }
        public string ParentCodeDescriptionValue { get; set; }
    }
    public class GetOrgGroupParentInput 
    {
        public List<int> OrgGroupIDs { get; set; }
        public string OrgType { get; set; }
    }
}