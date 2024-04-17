using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.KPIPosition
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_kpiposition_details")]
    public class TableVarKPIPositionDetails
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

        [Column("kpi_name")]
        public string KPIName { get; set; }

        [Column("kpi_description")]
        public string KPIDescription { get; set; }

        [Column("kpi_id")]
        public int KPIID { get; set; }

        [Column("kpi")]
        public string KPI { get; set; }

        [Column("weight")]
        public decimal Weight { get; set; }

        [Column("weight_no_service_bay")]
        public decimal WeightNoServiceBay { get; set; }

        [Column("position_id")]
        public int PositionID { get; set; }
    }
}