using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Recruitment.Data.Applicant
{
    [Table("applicant_history")]
    public class ApplicantHistory
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("applicant_id")]
        public int ApplicantID { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("status_result")]
        public string StatusResult { get; set; }
        [Column("date_scheduled")]
        public DateTime? DateScheduled { get; set; }
        [Column("date_completed")]
        public DateTime? DateCompleted { get; set; }
        [Column("remarks")]
        public string Remarks { get; set; }
        [Column("mrf_id")]
        public string MRFID { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}