using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_hierarchy_levels")]
    public class TableVarHierarchyLevel
    {
        [Key]
        [Column("hierarchy_level")]
        public int HierarchyLevel { get; set; }
    }
}