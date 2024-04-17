using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.IPM.Data.EmployeeScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("trans_employee_final_score")]
    public class TransEmployeeFinalScore
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("run_id")]
        public int RunID { get; set; }
        [Column("run_title")]
        public string RunTitle { get; set; }
        [Column("employee_id")]
        public int EmployeeID { get; set; }
        [Column("employee_name")]
        public string EmployeeName { get; set; }
        [Column("ipm_count")]
        public int IPMCount { get; set; }
        [Column("ipm_months")]
        public int IPMMonths { get; set; }
        [Column("final_score")]
        public decimal FinalScore { get; set; }
        [Column("is_old")]
        public bool isOld { get; set; }

        [Column("is_active")]
        public bool isActive { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

    }
}
