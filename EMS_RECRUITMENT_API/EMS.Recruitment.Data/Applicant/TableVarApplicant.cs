using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.Applicant
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_applicant")]
    public class TableVarApplicant
    {
        [Key]
        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }

        [Column("id")]
        public int ID { get; set; }

        [Column("scope_org_group")]
        public string ScopeOrgGroup { get; set; }

        [Column("applicant_name")]
        public string ApplicantName { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("middlename")]
        public string MiddleName { get; set; }

        [Column("suffix")]
        public string Suffix { get; set; }

        [Column("application_source")]
        public string ApplicationSource { get; set; }
        
        [Column("mrf_transaction_id")]
        public string MRFTransactionID { get; set; }

        [Column("current_step")]
        public string CurrentStep { get; set; }

        [Column("date_scheduled")]
        public string DateScheduled { get; set; }

        [Column("date_completed")]
        public string DateCompleted { get; set; }

        [Column("approver_remarks")]
        public string ApproverRemarks { get; set; }

        //[Column("workflow_status")]
        //public string WorkflowStatus { get; set; }

        //[Column("current_step")]
        //public string CurrentStep { get; set; }

        //[Column("workflow_description")]
        //public string WorkflowDescription { get; set; }

        [Column("position_remarks")]
        public string PositionRemarks { get; set; }

        [Column("course")]
        public string Course { get; set; }

        [Column("current_position")]
        public string CurrentPositionTitle { get; set; }

        [Column("expected_salary")]
        public decimal? ExpectedSalary { get; set; }

        [Column("date_applied")]
        public string DateApplied { get; set; }

        [Column("employee_id")]
        public int EmployeeID { get; set; }

        [Column("birthdate")]
        public string BirthDate { get; set; }

        [Column("address_line_1")]
        public string AddressLine1 { get; set; }

        [Column("address_line_2")]
        public string AddressLine2 { get; set; }

        //[Column("city")]
        //public string City { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("cellphone_number")]
        public string CellphoneNumber { get; set; }

        [Column("referred_by")]
        public string ReferredBy { get; set; }
    }
}