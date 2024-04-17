using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.LogActivity
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("applicant_log_activity_comments")]
    public class ApplicantLogActivityComments
    {
        [Key]
        [Column("id")]
        public long ID { get; set; }
        [Column("applicant_log_activity_id")]
        public int ApplicantLogActivityID { get; set; }
        [Column("comments")]
        public string Comments { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}
