using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Recruitment.Data.Applicant
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_applicant_name")]
    public class TableVarApplicantName
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("applicant_name")]
        public string ApplicantName { get; set; }
    }
}
