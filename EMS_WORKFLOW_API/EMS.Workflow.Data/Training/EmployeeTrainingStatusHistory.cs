using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Workflow.Data.Training
{
    [Table("employee_training_status_history")]
    public class EmployeeTrainingStatusHistory
    {
        [Key]
        [Column("id")]
        public long ID { get; set; }

        [Column("employee_training_id")]
        public int EmployeeTrainingID { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
    }
}
