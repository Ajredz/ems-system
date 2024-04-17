using EMS.Manpower.Data.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.DataDuplication.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("org_group_position")]
    public class OrgGroupPosition : SyncFields
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("org_group_id")]
        public int OrgGroupID { get; set; }
        [Column("position_id")]
        public int PositionID { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
    }
}