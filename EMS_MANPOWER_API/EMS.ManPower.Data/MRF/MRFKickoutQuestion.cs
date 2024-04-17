using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Manpower.Data.MRF
{

    [Table("mrf_kickout_question")]
    public class MRFKickoutQuestion
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("mrf_id")]
        public int MRFID { get; set; }

        [Column("kickout_question_id")]
        public int KickoutQuestionID { get; set; }

        [Column("order")]
        public int Order { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}
