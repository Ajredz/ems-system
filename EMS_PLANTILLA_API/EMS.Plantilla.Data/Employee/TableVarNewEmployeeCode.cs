using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;


namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_new_employee_code")]
    public class TableVarNewEmployeeCode
    {

        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("new_employee_code")]
        public string NewEmployeeCode { get; set; }
    }
}
