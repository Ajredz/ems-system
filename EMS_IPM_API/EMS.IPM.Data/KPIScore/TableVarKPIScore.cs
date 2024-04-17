using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.KPIScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_kpiscore")]
    public class TableVarKPIScore
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("employee")]
        public string Employee { get; set; }

        [Column("parent_org_group")]
        public string ParentOrgGroup { get; set; }

        [Column("org_group")]
        public string OrgGroup { get; set; }

        [Column("kpi")]
        public string KPI { get; set; }

        [Column("target")]
        public decimal Target { get; set; }

        [Column("actual")]
        public decimal Actual { get; set; }

        [Column("rate")]
        public decimal Rate { get; set; }
        
        [Column("period")]
        public string Period { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}