using EMS.IPM.Data.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.IPM.Data.DataDuplication.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("employee")]
    public class Employee : SyncFields

    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("old_employee_id")]
        public string OldEmployeeID { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("middlename")]
        public string MiddleName { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("suffix")]
        public string Suffix { get; set; }

        [Column("nickname")]
        public string Nickname { get; set; }

        [Column("gender")]
        public string Gender { get; set; }
        
        [Column("employment_status")]
        public string EmploymentStatus { get; set; }

        [Column("date_hired")]
        public DateTime? DateHired { get; set; }

        [Column("org_group_id")]
        public int OrgGroupID { get; set; }

        [Column("position_id")]
        public int PositionID { get; set; }

        //[Column("psgc_region_id")]
        //public int PSGCRegion { get; set; }

        //[Column("psgc_city_id")]
        //public int PSGCCity { get; set; }

        [Column("system_user_id")]
        public int SystemUserID { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}