using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_org_group_nprf")]
    public class TableVarOrgGroupNPRF
    {
        [Key]
        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }

        [Column("id")]
        public int ID { get; set; }

        [Column("nprf_number")]
        public string NPRFNumber { get; set; }

        [Column("approved_date")]
        public string ApprovedDate { get; set; }
    }
}
