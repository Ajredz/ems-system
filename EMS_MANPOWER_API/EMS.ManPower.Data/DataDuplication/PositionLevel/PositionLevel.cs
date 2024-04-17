using EMS.Manpower.Data.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.DataDuplication.PositionLevel
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("position_level")]
    public class PositionLevel : SyncFields
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("company_id")]
        public int CompanyID { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }
    }
}