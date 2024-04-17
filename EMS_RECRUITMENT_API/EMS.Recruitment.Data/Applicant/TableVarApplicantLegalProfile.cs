using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.Applicant
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_applicant_legal_profile")]
    public class TableVarApplicantLegalProfile
    {
        [Key]
        [Column("row_num")]
        public int? RowNum { get; set; }

        [Column("applicant_id")]
        public int? ApplicantID { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("legal_answer")]
        public string LegalAnswer { get; set; }

        [Column("created_by")]
        public int? CreatedBy { get; set; }

        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
    }
}