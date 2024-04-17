using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.Workflow.Transfer.Question
{
    public class GetQuestionOutput
    {
        public int QuestionID { get; set; }
        public string Code { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string AnswerType { get; set; }
        public int ParentQuestionID { get; set; }
        public int Tab { get; set; }
        public int Order { get; set; }
        public string AnswerID { get; set; }
        public string Answer { get; set; }
        public string AddReason { get; set; }
        public int? EmployeeAnswerID { get; set; }
        public string EmployeeAnswerDetails { get; set; }
    }

    public class QuestionOutput
    {
        public int ID { get; set; }
        public int? QuestionID { get; set; }
        public string QuestionName { get; set; }
        public string Answer { get; set; }
        public string AnswerType { get; set; }
    }

    public class QuestionTableOutput
    { 
        public List<GetQuestionTable> MainQuestion { get; set; }
    }

    public class GetQuestionTable
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string Code { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string AnswerType { get; set; }
        public int ParentQuestionID { get; set; }
        public int Tab { get; set; }
        public int Order { get; set; }
        public bool IsRequired { get; set; }
        public List<GetQuestionTable> SubQuestion { get; set; }
        public List<AnswerTable> Answer { get; set; }
    }

    public class AnswerTable
    { 
        public int ID { get; set; }
        public int QuestionID { get; set; }
        public string Answer { get; set; }
        public bool AddReason { get; set; }
    }
    public class SPGetQuestionEmployeeAnswerExport
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string ColumnG { get; set; }
        public string ColumnH { get; set; }
        public string ColumnI { get; set; }
        public string ColumnJ { get; set; }
        public string ColumnK { get; set; }
        public string ColumnL { get; set; }
        public string ColumnM { get; set; }
        public string ColumnN { get; set; }
        public string ColumnO { get; set; }
        public string ColumnP { get; set; }

        public string OrgGroup { get; set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public string DateHired { get; set; }
        public string DateSeparated { get; set; }
    }

    public class QuestionTable
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string Code { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string AnswerType { get; set; }
        public int ParentQuestionID { get; set; }
        public int Tab { get; set; }
        public int Order { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
    }
}
