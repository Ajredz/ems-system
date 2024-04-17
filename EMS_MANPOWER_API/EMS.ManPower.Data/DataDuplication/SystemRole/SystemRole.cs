using EMS.Manpower.Data.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.DataDuplication.SystemRole
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("system_role")]
    public class SystemRole : SyncFields
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("role_name")]
        public string RoleName { get; set; }

        [Column("company_id")]
        public short CompanyID { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

    }
}