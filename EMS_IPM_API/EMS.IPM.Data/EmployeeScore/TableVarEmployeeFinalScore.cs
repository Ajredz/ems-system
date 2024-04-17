using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.IPM.Data.EmployeeScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_final_Score")]
    public class TableVarEmployeeFinalScore
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
        public string IPMMonths { get; set; }

        [Column("final_score")]
        public decimal FinalScore { get; set; }

        [Column("final_quali")]
        public decimal FinalQuali { get; set; }

        [Column("final_remarks")]
        public string FinalRemarks { get; set; }

        [Column("is_old")]
        public string IsOld { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        public string CreatedDate { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}
