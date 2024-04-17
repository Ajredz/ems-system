using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Utilities.API;

namespace EMS.Workflow.Transfer.Accountability
{
    public class GetAccountabilityListInput : JQGridFilter
    {
        public int? ID { get; set; }

        public string PreloadName { get; set; }

        public string DateCreatedFrom { get; set; }

        public string DateCreatedTo { get; set; }

        public bool IsExport { get; set; }
    }

    public class AccountabilityForm
    {
        public int ID { get; set; }
        public string PreloadedName { get; set; }
        public int CreatedBy { get; set; }
        public List<AccountabilityDetails> AccountabilityDetailsList { get; set; }
    }

    public class AccountabilityDetails
    {
        public int ID { get; set; }
        public int AccountabilityID { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int OrgGroupID { get; set; }
        public int PositionID { get; set; }
        public int EmployeeID { get; set; }
        public string OrgGroupDescription { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class EmployeeAccountabilityForm
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string StatusUpdatedDate { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public int OrgGroupID { get; set; }
        public string OrgGroupDescription { get; set; }
        public int PositionID { get; set; }
        public string PositionDescription { get; set; }
        public int ApproverEmployeeID { get; set; }
        public string ApproverEmployeeName { get; set; }
    }

    public class TagToEmployeeForm
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public int OrgGroupID { get; set; }
        public int PositionID { get; set; }
        public int ApproverEmployeeID { get; set; }
    }

    public class EmployeeAccountabilityCommentsForm
    {
        public int EmployeeAccountabilityID { get; set; }
        public string Comments { get; set; }
        public int CreatedBy { get; set; }
        public bool IsExternal { get; set; }
    }

    public class EmployeeAccountabilityAttachmentForm
    {
        public int EmployeeAccountabilityID { get; set; }
        public List<AttachmentForm> AddAttachmentForm { get; set; }
        public List<AttachmentForm> DeleteAttachmentForm { get; set; }
    }

    public class AttachmentForm
    {
        public string AttachmentType { get; set; }

        public string Remarks { get; set; }

        [IgnoreDataMember]
        public IFormFile File { get; set; }

        public string SourceFile { get; set; }

        public string ServerFile { get; set; }

        public int CreatedBy { get; set; }

        public string UploadedBy { get; set; }

        public string Timestamp { get; set; }
    }

    public class AddEmployeePreLoadedAccountabilityInput
    {
        public List<int> AccountabilityPreloadedIDs { get; set; }
        public int EmployeeID { get; set; }
        public int PositionID { get; set; }
    }

    public class GetMyAccountabilitiesListInput : JQGridFilter
    {
        public int ID { get; set; }
        public string CreatedDateFrom { get; set; }
        public string CreatedDateTo { get; set; }
        public string EmployeeIDDelimited { get; set; }
        public string SeparationDateFrom { get; set; }
        public string SeparationDateTo { get; set; }
        public string HiredDateFrom { get; set; }
        public string HiredDateTo { get; set; }
        public string Title { get; set; }
        public string StatusDelimited { get; set; }
        public string StatusUpdatedDateFrom { get; set; }
        public string StatusUpdatedDateTo { get; set; }
        public string EmployeeOrgDelimited { get; set; }
        public string EmployeeOrgRegionDelimited { get; set; }
        public string EmployeePosDelimited { get; set; }
        public string ClearingOrgDelimited { get; set; }
        public string StatusUpdatedByDelimited { get; set; }
        public string StatusRemarks { get; set; }
        public string LastComment { get; set; }
        public string LastCommentDateFrom { get; set; }
        public string LastCommentDateTo { get; set; }

        public bool OpenStatusOnly { get; set; }
        public bool IsAdminAccess { get; set; }
        public bool IsClearance { get; set; }
        public bool IsExport { get; set; }
        public string OrgGroupDescendantAccess { get; set; }
        public string PositionAccess { get; set; }
        public string EmployeeIDDescendantAccess { get; set; }
        public int MyEmployeeID { get; set; }
    }

    public class BatchAccountabilityAddInput
    {
        public int EmployeeID { get; set; }
        public Transfer.Enums.AccountabilityStatus Status { get; set; }
    }

    public class AccountabilityUploadFile
    {
        public string RowNum { get; set; }
        public int EmployeeID { get; set; }
        public string OldEmployeeID { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public int OrgGroupID { get; set; }
        public string OrgGroupCode { get; set; }
        public string Status { get; set; }
        public DateTime StatusUpdatedDate { get; set; } 
        public int UploadedBy { get; set; }
    }

    public class ChangeStatusInput
    { 
        public List<long> ID { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public bool IsExternal { get; set; }
    }
    public class GetEmployeeAccountabilityStatusPercentageOutput
    { 
        public int EmployeeID { get; set; }
        public int OverDone { get; set; }
        public int OverAll { get; set; }
        public string Status { get; set; }
    }
    public class GetEmployeeAccountabilityStatusPercentageInput
    {
        public List<int> EmployeeIDs { get; set; }
        public string ClearingOrgIDDelimited { get; set; }
    }
    public class GetAccountabilityDashboardInput
    { 
        public string DashboardData { get; set; }
        public string OrgGroupID { get; set; }
        public string PositionID { get; set; }
        public string EmploymentStatus { get; set; }
        public string DateFilter { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string AccessOrg { get; set; }
    }
    public class PostClearedEmployeeCommentsInput
    { 
        public int ClearedEmployeeID { get; set; }
        public string Comments { get; set; }
    }
    public class PostClearedEmployeeComputationInput
    {
        public int ClearedEmployeeID { get; set; }
        public string Computation { get; set; }
    }
    public class PostClearedEmployeeStatusInput
    {
        public List<int> ID { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
    public class ClearedEmployeeListInput : JQGridFilter
    {
        public int ID { get; set; }
        public string EmployeeID { get; set; }
        public string OrgGroupID { get; set; }
        public string PositionID { get; set; }
        public string Status { get; set; }
        public string StatusUpdatedByID { get; set; }
        public string StatusUpdatedDateFrom { get; set; }
        public string StatusUpdatedDateTo { get; set; }
        public string StatusRemarks { get; set; }
        public string Computation { get; set; }
        public string LastComment { get; set; }
        public string LastCommentFrom { get; set; }
        public string LastCommentTo { get; set; }

        public bool IsExport { get; set; }

    }

}
