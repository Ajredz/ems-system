using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.Plantilla.Data.Position
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_position_with_level_autocomplete")]
    public class TableVarPositionWithLevelAutoComplete
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }
}
