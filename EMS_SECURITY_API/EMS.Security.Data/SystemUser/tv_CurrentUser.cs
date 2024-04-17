using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Security.Data.SystemUser
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_current_user")]
    public class tv_CurrentUser
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("middlename")]
        public string MiddleName { get; set; }

        [Column("company")]
        public string Company { get; set; }

        [Column("company_id")]
        public short CompanyID { get; set; }

        [Column("branch")]
        public string Branch { get; set; }

        [Column("branch_code")]
        public string BranchCode { get; set; }

        [Column("position")]
        public string Position { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("last_logged_in")]
        public DateTime? LastLoggedIn { get; set; }

        [Column("last_logged_out")]
        public DateTime? LastLoggedOut { get; set; }

        [Column("last_password_change")]
        public DateTime? LastPasswordChange { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }
    }
}
