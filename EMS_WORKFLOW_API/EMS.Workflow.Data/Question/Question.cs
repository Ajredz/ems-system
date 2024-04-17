using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Workflow.Data.Question
{
    [Table("question")]
    public class QuestionTable
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("category")]
        public string Category { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("question")]
        public string Question { get; set; }
        [Column("question_type")]
        public string QuestionType { get; set; }
        [Column("answer_type")]
        public string AnswerType { get; set; }
        [Column("parent_question_id")]
        public int ParentQuestionID { get; set; }
        [Column("tab")]
        public int Tab { get; set; }
        [Column("order")]
        public int Order { get; set; }
        [Column("is_required")]
        public bool IsRequired { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
    }
}
