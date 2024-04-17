using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Recruitment.Data.RecruiterTask
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_pending_task")]
    public class TableVarPendingTask
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }

        [Column("applicant")]
        public string Applicant { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("created_date")]
        public string CreatedDate { get; set; }

        [Column("modified_date")]
        public string ModifiedDate { get; set; }
    }
}
