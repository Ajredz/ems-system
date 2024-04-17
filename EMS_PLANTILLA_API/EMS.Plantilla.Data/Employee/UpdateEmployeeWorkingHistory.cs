using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("update_employee_working_history")]
    public class UpdateEmployeeWorkingHistory
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("udpdate_status")]
        public string UpdateStatus { get; set; }

        [Column("employee_id")]
        public int EmployeeID { get; set; }

        [Column("company_name")]
        public string CompanyName { get; set; }

        [Column("from")]
        public DateTime From { get; set; }

        [Column("to")]
        public DateTime To { get; set; }

        [Column("position")]
        public string Position { get; set; }

        [Column("reason_for_leaving")]
        public string ReasonForLeaving { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }
    }
}
