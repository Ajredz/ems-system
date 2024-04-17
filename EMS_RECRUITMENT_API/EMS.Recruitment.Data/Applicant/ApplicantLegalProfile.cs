using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Recruitment.Data.Applicant
{
    [Table("applicant_legal_profile")]
    public class ApplicantLegalProfile
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("applicant_id")]
        public int ApplicationId { get; set; }
        
        [Column("legal_number")]
        public string LegalNumber { get; set; }

        [Column("legal_answer")]
        public string LegalAnswer { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}