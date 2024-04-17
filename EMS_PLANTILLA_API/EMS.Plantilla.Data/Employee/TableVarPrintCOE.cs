using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;


namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_print_coe")]
    public class TableVarPrintCOE
    {

        [Key]
        [Column("hr_employee_name")]
        public string HREmployeeName { get; set; }
        
        [Column("hr_position")]
        public string HRPosition { get; set; }

        [Column("content")]
        public string Content { get; set; }


    }
}
