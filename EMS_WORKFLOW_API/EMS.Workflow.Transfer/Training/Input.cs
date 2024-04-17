using System;
using System.Collections.Generic;
using System.Text;
using Utilities.API;

namespace EMS.Workflow.Transfer.Training
{
    public class GetListInput : JQGridFilter
    {
        public int? ID { get; set; }

        public string TemplateName { get; set; }

        public string CreatedDateFrom { get; set; }

        public string CreatedDateTo { get; set; }

        public bool IsExport { get; set; }
    }

    public class TrainingTempateInput
    {
        public int ID { get; set; }
        public string TemplateName { get; set; }
        public int CreatedBy { get; set; }
        public List<TrainingTempateDetailsInput> TrainingTempateDetailsInputList { get; set; }
    }
    public class TrainingTempateDetailsInput
    {
        public int ID { get; set; }
        public int TrainingTemplateID { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }

    public class GetEmployeeTrainingListInput : JQGridFilter
    {
        public int? ID { get; set; }
        public string EmployeeID { get; set; }
        public string StatusDelimited { get; set; }
        public string StatusUpdateDateFrom { get; set; }
        public string StatusUpdateDateTo { get; set; }
        public string DateScheduleFrom { get; set; }
        public string DateScheduleTo { get; set; }
        public string TypeDelimited { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAdminAccess { get; set; }
        public bool IsResolver { get; set; }
        public bool IsExport { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDateFrom { get; set; }
        public string CreatedDateTo { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDateFrom { get; set; }
        public string ModifiedDateTo { get; set; }
        public string UnderAccess { get; set; }
    }

    public class AddEmployeeTrainingInput
    {
        public long EmployeeID { get; set; }
        public List<int> TrainingEmployeeID { get; set; }
    }

    public class EmployeeTrainingForm
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public DateTime? StatusUpdateDate { get; set; }
        public int? ClassroomID { get; set; }
        public string ClassroomName { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DateSchedule { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public class TrainingUploadFile
    {
        public string RowNum { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DateSchedule { get; set; }
        public DateTime? DateScheduleConvert { get; set; }
        public string ClassroomIDString { get; set; }
        public int ClassroomID { get; set; }
        public string ClassroomName { get; set; }
    }
}
