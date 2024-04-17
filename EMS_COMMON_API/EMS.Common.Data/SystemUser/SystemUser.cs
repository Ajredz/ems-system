using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.Common.Data.SystemUser
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("system_user")]
    public class SystemUser
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("salt")]
        public string Salt { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("middlename")]
        public string MiddleName { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("failed_attempt")]
        public short FailedAttempt { get; set; }

        [Column("is_password_changed")]
        public bool IsPasswordChanged { get; set; }

        [Column("is_logged_in")]
        public bool IsLoggedIn { get; set; }

        [Column("integration_key")]
        public string IntegrationKey { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("last_logged_in")]
        public DateTime? LastLoggedIn { get; set; }

        [Column("last_logged_out")]
        public DateTime? LastLoggedOut { get; set; }

        [Column("last_password_change")]
        public DateTime? LastPasswordChange { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

    }
}
