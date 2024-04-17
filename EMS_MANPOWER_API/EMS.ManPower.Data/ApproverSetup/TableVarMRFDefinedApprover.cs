using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.ApproverSetup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_mrf_defined_approver")]
    public class TableVarMRFDefinedApprover
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("org_group")]
        public string OrgGroup { get; set; }

        [Column("has_approver")]
        public string HasApprover { get; set; }

        [Column("last_modified_date")]
        public string LastModifiedDate { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}