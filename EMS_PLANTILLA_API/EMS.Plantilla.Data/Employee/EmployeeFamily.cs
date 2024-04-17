using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("employee_family")]
    public class EmployeeFamily
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("employee_id")]
        public int EmployeeID { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("relationship")]
        public string Relationship { get; set; }

        [Column("birthdate")]
        public DateTime? BirthDate { get; set; }

        [Column("occupation")]
        public string Occupation { get; set; }

        [Column("spouse_employer")]
        public string SpouseEmployer { get; set; }

        [Column("contact_number")]
        public string ContactNumber { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }
    }
}
