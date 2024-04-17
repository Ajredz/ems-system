using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Workflow.Data.Question
{
    [Table("tv_question_employee_answer")]
    public class TableVarQuestionEmployeeAnswer
    {
        [Key]
        [Column("id")]
        public int QuestionID { get; set; }

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

        [Column("answer_id")]
        public string AnswerID { get; set; }

        [Column("answer")]
        public string Answer { get; set; }

        [Column("add_reason")]
        public string AddReason { get; set; }

        [Column("employee_answer_id")]
        public int? EmployeeAnswerID { get; set; }

        [Column("employee_answer_details")]
        public string EmployeeAnswerDetails { get; set; }
    }
}
