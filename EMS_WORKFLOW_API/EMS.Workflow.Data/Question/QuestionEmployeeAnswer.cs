using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Workflow.Data.Question
{

    [Table("question_employee_answer")]
    public class QuestionEmployeeAnswer
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("employee_id")]
        public int EmployeeID { get; set; }
        [Column("question_id")]
        public int QuestionID { get; set; }
        [Column("answer_id")]
        public int? AnswerID { get; set; }
        [Column("answer_details")]
        public string AnswerDetails { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
    }
}
