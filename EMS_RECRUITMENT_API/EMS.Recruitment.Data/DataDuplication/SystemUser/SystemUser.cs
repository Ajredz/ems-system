using EMS.Recruitment.Data.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Recruitment.Data.DataDuplication.SystemUser
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("system_user")]
    public class SystemUser : SyncFields
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("middlename")]
        public string MiddleName { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("username")]
        public string UserName { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }
    }
}
