using System;
using System.Collections.Generic;
using System.Text;
using Utilities.API;

namespace EMS.Recruitment.Transfer.Applicant
{
    public class GetIDByAutoCompleteOutput
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }

    public class GetListOutput : JQGridResult
    {
        public int ID { get; set; }
        public string ScopeOrgGroup { get; set; }
        public string ApplicantName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string ApplicationSource { get; set; }
        public string MRFTransactionID { get; set; }
        public string WorkflowStatus { get; set; }
        public string CurrentStep { get; set; }
        public string DateScheduled { get; set; }
        public string DateCompleted { get; set; }
        public string ApproverRemarks { get; set; }
        public string WorkflowDescription { get; set; }
        public string PositionRemarks { get; set; }
        public string Course { get; set; }
        public string CurrentPositionTitle { get; set; }
        public decimal? ExpectedSalary { get; set; }
        public string DateApplied { get; set; }
        public int EmployeeID { get; set; }
        public string BirthDate { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string CellphoneNumber { get; set; }
        public string ReferredBy { get; set; }
    }
    
    public class GetApprovalListOutput : JQGridResult
    {
        public int ID { get; set; }
        public int WorkflowID { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicationSource { get; set; }
        public string CurrentStep { get; set; }
        //public string WorkflowDescription { get; set; }
        public string PositionRemarks { get; set; }
        public string Course { get; set; }
        public string CurrentPositionTitle { get; set; }
        public decimal? ExpectedSalary { get; set; }
        public string DateApplied { get; set; }
        public bool HasApproval { get; set; }
    }

    public class GetHistoryOutput
    {
        public int ID { get; set; }
        public int Order { get; set; }
        public string Step { get; set; }
        public string Status { get; set; }
        public string Timestamp { get; set; }
        public string Remarks { get; set; }
        public string ResultType { get; set; }
    }

    // GET THE LAST INSERTED DATA
    public class LastId
    {
        public int Id { get; set; }
    }
    public class GetApplicantLegalProfileOutput
    {
        public int? RowNum { get; set; }
        public int? ApplicantID { get; set; }
        public string Description { get; set; }
        public string LegalAnswer { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

}
