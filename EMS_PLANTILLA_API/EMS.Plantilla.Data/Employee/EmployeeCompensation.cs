using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("employee_compensation")]
    public class EmployeeCompensation

    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Column("employee_id")]
        public int EmployeeID { get; set; }
        
        [Column("monthly_salary")]
        public decimal MonthlySalary { get; set; }
        
        [Column("daily_salary")]
        public decimal DailySalary { get; set; }
        
        [Column("hourly_salary")]
        public decimal HourlySalary { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }
}