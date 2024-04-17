using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Recruitment.Data.Applicant
{
    [Table("applicant")]
    public class Applicant
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("application_source")]
        public string ApplicationSource { get; set; }
        
        [Column("position_remarks")]
        public string PositionRemarks { get; set; }

        [Column("referred_by_user_id")]
        public int ReferredByUserID { get; set; }

        [Column("expected_salary")]
        public decimal? ExpectedSalary { get; set; }

        [Column("date_applied")]
        public DateTime DateApplied { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("middlename")]
        public string MiddleName { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("suffix")]
        public string Suffix { get; set; }

        [Column("workflow_id")]
        public int? WorkflowID { get; set; }
        [Column("current_step_code")]
        public string CurrentStepCode { get; set; }
        [Column("current_step_description")]
        public string CurrentStepDescription { get; set; }
        [Column("workflow_status")]
        public string WorkflowStatus { get; set; }
        [Column("current_step_approver_role_ids")]
        public string CurrentStepApproverRoleIDs { get; set; }
        [Column("approver_remarks")]
        public string ApproverRemarks { get; set; }
        [Column("date_completed")]
        public DateTime? DateCompleted { get; set; }
        [Column("date_scheduled")]
        public DateTime? DateScheduled { get; set; }

        [Column("current_position")]
        public string CurrentPosition { get; set; }

        [Column("course")]
        public string Course { get; set; }

        [Column("address_line_1")]
        public string AddressLine1 { get; set; }

        [Column("address_line_2")]
        public string AddressLine2 { get; set; }

        //[Column("geographical_region")]
        //public string GeographicalRegion { get; set; }

        [Column("psgc_region_code")]
        public string PSGCRegionCode { get; set; }

        [Column("psgc_province_code")]
        public string PSGCProvinceCode { get; set; }

        [Column("psgc_city_mun_code")]
        public string PSGCCityMunicipalityCode { get; set; }

        [Column("psgc_barangay_code")]
        public string PSGCBarangayCode { get; set; }

        [Column("birthdate")]
        public DateTime BirthDate { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("mrf_transaction_id")]
        public string MRFTransactionID { get; set; }

        [Column("failed_mrf_transaction_id")]
        public string FailedMRFTransactionID { get; set; }

        [Column("employee_id")]
        public int? EmployeeID { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("cellphone_number")]
        public string CellphoneNumber { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }
}