using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_org_group")]
    public class TableVarOrgGroup
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("org_type_description")]
        public string OrgTypeDescription { get; set; }

        [Column("parent_org_description")]
        public string ParentOrgDescription { get; set; }

        [Column("is_branch_active")]
        public string IsBranchActive { get; set; }

        [Column("service_bay_count")]
        public int ServiceBayCount { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}