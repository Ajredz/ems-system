using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Security.Data._IntegrationModels
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tbl_users")]
    public class tbl_users
    {
        [Key]
        public int uid { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string status { get; set; }
        public int flag { get; set; }
        public int role { get; set; }
        public int emp_id { get; set; }
    }
}