using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.EmployeeScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("trans_employee_score_details")]
    public class TransEmployeeScoreDetails
    {
        [Key]
        [Column("id")]
        public long ID { get; set; }
        [Column("trans_id")]
        public int TransID { get; set; }
        [Column("kpi_id")]
        public int KPIID { get; set; }
        [Column("kpi_weight")]
        public decimal KPIWeight { get; set; }
        [Column("kpi_target")]
        public decimal? KPITarget { get; set; }
        [Column("kpi_actual")]
        public decimal? KPIActual { get; set; }
        [Column("is_editable")]
        public bool IsEditable { get; set; }
        [Column("kpi_score")]
        public decimal? KPIScore { get; set; }
        [Column("modified_by")]
        public int? ModifiedBy { get; set; }
        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

    }
}