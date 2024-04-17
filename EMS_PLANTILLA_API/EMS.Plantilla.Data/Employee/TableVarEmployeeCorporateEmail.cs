using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;


namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_corporate_email")]
    public class TableVarEmployeeCorporateEmail
    {

        [Key]

        [Column("id")]
        public int ID { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("org_group")]
        public string OrgGroup { get; set; }

        [Column("position")]
        public string Position { get; set; }

        [Column("employment_status")]
        public string EmploymentStatus { get; set; }
        [Column("corporate_email")]
        public string CorporateEmail { get; set; }
        [Column("office_mobile")]
        public string OfficeMobile { get; set; }
        [Column("is_display_directory")]
        public string IsDisplayDirectory { get; set; }

        [Column("old_employee_id")]
        public string OldEmployeeID { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }

    }
}
