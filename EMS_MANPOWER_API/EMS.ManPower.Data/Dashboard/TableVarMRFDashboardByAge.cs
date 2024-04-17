using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.Dashboard
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_mrf_dashboard_by_age")]
    public class TableVarDescendants
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("open_desc")]
        public string OpenDesc { get; set; }

        [Column("open_val")]
        public int OpenValue { get; set; }

        [Column("open_count")]
        public int OpenCount { get; set; }

        [Column("closed_desc")]
        public string ClosedDesc { get; set; }

        [Column("closed_val")]
        public int ClosedValue { get; set; }

        [Column("closed_count")]
        public int ClosedCount { get; set; }

        [Column("age")]
        public string Age { get; set; }
    }
}
