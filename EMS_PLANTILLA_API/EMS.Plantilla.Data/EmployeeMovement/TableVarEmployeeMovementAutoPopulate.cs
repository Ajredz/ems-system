using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;


namespace EMS.Plantilla.Data.EmployeeMovement
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_movement_autopopulate")]
    public class TableVarEmployeeMovementAutoPopulate
    {

        [Key]
        [Column("value")]
        public string Value { get; set; }

        [Column("description")]
        public string Description { get; set; }
        
    }
}
