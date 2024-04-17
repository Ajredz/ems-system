using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.KPI
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("kpi")]
    public class KPI
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("old_kpi_code")]
        public string OldKPICode { get; set; }

        [Column("description")]
        public string Description { get; set; }
        
        [Column("guidelines")]
        public string Guidelines { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("kra_group")]
        public int KRAGroup { get; set; }

        [Column("kra_sub_group")]
        public int? KRASubGroup { get; set; }

        [Column("kpi_type")]
        public string Type { get; set; }

        [Column("source_type")]
        public string SourceType { get; set; }

        [Column("modified_by")]
        public int ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime ModifiedDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }
    }
}