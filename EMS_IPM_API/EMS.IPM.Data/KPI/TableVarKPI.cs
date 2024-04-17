using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.KPI
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_kpi")]
    public class TableVarKPI
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("kra_type")]
        public string KRAType { get; set; }

        [Column("kra_group")]
        public string KRAGroup { get; set; }

        [Column("kra_sub_group")]
        public string KRASubGroup { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("old_kpi_code")]
        public string OldKPICode { get; set; }

        [Column("kpi_type")]
        public string KPIType { get; set; }

        [Column("source_type")]
        public string SourceType { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}