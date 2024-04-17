using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;


namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_get_employee_last_employment_status")]
    public class TablerVarGetEmployeeLastEmploymentStatus
    {

        [Key]

        [Column("id")]
        public int ID { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("status_updated_date")]
        public DateTime? StatusUpdatedDate { get; set; }

    }
}
