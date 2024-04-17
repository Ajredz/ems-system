using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.KPIScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("kpi_score")]
    public class KPIScore
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("org_group_id")]
        public int OrgGroup { get; set; }

        [Column("kpi_id")]
        public int KPI { get; set; }

        [Column("target")]
        public decimal? Target { get; set; }

        [Column("actual")]
        public decimal? Actual { get; set; }

        [Column("rate")]
        public decimal Rate { get; set; }
        
        [Column("period")]
        public DateTime Period { get; set; }

        [Column("formula")]
        public string Formula { get; set; }

        [Column("modified_by")]
        public int ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime ModifiedDate { get; set; }
    }
}