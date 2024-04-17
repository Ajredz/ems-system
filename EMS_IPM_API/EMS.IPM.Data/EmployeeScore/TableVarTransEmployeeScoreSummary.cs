using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.EmployeeScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_trans_employee_score_summary")]
    public class TableVarTransEmployeeScoreSummary
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("filter_by")]
        public string FilterBy { get; set; }
        [Column("filter_org_group")]
        public string FilterOrgGroup { get; set; }
        [Column("filter_include_lvl_below")]
        public bool FilterIncludeLevelBelow { get; set; }
        [Column("filter_position")]
        public string FilterPosition { get; set; }
        [Column("filter_employee")]
        public string FilterEmployee { get; set; }
        [Column("filter_override")]
        public bool FilterOverride { get; set; }
        [Column("filter_use_current")]
        public bool FilterUseCurrent { get; set; }
        [Column("tdate_from")]
        public string TDateFrom { get; set; }
        [Column("tdate_to")]
        public string TDateTo { get; set; }
        [Column("processed_employees")]
        public long ProcessedEmployees { get; set; }
        [Column("total_num_of_employees")]
        public long TotalNumOfEmployees { get; set; }
        [Column("employees_with_ipm")]
        public int EmployeesWithIPM { get; set; }
        [Column("rating_ee_employees")]
        public int RatingEEEmployees { get; set; }
        [Column("rating_me_employees")]
        public int RatingMEEmployees { get; set; }
        [Column("rating_sbe_employees")]
        public int RatingSBEEmployees { get; set; }
        [Column("rating_be_employees")]
        public int RatingBEEmployees { get; set; }
        [Column("rating_ee_min")]
        public string RatingEEMin { get; set; }
        [Column("rating_ee_max")]
        public string RatingEEMax { get; set; }
        [Column("rating_me_min")]
        public string RatingMEMin { get; set; }
        [Column("rating_me_max")]
        public string RatingMEMax { get; set; }
        [Column("rating_sbe_min")]
        public string RatingSBEMin { get; set; }
        [Column("rating_sbe_max")]
        public string RatingSBEMax { get; set; }
        [Column("rating_be_min")]
        public string RatingBEMin { get; set; }
        [Column("rating_be_max")]
        public string RatingBEMax { get; set; }
        [Column("is_done")]
        public bool IsDone { get; set; }
        [Column("is_trans_active")]
        public bool IsTransActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [Column("created_date")]
        public string CreatedDate { get; set; }
        /*[Column("total_employee_with_ipm")]
        public int TotalEmployeesWithIPM { get; set; }
        [Column("employee_with_multiple_ipm")]
        public int EmployeesWithMultiple { get; set; }
        [Column("total_ipm_result")]
        public int TotalIPMResult { get; set; }
        [Column("run_start")]
        public DateTime? RunStart { get; set; }
        [Column("run_end")]
        public DateTime? RunEnd { get; set; }*/

    }
}