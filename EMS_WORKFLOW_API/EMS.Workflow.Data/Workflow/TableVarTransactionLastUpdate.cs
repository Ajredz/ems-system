using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.Workflow
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_transaction_last_update")]
    public class TableVarTransactionLastUpdate
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("record_id")]
        public string RecordID { get; set; }
        [Column("end_date_time")]
        public string EndDateTime { get; set; }
        [Column("approved_by")]
        public int ApprovedBy { get; set; }
    }
}