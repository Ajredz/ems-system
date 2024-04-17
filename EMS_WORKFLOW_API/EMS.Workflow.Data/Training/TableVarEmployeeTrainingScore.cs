using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Workflow.Data.Training
{
    [Table("tv_employee_training_score")]
    public class TableVarEmployeeTrainingScore
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("take_exam_id")]
        public int TakeExamID { get; set; }

        [Column("exam_score")]
        public double ExamScore { get; set; }

        [Column("total_exam_score")]
        public double TotalExamScore { get; set; }

        [Column("average_score")]
        public double AverageScore { get; set; }

        [Column("total_exam_question")]
        public int TotalExamQuestion { get; set; }

        [Column("completed_at")]
        public string CompletedDate { get; set; }
    }
}
