using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.EmployeeScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employeescore")]
    public class TableVarEmployeeScore
    {
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

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}