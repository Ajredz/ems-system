using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.EmailServerCredential
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("cron_logs")]
    public class CronLogs
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("cron_name")]
        public string CronName { get; set; }

        [Column("cron_link")]
        public string CronLink { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
    }
}
