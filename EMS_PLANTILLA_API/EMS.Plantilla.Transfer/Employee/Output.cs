using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Utilities.API;

namespace EMS.Plantilla.Transfer.Employee
{

    public class GetListOutput : JQGridResult
    {
        public int? ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string OrgGroup { get; set; }
        public string EmploymentStatus { get; set; }
        public string DateStatus { get; set; }
        public string MovementDate { get; set; }
        public string Position { get; set; }
        public string CurrentStep { get; set; }
        public string DateScheduled { get; set; }
        public string DateCompleted { get; set; }
        public string Remarks { get; set; }
        public string DateHired { get; set; }
        public string BirthDate { get; set; }
        public string OldEmployeeID { get; set; }
        public string HomeBranch { get; set; }
        public string Company { get; set; }
        public string Cluster { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
        public string Percent { get; set; }
    }

    public class GetIDByAutoCompleteOutput
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }

    public class GetByIDOutput
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string OldEmployeeID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int OrgGroupID { get; set; }
        public string OrgGroupCode { get; set; }
        public string OrgGroupDescription { get; set; }
        public string OrgGroupConcatenated { get; set; }
        public int PositionID { get; set; }
        public string PositionCode { get; set; }
        public string PositionTitle { get; set; }
        public string PositionConcatenated { get; set; }
        public string Company { get; set; }
        public string RefValueCompanyTag { get; set; }
        public string Email { get; set; }
        public string CorporateEmail { get; set; }
        public string OfficeMobile { get; set; }
        public string OfficeLandline { get; set; }
    }

    public class GetByPositionIDOrgGroupIDOutput
    {
        public string EmployeeName { get; set; }
    }

    public class GetRovingByPositionIDOrgGroupIDOutput
    {
        public string EmployeeName { get; set; }
    }

    public class GetRovingByEmployeeIDOutput
    {
        public int EmployeeID { get; set; }
        public int OrgGroupID { get; set; }
        public int PositionID { get; set; }
    }

    public class GetFamilyOutput
    {
        public string Name { get; set; }
        public string Relationship { get; set; }
        public string BirthDate { get; set; }
        public string Occupation { get; set; }
        public string SpouseEmployer { get; set; }
        public string ContactNumber { get; set; }
    }

    public class GetEducationOutput
    {
        public string School { get; set; }
        public string SchoolAddress { get; set; }
        public string SchoolLevelCode { get; set; }
        public string Course { get; set; }
        public int YearFrom { get; set; }
        public int YearTo { get; set; }
        public string EducationalAttainmentDegreeCode { get; set; }
        public string EducationalAttainmentStatusCode { get; set; }
    }


    public class GetWorkingHistoryOutput
    {
        public string CompanyName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Position { get; set; }
        public string ReasonForLeaving { get; set; }
    }

    public class GetEmploymentStatusOutput
    {
        public int ID { get; set; }
        public string EmploymentStatus { get; set; }
        public string DateEffective { get; set; }
    }

    public class GetETFListOutput
    {
        public string Code { get; set; }
        public string OldEmployeeID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string Nickname { get; set; }
        public string Nationality { get; set; }
        public string Citizenship { get; set; }
        public string BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string Gender { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string CivilStatus { get; set; }
        public string Religion { get; set; }
        public string SSSStatus { get; set; }
        public string ExemptionStatus { get; set; }
        public string HomeAddress { get; set; }
        public string CellphoneNumber { get; set; }
        public string HomeCity { get; set; }
        public string HomeRegion { get; set; }
        public string PagibigNumber { get; set; }
        public string SSSNumber { get; set; }
        public string TIN { get; set; }
        public string PhilhealthNumber { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonRelationship { get; set; }
        public string SpouseName { get; set; }
        public string SpouseBirthdate { get; set; }
        public string SpouseEmployer { get; set; }
        public string SpouseOccupation { get; set; }
        public string FatherName { get; set; }
        public string FatherBirthdate { get; set; }
        public string FatherOccupation { get; set; }
        public string MotherName { get; set; }
        public string MotherBirthdate { get; set; }
        public string MotherOccupation { get; set; }
        public string School { get; set; }
        public string SchoolAddress { get; set; }
        public string SchoolLevel { get; set; }
        public string Course { get; set; }
        public string YearFrom { get; set; }
        public string YearTo { get; set; }
        public string AttainmentDegree { get; set; }
        public string AttainmentStatus { get; set; }
        public string CompanyCode { get; set; }
        public string HomeBranch { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string Cluster { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
        public decimal? MonthlySalary { get; set; }
        public decimal? DailySalary { get; set; }
        public decimal? HourlySalary { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string PositionCode { get; set; }
        public string PositionTitle { get; set; }
        public string PositionLevel { get; set; }
        public string JobClass { get; set; }
        public string DateHired { get; set; }
        public string PreviousEmployer { get; set; }
        public string PreviousDesignation { get; set; }
        public string YearStarted { get; set; }
        public string YearEnded { get; set; }
        public string EmploymentStatus { get; set; }
    }

    public class UploadInsertOutput
    {
        public List<UploadInsertSystemUser> NewSystemUsers { get; set; }
        public string Message { get; set; }
    }

    public class UploadInsertSystemUser
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }

    public class GetEmployeeByUsernameOutput
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string OldEmployeeID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmploymentStatus { get; set; }
        public int OrgGroupID { get; set; }
        public string OrgGroupCode { get; set; }
        public string OrgGroupDescription { get; set; }
        public string OrgGroupConcatenated { get; set; }
        public int PositionID { get; set; }
        public string PositionCode { get; set; }
        public string PositionTitle { get; set; }
        public string PositionConcatenated { get; set; }
        public string Company { get; set; }
        public string RefValueCompanyTag { get; set; }
        public string Email { get; set; }
    }

    public class GetPrintCOEOutput
    {
        public string Content { get; set; }
        public string HRPosition { get; set; }
        public string HREmployeeName { get; set; }
    }

    public class EmployeeSkillsFormOutput : JQGridResult
    {
        public int? ID { get; set; }
        public int EmployeeID { get; set; }
        public string SkillsCode { get; set; }
        public string SkillsDescription { get; set; }
        public string Rate { get; set; }
        public string Remarks { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class GetListCorporateEmailOutput : JQGridResult
    {
        public int? ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string OrgGroup { get; set; }
        public string EmploymentStatus { get; set; }
        public string CorporateEmail { get; set; }
        public string OfficeMobile { get; set; }
        public string IsDisplayDirectory { get; set; }
        public string OldEmployeeID { get; set; }
    }

    public class GetEmailOutput
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int SystemUserID { get; set; }
        public string Email { get; set; }
        public string OrgCode { get; set; }
        public string Description { get; set; }
        public string PosCode { get; set; }
        public string Title { get; set; }
    }

    public class EmployeeOutput
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string OldEmployeeID { get; set; }
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
        public string CivilStatusCode { get; set; }
        public string SSSStatusCode { get; set; }
        public string ExemptionStatusCode { get; set; }
        public string ReligionCode { get; set; }
        public int OrgGroupID { get; set; }
        public int PositionID { get; set; }
        public int HomeBranchID { get; set; }
        public int SystemUserID { get; set; }
        public int OnboardingWorkflowID { get; set; }
        public string CompanyTag { get; set; }
        public string Status { get; set; }
        public DateTime? DateHired { get; set; }
        public string EmploymentStatus { get; set; }
        public DateTime BirthDate { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PSGCRegionCode { get; set; }
        public string PSGCProvinceCode { get; set; }
        public string PSGCCityMunicipalityCode { get; set; }
        public string PSGCBarangayCode { get; set; }
        public string Email { get; set; }
        public string CorporateEmail { get; set; }
        public string OfficeMobile { get; set; }
        public string OfficeLandline { get; set; }
        public string CellphoneNumber { get; set; }
        public string SSSNumber { get; set; }
        public string TIN { get; set; }
        public string PhilhealthNumber { get; set; }
        public string PagibigNumber { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonNumber { get; set; }
        public string ContactPersonAddress { get; set; }
        public string ContactPersonRelationship { get; set; }
        public bool IsDisplayDirectory { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class GetEmployeeLastEmploymentStatusOutput
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public DateTime StatusUpdatedDate { get; set; }
    }

    public class AddEmployeeReportOutput
    {
        public int Employee { get; set; }
        public int Org { get; set; }
        public int ActiveEmployeeCount { get; set; }
        public int PlannedCount { get; set; }
        public string CountPercent { get; set; }
    }

    public class GetEmployeeReportByTDateOutput
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string OrgGroup { get; set; }
        public string OrgType { get; set; }
        public string HomeBranch { get; set; }
        public string Region { get; set; }
        public string Status { get; set; }
        public string StatusUpdatedDate { get; set; }
        public string DateHired { get; set; }
        public string OldEmployeeID { get; set; }
        public string CompanyTag { get; set; }
    }

    public class GetEmployeeReportOrgByTDateOutput
    {
        public int OrgGroupID { get; set; }
        public string OrgGroup { get; set; }
        public string Position { get; set; }
        public string OrgType { get; set; }
        public string Region { get; set; }
        public int PlannedCount { get; set; }
        public int Draft { get; set; }
        public int Probationary { get; set; }
        public int Regular { get; set; }
        public int Promoted { get; set; }
        public int Outgoing { get; set; }
        public int Awol { get; set; }
        public int Backout { get; set; }
        public int Resigned { get; set; }
        public int Deceased { get; set; }
        public int terminated { get; set; }
        public int TotalActive { get; set; }
        public int TotalInactive { get; set; }
        public int Variance { get; set; }
    }
}