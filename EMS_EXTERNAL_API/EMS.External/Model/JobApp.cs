using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.External.API.Model
{


    public class MRFForm
    {
        public int ID { get; set; }

        public string Status { get; set; }
        public string StatusCode { get; set; }

        public string MRFTransactionID { get; set; }

        public string OldMRFID { get; set; }

        public int OrgGroupID { get; set; }

        public int PositionLevelID { get; set; }
        public bool IsConfidential { get; set; }

        public string NatureOfEmploymentValue { get; set; }

        public int PositionID { get; set; }

        public string PurposeValue { get; set; }

        public int Vacancy { get; set; }

        public int TurnaroundTime { get; set; }

        public string Remarks { get; set; }

        public string Requester { get; set; }

        public int CreatedBy { get; set; }

        public string DateCreated { get; set; }

        public string DateModified { get; set; }
        public string DateApproved { get; set; }

        public int ApproverPositionID { get; set; }

        public int ApproverOrgGroupID { get; set; }

        public int AltApproverPositionID { get; set; }

        public int AltApproverOrgGroupID { get; set; }

        public List<MRFSignatoriesForm> Signatories { get; set; }

        public string ReasonForCancellation { get; set; }
        public string OnlinePosition { get; set; }
        public string OnlineLocation { get; set; }
        public string OnlineJobDescription { get; set; }
        public string OnlineJobQualification { get; set; }
        public bool IsAvailableOnline { get; set; }
    }
    public class MRFSignatoriesForm
    {
        public string ApproverDescription { get; set; }

        public int ApproverRoleID { get; set; }

        public int ApprovalActualTAT { get; set; }

        public string ApprovalStatus { get; set; }
    }
    public class UpdateMRFTransactionIDForm
    {
        public string MRFTransactionID { get; set; }
        public int HiredApplicantID { get; set; }
        public List<int> ApplicantIDs { get; set; }
    }

    public class UpdateCurrentWorkflowStepInput
    {
        public int ApplicantID { get; set; }
        public string CurrentStepCode { get; set; }
        public string CurrentStepDescription { get; set; }
        public string CurrentStepApproverRoleIDs { get; set; }
        public string WorkflowStatus { get; set; }
        public string DateScheduled { get; set; }
        public string DateCompleted { get; set; }
        public string ApproverRemarks { get; set; }
    }

    //API FOR GET ONLINE MRF
    public class OutputMrfOnline
    {
        public string Position { get; set; }
        public int PositionID { get; set; }
        public string Location { get; set; }
        public string JobDescription { get; set; }
        public string JobQualification { get; set; }
        public int MrfId { get; set; }
        public string ClosedDate { get; set; }
        public int ApplicantCount { get; set; }
        public string MrfCreatedDate { get; set; }
    }

    //API FOR GET COURSE
    public class GetReferenceValueListOutput
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    //API FOR POST APPLICANT
    public class MRFPickApplicantFormOnline
    {
        public MRFApplicantDetailsOnline Applicants { get; set; }
        public List<AddApplicantLegalProfileInput> addApplicantLegalProfileInputs { get; set; }
        public List<AddApplicantKickoutQuestionInput> addApplicantKickoutQuestionInputs { get; set; }
    }

    public class MRFApplicantDetailsOnline
    {
        public int MrfId { get; set; }
        public string PositionRemarks { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string PSGCRegionCode { get; set; }
        public string PSGCProvinceCode { get; set; }
        public string PSGCCityMunicipalityCode { get; set; }
        public string PSGCBarangayCode { get; set; }
        public string AddressLine1 { get; set; }
        public string Email { get; set; }
        public string CellphoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Course { get; set; }
        public string CurrentPosition { get; set; }
        public string ExpectedSalary { get; set; }

        //public string CurrentCompany { get; set; }
        //public string CurrentCompanyAddress { get; set; }
        public string Resume { get; set; }
        public string ResumeAttachment { get; set; }
        public string ApplicationSource { get; set; }
    }

    public class Form
    {
        public int ID { get; set; }

        public string PositionRemarks { get; set; }

        public string ReferredByUserIDDescription { get; set; }

        public int ReferredByUserID { get; set; }

        //public int WorkflowID { get; set; }

        //public string CurrentStepCode { get; set; }

        //public string CurrentStepDescription { get; set; }

        //public string CurrentStepApproverRoleIDs { get; set; }

        public string ApplicationSource { get; set; }

        public string MRFTransactionID { get; set; }

        public decimal? ExpectedSalary { get; set; }

        public DateTime DateApplied { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public PersonalInformation PersonalInformation { get; set; }

        public List<Attachments> Attachments { get; set; }

    }
    public class PersonalInformation
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Suffix { get; set; }

        public string CurrentPosition { get; set; }

        public string Course { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        //public string GeographicalRegion { get; set; }

        public string PSGCRegionCode { get; set; }
        public string PSGCProvinceCode { get; set; }
        public string PSGCCityMunicipalityCode { get; set; }
        public string PSGCBarangayCode { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string CellphoneNumber { get; set; }
    }

    public class Attachments
    {
        public int ApplicantID { get; set; }

        public string AttachmentType { get; set; }

        public string Remarks { get; set; }

        [IgnoreDataMember]
        public IFormFile File { get; set; }

        public string SourceFile { get; set; }

        public string ServerFile { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }
    }

    public class MRFPickApplicantForm
    {
        public string MRFTransactionID { get; set; }
        public int ID { get; set; }
        public int WorkflowID { get; set; }
        public List<MRFApplicantDetails> Applicants { get; set; }
    }

    public class MRFApplicantDetails
    {
        public int ApplicantID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }

        public string CurrentStepCode { get; set; }
        public string CurrentStepDescription { get; set; }
        public string CurrentStepApproverRoleIDs { get; set; }
        public string ApproverRemarks { get; set; }
        public string DateCompleted { get; set; }
        public string DateScheduled { get; set; }
        public string ResultType { get; set; }
    }

    public class LastId 
    { 
        public int Id { get; set; }
    }

    public class UpdateMrfCurrentWorkflowStepInput
    {
        public long ID { get; set; }
        public string CurrentStepCode { get; set; }
        public string CurrentStepDescription { get; set; }
        public string WorkflowStatus { get; set; }
        public string CurrentStepApproverRoleIDs { get; set; }
        public string ResultType { get; set; }
        public string DateScheduled { get; set; }
        public string DateCompleted { get; set; }
        public string ApproverRemarks { get; set; }

    }

    public class AddWorkflowTransaction
    {
        public int RecordID { get; set; }

        public int WorkflowID { get; set; }
        public string WorkflowCode { get; set; }

        public List<int> BatchUpdateRecordIDs { get; set; }
        public List<int> BatchUpdateApplicantIDs { get; set; }

        public string CurrentStepCode { get; set; }

        public string Result { get; set; }
        public string Remarks { get; set; }

        public string DateScheduled { get; set; }

        public string DateCompleted { get; set; }
        public DateTime? StartDatetime { get; set; }

    }
    public class ApplicantEmailLogsInput
    {
        public int ID { get; set; }
        public string CreatedBy { get; set; }
        public string SenderName { get; set; }

        public string FromEmailAddress { get; set; }

        public string ToEmailAddress { get; set; }

        public string JobTitle { get; set; }

        public string ApplicantName { get; set; }

        public string Status { get; set; }

        public string SystemCode { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
    public class ApplicantLegalProfileInput
    {
        public int ApplicantID { get; set; }
        public List<AddApplicantLegalProfileInput> addApplicantLegalProfileInputs { get; set; }
    }
    public class AddApplicantLegalProfileInput
    {
        public string LegalNumber { get; set; }
        public string LegalAnswer { get; set; }
    }

    public class KickoutQuestion
    { 
        public int ID { get; set; }
        public string Question { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
    }
    public class ApplicantKickoutQuestionInput
    {
        public int ApplicantID { get; set; }
        public List<AddApplicantKickoutQuestionInput> addApplicantKickoutQuestionInputs { get; set; }
    }
    public class AddApplicantKickoutQuestionInput
    {
        public int Question { get; set; }
        public string Answer { get; set; }
    }

    public class EmailForm
    {
        public string subject { get; set; }
        public string body { get; set; }
        public string template { get; set; }
        public List<EmailDetails> from { get; set; }
        public List<EmailDetails> to { get; set; }
        public List<EmailDetails> cc { get; set; }
        public List<EmailDetails> bcc { get; set; }
    }
    public class EmailDetails
    { 
        public string email { get; set; }
        public string name { get; set; }
    }

    public class GetResult
    { 
        public List<Data> data { get; set; }
        public int response { get; set; }

    }
    public class Data
    { 
        public int id { get; set; }
        public string system { get; set; }
        public string token { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }
}
