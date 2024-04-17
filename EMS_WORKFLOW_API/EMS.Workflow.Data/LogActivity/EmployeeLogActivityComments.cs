using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.LogActivity
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("employee_log_activity_comments")]
    public class EmployeeLogActivityComments
    {
        [Key]
        [Column("id")]
        public long ID { get; set; }
        [Column("employee_log_activity_id")]
        public int EmployeeLogActivityID { get; set; }
        [Column("comments")]
        public string Comments { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}
