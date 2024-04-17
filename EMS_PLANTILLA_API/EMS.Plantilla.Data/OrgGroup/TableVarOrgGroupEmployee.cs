using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_org_group_employee")]
    public class TableVarOrgGroupEmployee
    {
        [Key]
        [Column("row_num")]
        public int RowNum { get; set; }
        [Column("total_num")]
        public int TotalNum { get; set; }
        [Column("position")]
        public string Position { get; set; }

        [Column("employee_name")]
        public string EmployeeName { get; set; }
    }
}
