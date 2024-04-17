using System;
using System.Collections.Generic;
using System.Text;
using Utilities.API;

namespace EMS.Workflow.Transfer.Training
{
    public class GetListOutput : JQGridResult
    {
        public int? ID { get; set; }
        public string TemplateName { get; set; }
        public string CreatedDate { get; set; }
    }
    public class GetEmployeeTrainingListOutput : JQGridResult
    {
        public int? ID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public string StatusColor { get; set; }
        public string StatusUpdateDate { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DateSchedule { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedDate { get; set; }

        public string Position { get; set; }
        public string OrgGroup { get; set; }
        public string Clus { get; set; }
        public string Area { get; set; }
        public string Reg { get; set; }
        public string Zone { get; set; }
    }

    public class GetEmployeeTrainingScoreOutput
    {
        public int ID { get; set; }
        public int TakeExamID { get; set; }
        public double ExamScore { get; set; }
        public double TotalExamScore { get; set; }
        public double AverageScore { get; set; }
        public int TotalExamQuestion { get; set; }
        public string CompletedDate { get; set; }
    }

    public class GetEmployeeTrainingStatusHistoryOutput
    {
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string CreatedName { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
    }

    public class GetClassroomFromELMS
    {
        public List<GetClassroomFromELMSData> Data { get; set; }
    }
    public class GetClassroomFromELMSData
    {
        public int Id { get; set; }
        public string Classroom { get; set; }
    }

    public class EmployeeIDOutput
    { 
        public int ID { get; set; }
    }
}
