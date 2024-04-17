using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Recruitment.Data.SystemErrorLog
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("error_log")]
    public class ErrorLog
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
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
}
}
