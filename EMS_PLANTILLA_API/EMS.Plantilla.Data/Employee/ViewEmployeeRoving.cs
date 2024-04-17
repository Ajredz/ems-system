using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;


namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("vw_employee_roving")]
    public class ViewEmployeeRoving
    {

        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("employee_id")]
        public int EmployeeID { get; set; }
        [Column("org_group_id")]
        public int OrgGroupID { get; set; }
        [Column("position_id")]
        public int PositionID { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("firstname")]
        public string FirstName { get; set; }
        [Column("middlename")]
        public string MiddleName { get; set; }
        [Column("lastname")]
        public string LastName { get; set; }
        [Column("main_org_group_id")]
        public int MainOrgGroupID { get; set; }
        [Column("main_position_id")]
        public int MainPositionID { get; set; }
        [Column("system_user_id")]
        public int SystemUserID { get; set; }
    }
}
