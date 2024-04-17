using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.LogActivity
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_log_activity")]
    public class TableVarLogActivity
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("module")]
        public string Module { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("sub_type")]
        public string SubType { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("is_pass_fail")]
        public string IsPassFail { get; set; }

        [Column("is_assignment")]
        public string IsAssignment { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}
