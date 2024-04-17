using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("org_group_position")]
    public class OrgGroupPosition : JQGridResult
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("org_group_id")]
        public int OrgGroupID { get; set; }

        [Column("position_id")]
        public int PositionID { get; set; }

        [Column("planned_count")]
        public int PlannedCount { get; set; }

        [Column("active_count")]
        public int ActiveCount { get; set; }

        [Column("inactive_count")]
        public int InactiveCount { get; set; }

        [Column("is_head")]
        public bool IsHead { get; set; }

        [Column("reporting_position_id")]
        public int? ReportingPositionID { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

    }
}
