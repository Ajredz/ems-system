using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.DataDuplication.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_org_group_descendants")]
    public class TableVarOrgGroupDescendants
    {
        [Key]
        [Column("descendants")]
        public string Descendants { get; set; }
    }
}
