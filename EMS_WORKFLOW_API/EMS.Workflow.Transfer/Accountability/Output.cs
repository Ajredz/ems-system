using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.Workflow.Transfer.Accountability
{
    public class GetAccountabilityListOutput : JQGridResult
    {
        public int? ID { get; set; }

        public string PreloadName { get; set; }

        public string DateCreated { get; set; }
    }

    public class GetDetailsByIDOutput
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int OrgGroupID { get; set; }
        public int PositionID { get; set; }
        public int EmployeeID { get; set; }
        public string OrgGroupDescription { get; set; }
        public string PositionDescription { get; set; }
        public string EmployeeName { get; set; }
    }

    public class GetEmployeeAccountabilityByEmployeeIDOutput
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int OrgGroupID { get; set; }
        public int PositionID { get; set; }
        public int ApproverEmployeeID { get; set; }
        public string Status { get; set; }
        public int StatusUpdatedBy { get; set; }
        public string StatusUpdatedDate { get; set; }
        public string StatusRemarks { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string LastComment { get; set; }
        public string LastCommentDate { get; set; }

        public string StatusDescription { get; set; }
        public string StatusColor { get; set; }
        public string StatusUpdatedByName { get; set; }
        public string StatusUpdatedByNameNoCode { get; set; }
        public string OrgGroupDescription { get; set; }
    }

    public class EmployeeAccountabilityGetCommentsOutput
    {
        public string Timestamp { get; set; }

        public string Sender { get; set; }

        public int CreatedBy { get; set; }

        public string Comments { get; set; }
    }

    public class GetEmployeeAccountabilityStatusHistoryOutput
    {
        public string Status { get; set; }
        public string Timestamp { get; set; }
        public int UserID { get; set; }
        public string User { get; set; }
        public string Remarks { get; set; }
    }

    public class GetMyAccountabilitiesListOutput : JQGridResult
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int OrgGroupID { get; set; }
        public int PositionID { get; set; }
        public int ApproverEmployeeID { get; set; }
        public string Status { get; set; }
        public int StatusUpdatedBy { get; set; }
        public string StatusUpdateDate { get; set; }
        public string StatusRemarks { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string LastComment { get; set; }
        public string LastCommentDate { get; set; }

        public string EmployeeName { get; set; }
        public string EmployeeStatusUpdatedDate { get; set; }
        public string EmployeeDatehired { get; set; }
        public string StatusDescription { get; set; }
        public string StatusColor { get; set; }
        public string EmployeeOrg { get; set; }
        public string EmployeeOrgRegion { get; set; }
        public string EmployeePos { get; set; }
        public string ClearingOrg { get; set; }
        public string StatusUpdatedByName { get; set; }
        public string OldEmployeeID { get; set; }
        public string Over { get; set; }
        public string FinalStatus { get; set; }
    }
    public class GetEmployeeID
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
    }
    public class GetAccountabilityDashboardOutput
    {
        public string Description { get; set; }
        public int Actual { get; set; }
        public int Target { get; set; }
    }

    public class ClearedEmployeeOutput
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string Accountability { get; set; }
        public string Status { get; set; }
        public int StatusUpdatedBy { get; set; }
        public DateTime? StatusUpdatedDate { get; set; }
        public string StatusRemarks { get; set; }
        public string Computation { get; set; }
        public string Agreed { get; set; }
        public string LastComment { get; set; }
        public DateTime? LastCommentDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
    public class ClearedEmployeeListOutput : JQGridResult
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string OrgGroup { get; set; }
        public string Position { get; set; }
        public string Accountability { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string StatusUpdatedBy { get; set; }
        public string StatusUpdatedDate { get; set; }
        public string StatusRemarks { get; set; }
        public string Computation { get; set; }
        public string Agreed { get; set; }
        public string AgreedDate { get; set; }
        public string LastComment { get; set; }
        public string LastCommentDate { get; set; }
        public string StatusColor { get; set; }
    }
    public class ClearedEmployeeByIDOutput
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string FullName { get; set; }
        public string OrgGroup { get; set; }
        public string Position { get; set; }
        public string Accountability { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string StatusUpdatedBy { get; set; }
        public string StatusUpdatedDate { get; set; }
        public string Computation { get; set; }
        public string Agreed { get; set; }
        public string AgreedDate { get; set; }
        public string DateHired { get; set; }
    }
    public class GetClearedEmployeeCommentsOutput
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Comments { get; set; }
        public string CreatedDate { get; set; }
    }
    public class GetClearedEmployeeStatusHistoryOutput
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string Remarks { get; set; }
    }
    public class GetEmployeeAccountabilityOutput
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string OrgGroup { get; set; }
        public string Status { get; set; }
        public string StatusUpdatedDate { get; set; }
        public string StatusUpdatedBy { get; set; }
        public string Remarks { get; set; }
    }
}
