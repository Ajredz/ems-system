using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.EmployeeScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_kpi_score")]
    public class TableVarEmployeeKPIScore
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Column("kpi_id")]
        public int KPIID { get; set; }
        
        [Column("kra_group")]
        public string KRAGroup { get; set; }

        [Column("kpi_code")]
        public string KPICode { get; set; }
        
        [Column("kpi_name")]
        public string KPIName { get; set; }
        
        [Column("kpi_description")]
        public string KPIDescription { get; set; }
        
        [Column("kpi_guidelines")]
        public string KPIGuidelines { get; set; }

        [Column("weight")]
        public decimal Weight { get; set; }
        
        [Column("target")]
        public decimal Target  { get; set; }
        
        [Column("actual")]
        public decimal Actual { get; set; }
        
        [Column("rate")]
        public decimal Rate { get; set; }
        
        [Column("total")]
        public decimal Total { get; set; }

         [Column("grade")]
        public string Grade { get; set; }
        [Column("source_type")]
        public string SourceType { get; set; }

        [Column("is_editable")]
        public bool IsEditable { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}