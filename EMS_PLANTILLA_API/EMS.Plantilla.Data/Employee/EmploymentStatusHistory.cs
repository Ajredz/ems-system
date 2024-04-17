using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("employment_status_history")]
    public class EmploymentStatusHistory
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("employee_id")]
        public int EmployeeID { get; set; }

        [Column("employment_status")]
        public string EmploymentStatus { get; set; }

        [Column("date_effective")]
        public DateTime DateEffective { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }
    }
}
