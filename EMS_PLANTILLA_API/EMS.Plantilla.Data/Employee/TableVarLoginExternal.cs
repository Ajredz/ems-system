using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;


namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_login_external")]
    public class TableVarLoginExternal
    {

        [Key]

        [Column("id")]
        public int ID { get; set; }

        [Column("system_user_id")]
        public int SystemUserID { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("middlename")]
        public string MiddleName { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("fullname")]
        public string FullName { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("number")]
        public string Number { get; set; }

        [Column("date_hired")]
        public string DateHired { get; set; }

        [Column("resigned_date")]
        public string ResignedDate { get; set; }

        [Column("position_id")]
        public int PositionID { get; set; }

        [Column("position_code")]
        public string PositionCode { get; set; }

        [Column("position_title")]
        public string PositionTitle { get; set; }

        [Column("org_group_id")]
        public int OrgGroupID { get; set; }

        [Column("org_code")]
        public string OrgCode { get; set; }

        [Column("org_description")]
        public string OrgDescription { get; set; }

        [Column("company_code")]
        public string CompanyCode { get; set; }

        [Column("company_description")]
        public string CompanyDescription { get; set; }

        [Column("accountability_status")]
        public string AccountabilityStatus { get; set; }

        [Column("total_accountability")]
        public string TotalAccountability { get; set; }

        [Column("agreed")]
        public bool Agreed { get; set; }

        [Column("agreed_date")]
        public string AgreedDate { get; set; }

    }
}
