using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.Accountability
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_accountability_status_history")]
    public class TableVarEmployeeAccountabilityStatusHistory
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("user_id")]
        public int UserID { get; set; }

        [Column("timestamp")]
        public string Timestamp { get; set; }
    }
}
