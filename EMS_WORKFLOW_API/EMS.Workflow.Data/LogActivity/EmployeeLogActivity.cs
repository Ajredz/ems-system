using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.LogActivity
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("employee_log_activity")]
    public class EmployeeLogActivity
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("employee_id")]
        public int EmployeeID { get; set; }
        
        [Column("type")]
        public string Type { get; set; }

        [Column("sub_type")]
        public string SubType { get; set; }
        
        [Column("title")]
        public string Title { get; set; }
        
        [Column("description")]
        public string Description { get; set; }

        [Column("is_with_pass_fail")]
        public bool IsWithPassFail { get; set; }

        [Column("is_with_assignment")]
        public bool IsWithAssignment { get; set; }

        [Column("is_pass")]
        public bool IsPass { get; set; }
        
        [Column("current_status")]
        public string CurrentStatus { get; set; }
        
        [Column("current_timestamp")]
        public DateTime? CurrentTimestamp { get; set; }
        
        [Column("assigned_user_id")]
        public int AssignedUserID { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("is_email_sent")]
        public bool IsEmailSent { get; set; }

        [Column("sent_datetime")]
        public DateTime? SentDateTime { get; set; }

        [Column("employee_name")]
        public string EmployeeName { get; set; }

        [Column("assigned_org_group_id")]
        public int AssignedOrgGroupID { get; set; }

        [Column("is_visible")]
        public bool IsVisible { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

    }
}
