using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Workflow.Data.Training
{
    [Table("employee_training")]
    public class EmployeeTraining
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("employee_id")]
        public int EmployeeID { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("status_update_date")]
        public DateTime? StatusUpdateDate { get; set; }
        [Column("classroom_id")]
        public int? ClassroomID { get; set; }
        [Column("classroom_name")]
        public string ClassroomName { get; set; }
        [Column("type")]
        public string Type { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("description")]
        public string Description { get; set; }

        [Column("date_schedule")]
        public DateTime? DateSchedule { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
        [Column("modified_by")]
        public int? ModifiedBy { get; set; }
        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }
}
