using EMS.IPM.Data.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.IPM.Data.DataDuplication.EmployeeMovement
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("employee_movement")]
    public class EmployeeMovement

    {
        [Key]
        [Column("id")]
        public long ID { get; set; }

        [Column("sync_id")]
        public long SyncID { get; set; }

        [Column("sync_date")]
        public DateTime SyncDate { get; set; }

        [Column("employee_id")]
        public int EmployeeID { get; set; }

        [Column("movement_type")]
        public string MovementType { get; set; }

        [Column("employee_field")]
        public string EmployeeField { get; set; }

        [Column("reason")]
        public string Reason { get; set; }

        [Column("from")]
        public string From { get; set; }

        [Column("from_id")]
        public string FromID { get; set; }

        [Column("to")]
        public string To { get; set; }

        [Column("to_id")]
        public string ToID { get; set; }

        [Column("date_effective_from")]
        public DateTime DateEffectiveFrom { get; set; }

        [Column("date_effective_to")]
        public DateTime? DateEffectiveTo { get; set; }

        [Column("hrd_comments")]
        public string HRDComments { get; set; }

        [Column("is_latest")]
        public bool IsLatest { get; set; }

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