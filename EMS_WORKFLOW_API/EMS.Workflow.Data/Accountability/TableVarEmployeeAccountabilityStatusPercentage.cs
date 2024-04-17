using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.Accountability
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_accountability_status_percentage")]
    public class TableVarEmployeeAccountabilityStatusPercentage
    {
        [Key]
        [Column("employee_id")]
        public int EmployeeID { get; set; }

        [Column("overdone")]
        public int OverDone { get; set; }

        [Column("overall")]
        public int OverAll { get; set; }

        [Column("status")]
        public string Status { get; set; }
    }
}
