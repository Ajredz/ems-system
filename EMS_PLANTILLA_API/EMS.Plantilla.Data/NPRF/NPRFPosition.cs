using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.DBObjects.NPRF
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("nprf_position")]
    public class NPRFPosition
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("nprf_id")]
        public int NPRFID { get; set; }

        [Column("position_level_id")]
        public int PositionLevelID { get; set; }

        [Column("position_id")]
        public int PositionID { get; set; }

        [Column("headcount_existing")]
        public int HeadcoungExisting { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

    }


}
