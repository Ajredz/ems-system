using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.Accountability
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_my_accountabilities")]
    public class TableVarMyAccountabilities
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("employee_id")]
        public int EmployeeID { get; set; }

        [Column("type")]
        public string Type { get; set; }
        
        [Column("title")]
        public string Title { get; set; }
        
        [Column("description")]
        public string Description { get; set; }

        [Column("org_group_id")]
        public int OrgGroupID { get; set; }

        [Column("position_id")]
        public int PositionID { get; set; }

        [Column("approver_employee_id")]
        public int ApproverEmployeeID { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("status_updated_by")]
        public int StatusUpdatedBy { get; set; }

        [Column("status_updated_date")]
        public string StatusUpdateDate { get; set; }

        [Column("status_remarks")]
        public string StatusRemarks { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        public string CreatedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("modified_date")]
        public string ModifiedDate { get; set; }

        [Column("last_comment")]
        public string LastComment { get; set; }

        [Column("last_comment_date")]
        public string LastCommentDate { get; set; }


        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}
