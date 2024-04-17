using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Workflow.Data.Training
{
    [Table("tv_employee_training")]
    public class TableVarEmployeeTraining
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("employee_id")]
        public int EmployeeID { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("status_update_date")]
        public string StatusUpdateDate { get; set; }
        [Column("date_schedule")]
        public string DateSchedule { get; set; }
        [Column("type")]
        public string Type { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        public string CreatedDate { get; set; }
        [Column("modified_by")]
        public int? ModifiedBy { get; set; }
        [Column("modified_date")]
        public string ModifiedDate { get; set; }


        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}
