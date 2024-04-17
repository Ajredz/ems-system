using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_get_by_id")]
    public class TableVarEmployeeGetByID
    {
        [Key]

        [Column("id")]
        public int ID { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("firstname")]
        public string FirstName { get; set; }
        [Column("middlename")]
        public string MiddleName { get; set; }
        [Column("lastname")]
        public string LastName { get; set; }
        [Column("org_group_id")]
        public int OrgGroupID { get; set; }
        [Column("org_group_code")]
        public string OrgGroupCode { get; set; }
        [Column("org_group_description")]
        public string OrgGroupDescription { get; set; }
        [Column("org_group_concatenated")]
        public string OrgGroupConcatenated { get; set; }
        [Column("position_id")]
        public int PositionID { get; set; }
        [Column("position_code")]
        public string PositionCode { get; set; }
        [Column("position_title")]
        public string PositionTitle { get; set; }
        [Column("position_concatenated")]
        public string PositionConcatenated { get; set; }
        [Column("company")]
        public string Company { get; set; }
        [Column("ref_value_company_tag")]
        public string RefValueCompanyTag { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("corporate_email")]
        public string CorporateEmail { get; set; }
        [Column("office_mobile")]
        public string OfficeMobile { get; set; }
        [Column("office_landline")]
        public string OfficeLandline { get; set; }
        [Column("is_display_directory")]
        public bool IsDisplayDirectory { get; set; }
    }
}