using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.KPIPosition
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("kpi_position")]
    public class KPIPosition
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("tdate")]
        public DateTime TDate { get; set; }

        [Column("position_id")]
        public int Position { get; set; }

        [Column("kpi_id")]
        public int KPI { get; set; }

        [Column("weight")]
        public decimal Weight { get; set; }

        [Column("weight_no_service_bay")]
        public decimal WeightNoServiceBay { get; set; }

        [Column("is_latest")]
        public bool IsLatest { get; set; }

        [Column("modified_by")]
        public int ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime ModifiedDate { get; set; }
    }
}