using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.Applicant
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_applicant_history")]
    public class TableVarApplicantHistory
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("order")]
        public int Order { get; set; }
        [Column("step")]
        public string Step { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("timestamp")]
        public string Timestamp { get; set; }
        [Column("remarks")]
        public string Remarks { get; set; }
        [Column("result_type")]
        public string ResultType { get; set; }
    }
}