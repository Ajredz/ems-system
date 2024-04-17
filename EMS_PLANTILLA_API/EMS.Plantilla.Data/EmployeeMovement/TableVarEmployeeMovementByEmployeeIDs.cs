using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Plantilla.Data.EmployeeMovement
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_movement_by_employee_ids")]
    public class TableVarEmployeeMovementByEmployeeIDs

    {
        [Key]
        [Column("id")]
        public long ID { get; set; }

        [Column("employee_id")]
        public int EmployeeID { get; set; }

        [Column("movement_type")]
        public string MovementType { get; set; }

        [Column("employee_field")]
        public string EmployeeField { get; set; }

        [Column("from")]
        public string From { get; set; }

        [Column("to")]
        public string To { get; set; }

        [Column("date_effective_from")]
        public string DateEffectiveFrom { get; set; }

        [Column("date_effective_to")]
        public string DateEffectiveTo { get; set; }
    }
}
