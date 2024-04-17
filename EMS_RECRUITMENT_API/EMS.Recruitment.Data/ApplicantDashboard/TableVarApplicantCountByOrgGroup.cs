using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Recruitment.Data.ApplicantDashboard
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_applicant_count_by_org_group")]
    public class TableVarApplicantCountByOrgGroup
    {
        [Key]
        [Column("org_group")]
        public string OrgGroup { get; set; }

        [Column("position_title")]
        public string PositionTitle { get; set; }

        [Column("applicant_count")]
        public int ApplicantCount { get; set; }
    }
}
