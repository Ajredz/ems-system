using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EMS.Workflow.Data.LogActivity
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_log_activity_preloaded")]
    public class TableVarLogActivityPreLoaded
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("preload_name")]
        public string PreloadName { get; set; }

        [Column("date_created")]
        public string DateCreated { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}
