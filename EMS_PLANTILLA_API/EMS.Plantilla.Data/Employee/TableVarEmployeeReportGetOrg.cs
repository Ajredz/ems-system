using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;


namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_report_get_org")]
    public class TableVarEmployeeReportGetOrg
    {
        [Key]
        [Column("org_group_id")]
        public int OrgGroupID { get; set; }

        [Column("org_group")]
        public string OrgGroup { get; set; }

        [Column("position")]
        public string Position { get; set; }

        [Column("org_type")]
        public string OrgType { get; set; }

        [Column("region")]
        public string Region { get; set; }

        [Column("planned_count")]
        public int PlannedCount { get; set; }

        [Column("draft")]
        public int Draft { get; set; }

        [Column("prob")]
        public int Probationary { get; set; }

        [Column("regular")]
        public int Regular { get; set; }

        [Column("promoted")]
        public int Promoted { get; set; }

        [Column("outgoing")]
        public int Outgoing { get; set; }

        [Column("awol")]
        public int Awol { get; set; }

        [Column("backout")]
        public int Backout { get; set; }

        [Column("resigned")]
        public int Resigned { get; set; }

        [Column("deceased")]
        public int Deceased { get; set; }

        [Column("terminated")]
        public int terminated { get; set; }

        [Column("total_active")]
        public int TotalActive { get; set; }

        [Column("total_inactive")]
        public int TotalInactive { get; set; }

        [Column("variance")]
        public int Variance { get; set; }

    }
}
