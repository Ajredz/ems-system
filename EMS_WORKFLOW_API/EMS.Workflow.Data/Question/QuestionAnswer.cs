using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Workflow.Data.Question
{
    [Table("question_answer")]
    public class QuestionAnswer
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("question_id")]
        public int QuestionID { get; set; }
        [Column("answer")]
        public string Answer { get; set; }
        [Column("add_reason")]
        public bool AddReason { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
    }
}
