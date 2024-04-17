using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("employee")]
    public class Employee

    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("old_employee_id")]
        public string OldEmployeeID { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("middlename")]
        public string MiddleName { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }
        
        [Column("suffix")]
        public string Suffix { get; set; }
        
        [Column("nickname")]
        public string Nickname { get; set; }
        
        [Column("gender")]
        public string Gender { get; set; }
        
        [Column("nationality_code")]
        public string NationalityCode { get; set; }

        [Column("citizenship_code")]
        public string CitizenshipCode { get; set; }

        [Column("birth_place")]
        public string BirthPlace { get; set; }

        [Column("height_cm")]
        public string HeightCM { get; set; }

        [Column("weight_lbs")]
        public string WeightLBS { get; set; }

        [Column("civil_status_code")]
        public string CivilStatusCode { get; set; }

        [Column("sss_status_code")]
        public string SSSStatusCode { get; set; }

        [Column("exemption_status_code")]
        public string ExemptionStatusCode { get; set; }

        [Column("religion_code")]
        public string ReligionCode { get; set; }

        [Column("org_group_id")]
        public int OrgGroupID { get; set; }

        [Column("position_id")]
        public int PositionID { get; set; }
        [Column("home_branch_id")]
        public int HomeBranchID { get; set; }

        [Column("system_user_id")]
        public int SystemUserID { get; set; }

        [Column("onboarding_workflow_id")]
        public int OnboardingWorkflowID { get; set; }

        [Column("ref_value_company_tag")]
        public string CompanyTag { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("date_hired")]
        public DateTime? DateHired { get; set; }

        [Column("employment_status")]
        public string EmploymentStatus { get; set; }

        [Column("birthdate")]
        public DateTime BirthDate { get; set; }

        [Column("address_line_1")]
        public string AddressLine1 { get; set; }

        [Column("address_line_2")]
        public string AddressLine2 { get; set; }

        [Column("psgc_region_code")]
        public string PSGCRegionCode { get; set; }
        
        [Column("psgc_province_code")]
        public string PSGCProvinceCode { get; set; }

        [Column("psgc_city_mun_code")]
        public string PSGCCityMunicipalityCode { get; set; }

        [Column("psgc_barangay_code")]
        public string PSGCBarangayCode { get; set; }

        [Column("email")]
        public string Email { get; set; }
        
        [Column("corporate_email")]
        public string CorporateEmail { get; set; }

        [Column("office_mobile")]
        public string OfficeMobile { get; set; }
        [Column("office_landline")]
        public string OfficeLandline { get; set; }

        [Column("cellphone_number")]
        public string CellphoneNumber { get; set; }

        [Column("sss_number")]
        public string SSSNumber { get; set; }

        [Column("tin")]
        public string TIN { get; set; }

        [Column("philhealth_number")]
        public string PhilhealthNumber { get; set; }

        [Column("pagibig_number")]
        public string PagibigNumber { get; set; }

        [Column("contact_person_name")]
        public string ContactPersonName { get; set; }

        [Column("contact_person_number")]
        public string ContactPersonNumber { get; set; }
        
        [Column("contact_person_address")]
        public string ContactPersonAddress { get; set; }
        
        [Column("contact_person_relationship")]
        public string ContactPersonRelationship { get; set; }

        [Column("is_display_directory")]
        public bool IsDisplayDirectory { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }
}