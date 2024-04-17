using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.Plantilla.Data.Position
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("position")]
    public class Position

    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("position_level_id")]
        public int PositionLevelID { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("parent_position_id")]
        public int? ParentPositionID { get; set; }

        [Column("job_class_code")]
        public string JobClassCode { get; set; }

        [Column("OnlinePosition")]
        public string OnlinePosition { get; set; }

        [Column("OnlineLocation")]
        public string OnlineLocation { get; set; }

        [Column("OnlineJobDescription")]
        public string OnlineJobDescription { get; set; }

        [Column("OnlineJobQualification")]
        public string OnlineJobQualification { get; set; }

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