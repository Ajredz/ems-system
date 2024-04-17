using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.KPIScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_kpiscore_get_by_id")]
    public class TableVarKPIScoreGetByID
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("kra_group")]
        public string KRAGroup { get; set; }

        [Column("kpi_name")]
        public string KPIName { get; set; }

        [Column("kpi_code")]
        public string KPICode { get; set; }

        [Column("kpi_description")]
        public string KPIDescription { get; set; }

        [Column("Target")]
        public decimal Target { get; set; }

        [Column("Actual")]
        public decimal Actual { get; set; }

        [Column("rate")]
        public decimal Rate { get; set; }

        [Column("org_group_id")]
        public int OrgGroup { get; set; }
        
        [Column("employee_id")]
        public int EmployeeID { get; set; }

        [Column("kpi_id")]
        public int KPIID { get; set; }
    }
}