using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.Plantilla.Data.Position
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_position")]
    public class TableVarPosition
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("position_level_id")]
        public int PositionLevelID { get; set; }

        [Column("position_level_description")]
        public string PositionLevelDescription { get; set; }

        [Column("title")]
        public string Title { get; set; }
        
        [Column("code")]
        public string Code { get; set; }

        [Column("parent_position_id")]
        public int? ParentPositionID { get; set; }

        [Column("parent_position_description")]
        public string ParentPositionDescription { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}