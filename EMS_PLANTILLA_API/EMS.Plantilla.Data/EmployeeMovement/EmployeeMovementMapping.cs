using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;


namespace EMS.Plantilla.Data.EmployeeMovement
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("employee_movement_mapping")]
    public class EmployeeMovementMapping
    {

        [Key]

        [Column("id")]
        public int ID { get; set; }

        [Column("movement_type")]
        public string MovementType { get; set; }

        [Column("employee_field")]
        public string EmployeeField { get; set; }

        [Column("employee_field_code")]
        public string EmployeeFieldCode { get; set; }

        [Column("allow_multiple")]
        public bool AllowMultiple { get; set; }

    }
}
