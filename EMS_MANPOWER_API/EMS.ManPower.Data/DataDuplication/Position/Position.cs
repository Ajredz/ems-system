using EMS.Manpower.Data.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.Manpower.Data.DataDuplication.Position
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("position")]
    public class Position : SyncFields

    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("position_level_id")]
        public int PositionLevelID { get; set; }
        
        [Column("parent_position_id")]
        public int? ParentPositionID { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }
    }
}