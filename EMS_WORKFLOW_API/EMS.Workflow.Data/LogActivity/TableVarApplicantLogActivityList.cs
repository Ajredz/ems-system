using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.LogActivity
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_applicant_log_activity_list")]
    public class TableVarApplicantLogActivityList
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("sub_type")]
        public string SubType { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("current_status")]
        public string CurrentStatus { get; set; }

        [Column("current_timestamp")]
        public string CurrentTimestamp { get; set; }

        [Column("due_date")]
        public string DueDate { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("applicant_id")]
        public int ApplicantID { get; set; }

        [Column("assigned_user_id")]
        public int AssignedUserID { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("is_assignment")]
        public bool IsAssignment { get; set; }
    }
}
