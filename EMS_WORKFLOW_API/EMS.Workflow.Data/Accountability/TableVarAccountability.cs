using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.Accountability
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_accountability")]
    public class TableVarAccountability
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("preload_name")]
        public string PreloadName { get; set; }

        [Column("date_created")]
        public string DateCreated { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }

    public class TableVarAccountabilityDashboard
    {
        [Key]
        [Column("description")]
        public string Description { get; set; }

        [Column("actual")]
        public int Actual { get; set; }

        [Column("target")]
        public int Target { get; set; }
    }

}
