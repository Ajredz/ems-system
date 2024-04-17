using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Utilities.API;

namespace EMS.Recruitment.Transfer.Applicant
{
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

        public List<ApplicationHistory> ApplicationHistory { get; set; }

        [IgnoreDataMember]
        public List<DeletedAttachments> DeletedAttachments { get; set;}
    }

    public class UpdateStatusForm
    {
        public int ID { get; set; }

        public int WorkflowID { get; set; }

        public int CreatedBy { get; set; }

        public int CurrentStep { get; set; }

        public string CurrentResult { get; set; } 

        public List<ApplicationHistory> ApplicationHistory { get; set; }

    }

    public class DeletedAttachments
    {
        public string ServerFile { get; set; }
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

    public class ApplicationHistory
    {
        public int OrderNo { get; set; }
        public string Step { get; set; }
        public string Result { get; set; }
        public string Timestamp { get; set; }
        public string Remarks { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public string RoleDelimited { get; set; }

        public int? ID { get; set; }

        public string ScopeOrgType { get; set; }

        public string ScopeOrgGroupDelimited { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string Suffix { get; set; }

        //public string ApplicantName { get; set; }

        //public string WorkflowStatusDelimited { get; set; }

        public string ApplicationSourceDelimited { get; set; }
        public string MRFTransactionID { get; set; }

        public string CurrentStepDelimited { get; set; }

        public string DateScheduledFrom { get; set; }
        public string DateScheduledTo { get; set; }
        public string DateCompletedFrom { get; set; }
        public string DateCompletedTo { get; set; }

        public string ApproverRemarks { get; set; }

        //public string CurrentStepDelimited { get; set; }

        //public string WorkflowDelimited { get; set; }
        public string PositionRemarks { get; set; }

        public string Course { get; set; }

        public string CurrentPositionTitle { get; set; }

        public decimal? ExpectedSalaryFrom { get; set; }

        public decimal? ExpectedSalaryTo { get; set; }

        public string DateAppliedFrom { get; set; }

        public string DateAppliedTo { get; set; }

        public bool IsExport { get; set; }
    }

    public class GetApplicantPickerListInput : JQGridFilter
    {
        public string RoleDelimited { get; set; }

        public int? ID { get; set; }

        public string SelectedIDDelimited { get; set; }

        public string ScopeOrgType { get; set; }

        public string ScopeOrgGroupDelimited { get; set; }

        //public string ApplicantName { get; set; }
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string Suffix { get; set; }

        public string WorkflowStatusDelimited { get; set; }

        public string ApplicationSourceDelimited { get; set; }

        public string MRFTransactionID { get; set; }

        public string CurrentStepDelimited { get; set; }

        public string DateScheduledFrom { get; set; }
        public string DateScheduledTo { get; set; }
        public string DateCompletedFrom { get; set; }
        public string DateCompletedTo { get; set; }

        public string ApproverRemarks { get; set; }

        //public string CurrentStepDelimited { get; set; }

        //public string WorkflowDelimited { get; set; }
        public string PositionRemarks { get; set; }

        public string Course { get; set; }

        public string CurrentPositionTitle { get; set; }

        public decimal? ExpectedSalaryFrom { get; set; }

        public decimal? ExpectedSalaryTo { get; set; }

        public string DateAppliedFrom { get; set; }

        public string DateAppliedTo { get; set; }

        public bool IsHired { get; set; }

        public bool IsTaggedToMRF { get; set; }
    }

    public class GetAutoCompleteInput
    {
        public string Term { get; set; }
        public int TopResults { get; set; }
    }

    public class UpdateMRFTransactionIDForm
    {
        public string MRFTransactionID { get; set; }
        public int HiredApplicantID { get; set; }
        public List<int> ApplicantIDs { get; set; }
    }

    public class ApproverResponse
    {
        public int RecordID { get; set; }
     
        public Enums.ApproverResponseEnum Result { get; set; }
        
        public string Remarks { get; set; }

        public string ApplicantName { get; set; }

        public List<GetHistoryOutput> History { get; set; }
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
        public string Result { get; set; }
    }

    public class ApplicantAttachmentForm
    {
        public int ApplicantID { get; set; }
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

    public class UploadFile
    {
        public string RowNum { get; set; }
        public string DesiredPosition { get; set; }
        public string DateApplied { get; set; }
        public string ApplicationSource { get; set; }
        public string ExpectedSalary { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string BirthDate { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PSGCRegionCode { get; set; }
        public string PSGCProvinceCode { get; set; }
        public string PSGCCityMunicipalityCode { get; set; }
        public string PSGCBarangayCode { get; set; }
        public string CityPSGC { get; set; }
        public int CityPSGCID { get; set; }
        public string CurrentPosition { get; set; }
        public string Course { get; set; }
        public string EmailAddress { get; set; }
        public string CellphoneNumber { get; set; }
        public string ReferredByCode { get; set; }
        public int? ReferredByID { get; set; }
    }

    public class UpdateEmployeeIDInput
    {
        public int ApplicantID { get; set; }
        public int EmployeeID { get; set; }
    }
    public class ApplicantLegalProfileInput
    {
        public int ApplicantID { get; set; }
        public List <AddApplicantLegalProfileInput> addApplicantLegalProfileInputs { get; set; }
    }
    public class AddApplicantLegalProfileInput
    {
        public string LegalNumber { get; set; }
        public string LegalAnswer { get; set; }
    }
}