using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.KPI
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_new_kpi_code")]
    public class TableVarNewKPICode
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("new_kpi_code")]
        public string NewKPICode { get; set; }
    }
}