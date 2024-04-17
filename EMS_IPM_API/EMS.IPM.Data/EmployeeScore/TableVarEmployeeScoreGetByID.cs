using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.EmployeeScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employeescore_get_by_id")]
    public class TableVarEmployeeScoreGetByID
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

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
        public decimal? Target { get; set; }

        [Column("actual")]
        public decimal? Actual { get; set; }

        [Column("rate")]
        public decimal? Rate { get; set; }

        [Column("trans_summary_id")]
        public int TransSummaryID { get; set; }

        [Column("employee")]
        public string Employee { get; set; }

        [Column("employee_code")]
        public string EmployeeCode { get; set; }

        [Column("org_group")]
        public string OrgGroup { get; set; }

        [Column("position")]
        public string Position { get; set; }

        [Column("tdate_from")]
        public string TDateFrom { get; set; }

        [Column("tdate_to")]
        public string TDateTo { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("is_editable")]
        public bool IsEditable { get; set; }

        [Column("requestor")]
        public string Requestor { get; set; }

        [Column("grade")]
        public string Grade { get; set; }
        [Column("source_type")]
        public string SourceType { get; set; }
        
        [Column("has_edit_access")]
        public bool HasEditAccess { get; set; }
    }
}