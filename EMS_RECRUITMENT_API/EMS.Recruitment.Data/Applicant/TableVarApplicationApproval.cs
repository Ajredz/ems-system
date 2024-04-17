using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.Applicant
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_application_approval")]
    public class TableVarApplicationApproval
    {
        [Key]
        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }

        [Column("id")]
        public int ID { get; set; }
        
        [Column("workflow_id")]
        public int WorkflowID { get; set; }

        [Column("applicant_name")]
        public string ApplicantName { get; set; }

        [Column("application_source")]
        public string ApplicationSource { get; set; }

        //[Column("current_step")]
        //public string CurrentStep { get; set; }

        //[Column("workflow_description")]
        //public string WorkflowDescription { get; set; }

        [Column("position_remarks")]
        public string PositionRemarks { get; set; }

        [Column("course")]
        public string Course { get; set; }

        [Column("current_position")]
        public string CurrentPositionTitle { get; set; }

        [Column("expected_salary")]
        public decimal? ExpectedSalary { get; set; }

        [Column("date_applied")]
        public string DateApplied { get; set; }

        [Column("has_approval")]
        public bool HasApproval { get; set; }
    }
}