using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.KPI
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_kpi_details")]
    public class TableVarKPIDetails
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("kra_group")]
        public string KRAGroup { get; set; }

        [Column("kra_sub_group")]
        public string KRASubGroup { get; set; }

        [Column("kpi_code")]
        public string KPICode { get; set; }

        [Column("old_kpi_code")]
        public string OldKPICode { get; set; }

        [Column("kpi_name")]
        public string KPIName { get; set; }

        [Column("kpi_description")]
        public string KPIDescription { get; set; }
    }
}