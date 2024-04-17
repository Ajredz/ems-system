

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace EMS.External.API.Model
{
    public class FormUser
    {
        public int ID { get; set; }

        public short CompanyID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public short FailedAttempt { get; set; }

        public bool IsPasswordChanged { get; set; }

        public bool IsLoggedIn { get; set; }

        public string IntegrationKey { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LastLoggedIn { get; set; }

        public DateTime? LastLoggedOut { get; set; }

        public DateTime? LastPasswordChange { get; set; }

        public List<int> RoleIDs { get; set; }

    }
    public class InputEmployeeLogin
    {
        public string EmployeeId { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
    }
    public class OutputEmployeeLogin
    {
        /*public int Id { get; set; }
        public int SystemAccessId { get;set;}
        public string EmployeeId { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }*/


        public int ID { get; set; }
        public int SystemUserID { get; set; }
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string DateHired { get; set; }
        public string ResignedDate { get; set; }
        public int PositionID { get; set; }
        public string PositionCode { get; set; }
        public string PositionTitle { get; set; }
        public int OrgGroupID { get; set; }
        public string OrgCode { get; set; }
        public string OrgDescription { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyDescription { get; set; }
        public string AccountabilityStatus { get; set; }
        public string TotalAccountability { get; set; }
        public bool Agreed { get; set; }
        public string AgreedDate { get; set; }
    }

    public class OutputEmployeeAccountability
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

        public string StatusColor { get; set; }
        public string StatusDescription { get; set; }
        public string OrgGroupDescription { get; set; }
    }
    public class OutputPositionName
    {
        public int ID { get; set; }
        public int PositionLevelID { get; set; }
        public string PositionLevelDescription { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public int? ParentPositionID { get; set; }
        public string ParentPositionDescription { get; set; }
        public string JobClassCode { get; set; }
        public string JobClassDescription { get; set; }
        public int CreatedBy { get; set; }
    }

    public class OutputClearance
    {
        public OutputEmployeeLogin Profile { get; set; }
        public List<OutputEmployeeAccountability> EmployeeAccountability { get; set; }
    }
    public class EmployeeAccountabilityGetCommentsInput
    {
        public int ID { get; set; }
    }
    public class EmployeeAccountabilityGetCommentsOutput
    {
        public string Timestamp { get; set; }

        public string Sender { get; set; }

        public int CreatedBy { get; set; }

        public string Comments { get; set; }
    }
    public class EmployeeAccountabilityCommentsForm
    {
        public int EmployeeAccountabilityID { get; set; }
        public string Comments { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsExternal { get; set; }
    }
    public class UpdateEmployeeEmailInput
    {
        public int ID { get; set; }
        public string Email { get; set; }
    }
    public class OrgForm
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? CSODAM { get; set; }
        public int? HRBP { get; set; }
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
    public class OrgGroupPositionForm
    {
        public int PositionLevelID { get; set; }
        public int PositionID { get; set; }
        public int PlannedCount { get; set; }
        public int ActiveCount { get; set; }
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

    public class ChangeStatusInput
    {
        public List<long> ID { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public bool IsExternal { get; set; }
    }
    public class StatusOutput
    {
        public string Code { get; set; }
        public List<StatusColorOutput> workflowStepList { get; set; }
    }
    public class StatusColorOutput
    {
        public string StepCode { get; set; }
        public string StepDescription { get; set; }
        public string StatusColor { get; set; }
    }

    public class GetQuestionOutput
    {
        public int QuestionID { get; set; }
        public string Code { get; set; }
        public string Question { get; set; }
        public string AnswerType { get; set; }
        public int ParentQuestionID { get; set; }
        public int Tab { get; set; }
        public int Order { get; set; }
        public string AnswerID { get; set; }
        public string Answer { get; set; }
        public string AddReason { get; set; }
    }

    public class QuestionTableOutput
    {
        public List<GetQuestionTable> MainQuestion { get; set; }
    }

    public class GetQuestionTable
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string Code { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string AnswerType { get; set; }
        public int ParentQuestionID { get; set; }
        public int Tab { get; set; }
        public int Order { get; set; }
        public bool IsRequired { get; set; }
        public List<GetQuestionTable> SubQuestion { get; set; }
        public List<AnswerTable> Answer { get; set; }
    }

    public class AnswerTable
    {
        public int ID { get; set; }
        public int QuestionID { get; set; }
        public string Answer { get; set; }
        public bool AddReason { get; set; }
    }

    public class QuestionEmployeeAnswerInput
    {
        public int EmployeeID { get; set; }
        public int QuestionID { get; set; }
        public int AnswerID { get; set; }
        public string AnswerDetails { get; set; }
    }
    public class ClearedEmployee
    { 
        public int ID { get; set; }
        public string Computation { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public List<ClearedEmployeeCommentsOutput> clearedEmployeeCommentsOutput { get; set; }
    }
    public class ClearedEmployeeCommentsOutput
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Comments { get; set; }
        public string CreatedDate { get; set; }
    }
    public class ClearedEmployeeCommentsInput
    {
        public int ClearedEmployeeID { get; set; }
        public string Comments { get; set; }
    }
}
