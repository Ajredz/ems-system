using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.Accountability
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_accountability_list")]
    public class TableVarEmployeeAccountabilityList
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("created_date")]
        public string CreatedDate { get; set; }
        [Column("employee_name")]
        public string EmployeeName { get; set; }
        [Column("date_seperated")]
        public string DateSeperated { get; set; }
        [Column("date_hired")]
        public string DateHired { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("status_color")]
        public string StatusColor { get; set; }
        [Column("status_updated_date")]
        public string StatusUpdatedDate { get; set; }
        [Column("employee_org")]
        public string EmployeeOrg { get; set; }
        [Column("employee_reg")]
        public string EmployeeReg { get; set; }
        [Column("employee_pos")]
        public string EmployeePos { get; set; }
        [Column("approver_org")]
        public string ApproverOrg { get; set; }
        [Column("status_by_name")]
        public string StatusByName { get; set; }
        [Column("status_updated_by")]
        public int StatusUpdatedBy { get; set; }
        [Column("status_remarks")]
        public string StatusRemarks { get; set; }
        [Column("last_comment")]
        public string LastComment { get; set; }
        [Column("last_comment_date")]
        public string LastCommentDate { get; set; }
        [Column("employee_id")]
        public int EmployeeID { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("org_group_id")]
        public int OrgGroupID { get; set; }
    }
}
