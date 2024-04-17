using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.Security.Data.SystemErrorLog
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_error_log")]
    public class TableVarErrorLog
    {
        [Key]
        [Column("id")]
        public long ID { get; set; }
        [Column("layer")]
        public string Layer { get; set; }
        [Column("class")]
        public string Class { get; set; }
        [Column("method")]
        public string Method { get; set; }
        [Column("error_message")]
        public string ErrorMessage { get; set; }
        [Column("inner_exception")]
        public string InnerException { get; set; }
        [Column("user_id")]
        public int UserID { get; set; }
        [Column("user")]
        public string User { get; set; }
        [Column("created_date")]
        public string CreatedDate { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}
