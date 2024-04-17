using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.IPM.Data.EmployeeScore
{
    [Table("trans_employee_score_staging")]
    public class TransEmployeeScoreStaging
    {
        [Key]
        [Column("id")]
        public int? ID { get; set; }
        [Column("trans_summary_id")]
        public int? TransSummaryID { get; set; }
        [Column("employee_id")]
        public int EmployeeID { get; set; }
        [Column("org_group_id")]
        public int? OrgGroupID { get; set; }
        [Column("position_id")]
        public int? PositionID { get; set; }
        [Column("kpi_id")]
        public int? KpiID { get; set; }
        [Column("kpi_weight")]
        public decimal? KpiWeight { get; set; }
        [Column("kpi_target")]
        public decimal? KpiTarget { get; set; }
        [Column("kpi_actual")]
        public decimal? KpiActual { get; set; }
        [Column("kpi_score")]
        public decimal? KpiScore { get; set; }
        [Column("kpi_score_sum")]
        public decimal? KpiScoreSum { get; set; }
        [Column("kpi_position_id")]
        public int? KpiPositionID { get; set; }
        [Column("pdate_from")]
        public DateTime? PDateFrom { get; set; }
        [Column("pdate_to")]
        public DateTime? PDateTo { get; set; }
        [Column("gdate_from")]
        public DateTime? GDateFrom { get; set; }
        [Column("gdate_to")]
        public DateTime? GDateTo { get; set; }
        [Column("is_autocreate")]
        public bool? IsAutocreate { get; set; }


    }
}
