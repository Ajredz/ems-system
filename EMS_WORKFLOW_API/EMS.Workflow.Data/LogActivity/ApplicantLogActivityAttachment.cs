using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.LogActivity
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("applicant_log_activity_attachment")]
    public class ApplicantLogActivityAttachment
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("applicant_log_activity_id")]
        public int ApplicantLogActivityID { get; set; }
        [Column("attachment_type")]
        public string AttachmentType { get; set; }
        [Column("remarks")]
        public string Remarks { get; set; }
        [Column("source_file")]
        public string SourceFile { get; set; }
        [Column("server_file")]
        public string ServerFile { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
        [Column("modified_by")]
        public int? ModifiedBy { get; set; }
        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }
}
