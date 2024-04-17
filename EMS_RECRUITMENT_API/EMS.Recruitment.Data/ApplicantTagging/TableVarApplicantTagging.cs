using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Recruitment.Data.ApplicantTagging
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_applicant_tagging")]
    public class TableVarApplicantTagging
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }

        [Column("applicant_name")]
        public string ApplicantName { get; set; }

        [Column("application_source")]
        public string ApplicationSource { get; set; }

        [Column("position_remarks")]
        public string PositionRemarks { get; set; }

        [Column("referred_by")]
        public string ReferredBy { get; set; }
    }
}
