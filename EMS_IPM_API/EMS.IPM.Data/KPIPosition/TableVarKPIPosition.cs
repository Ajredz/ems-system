using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.KPIPosition
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_kpiposition")]
    public class TableVarKPIPosition
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("position")]
        public string Position { get; set; }

        [Column("weight")]
        public decimal Weight { get; set; }

        [Column("effective_date")]
        public string EffectiveDate { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}