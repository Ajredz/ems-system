using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.EmployeeScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("trans_employee_score_summary")]
    public class TransEmployeeScoreSummary
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("tdate_from")]
        public DateTime? TDateFrom { get; set; }
        [Column("tdate_to")]
        public DateTime? TDateTo { get; set; }
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
        [Column("is_done")]
        public bool IsDone { get; set; }
        [Column("is_trans_active")]
        public bool IsTransActive { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
        [Column("filter_by")]
        public string FilterBy { get; set; }
        [Column("filter_employee")]
        public string FilterEmployee { get; set; }
        [Column("filter_org_group")]
        public string FilterOrgGroup { get; set; }
        [Column("filter_include_lvl_below")]
        public bool filterIncludeLvlBelow { get; set; }
        [Column("filter_position")]
        public string FilterPosition { get; set; }



    }
}