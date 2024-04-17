using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.Accountability
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("employee_accountability_comments")]
    public class EmployeeAccountabilityComments
    {
        [Key]
        [Column("id")]
        public long ID { get; set; }

        [Column("employee_accountability_id")]
        public int EmployeeAccountabilityID { get; set; }

        [Column("comments")]
        public string Comments { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("is_external")]
        public bool IsExternal { get; set; }
    }
}
