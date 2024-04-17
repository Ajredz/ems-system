using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.MRF
{
    [Table("mrf_applicant_kickout_question")]
    public class ApplicantKickoutQuestion
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("applicant_id")]
        public int ApplicationId { get; set; }
        
        [Column("question_id")]
        public int QuestionID { get; set; }

        [Column("answer")]
        public string Answer { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}