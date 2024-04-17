using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.EmployeeScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employeescoreonly")]
    public class TableVarEmployeeScoreOnly    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("trans_summary_id")]
        public int TransSummaryID { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("is_active")]
        public string IsActive { get; set; }

        [Column("employee")]
        public string Employee { get; set; }

        [Column("parent_org_group")]
        public string ParentOrgGroup { get; set; }

        [Column("org_group")]
        public string OrgGroup { get; set; }

        [Column("position")]
        public string Position { get; set; }

        [Column("score")]
        public decimal Score { get; set; }

        [Column("tdate_from")]
        public string TDateFrom { get; set; }

        [Column("tdate_to")]
        public string TDateTo { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("pdate_from")]
        public string PDateFrom { get; set; }

        [Column("pdate_to")]
        public string PDateTo { get; set; }

        [Column("date_effective_from")]
        public string DateEffectiveFrom { get; set; }

        [Column("date_effective_to")]
        public string DateEffectiveTo { get; set; }

        [Column("quali_plan")]
        public decimal QualiPlan { get; set; }

        [Column("quali_actual")]
        public decimal QualiActual { get; set; }

        [Column("quali_branch_performance")]
        public decimal QualiBranchPerformance { get; set; }

        [Column("quali_pro_rate_performance")]
        public decimal QualiProRatePerformance { get; set; }

        [Column("quali_final")]
        public decimal QualiFinal { get; set; }

        [Column("quali_remarks")]
        public string QualiRemarks { get; set; }

        [Column("ipm_months")]
        public int IPMMonths { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}