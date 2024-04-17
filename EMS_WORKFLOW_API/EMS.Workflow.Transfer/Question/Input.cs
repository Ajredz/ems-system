using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Workflow.Transfer.Question
{
    public class QuestionEmployeeAnswerInput
    {
        public int EmployeeID { get; set; }
        public int QuestionID { get; set; }
        public int AnswerID { get; set; }
        public string AnswerDetails { get; set; }
    }
}
