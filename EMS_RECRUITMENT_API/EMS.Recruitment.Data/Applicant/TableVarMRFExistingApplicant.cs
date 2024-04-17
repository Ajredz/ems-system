using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.Applicant
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_mrf_existing_applicant")]
    public class TableVarMRFExistingApplicant
    {
        [Key]
        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }

        [Column("id")]
        public int ID { get; set; }

        [Column("applicant_name")]
        public string ApplicantName { get; set; }

        [Column("current_step")]
        public string CurrentStep { get; set; }

        //[Column("workflow_description")]
        //public string WorkflowDescription { get; set; }

        [Column("status")]
        public string Status { get; set; }
    }
}