using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Recruitment.Data.Applicant
{
    [Table("applicant_attachment")]
    public class ApplicantAttachment
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("applicant_id")]
        public int ApplicantID { get; set; }

        [Column("attachment_type")]
        public string AttachmentType { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("source_file")]
        public string SourceFile { get; set; }

        [Column("server_file")]
        public string ServerFile { get; set; }

        [Column("modified_by")]
        public int ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}