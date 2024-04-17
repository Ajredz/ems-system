using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMS_SecurityServiceModel.SystemURL
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("system_url")]
    public class SystemURL
    {
        [Key]
        [Column("id")]
        public long ID { get; set; }

        [Column("page_id")]
        public int PageID { get; set; }

        [Column("url")]
        public string URL { get; set; }

        [Column("function_type")]
        public string FunctionType { get; set; }
    }
}
