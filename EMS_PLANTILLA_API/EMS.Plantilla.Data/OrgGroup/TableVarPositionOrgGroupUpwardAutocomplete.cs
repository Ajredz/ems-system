using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_position_org_group_upward_autocomplete")]
    public class TableVarPositionOrgGroupUpwardAutocomplete
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("position")]
        public string Position { get; set; }
        [Column("org_group")]
        public string OrgGroup { get; set; }
    }
}