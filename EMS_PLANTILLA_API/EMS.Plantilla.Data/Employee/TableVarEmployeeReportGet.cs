using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;


namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_report_get")]
    public class TableVarEmployeeReportGet
    {

        [Key]

        [Column("id")]
        public int ID { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        [Column("position")]
        public string Position { get; set; }

        [Column("org_group")]
        public string OrgGroup { get; set; }

        [Column("org_type")]
        public string OrgType { get; set; }

        [Column("home_branch")]
        public string HomeBranch { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("status_updated_date")]
        public string StatusUpdatedDate { get; set; }

        [Column("date_hired")]
        public string DateHired { get; set; }

        [Column("old_employee_id")]
        public string OldEmployeeID { get; set; }

        [Column("company_tag")]
        public string CompanyTag { get; set; }

        [Column("region")]
        public string Region { get; set; }

    }
}
