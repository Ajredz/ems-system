using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Workflow.Data.Question
{

    [Table("sp_get_question_employee_answer_export")]
    public class SPGetQuestionEmployeeAnswerExport
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("employee_id")]
        public int EmployeeID { get; set; }
        [Column("column_g")]
        public string ColumnG { get; set; }
        [Column("column_h")]
        public string ColumnH { get; set; }
        [Column("column_i")]
        public string ColumnI { get; set; }
        [Column("column_j")]
        public string ColumnJ { get; set; }
        [Column("column_k")]
        public string ColumnK { get; set; }
        [Column("column_l")]
        public string ColumnL { get; set; }
        [Column("column_m")]
        public string ColumnM { get; set; }
        [Column("column_n")]
        public string ColumnN { get; set; }
        [Column("column_o")]
        public string ColumnO { get; set; }
        [Column("column_p")]
        public string ColumnP { get; set; }
    }
}
