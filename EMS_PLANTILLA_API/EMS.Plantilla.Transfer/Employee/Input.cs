using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using Utilities.API;

namespace EMS.Plantilla.Transfer.Employee
{
    public class Form
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string OldEmployeeID { get; set; }
        public int OrgGroupID { get; set; }
        public int HomeBranchID { get; set; }
        public string HomeBranchDescription { get; set; }
        public string Region { get; set; }
        public string RegionCode { get; set; }
        public int PositionID { get; set; }
        public string JobClass { get; set; }
        public int SystemUserID { get; set; }
        public int OnboardingWorkflowID { get; set; }
        public string CompanyTag { get; set; }
        public DateTime? DateHired { get; set; }
        public string EmploymentStatus { get; set; }
        public string CorporateEmail { get; set; }
        public string OfficeMobile { get; set; }
        public string OfficeLandline { get; set; }
        public int CreatedBy { get; set; }

        public bool IsViewedFamilyBackground { get; set; }
        public bool IsViewedWorkingHistory { get; set; }
        public bool IsViewedEducation { get; set; }
        public bool IsViewedSecondaryDesignation { get; set; }

        public List<EmployeeRovingForm> EmployeeRovingList { get; set; }
        public PersonalInformation PersonalInformation { get; set; }
        public List<EmployeeFamily> EmployeeFamilyList { get; set; }
        public List<EmployeeWorkingHistory> EmployeeWorkingHistoryList { get; set; }
        public EmployeeCompensationForm EmployeeCompensation { get; set; }
        public List<EmployeeEducation> EmployeeEducationList { get; set; }

        public bool IsDataPrivacy { get; set; }
        public bool IsDisplayDirectory { get; set; }
    }
    public class UpdateProfileForm
    {
        public int ID { get; set; }
        public int CreatedBy { get; set; }
        public List<UpdateProfile> UpdateProfile { get; set; }
        public List<EmployeeFamily> EmployeeFamilyList { get; set; }
        public List<EmployeeWorkingHistory> EmployeeWorkingHistoryList { get; set; }
        public List<EmployeeEducation> EmployeeEducationList { get; set; }
    }
    public class UpdateProfile
    {
        public string Fields { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }

    public class UploadFile
    {
        public string RowNum { get; set; }
        public int OnboardingWorkflowID { get; set; }
        public string NewEmployeeCode { get; set; }
        public string OldEmployeeCode { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string Nickname { get; set; }
        public string Nationality { get; set; }
        public string Citizenship { get; set; }
        public string BirthDate { get; set; }
        public DateTime BirthDateConverted { get; set; }
        public string BirthPlace { get; set; }
        public string Gender { get; set; }
        public string HeightCM { get; set; }
        public string WeightLBS { get; set; }
        public string CivilStatus { get; set; }
        public string Religion { get; set; }
        public string SSSStatus { get; set; }
        public string ExemptionStatus { get; set; }
        public string HomeAddress { get; set; }
        public string MobileNo { get; set; }

        //public string CityPSGC { get; set; }
        //public int CityPSGCID { get; set; }
        //public string RegionPSGC { get; set; }
        //public int RegionPSGCID { get; set; }

        public string PSGCRegionCode { get; set; }
        public string PSGCProvinceCode { get; set; }
        public string PSGCCityMunicipalityCode { get; set; }
        public string PSGCBarangayCode { get; set; }

        public string HDMFNo { get; set; }
        public string SSSNo { get; set; }
        public string TIN { get; set; }
        public string PhilHealthNo { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonRelationship { get; set; }
        public string Spouse { get; set; }
        public string SpouseBirthDate { get; set; }
        public DateTime? SpouseBirthDateConverted { get; set; }
        public string SpouseEmployer { get; set; }
        public string SpouseOccupation { get; set; }
        public string Father { get; set; }
        public string FatherBirthDate { get; set; }
        public DateTime FatherBirthDateConverted { get; set; }
        public string FatherOccupation { get; set; }
        public string Mother { get; set; }
        public string MotherBirthDate { get; set; }
        public DateTime MotherBirthDateConverted { get; set; }
        public string MotherOccupation { get; set; }
        public string School { get; set; }
        public string SchoolAddress { get; set; }
        public string SchoolLevel { get; set; }
        public string Course { get; set; }
        public string SchoolYearFrom { get; set; }
        public string SchoolYearTo { get; set; }
        public string EducationalAttainmentDegree { get; set; }
        public string EducationalAttainmentStatus { get; set; }
        public string CompanyCode { get; set; }
        public string BranchCode { get; set; }
        public int BranchID { get; set; }
        public string MonthlySalary { get; set; }
        public string DailySalary { get; set; }
        public string HourlySalary { get; set; }
        public string DepartmentCode { get; set; }
        public int DepartmentID { get; set; }
        public string DesignationCode { get; set; }
        public int DesignationID { get; set; }
        public string DateHired { get; set; }
        public DateTime DateHiredConverted { get; set; }
        public string PreviousEmployer { get; set; }
        public string PreviousDesignation { get; set; }
        public string YearStarted { get; set; }
        public DateTime? YearStartedConverted { get; set; }
        public string YearEnded { get; set; }
        public DateTime? YearEndedConverted { get; set; }
        public string EmploymentStatus { get; set; }
    }

        public class EmployeeRovingForm
    {   
        public int OrgGroupID { get; set; }
        public int PositionID { get; set; }

        public string ToID { get; set; }
        public string DateToID { get; set; }
    }

    public class PersonalInformation
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Nickname { get; set; }
        public string Gender { get; set; }
        public string NationalityCode { get; set; }
        public string CitizenshipCode { get; set; }
        public string BirthPlace { get; set; }
        public string HeightCM { get; set; }
        public string WeightLBS { get; set; }
        public string SSSStatusCode { get; set; }
        public string ExemptionStatusCode { get; set; }
        public string CivilStatusCode { get; set; }
        public string ReligionCode { get; set; }
        public DateTime BirthDate { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PSGCRegionCode { get; set; }
        public string PSGCProvinceCode { get; set; }
        public string PSGCCityMunicipalityCode { get; set; }
        public string PSGCBarangayCode { get; set; }
        public string Email { get; set; }
        public string CellphoneNumber { get; set; }
        public string SSSNumber { get; set; }
        public string TIN { get; set; }
        public string PagibigNumber { get; set; }
        public string PhilhealthNumber { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonNumber { get; set; }
        public string ContactPersonAddress { get; set; }
        public string ContactPersonRelationship { get; set; }
    }

    public class EmployeeFamily
    {
        public string Name { get; set; }
        public string Relationship { get; set; }
        public DateTime BirthDate { get; set; }
        public string Occupation { get; set; }
        public string SpouseEmployer { get; set; }
        public string ContactNumber { get; set; }
        public int CreatedBy { get; set; }

        // Used for UploadInsert feature
        public string UploadInsertEmployeeCode { get; set; }
    }

    public class EmployeeWorkingHistory
    {
        public string CompanyName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Position { get; set; }
        public string ReasonForLeaving { get; set; }
        public int CreatedBy { get; set; }

        // Used for UploadInsert feature
        public string UploadInsertEmployeeCode { get; set; }
    }

    public class EmployeeCompensationForm
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string MonthlySalary { get; set; }
        public string DailySalary { get; set; }
        public string HourlySalary { get; set; }

        // Used for UploadInsert feature
        public string UploadInsertEmployeeCode { get; set; }
    }

    public class EmployeeEducation
    {
        public string School { get; set; }
        public string SchoolAddress { get; set; }
        public string SchoolLevelCode { get; set; }
        public string Course { get; set; }
        public int YearFrom { get; set; }
        public int YearTo { get; set; }
        public string EducationalAttainmentDegreeCode { get; set; }
        public string EducationalAttainmentStatusCode { get; set; }

        // Used for UploadInsert feature
        public string UploadInsertEmployeeCode { get; set; }
    }

    public class EmploymentStatusHistory
    {
        public int EmployeeID { get; set; }
        public string EmploymentStatus { get; set; }
        public DateTime DateEffective { get; set; }
        public int CreatedBy { get; set; }
    }

    public class GetAutoCompleteInput
    {
        public string Term { get; set; }
        public int TopResults { get; set; }
    }

    public class GetByPositionIDOrgGroupIDInput
    {
        public int PositionID { get; set; }
        public int OrgGroupID { get; set; }
    } 
    
    public class GetRovingByPositionIDOrgGroupIDInput
    {
        public int PositionID { get; set; }
        public int OrgGroupID { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public int? ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string OrgGroupDelimited { get; set; }
        public string PositionDelimited { get; set; }
        public string EmploymentStatusDelimited { get; set; }
        public string CurrentStepDelimited { get; set; }
        public string DateScheduledFrom { get; set; }
        public string DateScheduledTo { get; set; }
        public string DateCompletedFrom { get; set; }
        public string DateCompletedTo { get; set; }
        public string Remarks { get; set; }
        public string DateStatusFrom { get; set; }
        public string DateStatusTo { get; set; }
        public string DateHiredFrom { get; set; }
        public string DateHiredTo { get; set; }
        public string MovementDateFrom { get; set; }
        public string MovementDateTo { get; set; }
        public string BirthDateFrom { get; set; }
        public string BirthDateTo { get; set; }
        public bool IsExport { get; set; }
        public string OldEmployeeID { get; set; }
        public string OrgGroupDelimitedClus { get; set; }
        public string OrgGroupDelimitedArea { get; set; }
        public string OrgGroupDelimitedReg { get; set; }
        public string OrgGroupDelimitedZone { get; set; }
        public bool ShowActiveEmployee { get; set; }
        public string Percentage { get; set; }
    }

    public class GetByIDInput
    {
        public int ID { get; set; }
    }

    public class UpdateOnboardingCurrentWorkflowStepInput
    {
        public int WorkflowID { get; set; }
        public int EmployeeID { get; set; }
        public string CurrentStepCode { get; set; }
        public string CurrentStepDescription { get; set; }
        public string WorkflowStatus { get; set; }
        public string CurrentStepApproverRoleIDs { get; set; }
        public string DateScheduled { get; set; }
        public string DateCompleted { get; set; }
        public string Remarks { get; set; }
    }

    public class UpdateSystemUserInput
    {
        public int EmployeeID { get; set; }
        public int SystemUserID { get; set; }
    }

    public class UploadInsertUpdateSystemUserInput
    {
        public int SystemUserID { get; set; }
        public string NewEmployeeCode { get; set; }
    }

    public class AttachmentForm
    {
        public int EmployeeID { get; set; }
        public string Description { get; set; }
        [IgnoreDataMember]
        public IFormFile File { get; set; }
        public string SourceFile { get; set; }
        public string ServerFile { get; set; }
        public int CreatedBy { get; set; }
        public string UploadedBy { get; set; }
        public string Timestamp { get; set; }
    }

    public class EmployeeAttachmentForm
    {
        public int EmployeeID { get; set; }
        public List<AttachmentForm> AddAttachmentForm { get; set; }
        public List<AttachmentForm> DeleteAttachmentForm { get; set; }
    }

    public class EmployeeSkillsForm
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string Skills { get; set; }
        public string SkillsCode { get; set; }
        public string SkillsDescription { get; set; }
        public string Rate { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public class EmployeeSkillsFormInput : JQGridFilter
    {
        public int? ID { get; set; }
        public int? EmployeeID { get; set; }
        public string SkillsCode { get; set; }
        public string SkillsDescription { get; set; }
        public string Rate { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsExport { get; set; }
    }

    public class ExternalLogin
    {
        public string EmployeeId { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
    }
    public class GetListCorporateEmailInput : JQGridFilter
    {
        public int? ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string OrgGroupDelimited { get; set; }
        public string PositionDelimited { get; set; }
        public string EmploymentStatusDelimited { get; set; }
        public string CorporateEmail { get; set; }
        public string OfficeMobile { get; set; }
        public string IsDisplayDirectory { get; set; }
        public bool IsExport { get; set; }
        public string OldEmployeeID { get; set; }
    }
    public class UpdateEmployeeEmailInput
    {
        public int ID { get; set; }
        public string Email { get; set; }
    }

    public class GetEmailInput
    { 
        public int? ID { get; set; }
        public string Condition { get; set; }
    }

    public class NewEmployeeForm
    {
        public string RowNum { get; set; }
        public int ID { get; set; }
        public string Code { get; set; }
        public string CompanyTag { get; set; }
        public string OldEmployeeID { get; set; }
        public string Email { get; set; }
    }

    public class NewEmployee
    { 
        public int EmployeeID { get; set; }
        public int ApplicantID { get; set; }
    }

    public class EmployeeDetails
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string OldEmployeeID { get; set; }
        public int OrgGroupID { get; set; }
        public int HomeBranchID { get; set; }
        public string HomeBranchDescription { get; set; }
        public string Region { get; set; }
        public string RegionCode { get; set; }
        public int PositionID { get; set; }
        public string JobClass { get; set; }
        public int SystemUserID { get; set; }
        public int OnboardingWorkflowID { get; set; }
        public string CompanyTag { get; set; }
        public DateTime? DateHired { get; set; }
        public string EmploymentStatus { get; set; }
        public string CorporateEmail { get; set; }
        public string OfficeMobile { get; set; }
        public string OfficeLandline { get; set; }
        public int CreatedBy { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Nickname { get; set; }
        public string Gender { get; set; }
        public string NationalityCode { get; set; }
        public string CitizenshipCode { get; set; }
        public string BirthPlace { get; set; }
        public string HeightCM { get; set; }
        public string WeightLBS { get; set; }
        public string SSSStatusCode { get; set; }
        public string ExemptionStatusCode { get; set; }
        public string CivilStatusCode { get; set; }
        public string ReligionCode { get; set; }
        public DateTime BirthDate { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PSGCRegionCode { get; set; }
        public string PSGCProvinceCode { get; set; }
        public string PSGCCityMunicipalityCode { get; set; }
        public string PSGCBarangayCode { get; set; }
        public string Email { get; set; }
        public string CellphoneNumber { get; set; }
        public string SSSNumber { get; set; }
        public string TIN { get; set; }
        public string PagibigNumber { get; set; }
        public string PhilhealthNumber { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonNumber { get; set; }
        public string ContactPersonAddress { get; set; }
        public string ContactPersonRelationship { get; set; }
    }

    public class GetEmployeeLastEmploymentStatusByDateInput
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }

    public class GetEmployeeEvaluationOutput
    {
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string DateHired { get; set; }
        public string Fivemonths { get; set; }
        public string DismissedDate { get; set; }
        public string Regularization { get; set; }
        public string CorporateEmail { get; set; }
        public int OrgGroupID { get; set; }
        public int PositionID { get; set; }
        public string HRBP { get; set; }
        public string DEPT { get; set; }
        public string OrgGroup { get; set; }
        public string Position { get; set; }
        public string SupervisorName { get; set; }
        public string Supervisor { get; set; }
        public string CCEmail { get; set; }
    }

    public class GetEmployeeIfExistInput
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string BDate { get; set; }
    }
}